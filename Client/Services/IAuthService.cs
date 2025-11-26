using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    internal interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
        Task<AuthResponse> RegisterAsync(CreateUserRequest createUser);
        Task<bool> IsTokenValidAsync();
        Task<string> RefreshTokenAsync();
    }
}
