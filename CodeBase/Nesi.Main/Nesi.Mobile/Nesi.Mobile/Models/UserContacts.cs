using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nesi.Mobile.Models
{
    public class UserContacts
    {
        
        [JsonProperty(PropertyName = "firstName")]
        public string _FirstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string _LastName { get; set; }
        [JsonProperty(PropertyName = "neEmail")]
        public string _Email { get; set; }
        [JsonProperty(PropertyName = "companyName")]
        public string _CompanyName { get; set; }
        [JsonProperty(PropertyName = "phoneNumber")]
        public string _PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "phoneExtension")]
        public string _PhoneExtension { get; set; }

    }
}
