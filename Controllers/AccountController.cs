﻿using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid username");

        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

        if (!result.Succeeded) return Unauthorized("Username or password is incorrect");

        return Ok(new NewUserDto
        {
            Username = user.UserName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var created_user = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (created_user.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "USER");
                if (roleResult.Succeeded)
                {
                    return Ok(
                        new NewUserDto
                        {
                            Username = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenService.CreateToken(appUser)
                        });
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, created_user.Errors);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}