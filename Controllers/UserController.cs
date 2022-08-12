using coop2._0.Model;
using coop2._0.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("list/{page}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ItemsModel<UserItemModel>>> GetUsers(int page)
        {
            try
            {
                ItemsModel<UserItemModel> users = await _userService.FindUsers(page);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{cif}")]
        public async Task<ActionResult<UserItemModel>> GetUser(string cif)
        {
            try
            {
                UserItemModel user = await _userService.FindUser(cif);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search/{value}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<UserItemModel>>> GetBy(string value)
        {
            try
            {
                IEnumerable<UserItemModel> users = await _userService.FindBy(value);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("validate")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> ValidateUsers([FromBody] List<string> users)
        {
            try
            {
                Response response = await _userService.Validate(users);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> RejectUsers([FromBody] List<string> users)
        {
            try
            {
                Response response = await _userService.Reject(users);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Response>> DeleteUsers([FromBody] List<string> users)
        {
            try
            {
                Response response = await _userService.Delete(users);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/info")]
        public async Task<ActionResult<Response>> ChangeUserInfo([FromBody] UserInfoModel model)
        {
            try
            {
                Response response = await _userService.ChangeInfo(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/email")]
        public async Task<ActionResult<Response>> ChangeUserEmail([FromBody] EmailUpdateModel model)
        {
            try
            {
                Response response = await _userService.ChangeEmail(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/password")]
        public async Task<ActionResult<Response>> ChangeUserPassword([FromBody] PasswordUpdateModel model)
        {
            try
            {
                Response response = await _userService.ChangePassword(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data);
            }
        }
    }
}