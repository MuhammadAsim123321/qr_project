// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Identity_Login.Data;
using Identity_Login.Models;
using Identity_Login.Models.dbModels;
using Identity_Login.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Identity_Login.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [ValidateNever]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [DisplayName("First Name")]
            public string FirstName { get; set; }
            [Required]
            [DisplayName("Last Name")]
            public string LastName { get; set; }
            //[Required]
            //[DisplayName("Company Name")]
            //public string CompanyName { get; set; }
            //[Required]
            //[DisplayName("Address 1")]
            //public string Address1 { get; set; }
            //[Display(Name = "Address 2")]
            //public string Address2 { get; set; }
            //[Required]
            //[Display(Name = "Post Code")]
            //public string PostCode { get; set; }
            //public string City { get; set; }
            //[Required]
            //public string Country { get; set; }
            //[Required]
            //[Display(Name = "Phone")]
            //public string PhoneNumber { get; set; }
            //[Required]
            //[Display(Name = "TVA Number")]
            //public string TvaNumber { get; set; }

            //[ValidateNever]
            //public IEnumerable<SelectListItem> EuropeanCountries { get; set; }

            public string? Role { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
            public int? StationId { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> Stations { get; set; }
        }

        private List<SelectListItem> GetCountryList()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem{Value = "Belgium",Text = "Belgium"},
                new SelectListItem { Value = "Austria", Text = "Austria" },
                new SelectListItem { Value = "Croatia", Text = "Croatia" },
                new SelectListItem { Value = "Czech Republic", Text = "Czech Republic" },
                new SelectListItem { Value = "Denmark", Text = "Denmark" },
                new SelectListItem { Value = "Estonia", Text = "Estonia" },
                new SelectListItem { Value = "Finland", Text = "Finland" },
                new SelectListItem { Value = "France", Text = "France" },
                new SelectListItem { Value = "Germany", Text = "Germany" },
                new SelectListItem { Value = "Greece", Text = "Greece" },
                new SelectListItem { Value = "Hungary", Text = "Hungary" },
                new SelectListItem { Value = "Iceland", Text = "Iceland" },
                new SelectListItem { Value = "Ireland", Text = "Ireland" },
                new SelectListItem { Value = "Italy", Text = "Italy" },
                new SelectListItem { Value = "Latvia", Text = "Latvia" },
                new SelectListItem { Value = "Lithuania", Text = "Lithuania" },
                new SelectListItem { Value = "Luxembourg", Text = "Luxembourg" },
                new SelectListItem { Value = "Netherlands", Text = "Netherlands" },
                new SelectListItem { Value = "Norway", Text = "Norway" },
                new SelectListItem { Value = "Poland", Text = "Poland" },
                new SelectListItem { Value = "Portugal", Text = "Portugal" },
                new SelectListItem { Value = "Romania", Text = "Romania" },
                new SelectListItem { Value = "Slovakia", Text = "Slovakia" },
                new SelectListItem { Value = "Slovenia", Text = "Slovenia" },
                new SelectListItem { Value = "Spain", Text = "Spain" },
                new SelectListItem { Value = "Sweden", Text = "Sweden" }
            };
            return list;
        }
        private SelectList GetStations()
        {
            return new SelectList(_context.Stations.ToList(), "StationId", "Name");
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            //if (!_roleManager.RoleExistsAsync(RolesSD.USER).GetAwaiter().GetResult())
            //{
            //    _roleManager.CreateAsync(new IdentityRole(RolesSD.ADMIN)).GetAwaiter().GetResult();
            //    await _roleManager.CreateAsync(new IdentityRole(RolesSD.USER));
            //}
            if (!_roleManager.RoleExistsAsync(RolesSD.ADMIN).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(RolesSD.ADMIN));
            }
            if (!_roleManager.RoleExistsAsync(RolesSD.SUPERVISOR).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(RolesSD.SUPERVISOR));
            }
            //if (!_roleManager.RoleExistsAsync(RolesSD.USER).GetAwaiter().GetResult())
            //{
            //    await _roleManager.CreateAsync(new IdentityRole(RolesSD.USER));
            //}

            //Input = new InputModel()
            //{
            //    //EuropeanCountries = GetCountryList(),
            //    RoleList = _roleManager.Roles.Select(r => r.Name).Select(n => new SelectListItem() {
            //        Value = n,
            //        Text = n
            //    }),
            //    Stations = GetStations()
            //};
            Input = new InputModel()
            {
                Role = "Supervisor",
                RoleList = _roleManager.Roles
                .Where(r => r.Name == RolesSD.ADMIN || r.Name == RolesSD.SUPERVISOR)
                .Select(n => new SelectListItem()
                {
                    Value = n.Name,
                    Text = n.Name
                }),
                        Stations = GetStations()
                    };

                    ReturnUrl = returnUrl;
                    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/"); // Default to home page if returnUrl is not provided
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // 1. Station validation FIRST!
            if (Input.Role == "Supervisor" && (Input.StationId == null || Input.StationId == 0))
            {
                TempData["error"] = "Station selection is required for Staff/ Supervisor role.";
                // Repopulate dropdowns
                Input.RoleList = _roleManager.Roles
                    .Where(r => r.Name == RolesSD.ADMIN || r.Name == RolesSD.SUPERVISOR)
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    }).ToList();
                Input.Stations = GetStations();
                return Page();
            }
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                //user.City = Input.City;
                //user.Country = Input.Country;
                //user.Address1 = Input.Address1;
                //user.Address2 = Input.Address2;
                //user.PostCode = Input.PostCode;
                //user.CompanyName = Input.CompanyName;
                //user.PhoneNumber = Input.PhoneNumber;
                //user.TVAnumber = Input.TvaNumber;
                user.EmailConfirmed = true;   // We checked it later
                Input.Password = Input.Password;

                var result = await _userManager.CreateAsync(user, Input.Password); // Using Email As Default Password


                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!string.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    //else
                    //{
                    //    await _userManager.AddToRoleAsync(user, RolesSD.USER);
                    //}

                    await AddUserStation(user.Id, Input.StationId ?? 0);
                   

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Repopulate the RoleList and EuropeanCountries dropdown if ModelState is invalid
            Input.RoleList = _roleManager.Roles
    .Where(r => r.Name == RolesSD.ADMIN || r.Name == RolesSD.SUPERVISOR)
    .Select(r => new SelectListItem
    {
        Value = r.Name,
        Text = r.Name
    }).ToList();
            //Input.RoleList = _roleManager.Roles
            //    .Select(r => new SelectListItem
            //    {
            //        Value = r.Name,
            //        Text = r.Name
            //    }).ToList();

            //Input.EuropeanCountries = GetCountryList();
            Input.Stations = GetStations();

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task<bool> AddUserStation(string Id, int StationId)
        {
            if (!string.IsNullOrEmpty(Id) && StationId > 0)
            {
                StaffStationMapping staffStationMapping = new StaffStationMapping();
                staffStationMapping.Id = Id;
                staffStationMapping.StationId = StationId;
                staffStationMapping.CreatedOn = DateTime.UtcNow;
                staffStationMapping.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier); // Current user ID
                staffStationMapping.IsDeleted = false;
                _context.Add(staffStationMapping);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
