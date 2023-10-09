﻿using Contracts.Services;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Linq;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public UsersController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] UserForAuthDto userForAuth)
    {

        (bool result, UserDto userReturn) = await serviceManager.UserService.AuthenticateUser(userForAuth);
        if (!result)
            return Unauthorized("Wrong password");
        if (userReturn.IsNull())
            return NotFound("User with email not found");
        return Ok(new { user = userReturn });

    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
    {
         (bool result, UserDto userReturn) = await serviceManager.UserService.CreateUser(createUserDto);
        if (!result)
            return BadRequest("Email exists already");
        return Ok(new { user = userReturn });
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {

    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UserDto user)
    {

    }
}
