﻿using System;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Business.DTOs;
using SocialChef.Business.Requests;
using SocialChef.Business.Services;
using Controller = LittleByte.Asp.Application.Controller;

namespace SocialChef.Application.Controllers
{
    public class ChefController : Controller
    {
        private readonly IChefService chefService;

        public ChefController(IChefService chefService)
        {
            this.chefService = chefService;
        }

        [AllowAnonymous]
        [HttpPost]
        [ResponseType(HttpStatusCode.Created, typeof(ChefDto))]
        public async Task<IActionResult> Create(CreateChefRequest request)
        {
            var dto = await chefService.CreateAsync(request);
            var response = new CreatedResult<ChefDto>(dto);
            return CreatedAtAction("Get", response);
        }

        [HttpGet]
        [ResponseType(HttpStatusCode.OK, typeof(ChefDto))]
        public async Task<ApiResult<ChefDto>> Get()
        {
            var userID = HttpContext.GetUserID();
            var dto = await chefService.GetChefByUserIDAsync(userID);
            return new OkResult<ChefDto>(dto);
        }
    }
}