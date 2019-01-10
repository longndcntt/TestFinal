using Prism;
using Prism.Ioc;
using TestFinal.Helpers;
using TestFinal.ViewModels;
using TestFinal.Views;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TestFinal
{
    public partial class App 
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            App.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, LoginViewModel>();
            containerRegistry.RegisterForNavigation<PrismPage, PrismPageViewModel>();
            containerRegistry.RegisterForNavigation<ReceiptPage, ReceiptPageViewModel>();
            containerRegistry.RegisterForNavigation<ExpenditurePage, ExpenditurePageViewModel>();
            containerRegistry.RegisterForNavigation<StatisticPage, StatisticPageViewModel>();
            containerRegistry.RegisterForNavigation<AddReceipt, AddReceiptViewModel>();
            containerRegistry.RegisterForNavigation<EditReceipt, EditReceiptViewModel>();
            containerRegistry.RegisterForNavigation<ReceiptDetail, ReceiptDetailViewModel>();
            containerRegistry.RegisterForNavigation<StatisticReceipt, StatisticReceiptViewModel>();
            containerRegistry.RegisterForNavigation<SignUp, SignUpViewModel>();
            containerRegistry.RegisterForNavigation<Settingpage, SettingpageViewModel>();
            containerRegistry.RegisterForNavigation<AddExpenditure, AddExpenditureViewModel>();
            containerRegistry.RegisterForNavigation<EditExpenditure, EditExpenditureViewModel>();
            containerRegistry.RegisterForNavigation<ExpenditureDetail, ExpenditureDetailViewModel>();
            containerRegistry.RegisterForNavigation<StatisticExpenditure, StatisticExpenditureViewModel>();
        }
    }
}
