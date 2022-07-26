using coop2._0.Model;
using coop2._0.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                Response response = await _userService.Register(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Data);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                TokenModel token = await _userService.Login(model);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("email-confirmation")]
        public async Task<IActionResult> Confirm([FromQuery] string param)
        {
            try
            {
                Response response = await _userService.ConfirmUser(param);
                return Ok(response);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("forget-password")]
        public async Task<IActionResult> Forget(ForgetPasswordModel model)
        {
            try
            {
                Response response = await _userService.ForgetPassword(model);
                return Ok(response);

            } catch (Exception ex)
            {
                return BadRequest(ex.Data);
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                Response response = await _userService.ResetPassword(model);
                return Ok(response);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("list/{page}")]
        public async Task<ActionResult<IEnumerable<UserItemModel>>> GetUsers(int page)
        {
            try
            {
                IEnumerable<UserItemModel> users = await _userService.GetAll(page);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("validate/{page}")]
        public async Task<ActionResult> ValidateUsers([FromBody] List<string> users, int page)
        {
            try
            {
                await _userService.Validate(users, page);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("reject/{page}")]
        public async Task<ActionResult> RejectUsers([FromBody] List<string> users, int page)
        {
            try
            {
                await _userService.Reject(users, page);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("delete/{page}")]
        public async Task<ActionResult> DeleteUsers([FromBody] List<string> users, int page)
        {
            try
            {
                await _userService.Delete(users, page);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}