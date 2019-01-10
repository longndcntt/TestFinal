using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Services;
using TestFinal.Enums;
using TestFinal.Helpers;
using TestFinal.Model;
using Xamarin.Forms;

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
            get => _selectedExpenditure;
            set
            {
                SetProperty(ref _selectedExpenditure, value);
                if (SelectedExpenditure != null)
                {
                    TitleExpenditure = SelectedExpenditure.TitleExpenditure;
                    Kind = SelectedExpenditure.Kind;
                    SelectedCategory = SelectedExpenditure.Category;
                    AmountOfMoneyString = SelectedExpenditure.AmountOfMoney.ToString();
                    DateOfExpenditure = SelectedExpenditure.DateOfExpenditure;
                    Note = SelectedExpenditure.Note;
                    ImageStream = SelectedExpenditure.Image;
                }
            }
        }
        #endregion

        #region DelegateCommand
        public DelegateCommand ConfirmCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        #endregion

        #region Constructor
        public EditExpenditureViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
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
            TakePhotoReceiveCommand = new DelegateCommand(async () => await TakePhotoReceiveExecute());
            ChoosePhotoReceiveCommand = new DelegateCommand(async () => await ChoosePhotoReceiveExecute());
        }

        #endregion

        private ImageSource _image;

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private byte[] _imageStream;

        public byte[] ImageStream
        {
            get => _imageStream;
            set => SetProperty(ref _imageStream, value);
        }

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
                    if (AmountOfMoney >0)
                    {
                        Expenditure.Category = SelectedCategory;
                        Expenditure.AmountOfMoney = AmountOfMoney;
                        Expenditure.DateOfExpenditure = DateOfExpenditure;
                        Expenditure.TitleExpenditure = TitleExpenditure;
                        Expenditure.Note = Note;
                        Expenditure.Image = ImageStream;

                        if (db.UpdateExpenditure(Expenditure))
                        {
                            await App.Current.MainPage.DisplayAlert("Notify", "Update Successfully", "OK");
                            await _navigationService.NavigateAsync(PageManager.PrismPage);
                        }
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Notify", "Amount of money is not correct", "OK");
                    }
                }
            }
        }

        private string _imagePath;

        public override async Task ChangeImage(string filePath, byte[] bytes)
        {
            _imagePath = filePath;
            ImageStream = bytes;
        }

        public ICommand TakePhotoReceiveCommand { get; }

        private async Task TakePhotoReceiveExecute()
        {
            await TakePhotoExecute();
            Image = ImageSource.FromStream(() => new MemoryStream(ImageStream));
        }

        public ICommand ChoosePhotoReceiveCommand { get; }

        private async Task ChoosePhotoReceiveExecute()
        {
            await ChoosePhotoExecute();
            Image = ImageSource.FromStream(() => new MemoryStream(ImageStream));
        }
        #endregion

        #region Method
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(ParamKey.EditExpenditure.ToString()))
            {
                SelectedExpenditure = (Expenditure)parameters[ParamKey.EditExpenditure.ToString()];
                ImageStream = SelectedExpenditure.Image;
                Image = ImageSource.FromStream(() => new MemoryStream(ImageStream));
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
