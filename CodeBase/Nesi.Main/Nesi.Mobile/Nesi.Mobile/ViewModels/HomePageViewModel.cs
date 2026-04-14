using Nesi.Mobile.Models;
using Nesi.Mobile.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Nesi.Mobile.Services.ApiServices;



namespace Nesi.Mobile.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        // private ApiServices _apiServices = new ApiServices();
        private LoginPageViewModel lvm = new LoginPageViewModel();
        CancellationTokenSource cts;
        bool isSynced = App.Current.Properties.ContainsKey("isSynced") ? Convert.ToBoolean(App.Current.Properties["isSynced"]) : false;

        public bool Syncing
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool SwitchEnabled
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool UpdateVisible
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public bool ShowCancel
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string SyncText
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

       // public bool pressedCancelButton = false;
   
        


        public HomePageViewModel()
        {
            SwitchEnabled = true;

            if (isSynced)
            {
                SyncContactsSwitch = true;
                UpdateVisible = true;
            }
        }

        public async Task CheckSum()
        {
            var checkSum = await SecureStorage.GetAsync("checkSum");
            
            await ApiServices.GetContacts();

            if (ApiServices.disconnected == false)
            {

                if (checkSum != ApiServices.contacts.Count.ToString())
                {
                    await Application.Current.MainPage.DisplayAlert("Update Contacts", "You have new business contacts to add", "Update");
                    isSynced = false;
                    Application.Current.Properties["isSynced"] = Boolean.FalseString;
                    SyncContactsSwitch = true;
                    UpdateVisible = false;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Up to date", "Your business contacts are up to date", "Ok");
                }
            }
        }

        public bool SyncContactsSwitch
        {
            get { return GetValue<bool>(); }
            set
            {
                SwitchEnabled = false;
                SetValue(value);
                
                MainSync();


            }

        }

        //TODO: WORK WITH PRESSEDCANCELBUTTON

        public void MainSync()
        {

            cts = new CancellationTokenSource();

            if (SyncContactsSwitch == false)
            {
                isSynced = false;
                
            }
            if (ShowCancel == true || isSynced == true)
            {
                SwitchEnabled = true;
                return;
            }
            else
            {
                SyncContactsCommand(cts.Token);
            }
        }

        public async Task SyncContactsCommand(CancellationToken ct)
        {
            

                await ApiServices.GetContacts();

                try
                {


                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
                    if (status != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Contacts))
                        {
                        
                            Syncing = false;
                            await Application.Current.MainPage.DisplayAlert("Need Contacts", "Going to need access to your contacts", "OK");
                        }

                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);
                        //Best practice to always check that the key exists
                        if (results.ContainsKey(Permission.Contacts))
                            status = results[Permission.Contacts];
                    }

                    if (status == PermissionStatus.Granted)
                    {
                        //SYNC SWITCH ON...ADD CONTACTS

                        if (SyncContactsSwitch == true && ApiServices.disconnected == false)
                        {
                            

                            await Task.Run(async () =>
                             {
                                 
                                 SwitchEnabled = false;
                                 Syncing = true;
                                 ShowCancel = true;
                                 SyncText = "Syncing Business Contacts...";




                                 for (int i = 0; i < ApiServices.contacts.Count; i++)
                                 {
                                     ct.ThrowIfCancellationRequested();
                                     DependencyService.Get<INotification>().ShowNotification("Your business contacts are syncing...", "Syncing " + i + " of " + ApiServices.contacts.Count + " Contacts");
                                     await DependencyService.Get<IContacts>().AddContacts(ApiServices.contacts[i]._FirstName, ApiServices.contacts[i]._LastName, ApiServices.contacts[i]._PhoneNumber, ApiServices.contacts[i]._Email, ApiServices.contacts[i]._CompanyName);
                                 }
                                 Syncing = false;
                                 ShowCancel = false;
                                 SyncText = "";
                                 SwitchEnabled = true;
                                 UpdateVisible = true;

                                 DependencyService.Get<INotification>().CancelNotification();

                                 Application.Current.Properties["isSynced"] = Boolean.TrueString;

                                 Device.BeginInvokeOnMainThread(() =>
                                 {
                                     DependencyService.Get<IMessage>().ShortAlert("Business Contacts Saved");
                                 });
                                 

                             }, cts.Token);
                            
                        }
                        else if(ApiServices.disconnected == false)
                        {
                        //SYNC SWITCH OFF...REMOVE CONTACTS

                       
                            await Task.Run(async () =>
                                                 {
                                                     Application.Current.Properties["isSynced"] = Boolean.FalseString;
                                                     UpdateVisible = false;
                                                     SwitchEnabled = false;
                                                     Syncing = true;
                                                     ShowCancel = true;
                                                     SyncText = "Removing Business Contacts...";


                                                     for (int i = 0; i < ApiServices.contacts.Count; i++)
                                                     {
                                                         ct.ThrowIfCancellationRequested();
                                                         DependencyService.Get<INotification>().ShowNotification("Your business contacts are being removed ", "Removing " + i + " of " + ApiServices.contacts.Count + " Contacts");
                                                         await DependencyService.Get<IContacts>().DeleteContacts(ApiServices.contacts[i]._FirstName, ApiServices.contacts[i]._LastName, ApiServices.contacts[i]._PhoneNumber, ApiServices.contacts[i]._Email, ApiServices.contacts[i]._CompanyName);
                                                     }

                                                     SwitchEnabled = true;
                                                     Syncing = false;
                                                     ShowCancel = false;
                                                     SyncText = "";


                                                     DependencyService.Get<INotification>().CancelNotification();

                                                     

                                                     Device.BeginInvokeOnMainThread(() =>
                                                     {
                                                         DependencyService.Get<IMessage>().ShortAlert("Business Contacts Removed");
                                                     });

                                                 }, cts.Token);
                        
                       
                       
                        }
                       
                    }

                    else if (status != PermissionStatus.Unknown)
                    {
                        Syncing = false;
                        await Application.Current.MainPage.DisplayAlert("Contacts Denied", "Can not continue, try again.", "OK");
                    }
                    
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception ex)
                {
                    Syncing = false;
                    var e = ex.Message;
                }
            }


        public ICommand CancelSync
        {
            get
            {
                return new Command(() =>
                {

                    if (cts != null)
                    {
                        cts.Cancel();
                        DependencyService.Get<INotification>().CancelNotification();
                        Syncing = false;
                       
                        if(SyncContactsSwitch == true)
                        {
                            SyncText = "You have not added all of your business contacts";

                        }
                        else
                        {
                            SyncText = "You have not removed all of your business contacts";
                        }
                        ShowCancel = false;
                        SwitchEnabled = true;
                        DependencyService.Get<IMessage>().ShortAlert("Contact Sync Cancelled");
                    }

                });
               
            }
            
        }


        public ICommand LogoutCommand
        {
            get
            {
                return new Command(() =>
                {

                    if (cts != null)
                    {
                        cts.Cancel();
                        DependencyService.Get<INotification>().CancelNotification();
                    }
                    
                    Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                    GlobalVariables.AccessToken = "";
                    Application.Current.Properties["IsLoggedIn"] = Boolean.FalseString;
                    SecureStorage.Remove("token");
                    SecureStorage.Remove("username");

                });
            }
        }

        public ICommand UpdateContacts
        {
            get
            {
                return new Command(async() =>
                {

                    await CheckSum();    

                });
            }
        }



    }
}
