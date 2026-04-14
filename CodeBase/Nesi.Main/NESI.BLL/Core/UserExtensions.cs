using NESI.BLL.Core.Member;

namespace NESI.BLL.Core
{
    public static class UserExtensions
    {
        public static bool IsContact (this User.User user) => user is Contact;
        
        /// <summary>
        /// If user is an vi
        /// </summary>
        public static bool IsEmployee(this User.User user) => user is Employee.Employee;

        /// <summary>
        /// If user is a customer
        /// </summary>
        public static bool IsCustomer(this User.User user) => user is Customer;

        /// <summary>
        /// If User is a vendor
        /// </summary>
        public static bool IsVendor(this User.User user) => user is Vendor;
    }
}
