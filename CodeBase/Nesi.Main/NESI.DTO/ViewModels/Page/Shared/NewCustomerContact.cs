using System.ComponentModel.DataAnnotations;

namespace NESI.DTO.ViewModels.Page.Shared
{
	public class NewCustomerContact
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Title { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		[Required]
		public int Customer_id { get; set; }
	}

    public class CustomerContact
    {
        [Required]
        public int Contact_ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public int Customer_id { get; set; }
    }
}