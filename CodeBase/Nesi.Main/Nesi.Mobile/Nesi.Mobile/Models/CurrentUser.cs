using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
namespace Nesi.Mobile.Models
{


   public class CurrentUser 
    {
        [JsonProperty(PropertyName = "user")]
        public User User { get; set; }
    }

 
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "member_user")]
        public string Member_User { get; set; }

    }
}
