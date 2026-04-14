using FreshMvvm;
using Nesi.Mobile.Models;
using Nesi.Mobile.Services;
using Nesi.Mobile.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nesi.Mobile.ViewModels
{
    class LoginPageViewModel: ViewModelBase
    {
        
        //private ApiServices _apiServices = new ApiServices();
       
        public string welcomeText;
        //HomePageViewModel homePageViewModel = new HomePageViewModel();

        //CurrentUser user = new CurrentUser();
            
            

            public string Username {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

            public string Password {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }


        
        public bool IsBusy {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }


        public bool AreCredentialsInvalid {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        } 

        public ICommand LoginCommand
        {

           
            get
            {
                return new Command(async() =>
                {

                    IsBusy = true;
                   

                    await ApiServices.LoginAsync(Username, Password);
                    AreCredentialsInvalid = ApiServices.invalid;
                    IsBusy = false;

                    if (AreCredentialsInvalid == false)
                    {
                        IsBusy = true;
                        await ApiServices.GetUser();
                        welcomeText = ApiServices.name;
                        try
                        {
                            await SecureStorage.SetAsync("welcomeText", welcomeText);
                        }
                        catch (Exception ex)
                        {
                            // Possible that device doesn't support secure storage on device.
                        }
                        IsBusy = false;
                        Application.Current.Properties["IsLoggedIn"] = Boolean.TrueString;
                        ApiServices.disconnected = false;
                        await Application.Current.MainPage.Navigation.PushAsync(new HomePage(welcomeText));


                    }
                    
                    
                });
            }
        }

        public LoginPageViewModel()
        {
            
        }



   



    }
}
