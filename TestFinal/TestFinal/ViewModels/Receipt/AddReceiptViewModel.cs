﻿using Prism.Commands;
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
using TestFinal.Model;
using Xamarin.Forms;

namespace TestFinal.ViewModels
{
    public class AddReceiptViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private string _title;
        private string _kind;
        private string _selectedcategory;
        private string _amountOfMoneyString;
        private double _amountOfMoney;
        private DateTime _dateOfReceipt;
        private string _note;
        
        private List<string> _category;

        public string TitleReceipt { get => _title; set { SetProperty(ref _title, value); } }
        public string Kind { get => _kind; set { SetProperty(ref _kind, value); } }
        public string SelectedCategory { get => _selectedcategory; set { SetProperty(ref _selectedcategory, value); } }
        public double AmountOfMoney { get => _amountOfMoney; set { SetProperty(ref _amountOfMoney, value); } }
        public string AmountOfMoneyString { get => _amountOfMoneyString; set { SetProperty(ref _amountOfMoneyString, value); } }
        public DateTime DateOfReceipt { get => _dateOfReceipt; set { SetProperty(ref _dateOfReceipt, value); } }
        public string Note { get => _note; set { SetProperty(ref _note, value); } }
        public List<string> Category { get => _category; set { SetProperty(ref _category, value); } }

        #endregion

        #region DelegateCommand
        public DelegateCommand AddReceiptCommand { get;private set; }
        public DelegateCommand ClearCommand { get;private set; }
        public DelegateCommand CancelCommand { get;private set; }
        public DelegateCommand BackCommand { get;private set; }
        #endregion

        #region Constructor
        public AddReceiptViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();
            //String format cho datetime
            String.Format("{0:dd/MM/yyyy}", DateOfReceipt);
            //Loadd category list
            LoadCategory();
            //Khởi tạo delegate
            AddReceiptCommand = new DelegateCommand(AddReceipt);
            ClearCommand = new DelegateCommand(ClearInfo);
            CancelCommand = new DelegateCommand(Cancel);
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

        private async void Cancel()
        {
            await _navigationService.GoBackAsync();
        }

        private void ClearInfo()
        {
            TitleReceipt = "";
            LoadCategory();
            AmountOfMoneyString = "";
            Note = "";
        }

        private async void AddReceipt()
        {
            if(SelectedCategory != null && TitleReceipt!=null && IsNumber(AmountOfMoneyString))
            {
                AmountOfMoney = double.Parse(AmountOfMoneyString);
                if (AmountOfMoney > 0)
                {
                    Receipt receipt = new Receipt()
                    {
                        TitleReceipt = TitleReceipt,
                        Kind = "Receipt",
                        Category = SelectedCategory,
                        AmountOfMoney = AmountOfMoney,
                        DateOfReceipt = DateOfReceipt,
                        Note = Note,
                        Image = ImageStream,
                    };
                    if (db.InsertReceipt(receipt))
                    {
                        await App.Current.MainPage.DisplayAlert("Notify", "Add successfully receipt", "Ok");
                        await _navigationService.NavigateAsync("PrismPage");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Notify", "Amount of money is not correct", "Ok");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Notify", "You haven't entered information yet!", "Ok");
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
