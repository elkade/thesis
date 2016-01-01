using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Http;
using System.Web.Mvc;
using MVCForum.Domain.Constants;
using MVCForum.Domain.DomainModel;
using MVCForum.Domain.Interfaces.Services;
using MVCForum.Domain.Interfaces.UnitOfWork;
using MVCForum.Website.Application;

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

        protected MembershipUser LoggedOnReadOnlyUser;
        protected MembershipRole UsersRole;



        private readonly IPostService _postService;
        private readonly IReportService _reportService;
        private readonly IEmailService _emailService;
        private readonly IPrivateMessageService _privateMessageService;
        private readonly IBannedEmailService _bannedEmailService;
        private readonly IBannedWordService _bannedWordService;
        private readonly ITopicNotificationService _topicNotificationService;
        private readonly IPollAnswerService _pollAnswerService;
        private readonly IVoteService _voteService;
        private readonly ICategoryService _categoryService;

        public SsoController(ILoggingService loggingService, IUnitOfWorkManager unitOfWorkManager, IMembershipService membershipService, ILocalizationService localizationService,
            IRoleService roleService, ISettingsService settingsService, IPostService postService, IReportService reportService, IEmailService emailService, IPrivateMessageService privateMessageService, IBannedEmailService bannedEmailService, IBannedWordService bannedWordService, ITopicNotificationService topicNotificationService, IPollAnswerService pollAnswerService, IVoteService voteService, ICategoryService categoryService)
        {
            _postService = postService;
            _reportService = reportService;
            _emailService = emailService;
            _privateMessageService = privateMessageService;
            _bannedEmailService = bannedEmailService;
            _bannedWordService = bannedWordService;
            _topicNotificationService = topicNotificationService;
            _pollAnswerService = pollAnswerService;
            _voteService = voteService;
            _categoryService = categoryService;


            UnitOfWorkManager = unitOfWorkManager;
            MembershipService = membershipService;
            LocalizationService = localizationService;
            RoleService = roleService;
            SettingsService = settingsService;
            LoggingService = loggingService;
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
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("register")]
        [System.Web.Http.Authorize(Roles = AppConstants.AdminRoleName)]
        public JsonResult Register(CreateUserViewModel userModel)
        {
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
                                    new
                                    {
                                        IsSuccess = false,
                                        Message = LocalizationService.GetResourceString("Error.EmailIsBanned")
                                    });
                        }
                    }

                    // Do the register logic
                    return MemberRegisterLogic(userModel);

                }
                return Json(new {IsSuccess = false, Message = ""});
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = true, Message = ex.ToString() });
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
                        if (userModel.IsAdmin)
                        {
                            var user = MembershipService.GetUserByEmail(userModel.Email);
                            var role = RoleService.GetRole(AppConstants.AdminRoleName);
                            user.Roles = new List<MembershipRole> { role };
                        }
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        unitOfWork.Rollback();
                        LoggingService.Error(ex);
                        return Json(new { IsSuccess = true, Message = ex.ToString() });
                    }
                }
            }

            return Json(new { IsSuccess = true });
        }

    }
}