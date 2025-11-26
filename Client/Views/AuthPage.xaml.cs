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

    private async void EnterButton_Clicked(object sender, EventArgs e)
    {
		var loginRequest = new LoginRequest
		{
			LoginOrEmail = LoginEntry.Text,
			Password = PasswordEntry.Text
		};
		var responce = await _authService.LoginAsync(loginRequest);
        DisplayAlert("результат авторизации", responce.Token, "ok");
    }
	private string getToken(LoginRequest req)
	{
		var responce =  _authService.LoginAsync(req).Result;
		return responce.Token;
	}

}