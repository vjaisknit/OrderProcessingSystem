using Application.Common.Constants;
using Application.Services.Contract;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Auth;
using ViewModel.HttpResponse;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            TokenService tokenService,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterVM model)
        {
            _logger.LogInformation("Attempting to register user: {Username}", model.Username);

            var user = new AppUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("User registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return ApiResponse<string>.FailResponse(result.Errors.Select(e => e.Description).ToList());
            }

            _logger.LogInformation("User registered successfully: {Username}", user.UserName);
            return ApiResponse<string>.SuccessResponse(AuthMessages.UserCreated, StatusCodes.Status201Created);
        }

        public async Task<ApiResponse<string>> LoginAsync(LoginVM model)
        {
            _logger.LogInformation("Attempting to log in user: {Username}", model.Username);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                _logger.LogWarning("Login failed: Invalid username {Username}", model.Username);
                return ApiResponse<string>.FailResponse(AuthMessages.InvalidCredentials, StatusCodes.Status401Unauthorized);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Login failed: Invalid credentials for user {Username}", model.Username);
                return ApiResponse<string>.FailResponse(AuthMessages.InvalidCredentials, StatusCodes.Status401Unauthorized);
            }

            var token = _tokenService.GenerateJwtToken(user.UserName);
            _logger.LogInformation("Login successful for user: {Username}", model.Username);
            return ApiResponse<string>.SuccessResponse(token);
        }
    }

}

