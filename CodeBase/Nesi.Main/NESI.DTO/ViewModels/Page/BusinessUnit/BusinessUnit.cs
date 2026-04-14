// ReSharper disable InconsistentNaming
namespace NESI.DTO.ViewModels.Page.BusinessUnit
{
    public class BusinessUnit
    {
        public int Id { get; set; }
        public int Tax_Entity_Id { get; set; }
        public string Tax_Entity { get; set; }
        public string Business_Unit { get; set; }
        public string Tax_No { get; set; }
        public string Manager { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Active { get; set; }
    }
}