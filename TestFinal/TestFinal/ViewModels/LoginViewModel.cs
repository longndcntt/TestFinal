using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestFinal.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationservice;
        Database db;
        private string _username;
        private string _password;

        public string Username { get => _username; set { SetProperty(ref _username, value); } }
        public string Password { get => _password; set { SetProperty(ref _password, value); } }
        #endregion

        #region Delegate
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand SignUpCommand { get; private set; }
        #endregion

        #region Constructor
        public LoginViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationservice = navigationService;
            db = new Database();
            db.createDatabase();
            LoginCommand = new DelegateCommand(Login);
            SignUpCommand = new DelegateCommand(SignUp);
        }
        #endregion

        #region Handle event
        private async void Login()
        {
            if (Username != null || Password != null)
            {
                if (db.CountUsers(Username, Password) > 0)
                {
                    await _navigationservice.NavigateAsync("PrismPage");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Notify", "Incorrect username or password", "Ok");

                }
            }
            
        }

        private async void SignUp()
        {
            await _navigationservice.NavigateAsync("SignUp");
        }
        #endregion

        #region Method

        #endregion
    }
}
