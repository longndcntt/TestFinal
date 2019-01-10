using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Services;
using TestFinal.Enums;
using TestFinal.Helpers;
using TestFinal.Model;

namespace TestFinal.ViewModels
{
    public class ExpenditurePageViewModel : ViewModelBase
    {
        #region Properties
        INavigationService _navigationService;
        Database db;
        private double _total;
        private string _searchContent;
        private ObservableCollection<Expenditure> _ExpenditureList;
        private Expenditure _selectedExpenditure;
        private int _selectedWay;

        public double Total { get => _total; set { SetProperty(ref _total, value); } }
        public string SearchContent { get => _searchContent; set { SetProperty(ref _searchContent, value); } }
        public int SelectedWay { get => _selectedWay; set { SetProperty(ref _selectedWay, value); } }
        public List<String> SortWay { get; set; }
        public ObservableCollection<Expenditure> ExpenditureList { get => _ExpenditureList; set { SetProperty(ref _ExpenditureList, value); } }
        public Expenditure SelectedExpenditure
        {
            get => _selectedExpenditure; set
            {
                SetProperty(ref _selectedExpenditure, value);
                if (SelectedExpenditure != null)
                {

                }
            }
        }
        #endregion

        #region DelegateCommand
        public DelegateCommand StatisticCommand { get; private set; }
        public DelegateCommand TextChangedSearchCommand { get; private set; }
        public DelegateCommand SelectedExpenditureCommand { get; private set; }
        public DelegateCommand AddExpenditureCommand { get; private set; }
        public DelegateCommand SelectedChangedSortWayCommand { get; private set; }
        #endregion

        #region Constructor
        public ExpenditurePageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            db = new Database();

            //Load the way to sort
            SortWay = new List<string>()
            {
                "By amount of money","By Date"
            };

            //Load Expenditure list on listview
            LoadExpenditureList();

            //Get reciept total
            double total = GetExpenditureTotal();
            if (total > 0)
            {
                Total = total;
            }

            //Delegate Command
            StatisticCommand = new DelegateCommand(StatisticExpenditure);
            TextChangedSearchCommand = new DelegateCommand(TextChangedSearch);
            SelectedExpenditureCommand = new DelegateCommand(SelectedExpenditureToDetail);
            AddExpenditureCommand = new DelegateCommand(AddExpenditure);
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
                        SortExpenditureByMoney();
                        break;
                    case 1:
                        SortExpenditureByDate();
                        break;
                }
            }
        }

        //Hàm chuyển đến trang thống kê
        private async void StatisticExpenditure()
        {
            await _navigationService.NavigateAsync("StatisticExpenditure");
        }

        //Hàm chuyển đến trang ExpenditureDetail khi chọn 1 Expenditure
        private async void SelectedExpenditureToDetail()
        {
            if (SelectedExpenditure != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add(ParamKey.ExpenditureDetail.ToString(), SelectedExpenditure);
                await _navigationService.NavigateAsync(ParamKey.ExpenditureDetail.ToString(), parameters);
            }
        }

        //Hàm tìm kiếm theo danh mục và nội dung ghi chú
        private void TextChangedSearch()
        {
            if (ExpenditureList != null || db != null)
            {
                List<Expenditure> TempList = new List<Expenditure>();

                TempList = db.SearchExpenditureByContent(SearchContent);
                if (TempList != null)
                {
                    ExpenditureList.Clear();
                    foreach (var item in TempList)
                    {
                        ExpenditureList.Add(item);
                    }
                }
            }
            //Kiểm tra nội dung search có null -> load lại ExpenditureList
            else if (SearchContent == null)
            {
                LoadExpenditureList();
            }


        }

        //Hàm chuyển đến trang AddExpenditure
        private async void AddExpenditure()
        {
            await _navigationService.NavigateAsync(PageManager.AddExpenditure);
        }

        #endregion

        #region Method

        //Sort by date
        private void SortExpenditureByDate()
        {
            if (ExpenditureList != null && ExpenditureList.Count > 0 && db != null)
            {
                ExpenditureList.Clear();
                List<Expenditure> TempList = db.SortExpenditureByDate();
                foreach (var item in TempList)
                {
                    ExpenditureList.Add(item);
                }
            }
        }

        //Sort by money
        private void SortExpenditureByMoney()
        {
            if (ExpenditureList != null && ExpenditureList.Count > 0 && db != null)
            {
                ExpenditureList.Clear();
                List<Expenditure> TempList = db.SortExpenditureByMoney();
                foreach (var item in TempList)
                {
                    ExpenditureList.Add(item);
                }
            }
        }

        //Load danh sách Expenditure
        private void LoadExpenditureList()
        {
            List<Expenditure> TempList = new List<Expenditure>();
            TempList = db.SelectExpenditure();
            if (TempList != null)
            {
                ExpenditureList = new ObservableCollection<Expenditure>();
                foreach (var item in TempList)
                {
                    ExpenditureList.Add(item);

                }
            }
        }

        //Lấy tổng tiền Expenditure
        private double GetExpenditureTotal()
        {
            if (ExpenditureList != null)
            {
                double total = 0;
                foreach (var item in ExpenditureList)
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
