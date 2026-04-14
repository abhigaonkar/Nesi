using System;

namespace NESI.DTO.ViewModels.CurrentUser.Layout
{
    public class UpcomingVacations
    {
        public string BusinessUnitName { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}