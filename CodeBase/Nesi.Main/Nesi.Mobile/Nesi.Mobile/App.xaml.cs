using Nesi.Mobile.Services;
using Nesi.Mobile.ViewModels;
using Nesi.Mobile.Views;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Nesi.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            bool isLoggedIn = Current.Properties.ContainsKey("IsLoggedIn") ? Convert.ToBoolean(Current.Properties["IsLoggedIn"]) : false;
            bool isSynced = Current.Properties.ContainsKey("isSynced") ? Convert.ToBoolean(Current.Properties["isSynced"]) : false;
            if (!isLoggedIn)
            {
                //Load if Not Logged In
                MainPage = new NavigationPage(new MainPage());
                
            }
            else
            {
                //Load if Logged In
                try
                {
                    var welcomeText = SecureStorage.GetAsync("welcomeText");
                    
                    MainPage = new NavigationPage(new HomePage(welcomeText.Result));
                }
                catch (Exception ex)
                {
                    var e = ex.Message;
                }
                
            }

            if (isSynced)
            {
                HomePageViewModel hvm = new HomePageViewModel
                {
                    SyncContactsSwitch = true,
                    UpdateVisible = true
                };
            }

        }

        protected override void OnStart()
        {
            

        }

        protected override void OnSleep()
        {
           
        }

        protected override void OnResume()
        {
            
        }
    }
}
