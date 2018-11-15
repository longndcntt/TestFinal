using Microcharts;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestFinal.Model;

namespace TestFinal.ViewModels
{
    public class StatisticExpenditureViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private DonutChart _ExpenditureChart;
        private ObservableCollection<Expenditure> _ExpenditureList;
        private DateTime _fromDate;
        private DateTime _toDate;
        private bool _isExist;
        private Expenditure _selectedExpenditure;
        private double _total;


        public DonutChart ExpenditureChart { get => _ExpenditureChart; set { SetProperty(ref _ExpenditureChart, value); } }
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
        public Expenditure SelectedExpenditure
        {
            get => _selectedExpenditure; set
            {
                SetProperty(ref _selectedExpenditure, value);
            }
        }
        public ObservableCollection<Expenditure> ExpenditureList { get => _ExpenditureList; set { SetProperty(ref _ExpenditureList, value); } }
        #endregion

        #region DelegateCommand
        public DelegateCommand ChangeDateCommand { get; private set; }
        public DelegateCommand SelectedExpenditureCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }
        #endregion

        #region Constructor
        public StatisticExpenditureViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            db = new Database();
            db.createDatabase();
            Total = 0;
            LoadExpenditureChart();
            //Delegate Command
            ChangeDateCommand = new DelegateCommand(ChangeDateEvent);
            SelectedExpenditureCommand = new DelegateCommand(SelectedExpenditureToDetail);
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
                ExpenditureList = new ObservableCollection<Expenditure>();
                List<Expenditure> TempList = db.SelectExpenditureByDate(FromDate, ToDate);
                if (TempList != null && TempList.Count() > 0)
                {
                    foreach (var item in TempList)
                    {
                        ExpenditureList.Add(item);
                    }
                    LoadExpenditureChart();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Notify",
                        "Don't have anything Expenditure from" + FromDate.ToString() + " To " + ToDate.ToString(),
                        "OK");
                }
            }
        }

        //Chuyển đến màn hình detail
        private async void SelectedExpenditureToDetail()
        {
            if (SelectedExpenditure != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("ExpenditureDetail", SelectedExpenditure);
                await _navigationService.NavigateAsync("ExpenditureDetail", parameters);
            }
        }
        #endregion

        #region Method

        //Load đồ thị thống kê
        public void LoadExpenditureChart()
        {
            if (ExpenditureList != null)
            {
                IsExist = true;
                double eat_and_drink = 0;
                double gift = 0;
                double party = 0;
                double entertainment = 0;
                double medical = 0;
                double travel = 0;
                double shopping = 0;
                double telephone = 0;
                double transport = 0;
                double household = 0;
                double eletricity_water = 0;
                double school_fee = 0;
                double loan = 0;
                double acquittancce = 0;

                double another = 0;
                foreach (var item in ExpenditureList)
                {
                    switch (item.Category)
                    {
                        case "Ăn uống, nội trợ":
                            eat_and_drink += item.AmountOfMoney;
                            break;
                        case "Quà tặng":
                            gift += item.AmountOfMoney;
                            break;
                        case "Tiệc tùng":
                            party += item.AmountOfMoney;
                            break;
                        case "Giải trí":
                            entertainment += item.AmountOfMoney;
                            break;
                        case "Y tế":
                            medical += item.AmountOfMoney;
                            break;
                        case "Du lịch":
                            travel += item.AmountOfMoney;
                            break;
                        case "Mua sắm":
                            shopping += item.AmountOfMoney;
                            break;
                        case "Điện thoại":
                            telephone += item.AmountOfMoney;
                            break;
                        case "Phương tiện di chuyển":
                            transport += item.AmountOfMoney;
                            break;
                        case "Gia dụng":
                            household += item.AmountOfMoney;
                            break;
                        case "Điện nước, tiền nhà":
                            eletricity_water += item.AmountOfMoney;
                            break;
                        case "Học phí":
                            school_fee += item.AmountOfMoney;
                            break;
                        case "Cho vay":
                            loan += item.AmountOfMoney;
                            break;
                        case "Trả nợ ":
                            acquittancce += item.AmountOfMoney;
                            break;
                        default:
                            another += item.AmountOfMoney;
                            break;
                    }
                    Total += item.AmountOfMoney;
                }

                var entries = new[]
                {
                    new Entry((float)eat_and_drink)
                {
                    Label = "Ăn uống, nội trợ",
                    ValueLabel = DoubleToCurrency(eat_and_drink),
                    Color = SKColor.Parse("#39241e")
                },
                new Entry((float)gift)
                {
                    Label = "Quà tặng",
                    ValueLabel = DoubleToCurrency(gift),
                    Color = SKColor.Parse("#1d8b24")
                },
                new Entry((float)party)
                {
                     Label = "Tiệc tùng",
                     ValueLabel = DoubleToCurrency(party),
                     Color = SKColor.Parse("#809c79")
                },
                new Entry((float)entertainment)
                {
                     Label = "Giải trí",
                     ValueLabel = DoubleToCurrency(entertainment),
                     Color = SKColor.Parse("#3ec797")
                },
                    new Entry((float)medical)
                {
                    Label = "Y tế",
                    ValueLabel = DoubleToCurrency(medical),
                    Color = SKColor.Parse("#86825a")
                },
                     new Entry((float)travel)
                {
                    Label = "Du lịch",
                    ValueLabel = DoubleToCurrency(travel),
                    Color = SKColor.Parse("#947565")
                },
                      new Entry((float)shopping)
                {
                    Label = "Mua sắm",
                    ValueLabel = DoubleToCurrency(shopping),
                    Color = SKColor.Parse("#a0db8e")
                },
                       new Entry((float)telephone)
                {
                    Label = "Điện thoại",
                    ValueLabel = DoubleToCurrency(telephone),
                    Color = SKColor.Parse("#9dd7b4")
                },
                        new Entry((float)transport)
                {
                    Label = "Phương tiện di chuyển",
                    ValueLabel = DoubleToCurrency(transport),
                    Color = SKColor.Parse("#ffa500")
                },
                         new Entry((float)household)
                {
                    Label = "Gia dụng",
                    ValueLabel = DoubleToCurrency(household),
                    Color = SKColor.Parse("#000080")
                },
                          new Entry((float)eletricity_water)
                {
                    Label = "Điện nước, tiền nhà",
                    ValueLabel = DoubleToCurrency(eletricity_water),
                    Color = SKColor.Parse("#b6fcd5")
                },
                           new Entry((float)school_fee)
                {
                    Label = "Học phí",
                    ValueLabel = DoubleToCurrency(school_fee),
                    Color = SKColor.Parse("#eadcce")
                },
                           new Entry((float)loan)
                {
                    Label = "Cho vay",
                    ValueLabel = DoubleToCurrency(loan),
                    Color = SKColor.Parse("#b2bd7e")
                },
                     new Entry((float)acquittancce)
                {
                    Label = "Trả nợ",
                    ValueLabel = DoubleToCurrency(acquittancce),
                    Color = SKColor.Parse("#008282")
                },
                      new Entry((float)another)
                {
                    Label = "Các khoản khác",
                    ValueLabel = DoubleToCurrency(another),
                    Color = SKColor.Parse("#eadcce")
                },
                };
                ExpenditureChart = new DonutChart();
                ExpenditureChart.Entries = entries.Where(x => x.Value > 0);
                ExpenditureChart.LabelTextSize = 26;
                ExpenditureChart.BackgroundColor = SKColor.Parse("#fefefe");

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
