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

        [HttpGet]
        [Route("list/{page}")]
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

        [HttpGet]
        [Route("{cif}")]
        public async Task<ActionResult<UserItemModel>> GetUser(string cif)
        {
            try
            {
                UserItemModel user = await _userService.FindUser(cif);
                return Ok(user);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("search/{value}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<UserItemModel>>> GetBy(string value)
        {
            try
            {
                IEnumerable<UserItemModel> users = await _userService.FindBy(value);
                return Ok(users);

            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("validate/{page}")]
        [Authorize(Roles = "ADMIN")]
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
        [Authorize(Roles = "ADMIN")]
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
        [Authorize(Roles = "ADMIN")]
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