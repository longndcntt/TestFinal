using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Services;
using TestFinal.Enums;
using TestFinal.Model;
using Xamarin.Forms;

namespace TestFinal.ViewModels
{
    public class ReceiptPageViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private double _total;
        private string _searchContent;
        private ObservableCollection<Receipt> _receiptList;
        private Receipt _selectedReceipt;
        private int _selectedWay;

        public double Total { get => _total; set { SetProperty(ref _total, value); } }
        public string SearchContent { get => _searchContent; set { SetProperty(ref _searchContent, value); } }
        public int SelectedWay { get => _selectedWay; set { SetProperty(ref _selectedWay, value); } }
        public List<String> SortWay { get; set; }
        public ObservableCollection<Receipt> ReceiptList { get => _receiptList; set { SetProperty(ref _receiptList, value); } }
        public Receipt SelectedReceipt
        {
            get => _selectedReceipt; set
            {
                SetProperty(ref _selectedReceipt, value);
                if (SelectedReceipt != null)
                {

                }
            }
        }
        #endregion

        #region DelegateCommand
        public DelegateCommand StatisticCommand { get; private set; }
        public DelegateCommand TextChangedSearchCommand { get; private set; }
        public DelegateCommand SelectedReceiptCommand { get; private set; }
        public DelegateCommand AddReceiptCommand { get; private set; }
        public DelegateCommand SelectedChangedSortWayCommand { get; private set; }
        #endregion

        #region Constructor
        public ReceiptPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            db = new Database();

            //Load the way to sort
            SortWay = new List<string>()
            {
                "By amount of money","By Date"
            };

            //Load receipt list on listview
            LoadReceiptList();

            //Get reciept total
            double total = GetReceiptTotal();
            if (total > 0)
            {
                Total = total;
            }

            //Delegate Command
            StatisticCommand = new DelegateCommand(StatisticReceipt);
            TextChangedSearchCommand = new DelegateCommand(TextChangedSearch);
            SelectedReceiptCommand = new DelegateCommand(SelectedReceiptToDetail);
            AddReceiptCommand = new DelegateCommand(AddReceipt);
            SelectedChangedSortWayCommand = new DelegateCommand(SelectedWayCommand);
        }





        #endregion

        #region Handle Event
        //Chọn cách sắp xếp 
        private void SelectedWayCommand()
        {
            if (SelectedWay >= 0)
            {
                switch (SelectedWay)
                {
                    case 0:
                        SortReceiptByMoney();
                        break;
                    case 1:
                        SortReceiptByDate();
                        break;
                }
            }
        }

        //Hàm chuyển đến trang thống kê
        private async void StatisticReceipt()
        {
            await _navigationService.NavigateAsync("StatisticReceipt");
        }

        //Hàm chuyển đến trang ReceiptDetail khi chọn 1 Receipt
        private async void SelectedReceiptToDetail()
        {
            if (SelectedReceipt != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add(ParamKey.ReceiptDetail.ToString(), SelectedReceipt);
                await _navigationService.NavigateAsync(ParamKey.ReceiptDetail.ToString(), parameters);
            }
        }

        //Hàm tìm kiếm theo danh mục và nội dung ghi chú
        private void TextChangedSearch()
        {
            if (ReceiptList != null || db != null)
            {
                List<Receipt> TempList = new List<Receipt>();

                TempList = db.SearchReceiptByContent(SearchContent);
                if (TempList != null)
                {
                    ReceiptList.Clear();
                    foreach (var item in TempList)
                    {
                        ReceiptList.Add(item);
                    }
                }
            }
            //Kiểm tra nội dung search có null -> load lại ReceiptList
            else if (SearchContent == null)
            {
                LoadReceiptList();
            }


        }

        //Hàm chuyển đến trang AddReceipt
        private async void AddReceipt()
        {
            await _navigationService.NavigateAsync("AddReceipt");
        }

        #endregion

        #region Method

        //Sort by date
        private void SortReceiptByDate()
        {
            if (ReceiptList != null && ReceiptList.Count > 0 && db != null)
            {
                ReceiptList.Clear();
                List<Receipt> TempList = db.SortReceiptByDate();
                foreach (var item in TempList)
                {
                    ReceiptList.Add(item);
                }
            }
        }

        //Sort by money
        private void SortReceiptByMoney()
        {
            if (ReceiptList != null && ReceiptList.Count > 0 && db != null)
            {
                ReceiptList.Clear();
                List<Receipt> TempList = db.SortReceiptByMoney();
                foreach (var item in TempList)
                {
                    ReceiptList.Add(item);
                }
            }
        }

        //Load danh sách Receipt
        private void LoadReceiptList()
        {
            List<Receipt> TempList = new List<Receipt>();
            TempList = db.SelectReceipt();
            if (TempList != null)
            {
                ReceiptList = new ObservableCollection<Receipt>();
                foreach (var item in TempList)
                {
                    ReceiptList.Add(item);

                }
            }
        }

        //Lấy tổng tiền Receipt
        private double GetReceiptTotal()
        {
            if (ReceiptList != null)
            {
                double total = 0;
                foreach (var item in ReceiptList)
                {
                    total += item.AmountOfMoney;
                }
                return total;
            }
            return -1;
        }


        #endregion

    }
}
