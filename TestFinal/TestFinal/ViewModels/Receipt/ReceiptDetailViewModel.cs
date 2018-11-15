using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using TestFinal.Enums;
using TestFinal.Model;

namespace TestFinal.ViewModels
{
	public class ReceiptDetailViewModel : ViewModelBase
	{
        #region Properties
        INavigationService _navigationService;
        Database db;
        private string _title;
        private string _kind;
        private string _category;
        private double _amountOfMoney;
        private DateTime _dateOfReceipt;
        private string _note;
        private Receipt _selectedReceipt;

        public string TitleReceipt { get => _title; set { SetProperty(ref _title, value); } }
        public string Kind { get => _kind; set { SetProperty(ref _kind, value); } }
        public string Category { get => _category; set { SetProperty(ref _category, value); } }
        public double AmountOfMoney { get => _amountOfMoney; set { SetProperty(ref _amountOfMoney, value); } }
        public DateTime DateOfReceipt { get => _dateOfReceipt; set { SetProperty(ref _dateOfReceipt, value); } }
        public string Note { get => _note; set { SetProperty(ref _note, value); } }
        public Receipt SelectedReceipt { get => _selectedReceipt; set { SetProperty(ref _selectedReceipt, value);
                if (SelectedReceipt != null)
                {
                    TitleReceipt = SelectedReceipt.TitleReceipt;
                    Kind = SelectedReceipt.Kind;
                    Category = SelectedReceipt.Category;
                    AmountOfMoney = SelectedReceipt.AmountOfMoney;
                    DateOfReceipt = SelectedReceipt.DateOfReceipt;
                    Note = SelectedReceipt.Note;
                }
            } }

        #endregion

        #region DelegateCommand
        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand BackCommand { get; set; }
        #endregion

        #region Constructor
        public ReceiptDetailViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();

            //Delegate Command
            EditCommand = new DelegateCommand(EditReicept);
            DeleteCommand = new DelegateCommand(DeleteReceipt);
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
            if (SelectedReceipt != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add(ParamKey.EditReceipt.ToString(), SelectedReceipt);
                await _navigationService.NavigateAsync(ParamKey.EditReceipt.ToString(), parameters);
            }
        }

        //Delete event
        private async void DeleteReceipt()
        {
            if (SelectedReceipt != null && db != null)
            {
                if (db.DeleteReceipt(SelectedReceipt))
                {
                    await App.Current.MainPage.DisplayAlert("Notify","Delete successfully","OK");
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
            if (parameters.ContainsKey(ParamKey.ReceiptDetail.ToString()))
            {
                SelectedReceipt = (Receipt)parameters[ParamKey.ReceiptDetail.ToString()];
            }
        }
        #endregion
    }
}
