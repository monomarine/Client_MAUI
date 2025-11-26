using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.DTO
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "необходимо ввести логин или почту")]
        public string LoginOrEmail { get; set; } = null!;
        [Required(ErrorMessage = "необходимо ввести пароль")]
        [MinLength(8, ErrorMessage = "минимальная длина пароля 8 символов")]
        public string Password { get; set; } = null!;
    }
}
