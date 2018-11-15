using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Microcharts;
using System.Collections.ObjectModel;
using TestFinal.Model;
using SkiaSharp;

namespace TestFinal.ViewModels
{
    public class StatisticReceiptViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private DonutChart _receiptChart;
        private ObservableCollection<Receipt> _receiptList;
        private DateTime _fromDate;
        private DateTime _toDate;
        private bool _isExist;
        private Receipt _selectedReceipt;
        private double _total;


        public DonutChart ReceiptChart { get => _receiptChart; set { SetProperty(ref _receiptChart, value); } }
        public bool IsExist { get => _isExist; set { SetProperty(ref _isExist, value); } }
        public double Total { get => _total; set { SetProperty(ref _total, value); } }
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
        public Receipt SelectedReceipt
        {
            get => _selectedReceipt; set
            {
                SetProperty(ref _selectedReceipt, value);
            }
        }
        public ObservableCollection<Receipt> ReceiptList { get => _receiptList; set { SetProperty(ref _receiptList, value); } }
        #endregion

        #region DelegateCommand
        public DelegateCommand ChangeDateCommand { get; private set; }
        public DelegateCommand SelectedReceiptCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public StatisticReceiptViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();
            Total = 0;
            LoadReceiptChart();
            //Delegate Command
            ChangeDateCommand = new DelegateCommand(ChangeDateEvent);
            SelectedReceiptCommand = new DelegateCommand(SelectedReceiptToDetail);
            BackCommand = new DelegateCommand(BackEvent);
        }

        #endregion

        #region Handle Event
        private async void BackEvent()
        {
            await _navigationService.GoBackAsync();
        }


        //Sự kiện thay đổi ngày
        private async void ChangeDateEvent()
        {
            if (FromDate != null && ToDate != null && db != null)
            {
                ReceiptList = new ObservableCollection<Receipt>();
                List<Receipt> TempList = db.SelectReceiptByDate(FromDate, ToDate);
                if (TempList != null && TempList.Count() > 0)
                {
                    foreach (var item in TempList)
                    {
                        ReceiptList.Add(item);
                    }
                    LoadReceiptChart();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Notify",
                        "Don't have anything receipt from" + FromDate.ToString() + " To " + ToDate.ToString(),
                        "OK");
                }
            }
        }

        //Chuyển đến màn hình detail
        private async void SelectedReceiptToDetail()
        {
            if (SelectedReceipt != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("ReceiptDetail", SelectedReceipt);
                await _navigationService.NavigateAsync("ReceiptDetail", parameters);
            }
        }
        #endregion

        #region Method

        //Load đồ thị thống kê
        public void LoadReceiptChart()
        {
            if (ReceiptList != null)
            {
                IsExist = true;
                double salary = 0;
                double bonus = 0;
                double subsidy = 0;
                double gift = 0;
                double part_time = 0;
                double business = 0;
                double real_estimate = 0;
                double stock = 0;
                double acquittancce = 0;
                double interest_rate = 0;
                double loan = 0;
                double another = 0;
                foreach (var item in ReceiptList)
                {
                    switch (item.Category)
                    {
                        case "Tiền lương":
                            salary += item.AmountOfMoney;
                            break;
                        case "Tiền thưởng":
                            bonus += item.AmountOfMoney;
                            break;
                        case "Quà tặng":
                            gift += item.AmountOfMoney;
                            break;
                        case "Trợ cấp":
                            subsidy += item.AmountOfMoney;
                            break;
                        case "Làm thêm":
                            part_time += item.AmountOfMoney;
                            break;
                        case "Kinh doanh":
                            business += item.AmountOfMoney;
                            break;
                        case "Bất động sản":
                            real_estimate += item.AmountOfMoney;
                            break;
                        case "Cổ phiếu":
                            stock += item.AmountOfMoney;
                            break;
                        case "Nợ trả":
                            acquittancce += item.AmountOfMoney;
                            break;
                        case "Lãi suất":
                            interest_rate += item.AmountOfMoney;
                            break;
                        case "Vay nợ":
                            loan += item.AmountOfMoney;
                            break;
                        case "Các khoản khác":
                            another += item.AmountOfMoney;
                            break;
                        default:
                            another += item.AmountOfMoney;
                            break;
                    }
                    Total += item.AmountOfMoney;
                }

                var entries = new[]
                {
                    new Entry((float)salary)
                {
                    Label = "Tiền lương",
                    ValueLabel =  DoubleToCurrency(salary),
                    Color = SKColor.Parse("#39241e")
                },
                new Entry((float)bonus)
                {
                    Label = "Tiền thưởng",
                    ValueLabel =  DoubleToCurrency(bonus),
                    Color = SKColor.Parse("#1d8b24")
                },
                new Entry((float)subsidy)
                {
                     Label = "Tiền trợ cấp",
                     ValueLabel =  DoubleToCurrency(subsidy),
                     Color = SKColor.Parse("#809c79")
                },
                new Entry((float)gift)
                {
                     Label = "Quà tặng",
                     ValueLabel =  DoubleToCurrency(gift),
                     Color = SKColor.Parse("#3ec797")
                },
                    new Entry((float)part_time)
                {
                    Label = "Làm thêm",
                    ValueLabel =  DoubleToCurrency(part_time),
                    Color = SKColor.Parse("#86825a")
                },
                     new Entry((float)business)
                {
                    Label = "Kinh doanh",
                    ValueLabel =  DoubleToCurrency(business),
                    Color = SKColor.Parse("#947565")
                },
                      new Entry((float)real_estimate)
                {
                    Label = "Bất động sản",
                    ValueLabel =  DoubleToCurrency(real_estimate),
                    Color = SKColor.Parse("#a0db8e")
                },
                       new Entry((float)stock)
                {
                    Label = "Cổ phiếu",
                    ValueLabel =  DoubleToCurrency(stock),
                    Color = SKColor.Parse("#9dd7b4")
                },
                        new Entry((float)acquittancce)
                {
                    Label = "Trả nợ",
                    ValueLabel =  DoubleToCurrency(acquittancce),
                    Color = SKColor.Parse("#ffa500")
                },
                         new Entry((float)interest_rate)
                {
                    Label = "Tiền lãi",
                    ValueLabel = DoubleToCurrency(interest_rate),
                    Color = SKColor.Parse("#000080")
                },
                          new Entry((float)loan)
                {
                    Label = "Vay nợ",
                    ValueLabel = DoubleToCurrency(loan),
                    Color = SKColor.Parse("#b6fcd5")
                },
                           new Entry((float)another)
                {
                    Label = "Các khoản khác",
                    ValueLabel = DoubleToCurrency(another),
                    Color = SKColor.Parse("#eadcce")
                },
                };
                ReceiptChart = new DonutChart();
                ReceiptChart.Entries = entries.Where(x => x.Value > 0);
                ReceiptChart.LabelTextSize = 26;
                ReceiptChart.BackgroundColor = SKColor.Parse("#fefefe");

            }
            else
            {
                IsExist = false;
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
