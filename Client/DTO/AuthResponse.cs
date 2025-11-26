using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DTO
{
    internal class AuthResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ValidTo { get; set; }
        public UserDTO User { get; set; } = new UserDTO();
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
