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
    public class EditExpenditureViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private string _title;
        private string _kind;
        private List<string> _category;
        private string _amountOfMoneyString;
        private string _selectedcategory;
        private double _amountOfMoney;
        private DateTime _dateOfExpenditure;
        private string _note;
        private Expenditure _selectedExpenditure;

        public string TitleExpenditure { get => _title; set { SetProperty(ref _title, value); } }
        public string Kind { get => _kind; set { SetProperty(ref _kind, value); } }
        public List<string> Category { get => _category; set { SetProperty(ref _category, value); } }
        public string AmountOfMoneyString { get => _amountOfMoneyString; set { SetProperty(ref _amountOfMoneyString, value); } }
        public string SelectedCategory { get => _selectedcategory; set { SetProperty(ref _selectedcategory, value); } }
        public double AmountOfMoney { get => _amountOfMoney; set { SetProperty(ref _amountOfMoney, value); } }
        public DateTime DateOfExpenditure
        {
            get => _dateOfExpenditure; set
            {
                SetProperty(ref _dateOfExpenditure, value);
            }
        }
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
                    SelectedCategory = SelectedExpenditure.Category;
                    AmountOfMoney = SelectedExpenditure.AmountOfMoney;
                    DateOfExpenditure = SelectedExpenditure.DateOfExpenditure;
                    Note = SelectedExpenditure.Note;
                }
            }
        }
        #endregion

        #region DelegateCommand
        public DelegateCommand ConfirmCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        #endregion

        #region Constructor
        public EditExpenditureViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();
            //String format cho datetime
            String.Format("{0:dd/MM/yyyy}", DateOfExpenditure);
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
                Expenditure Expenditure = db.SearchExpenditureById(SelectedExpenditure.Id);
                if (Expenditure != null && IsNumber(AmountOfMoneyString))
                {
                    AmountOfMoney = double.Parse(AmountOfMoneyString);
                    Expenditure.Category = SelectedCategory;
                    Expenditure.AmountOfMoney = AmountOfMoney;
                    Expenditure.DateOfExpenditure = DateOfExpenditure;
                    Expenditure.TitleExpenditure = TitleExpenditure;
                    Expenditure.Note = Note;

                    if (db.UpdateExpenditure(Expenditure))
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
            if (parameters.ContainsKey("EditExpenditure"))
            {
                SelectedExpenditure = (Expenditure)parameters["EditExpenditure"];
            }
        }

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
