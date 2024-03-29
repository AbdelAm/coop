﻿using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Services;
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
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<object> GetRequests([FromQuery] PaginationFilter filter)
        {
            return await _requestService.GetRequests(filter);
        }
        [HttpGet("filter/{status}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<object> FilterRequests( Status status, [FromQuery] PaginationFilter filter)
        {
            return await _requestService.FilterRequests(status,filter);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<object> GetRequestsByUser([FromQuery] PaginationFilter filter,string userId)
        {
            return await _requestService.GetRequestsByUser(userId, filter);
        }

        [HttpPost("list/{id}/{page}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _requestService.GetRequest(id);
            if (request == null)
                return NotFound();
            return Ok(request);
        }

        [HttpPost("delete")]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> RemoveRequest([FromBody] List<int> requests)
        {
            try
            {
                var deletedRequest = await _requestService.RemoveRequest(requests);
                return Ok(deletedRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> RejectRequest([FromBody] List<int> requests)
        {
            try
            {
                var res = await _requestService.RejectRequest(requests);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("validate")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> ValidateRequest([FromBody] List<int> requests)
        {
            try
            {
                var res = await _requestService.ValidateRequest(requests);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<Request>> AddRequest(RequestModel model)
        {
            return await _requestService.AddRequest(model);
        }
    }
}