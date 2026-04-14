using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Nesi.Mobile.Models;
using Nesi.Mobile.ViewModels;
using System.Net;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Nesi.Mobile.Services
{
    public static class ApiServices
    {
        
        
        public static string name;
        public static string username;

        //for testing on different devices
        public static string device = "localhost:4070";
        public static string emulator = "172.17.194.97";
        public static string prod = "api.sparkpowercorp.com";

        public static class GlobalVariables
        {
             public static string AccessToken { get; set; }
             
        }
        public static DateTime accessTokenExpiration;
        public static bool invalid;
        public static List<UserContacts> contacts;
        public static int checkSum;
        public static bool disconnected = false;

        public static async Task LoginAsync(string userName, string password)
        {
            username = userName;
            var keyValues = new List<KeyValuePair<string, string>>
    {
        new KeyValuePair<string, string>("grant_type", "password"),
        new KeyValuePair<string, string>("username", username),
        new KeyValuePair<string, string>("password", password)
    };

            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (sender, cert, chain, sslPolicyErrors) =>
            {
                if (cert != null) System.Diagnostics.Debug.WriteLine(cert);
                return true;
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://" + prod + "/token");
            request.Content = new FormUrlEncodedContent(keyValues);
            var client = new HttpClient();
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                invalid = true;

            }
            else
            {
                invalid = false;

                //add username to secure storage
                try
                {
                    await SecureStorage.SetAsync("username", username);
                }
                catch (Exception ex)
                {
                    var e = ex.Message;
                }
                JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(content);
                GlobalVariables.AccessToken = jwtDynamic.Value<string>("access_token");

                try
                {
                    await SecureStorage.SetAsync("token", GlobalVariables.AccessToken);
                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                }
                
                accessTokenExpiration = jwtDynamic.Value<DateTime>(".expires");
            }

            Debug.WriteLine(content);
            
        }

        public static async Task GetUser()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://"+ prod + "/api/CurrentUser/Active");
            request.Headers.Add("Authorization", "Bearer " + GlobalVariables.AccessToken);
            var client = new HttpClient();
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<CurrentUser>(content);
            name = user.User.Name;
            
            

            Debug.WriteLine(user);
            
         
        }

        public static async Task GetContacts()
        {
            try
            {
                var existingUsername = SecureStorage.GetAsync("username");
                var existingAccessToken = SecureStorage.GetAsync("token");
                HomePageViewModel hvm = new HomePageViewModel();
                

                var request = new HttpRequestMessage(HttpMethod.Get, "https://" + prod + "/api/Contacts?member_user=" + existingUsername.Result);
                request.Headers.Add("Authorization", "Bearer " + existingAccessToken.Result);
                var client = new HttpClient();
                var response = await client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    disconnected = true;
                    hvm.SwitchEnabled = false;
                    await Application.Current.MainPage.DisplayAlert("Session Expired", "Your session has expired, please log back in", "Ok");
                    hvm.ShowCancel = false;
                    hvm.LogoutCommand.Execute(null);
                    return;
                }
                contacts = JsonConvert.DeserializeObject<List<UserContacts>>(content);
                checkSum = contacts.Count;

               
                await SecureStorage.SetAsync("checkSum", checkSum.ToString());
               

                Debug.WriteLine(content);

            }
            catch (Exception ex)
            {
                var e = ex.Message;
            }
        }
}
}
