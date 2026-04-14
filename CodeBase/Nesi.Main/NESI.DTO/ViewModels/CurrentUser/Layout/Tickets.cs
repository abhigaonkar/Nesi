using System;

// ReSharper disable InconsistentNaming

namespace NESI.DTO.ViewModels.CurrentUser.Layout
{
    public class Tickets
    {
		public int Id { get; set; }
		public string Ordered_Id { get; set; }
		public string Issue { get; set; }
		public DateTime Mod_Date { get; set; }
		public int Ticketheader_Id { get; set; }
		public string RaisedIssue { get; set; }
		public DateTime Ticketheader_modified_date { get; set; }
		public bool Ticketheader_private { get; set; }
		public DateTime DateCreated { get; set; }
		public string PageName { get; set; }
		public int? Release_Id { get; set; }
		public string Status { get; set; }
		public string TypeOfTicket { get; set; }
		public string AsignedTo { get; set; }
		public string RaisedBy { get; set; }
		public string Priority { get; set; }
		public int Priority_Id { get; set; }
		public string Groupname { get; set; }
		public DateTime? Exp_fin { get; set; }
		public int G_admin { get; set; }
		public int Status_id { get; set; }
		public int Type { get; set; }
		public int V { get; set; }
	}
}