using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nesi.Mobile.Models
{
    public interface IContacts
    {
        Task AddContacts(string FirstName, string LastName, string PhoneNumber, string Email, string CompanyName);
        Task DeleteContacts(string FirstName, string LastName, string PhoneNumber, string Email, string CompanyName);
    }
}
