//using System;
//using System.Collections.Generic;
//using MVCForum.Domain.Constants;
//using MVCForum.Domain.DomainModel;
//using MVCForum.Domain.Interfaces.Services;
//using MVCForum.Domain.Interfaces.UnitOfWork;

//namespace UniversityWebsite.Forum
//{
//    public class CreateUserViewModel
//    {
//        public string Email { get; set; }
//        public string Password { get; set; }
//        public bool IsAdmin { get; set; }
//    }
//    public interface IForumIntegrationService
//    {
//        void Register(CreateUserViewModel userModel);
//    }
//    public class ForumIntegrationService : IForumIntegrationService
//    {

//        protected readonly IUnitOfWorkManager UnitOfWorkManager;
//        protected readonly IMembershipService MembershipService;
//        protected readonly ILocalizationService LocalizationService;
//        protected readonly IRoleService RoleService;
//        protected readonly ISettingsService SettingsService;
//        protected readonly ILoggingService LoggingService;


//        private readonly IBannedEmailService _bannedEmailService;
//        private readonly IBannedWordService _bannedWordService;

//        public ForumIntegrationService(ILoggingService loggingService, IUnitOfWorkManager unitOfWorkManager, IMembershipService membershipService, ILocalizationService localizationService,
//            IRoleService roleService, ISettingsService settingsService, IBannedEmailService bannedEmailService, IBannedWordService bannedWordService)
//        {
//            _bannedEmailService = bannedEmailService;
//            _bannedWordService = bannedWordService;


//            UnitOfWorkManager = unitOfWorkManager;
//            MembershipService = membershipService;
//            LocalizationService = localizationService;
//            RoleService = roleService;
//            SettingsService = settingsService;
//            LoggingService = loggingService;
//        }

//        //public class ResultViewModel
//        //{
//        //    public ResultViewModel()
//        //    {
                
//        //    }
//        //    public ResultViewModel(bool isSuccess, string message=null)
//        //    {
//        //        Message = message;
//        //        IsSuccess = isSuccess;  
//        //    }
//        //    public string Message { get; set; }
//        //    public bool IsSuccess { get; set; }
//        //}

//        public void Register(CreateUserViewModel userModel)
//        {
//            if (SettingsService.GetSettings().SuspendRegistration != true)
//            {
//                using (UnitOfWorkManager.NewUnitOfWork())
//                    if (_bannedEmailService.EmailIsBanned(userModel.Email))
//                        throw new Exception(LocalizationService.GetResourceString("Error.EmailIsBanned"));
//                MemberRegisterLogic(userModel);

//            }
//            throw new Exception();
//        }

//        public void MemberRegisterLogic(CreateUserViewModel userModel)
//        {
//            using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
//            {
//                var userToSave = new MembershipUser
//                {
//                    UserName = _bannedWordService.SanitiseBannedWords(userModel.Email),
//                    Email = userModel.Email,
//                    Password = userModel.Password,
//                    IsApproved = true,
//                    Comment = string.Empty,
//                };

//                var manuallyAuthoriseMembers = SettingsService.GetSettings().ManuallyAuthoriseNewMembers;
//                var memberEmailAuthorisationNeeded = SettingsService.GetSettings().NewMemberEmailConfirmation ?? false;
//                if (manuallyAuthoriseMembers || memberEmailAuthorisationNeeded)
//                {
//                    userToSave.IsApproved = false;
//                }

//                var createStatus = MembershipService.CreateUser(userToSave);

//                if (createStatus != MembershipCreateStatus.Success)
//                {
//                    throw new Exception(MembershipService.ErrorCodeToString(createStatus));
//                }
//                try
//                {
//                    unitOfWork.Commit();
//                    if (userModel.IsAdmin)
//                    {
//                        var user = MembershipService.GetUserByEmail(userModel.Email);
//                        var role = RoleService.GetRole(AppConstants.AdminRoleName);
//                        user.Roles = new List<MembershipRole> { role };
//                    }
//                    unitOfWork.Commit();
//                }
//                catch (Exception ex)
//                {
//                    unitOfWork.Rollback();
//                    LoggingService.Error(ex);
//                    throw;
//                }
//            }

//        }

//        //public class ChangeUserRoleViewModel
//        //{
//        //    [Required]
//        //    public string Email { get; set; }
//        //    [Required]
//        //    public bool IsAdmin { get; set; }
//        //}

//        //[HttpPost]
//        //[Route("changeRole")]
//        ////[Authorize(Roles = AppConstants.AdminRoleName)]
//        //public JsonResult ChangeRole(SsoController.ChangeUserRoleViewModel userModel)
//        //{
//        //    try
//        //    {
//        //        using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
//        //        {
//        //            var user = MembershipService.GetUserByEmail(userModel.Email);
//        //            MembershipRole role;
//        //            role = RoleService.GetRole(userModel.IsAdmin ? AppConstants.AdminRoleName : "Standard Members");
//        //            user.Roles = new List<MembershipRole> { role };
//        //            try
//        //            {
//        //                unitOfWork.Commit();
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                unitOfWork.Rollback();
//        //                LoggingService.Error(ex);
//        //                return Json(new SsoController.ResultViewModel { IsSuccess = false, Message = ex.ToString() });
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        LoggingService.Error(ex);
//        //        return Json(new SsoController.ResultViewModel { IsSuccess = false, Message = ex.ToString() });
//        //    }
//        //    return Json(new SsoController.ResultViewModel { IsSuccess = true });
//        //}
//        //public class DeleteUserViewModel
//        //{
//        //    [Required]
//        //    public string Email { get; set; }
//        //}

//        //[HttpPost]
//        //[Route("changeRole")]
//        ////[Authorize(Roles = AppConstants.AdminRoleName)]
//        //public JsonResult DeleteUser(SsoController.ChangeUserRoleViewModel userModel)
//        //{
//        //    try
//        //    {
//        //        using (var unitOfWork = UnitOfWorkManager.NewUnitOfWork())
//        //        {
//        //            var user = MembershipService.GetUserByEmail(userModel.Email);
//        //            if (user == null)
//        //            {
//        //                throw new ApplicationException("Cannot delete user - user does not exist");
//        //            }
//        //            MembershipService.Delete(user, unitOfWork);
//        //            try
//        //            {
//        //                unitOfWork.Commit();
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                unitOfWork.Rollback();
//        //                LoggingService.Error(ex);
//        //                return Json(new SsoController.ResultViewModel { IsSuccess = false, Message = ex.ToString() });
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        LoggingService.Error(ex);
//        //        return Json(new SsoController.ResultViewModel { IsSuccess = false, Message = ex.ToString() });
//        //    }
//        //    return Json(new SsoController.ResultViewModel { IsSuccess = true });
//        //}
//    }
//}