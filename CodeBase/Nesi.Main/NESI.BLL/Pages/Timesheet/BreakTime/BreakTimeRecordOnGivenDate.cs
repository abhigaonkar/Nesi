namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public class BreakTimeRecordOnGivenDate
    {
        public bool showAddingOverlay { get; set; }
        public bool showEditingButton { get; set; }

        public int memberId { get; set; }
        public string date { get; set; }
        public int recordId { get; set; }

        public BreakTimeRecord breaktimeRecord { get; set; }
    }
}