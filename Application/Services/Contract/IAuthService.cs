using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Auth;
using ViewModel.HttpResponse;

namespace Application.Services.Contract
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterVM model);
        Task<ApiResponse<string>> LoginAsync(LoginVM model);
    }
}
