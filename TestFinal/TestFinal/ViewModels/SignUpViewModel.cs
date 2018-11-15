using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using TestFinal.Model;

namespace TestFinal.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private string _username;
        private string _password;
        private string _re_password;
        private string _displayName;
        private string _address;
        private string _phone;
        private string _company;
        private int _age;

        public string Username { get => _username; set { SetProperty(ref _username, value); } }
        public string Password { get => _password; set { SetProperty(ref _password, value); } }
        public string Re_password { get => _re_password; set { SetProperty(ref _re_password, value); } }
        public string DisplayName { get => _displayName; set { SetProperty(ref _displayName, value); } }
        public int Age { get => _age; set { SetProperty(ref _age, value); } }
        public string Address { get => _address; set { SetProperty(ref _address, value); } }
        public string Phone { get => _phone; set { SetProperty(ref _phone, value); } }
        public string Comapny { get => _company; set { SetProperty(ref _company, value); } }

        #endregion

        #region DelegateCommand
        public DelegateCommand SignUpCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        #endregion

        #region Constructor
        public SignUpViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();

            SignUpCommand = new DelegateCommand(SignUp);
            BackCommand = new DelegateCommand(BackEvent);
        }

        #endregion

        #region Handle Event

        private async void BackEvent()
        {
            await _navigationService.GoBackAsync();
        }


        private async void SignUp()
        {
            if (Username != null || Password != null)
            {
                User us = new User()
                {
                    UserName = Username,
                    Password = Password,
                    DisplayName = DisplayName,
                    Age = Age,
                    Company = Comapny,
                    Phone = Phone,
                    Address = Address,
                };
                if (db.InsertUser(us))
                {
                    await App.Current.MainPage.DisplayAlert("Notify", "Sign Up successfully", "Ok");
                    await _navigationService.GoBackToRootAsync();
                }

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Notify","You haven't entered username or password","Ok");
                return;
            }
                
        }
        #endregion

        #region Method

        #endregion
    }
}
