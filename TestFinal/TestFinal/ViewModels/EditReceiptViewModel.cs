using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TestFinal.Model;

namespace TestFinal.ViewModels
{
    public class EditReceiptViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private string _title;
        private string _kind;
        private string _amountOfMoneyString;
        private List<string> _category;
        private string _selectedcategory;
        private double _amountOfMoney;
        private DateTime _dateOfReceipt;
        private string _note;
        private Receipt _selectedReceipt;

        public string TitleReceipt { get => _title; set { SetProperty(ref _title, value); } }
        public string Kind { get => _kind; set { SetProperty(ref _kind, value); } }
        public string AmountOfMoneyString { get => _amountOfMoneyString; set { SetProperty(ref _amountOfMoneyString, value); } }
        public List<string> Category { get => _category; set { SetProperty(ref _category, value); } }
        public string SelectedCategory { get => _selectedcategory; set { SetProperty(ref _selectedcategory, value); } }
        public double AmountOfMoney { get => _amountOfMoney; set { SetProperty(ref _amountOfMoney, value); } }
        public DateTime DateOfReceipt
        {
            get => _dateOfReceipt; set
            {
                SetProperty(ref _dateOfReceipt, value);  } }
        public string Note { get => _note; set { SetProperty(ref _note, value); } }
        public Receipt SelectedReceipt
        {
            get => _selectedReceipt; set
            {
                SetProperty(ref _selectedReceipt, value);
                if (SelectedReceipt != null)
                {
                    TitleReceipt = SelectedReceipt.TitleReceipt;
                    Kind = SelectedReceipt.Kind;
                    SelectedCategory = SelectedReceipt.Category;
                    AmountOfMoney = SelectedReceipt.AmountOfMoney;
                    DateOfReceipt = SelectedReceipt.DateOfReceipt;
                    Note = SelectedReceipt.Note;
                }
            }
        }
        #endregion

        #region DelegateCommand
        public DelegateCommand ConfirmCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        #endregion

        #region Constructor
        public EditReceiptViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();
            //String format cho datetime
            String.Format("{0:dd/MM/yyyy}", DateOfReceipt);
            LoadCategory();
            //Delegate Command
            ConfirmCommand = new DelegateCommand(CofirmToSaveChange);
            BackCommand = new DelegateCommand(BackEvent);
        }

        #endregion

        #region Handle Event

        private async void BackEvent()
        {
            await _navigationService.GoBackAsync();
        }

        private async void CofirmToSaveChange()
        {
            if (db != null)
            {
                Receipt receipt = db.SearchReceiptById(SelectedReceipt.Id);
                if (receipt != null && IsNumber(AmountOfMoneyString))
                {
                    AmountOfMoney = double.Parse(AmountOfMoneyString);
                    receipt.Category = SelectedCategory;
                    receipt.AmountOfMoney = AmountOfMoney;
                    receipt.DateOfReceipt = DateOfReceipt;
                    receipt.TitleReceipt = TitleReceipt;
                    receipt.Note = Note;
                    
                    if (db.UpdateReceipt(receipt))
                    {
                        await App.Current.MainPage.DisplayAlert("Notify", "Update Successfully", "OK");
                        
                        await _navigationService.NavigateAsync("PrismPage");
                    }
                }
            }
        }
        #endregion

        #region Method
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("EditReceipt"))
            {
                SelectedReceipt = (Receipt)parameters["EditReceipt"];
            }
        }

        private void LoadCategory()
        {
            Category = new List<string>(new string[]
            {
                "Tiền lương","Tiền thưởng","Trợ cấp","Quà tặng","Làm thêm",
                "Kinh doanh","Bất động sản","Cổ phiếu","Nợ trả","Lãi suất",
                "Vay nợ","Các khoản khác"
            });
        }

        public bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }
        #endregion

    }
}
