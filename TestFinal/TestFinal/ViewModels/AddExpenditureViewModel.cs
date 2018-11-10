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
    public class AddExpenditureViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private string _title;
        private string _kind;
        private string _selectedcategory;
        private double _amountOfMoney;
        private string _amountOfMoneyString;
        private DateTime _dateOfExpenditure;
        private string _note;

        private List<string> _category;

        public string TitleExpenditure { get => _title; set { SetProperty(ref _title, value); } }
        public string Kind { get => _kind; set { SetProperty(ref _kind, value); } }
        public string SelectedCategory { get => _selectedcategory; set { SetProperty(ref _selectedcategory, value); } }
        public double AmountOfMoney { get => _amountOfMoney; set { SetProperty(ref _amountOfMoney, value); } }
        public string AmountOfMoneyString { get => _amountOfMoneyString; set { SetProperty(ref _amountOfMoneyString, value); } }

        public DateTime DateOfExpenditure { get => _dateOfExpenditure; set { SetProperty(ref _dateOfExpenditure, value); } }
        public string Note { get => _note; set { SetProperty(ref _note, value); } }
        public List<string> Category { get => _category; set { SetProperty(ref _category, value); } }

        #endregion

        #region DelegateCommand
        public DelegateCommand AddExpenditureCommand { get; private set; }
        public DelegateCommand ClearCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public AddExpenditureViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();
            //String format cho datetime
            String.Format("{0:dd/MM/yyyy}", DateOfExpenditure);
            //Loadd category list
            LoadCategory();
            //Khởi tạo delegate
            AddExpenditureCommand = new DelegateCommand(AddExpenditure);
            ClearCommand = new DelegateCommand(ClearInfo);
            CancelCommand = new DelegateCommand(Cancel);
            BackCommand = new DelegateCommand(BackEvent);
        }


        #endregion

        #region Handle Event

        private async void BackEvent()
        {
            await _navigationService.GoBackAsync();
        }

        private async void Cancel()
        {
            await _navigationService.GoBackAsync();
        }

        private void ClearInfo()
        {
            TitleExpenditure = "";
            LoadCategory();
            AmountOfMoney = 0;
            Note = "";
        }

        private async void AddExpenditure()
        {
            if (SelectedCategory != null && TitleExpenditure != null && IsNumber(AmountOfMoneyString))
            {
                AmountOfMoney = double.Parse(AmountOfMoneyString);
                if (AmountOfMoney >0)
                {
                    Expenditure expenditure = new Expenditure()
                    {
                        TitleExpenditure = TitleExpenditure,
                        Kind = "Expenditure",
                        Category = SelectedCategory,
                        AmountOfMoney = AmountOfMoney,
                        DateOfExpenditure = DateOfExpenditure,
                        Note = Note
                    };
                    if (db.InsertExpenditure(expenditure))
                    {
                        await App.Current.MainPage.DisplayAlert("Notify", "Add successfully Expenditure", "Ok");
                        await _navigationService.NavigateAsync("PrismPage");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Notify", "You haven't entered amount of money yet!", "Ok");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Notify", "You haven't entered information yet!", "Ok");
            }
        }
        #endregion

        #region Method
        private void LoadCategory()
        {
            Category = new List<string>(new string[]
            {
                "Ăn uống, nội trợ","Quà tặng","Tiệc tùng","Giải trí","Y tế","Du lịch",
                "Mua sắm","Điện thoại","Phương tiện di chuyển","Gia dụng",
                "Điện nước, tiền nhà","Học phí","Cho vay","Trả nợ","Các khoản khác"
                
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
