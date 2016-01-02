using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCForum.Domain.Constants;
using MVCForum.Domain.DomainModel;
using MVCForum.Domain.Interfaces.Services;
using MVCForum.Domain.Interfaces.UnitOfWork;

namespace UniversityWebsite.Forum.Controllers
{
    [System.Web.Http.RoutePrefix("sso")]
    public class SsoController : Controller
    {
        protected readonly IUnitOfWorkManager UnitOfWorkManager;
        protected readonly IMembershipService MembershipService;
        protected readonly ILocalizationService LocalizationService;
        protected readonly IRoleService RoleService;
        protected readonly ISettingsService SettingsService;
        protected readonly ILoggingService LoggingService;


        private readonly IBannedEmailService _bannedEmailService;
        private readonly IBannedWordService _bannedWordService;

        public SsoController(ILoggingService loggingService, IUnitOfWorkManager unitOfWorkManager, IMembershipService membershipService, ILocalizationService localizationService,
            IRoleService roleService, ISettingsService settingsService, IBannedEmailService bannedEmailService, IBannedWordService bannedWordService)
        {
            _bannedEmailService = bannedEmailService;
            _bannedWordService = bannedWordService;


            UnitOfWorkManager = unitOfWorkManager;
            MembershipService = membershipService;
            LocalizationService = localizationService;
            RoleService = roleService;
            SettingsService = settingsService;
            LoggingService = loggingService;
        }

        public class ResultViewModel
        {
            public ResultViewModel()
            {
                
            }
            public ResultViewModel(bool isSuccess, string message=null)
            {
                Message = message;
                IsSuccess = isSuccess;  
            }
            public string Message { get; set; }
            public bool IsSuccess { get; set; }
        }

        public class CreateUserViewModel
        {
            [Required]
            [StringLength(150, MinimumLength = 4)]
            public string Email { get; set; }
            [DataType(DataType.Password)]
            [Required]
            [StringLength(100, MinimumLength = 6)]
            public string Password { get; set; }
            [Required]
            public bool IsAdmin { get; set; }
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Register(CreateUserViewModel userModel)
        {
            if (!ModelState.IsValid)
                throw new Exception("Model invalid");

            try
            {
                if (SettingsService.GetSettings().SuspendRegistration != true)
                {
                    using (UnitOfWorkManager.NewUnitOfWork())
                    {
                        // Secondly see if the email is banned
                        if (_bannedEmailService.EmailIsBanned(userModel.Email))
                        {
                            return
                                Json(
                                    new ResultViewModel
                                    {
                                        IsSuccess = false,
                                        Message = LocalizationService.GetResourceString("Error.EmailIsBanned")
                                    });
                        }
                    }

                    // Do the register logic
                    return MemberRegisterLogic(userModel);

                }
                return Json(new ResultViewModel { IsSuccess = false, Message = "" });
            }
            catch (Exception ex)
            {
                return Json(new ResultViewModel { IsSuccess = false, Message = ex.Message });
            }
        }

        public JsonResult MemberRegisterLogic(CreateUserViewModel userModel)
        {
            using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
            {
                var userToSave = new MembershipUser
                {
                    UserName = _bannedWordService.SanitiseBannedWords(userModel.Email),
                    Email = userModel.Email,
                    Password = userModel.Password,
                    IsApproved = true,
                    Comment = string.Empty,
                };

                // Now check settings, see if users need to be manually authorised
                // OR Does the user need to confirm their email
                var manuallyAuthoriseMembers = SettingsService.GetSettings().ManuallyAuthoriseNewMembers;
                var memberEmailAuthorisationNeeded = SettingsService.GetSettings().NewMemberEmailConfirmation ?? false;
                if (manuallyAuthoriseMembers || memberEmailAuthorisationNeeded)
                {
                    userToSave.IsApproved = false;
                }

                var createStatus = MembershipService.CreateUser(userToSave);

                if (createStatus != MembershipCreateStatus.Success)
                {
                    ModelState.AddModelError(string.Empty, MembershipService.ErrorCodeToString(createStatus));
                }
                else
                {
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.Rollback();
                        LoggingService.Error(ex);
                        return Json(new ResultViewModel { IsSuccess = false, Message = ex.ToString() });
                    }
                }
            }
            using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
            {
                if (userModel.IsAdmin)
                {
                    var user = MembershipService.GetUserByEmail(userModel.Email);
                    var role = RoleService.GetRole(AppConstants.AdminRoleName);
                    user.Roles = new List<MembershipRole> { role };
                }
                try
                {
                    unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    LoggingService.Error(ex);
                    return Json(new ResultViewModel {IsSuccess = false, Message = ex.ToString()});
                }
            }
            return Json(new ResultViewModel { IsSuccess = true });
        }

        public class EditUserViewModel
        {
            [Required]
            public string OldEmail { get; set; }
            [Required]
            [StringLength(150, MinimumLength = 4)]
            public string NewEmail { get; set; }
            [Required]
            public bool IsAdmin { get; set; }
        }

        [HttpPost]
        public JsonResult Edit(EditUserViewModel userModel)
        {
            if (!ModelState.IsValid)
                throw new Exception("Model invalid");

            try
            {
                using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
                {
                    var user = MembershipService.GetUserByEmail(userModel.OldEmail);
                    if (user == null)
                    {
                        throw new ApplicationException("Cannot edit user - user does not exist");
                    }
                    if (userModel.OldEmail != userModel.NewEmail)
                    {
                        user.Email = userModel.NewEmail;
                        user.UserName = userModel.NewEmail;
                    }
                    MembershipRole role = RoleService.GetRole(userModel.IsAdmin ? AppConstants.AdminRoleName : "Standard Members");
                    user.Roles = new List<MembershipRole> { role };
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.Rollback();
                        LoggingService.Error(ex);
                        return Json(new ResultViewModel { IsSuccess = false, Message = ex.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex);
                return Json(new ResultViewModel { IsSuccess = false, Message = ex.ToString() });
            }
            return Json(new ResultViewModel { IsSuccess = true });
        }
        public class DeleteUserViewModel
        {
            [Required]
            public string Email { get; set; }
        }

        [HttpPost]
        public JsonResult Remove(DeleteUserViewModel userModel)
        {
            if(!ModelState.IsValid)
                throw new Exception("Model invalid");
            try
            {
                using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
                {
                    var user = MembershipService.GetUserByEmail(userModel.Email);
                    if (user == null)
                    {
                        throw new ApplicationException("Cannot delete user - user does not exist");
                    }
                    MembershipService.Delete(user, unitOfWork);
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.Rollback();
                        LoggingService.Error(ex);
                        return Json(new ResultViewModel { IsSuccess = false, Message = ex.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex);
                return Json(new ResultViewModel { IsSuccess = false, Message = ex.Message });
            }
            return Json(new ResultViewModel { IsSuccess = true });
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            //Log Exception e
            filterContext.ExceptionHandled = true;
            filterContext.Result = Json(new ResultViewModel { IsSuccess = false, Message = e.ToString() });
        }
        //protected override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    base.OnAuthorization(filterContext);

        //    if (!User.Identity.IsAuthenticated)
        //        filterContext.Result = Json(new { IsSuccess = false, Message = "Access denied." });
        //}
    }
}