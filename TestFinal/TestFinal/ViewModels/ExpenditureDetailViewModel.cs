using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using TestFinal.Model;

namespace TestFinal.ViewModels
{
    public class ExpenditureDetailViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private string _title;
        private string _kind;
        private string _category;
        private double _amountOfMoney;
        private DateTime _dateOfExpenditure;
        private string _note;
        private Expenditure _selectedExpenditure;

        public string TitleExpenditure { get => _title; set { SetProperty(ref _title, value); } }
        public string Kind { get => _kind; set { SetProperty(ref _kind, value); } }
        public string Category { get => _category; set { SetProperty(ref _category, value); } }
        public double AmountOfMoney { get => _amountOfMoney; set { SetProperty(ref _amountOfMoney, value); } }
        public DateTime DateOfExpenditure { get => _dateOfExpenditure; set { SetProperty(ref _dateOfExpenditure, value); } }
        public string Note { get => _note; set { SetProperty(ref _note, value); } }
        public Expenditure SelectedExpenditure
        {
            get => _selectedExpenditure; set
            {
                SetProperty(ref _selectedExpenditure, value);
                if (SelectedExpenditure != null)
                {
                    TitleExpenditure = SelectedExpenditure.TitleExpenditure;
                    Kind = SelectedExpenditure.Kind;
                    Category = SelectedExpenditure.Category;
                    AmountOfMoney = SelectedExpenditure.AmountOfMoney;
                    DateOfExpenditure = SelectedExpenditure.DateOfExpenditure;
                    Note = SelectedExpenditure.Note;
                }
            }
        }

        #endregion

        #region DelegateCommand
        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }
        #endregion

        #region Constructor
        public ExpenditureDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();

            //Delegate Command
            EditCommand = new DelegateCommand(EditReicept);
            DeleteCommand = new DelegateCommand(DeleteExpenditure);
            BackCommand = new DelegateCommand(BackEvent);
        }

        #endregion

        #region Handle Event

        private async void BackEvent()
        {
            await _navigationService.GoBackAsync();
        }

        //Edit event
        private async void EditReicept()
        {
            if (SelectedExpenditure != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("EditExpenditure", SelectedExpenditure);
                await _navigationService.NavigateAsync("EditExpenditure", parameters);
            }
        }

        //Delete event
        private async void DeleteExpenditure()
        {
            if (SelectedExpenditure != null && db != null)
            {
                if (db.DeleteExpenditure(SelectedExpenditure))
                {
                    await App.Current.MainPage.DisplayAlert("Notify", "Delete successfully", "OK");
                    await _navigationService.NavigateAsync("PrismPage");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Notify", "Delete failed", "OK");
            }
        }
        #endregion

        #region Method
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ExpenditureDetail"))
            {
                SelectedExpenditure = (Expenditure)parameters["ExpenditureDetail"];
            }
        }

        #endregion
    }
}
