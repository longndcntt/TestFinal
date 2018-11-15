using Plugin.Multilingual;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using TestFinal.Model;
using TestFinal.Resources;

namespace TestFinal.ViewModels
{
    public class SettingpageViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        private ObservableCollection<Language> _languages;
        private Language _selectedLanguage;

        public int MyProperty { get; set; }
        public ObservableCollection<Language> Languagess { get => _languages; set { SetProperty(ref _languages, value); } }
        public Language SelectedLanguage { get => _selectedLanguage; set { SetProperty(ref _selectedLanguage, value); } }

        #endregion

        #region Delegate
        public DelegateCommand SelectedLanguageChangedCommand { get; private set; }

        #endregion

        #region constructor
        public SettingpageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Languagess = new ObservableCollection<Language>()
            {
                new Language { DisplayName =  "English", ShortName = "en-US" },
                new Language { DisplayName =  "Việt Nam", ShortName = "vi-Vn" }
            };
            SelectedLanguageChangedCommand = new DelegateCommand(SelectedLanguageChangedEvent);
        }
        #endregion

        #region Event
        private void SelectedLanguageChangedEvent()
        {
            try
            {
                var culture = new CultureInfo(SelectedLanguage.ShortName);
                AppResources.Culture = culture;
                CrossMultilingual.Current.CurrentCultureInfo = culture;
                _navigationService.GoBackToRootAsync();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        #endregion

    }
}
