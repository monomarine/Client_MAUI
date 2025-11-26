using Client.DTO;
using Client.Services;
using System.Threading.Tasks;

namespace Client.Views;

public partial class AuthPage : ContentPage
{
	private readonly IAuthService _authService;
	public AuthPage(IAuthService service)
	{
		InitializeComponent();
		_authService = service;
	}

    private void EnterButton_Clicked(object sender, EventArgs e)
    {
		var loginRequest = new LoginRequest
		{
			LoginOrEmail = LoginEntry.Text,
			Password = PasswordEntry.Text
		};

		 DisplayAlert("результат авторизации", getToken(loginRequest), "ok");
    }
	private string getToken(LoginRequest req)
	{
		var responce =  _authService.LoginAsync(req).Result;
		return responce.Token;
	}

}