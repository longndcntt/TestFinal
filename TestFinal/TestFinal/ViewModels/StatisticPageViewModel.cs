using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestFinal.Model;
using Microcharts;
using Prism.Services;
using SkiaSharp;

namespace TestFinal.ViewModels
{
    public class StatisticPageViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private DateTime _fromDate;
        private DateTime _toDate;
        private bool _isExist;
        private double _totalReceipt;
        private double _totalExpenditure;
        private ObservableCollection<Receipt> _receiptList;
        private ObservableCollection<Expenditure> _expenditure;
        private DonutChart _chart;

        public bool IsExist { get => _isExist; set { SetProperty(ref _isExist, value); } }
        public double TotalReceipt { get => _totalReceipt; set { SetProperty(ref _totalReceipt, value); } }
        public DonutChart Chart { get => _chart; set { SetProperty(ref _chart, value); } }
        public double TotalExpenditure { get => _totalExpenditure; set { SetProperty(ref _totalExpenditure, value); } }
        public DateTime FromDate
        {
            get => _fromDate; set
            {
                SetProperty(ref _fromDate, value); if (FromDate != null)
                {
                    String.Format("{0:dd/MM/yyyy}", FromDate);
                }
            }
        }
        public DateTime ToDate
        {
            get => _toDate; set
            {
                SetProperty(ref _toDate, value); if (ToDate != null)
                {
                    String.Format("{0:dd/MM/yyyy}", ToDate);
                }
            }
        }
        public ObservableCollection<Receipt> ReceiptList { get => _receiptList; set { SetProperty(ref _receiptList, value); } }
        public ObservableCollection<Expenditure> ExpenditureList { get => _expenditure; set { SetProperty(ref _expenditure, value); } }
        #endregion

        #region DelegateCommand
        public DelegateCommand ChangeDateCommand { get; set; }
        #endregion

        #region Constructor
        public StatisticPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();

            //Delegate
            ChangeDateCommand = new DelegateCommand(ChangeDateEvent);
        }
        #endregion

        #region Event

        //Sự kiện thay đổi ngày tháng
        private async void ChangeDateEvent()
        {
            if (FromDate != null && ToDate != null && db != null)
            {
                ReceiptList = new ObservableCollection<Receipt>();
                ExpenditureList = new ObservableCollection<Expenditure>();
                List<Receipt> TempReceiptList = db.SelectReceiptByDate(FromDate, ToDate);
                List<Expenditure> TempExpenditureList = db.SelectExpendituretByDate(FromDate, ToDate);
                if ((TempReceiptList != null && TempReceiptList.Count() > 0) ||
                    (TempExpenditureList != null && TempExpenditureList.Count() > 0))
                {
                    foreach (var item in TempReceiptList)
                    {
                        ReceiptList.Add(item);
                    }
                    foreach (var item in TempExpenditureList)
                    {
                        ExpenditureList.Add(item);
                    }
                    LoadChart();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Notify",
                        "Don't have anything receipt and expenditure from" + FromDate.ToString() + " To " + ToDate.ToString(),
                        "OK");
                }
            }
        }
        #endregion

        #region Method

        //Load Chart
        private void LoadChart()
        {
            if (ReceiptList != null && ExpenditureList != null)
            {
                TotalExpenditure = 0;
                TotalReceipt = 0;
                IsExist = true;
                foreach (var item in ReceiptList)
                {
                    TotalReceipt += item.AmountOfMoney;
                }
                foreach (var item in ExpenditureList)
                {
                    TotalExpenditure += item.AmountOfMoney;
                }
                var entries = new[]
                {
                    new Entry((float)TotalReceipt)
                    {
                         Label = "Receipt",
                        ValueLabel = DoubleToCurrency(TotalReceipt),
                        Color = SKColor.Parse("#005900")
                    },
                    new Entry((float)TotalExpenditure)
                    {
                         Label = "Expenditure",
                        ValueLabel = DoubleToCurrency(TotalExpenditure),
                        Color = SKColor.Parse("#e00707")
                    }
                };
                Chart = new DonutChart();
                Chart.Entries = entries.Where(x => x.Value > 0);
                Chart.LabelTextSize = 26;
                Chart.BackgroundColor = SKColor.Parse("#fefefe");
            }
        }
        
        public string DoubleToCurrency(double c)
        {
            c = c / 22000;
            string s = string.Format("{0:C2}", c);
            return s;
        }
        #endregion

    }
}
