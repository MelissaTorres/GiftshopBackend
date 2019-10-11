using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SS.GiftShop.Api.Identity;
using SS.GiftShop.Api.Identity.Models;
using SS.GiftShop.Core;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SS.GiftShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IDateTime _dateTime;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger<AccountController> logger, IDateTime dateTime)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _dateTime = dateTime;
        }

        [HttpGet("info")]
        [ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Info()
        {
            var user = await FindUser();
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = User.Identity.GetRoles()
                .ToList();

            var expiration = User.Identity.GetExpiration();
            return Ok(new UserInfo
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Roles = roles,
                ExpiresIn = expiration.HasValue
                    ? Convert.ToInt64((expiration.Value - _dateTime.Now).TotalSeconds)
                    : default(long?)
            });
        }

        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            var identityResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (identityResult.Succeeded)
            {
                return await Refresh();
            }

            return IdentityError(identityResult.Errors);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var status = LoginStatus.NotAllowed;
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    status = LoginStatus.NotConfirmed;
                }
                else
                {
                    status = await DoLogin(user, model);
                }
            }

            return Ok(new LoginResult { Status = status });
        }
        /*
        [HttpPost("sign-up")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody]SignUpModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };
            var identityResult = await _userManager.CreateAsync(user, model.Password);
            if (identityResult.Succeeded)
            {
                return NoContent();
            }

            return IdentityError(identityResult.Errors);
        }
        */
        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return NoContent();
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // This can happen when switching database connections during development
                // because user Id's won't match
                return Unauthorized();
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return NoContent();
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return NoContent();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return IdentityError(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("update-password")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var identityResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (identityResult.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(user, model.NewPassword, false, true);
                return NoContent();
            }

            return IdentityError(identityResult.Errors);
        }

        [AllowAnonymous]
        [HttpGet("authenticated")]
        [ProducesResponseType(typeof(AuthenticatedModel), StatusCodes.Status200OK)]
        public IActionResult IsAuthenticated()
        {
            return Ok(new AuthenticatedModel { IsAuthenticated = User.Identity.IsAuthenticated });
        }

        private async Task<LoginStatus> DoLogin(User user, LoginViewModel model)
        {
            //if (user.Status != EnabledStatus.Enabled)
            //{
            //    _logger.LogWarning("User account is not active.");
            //    return LoginStatus.Inactive;
            //}
            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("User email is not confirmed.");
                return LoginStatus.Inactive;
            }

            //var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);

            if (result.RequiresTwoFactor)
            {
                return LoginStatus.RequiresTwoFactor;
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return LoginStatus.IsLockedOut;
            }

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return LoginStatus.Success;
            }

            if (result.IsNotAllowed)
            {
                return LoginStatus.NotAllowed;
            }

            if (result == SignInResult.Failed)
            {
                return LoginStatus.Failed;
            }

            throw new ApplicationException("Unexpected login result");
        }

        private async Task<User> FindUser()
        {
            var id = User.Identity.GetUserId();

            return await _userManager.FindByIdAsync(id.ToString());
        }

        [NonAction]
        private BadRequestObjectResult IdentityError(IEnumerable<IdentityError> errors)
        {
            foreach (var identityError in errors)
            {
                ModelState.AddModelError(identityError.Code, identityError.Description);
            }

            return BadRequest(ModelState);
        }
    }
}
