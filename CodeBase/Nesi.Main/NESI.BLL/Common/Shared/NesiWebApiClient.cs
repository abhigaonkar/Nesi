using log4net;
using nesi.core;
using NESI.BLL.Core.Member;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;

namespace NESI.BLL.Common.Shared
{
    public class AuthenticationToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("guid")]
        public Guid Id { get; set; }
        [JsonProperty("userId")]
        public int UserId { get; set; }
        [JsonProperty("isContact")]
        public bool IsContact { get; set; }
        [JsonProperty("isDeveloper")]
        public bool IsDeveloper { get; set; }
        [JsonProperty("expireSecond")]
        public long ExpireSecond { get; set; }
        [JsonProperty(".issued")]
        public string DateIssued { get; set; }
        [JsonProperty(".expires")]
        public string ExpiryDate { get; set; }
    }

    public class ActiveUserData
    {
        [JsonProperty("user")]
        public ActiveUser User { get; set; }
    }

    /// <summary>
    /// Class that proxies communication to the Nesi web api
    /// </summary>
    public sealed class NesiWebApiClient
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AuthenticationToken AuthenticateWithPassport(passport passport)
        {
            if(passport == null || !passport.active) throw new InvalidOperationException("Invalid passport");
            var client = new HttpClient {BaseAddress = new Uri(Toolbox.GetRequiredAppSetting("WebApiUrl"))};
            try
            {
                var response = client.PostAsync("/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "passport"),
                    new KeyValuePair<string, string>("passport_hash", passport.reqhash),
                })).Result;
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<AuthenticationToken>().Result;
                return result;
            }
            catch (Exception exception)
            {
                Logger.Error("Failed to get api token from passport", exception);
                return null;
            }
        }

        public ActiveUser GetActiveUser(AuthenticationToken token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            var client = new HttpClient();
            try
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{Toolbox.GetRequiredAppSetting("WebApiUrl")}/api/CurrentUser/Active"),
                    Method = HttpMethod.Get,
                    Headers = {
                            { HttpRequestHeader.Authorization.ToString(), $"Bearer {token.AccessToken}" },
                            { HttpRequestHeader.ContentType.ToString(), "application/json" },
                        }
                };
                var response = client.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<ActiveUserData>().Result;
                return result.User;
            }
            catch (Exception exception)
            {
                Logger.Error("Failed to get user", exception);
                return null;
            }
        }
    }

    public sealed class ResponseUtility
    {
        private readonly HttpResponse _response;

        public ResponseUtility(HttpResponse response)
        {
            _response = response;
        }

        public void ForceApiLoginAndRedirect(AuthenticationToken token, ActiveUser user, string redirectUrl)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (redirectUrl == null) throw new ArgumentNullException(nameof(redirectUrl));
            var loginData = JsonConvert.SerializeObject(token);
            loginData = loginData.Replace(@"""", @"\""");
            var currentUserData = JsonConvert.SerializeObject(user);
            currentUserData = currentUserData.Replace(@"""", @"\""");
            var safeRedirect = HttpContext.Current.Server.UrlEncode(redirectUrl);

            var addTokenToStore = $@"
                                    window.localStorage.setItem('authData_V2', btoa(""{loginData}""));
                                    window.localStorage.setItem('currentUserData_V2', btoa(""{currentUserData}""));
                                    window.localStorage.setItem('signOutCheck_V1', '0');
                                    top.document.location.href = ""/#/signin?origin={safeRedirect}"";
                                ";
            _response.Cookies["nesi2"].Value =token.Id.ToString();
            _response.Write($"<script type='text/javascript'>{addTokenToStore}</script>");
            _response.End();
        }

    }

}
