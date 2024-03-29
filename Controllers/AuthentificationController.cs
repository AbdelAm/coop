﻿using coop2._0.Model;
using coop2._0.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private readonly IAuthentificationService _authService;

        public AuthentificationController(IAuthentificationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<Response>> Register([FromBody] RegisterModel model)
        {
            try
            {
                Response response = await _authService.Register(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Data);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenModel>> Login([FromBody] LoginModel model)
        {
            try
            {
                TokenModel token = await _authService.Login(model);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("email-confirmation")]
        public async Task<ActionResult<Response>> Confirm([FromQuery] string param)
        {
            try
            {
                Response response = await _authService.ConfirmUser(param);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("forget-password")]
        public async Task<ActionResult<Response>> Forget([FromBody] ForgetPasswordModel model)
        {
            try
            {
                Response response = await _authService.ForgetPassword(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Data);
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<ActionResult<Response>> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                Response response = await _authService.ResetPassword(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("administrateur/register")]
        public async Task<ActionResult<Response>> RegisterAdmin([FromBody] RegisterModel model)
        {
            try
            {
                Response response = await _authService.RegisterAdmin(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Data);
            }
        }
    }
}