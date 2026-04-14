using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.emp_offer;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for emp_offer_cr_details
	/// </summary>
	public class emp_offer_cr_details : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private dt_emp_offer_cr_details dt_emp_offer_cr_details1;
		private Nesi.Web.Reports.emp_offer.dt_emp_offer_cr_detailsTableAdapters.dt_emp_offer_cr_details_xsd dt_emp_offer_cr_details_xsd1;
		private DevExpress.XtraReports.Parameters.Parameter moid;
		private DevExpress.XtraReports.Parameters.Parameter crid;
		private XRLabel lbl_asneeded;
		private XRLabel lbl_annually;
		private XRLabel lbl_quarterly;
		private XRLabel lbl_monthly;
		private XRLabel lbl_weekly;
		private XRLabel lbl_daily;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public emp_offer_cr_details()
			{
			InitializeComponent();
			//
			//  
			//
			}
	
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
				}
			base.Dispose(disposing);
			}

		#region Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.dt_emp_offer_cr_details1 = new dt_emp_offer_cr_details();
			this.dt_emp_offer_cr_details_xsd1 = new Nesi.Web.Reports.emp_offer.dt_emp_offer_cr_detailsTableAdapters.dt_emp_offer_cr_details_xsd();
			this.moid = new DevExpress.XtraReports.Parameters.Parameter();
			this.crid = new DevExpress.XtraReports.Parameters.Parameter();
			this.lbl_daily = new DevExpress.XtraReports.UI.XRLabel();
			this.lbl_weekly = new DevExpress.XtraReports.UI.XRLabel();
			this.lbl_monthly = new DevExpress.XtraReports.UI.XRLabel();
			this.lbl_quarterly = new DevExpress.XtraReports.UI.XRLabel();
			this.lbl_annually = new DevExpress.XtraReports.UI.XRLabel();
			this.lbl_asneeded = new DevExpress.XtraReports.UI.XRLabel();
			((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer_cr_details1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.lbl_asneeded,
																						this.lbl_annually,
																						this.lbl_quarterly,
																						this.lbl_monthly,
																						this.lbl_weekly,
																						this.lbl_daily});
			this.Detail.HeightF = 13.01036F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// TopMargin
			// 
			this.TopMargin.HeightF = 0F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// BottomMargin
			// 
			this.BottomMargin.HeightF = 0F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// dt_emp_offer_cr_details1
			// 
			this.dt_emp_offer_cr_details1.DataSetName = "dt_emp_offer_cr_details";
			this.dt_emp_offer_cr_details1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// dt_emp_offer_cr_details_xsd1
			// 
			this.dt_emp_offer_cr_details_xsd1.ClearBeforeFill = true;
			// 
			// moid
			// 
			this.moid.Name = "moid";
			this.moid.Type = typeof(int);
			this.moid.ValueInfo = "0";
			// 
			// crid
			// 
			this.crid.Name = "crid";
			this.crid.Type = typeof(int);
			this.crid.ValueInfo = "0";
			// 
			// lbl_daily
			// 
			this.lbl_daily.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																								new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_emp_offer_cr_details_xsd.daily")});
			this.lbl_daily.Font = new System.Drawing.Font("Arial", 9.75F);
			this.lbl_daily.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 0.4166365F);
			this.lbl_daily.Name = "lbl_daily";
			this.lbl_daily.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.lbl_daily.SizeF = new System.Drawing.SizeF(601.8749F, 2.083333F);
			this.lbl_daily.StylePriority.UseFont = false;
			this.lbl_daily.Text = "lbl_daily";
			this.lbl_daily.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbl_daily_BeforePrint);
			// 
			// lbl_weekly
			// 
			this.lbl_weekly.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																								new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_emp_offer_cr_details_xsd.weekly")});
			this.lbl_weekly.Font = new System.Drawing.Font("Arial", 9.75F);
			this.lbl_weekly.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 2.583289F);
			this.lbl_weekly.Name = "lbl_weekly";
			this.lbl_weekly.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.lbl_weekly.SizeF = new System.Drawing.SizeF(601.875F, 2.083333F);
			this.lbl_weekly.StylePriority.UseFont = false;
			this.lbl_weekly.Text = "lbl_weekly";
			this.lbl_weekly.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbl_weekly_BeforePrint);
			// 
			// lbl_monthly
			// 
			this.lbl_monthly.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																								new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_emp_offer_cr_details_xsd.monthly")});
			this.lbl_monthly.Font = new System.Drawing.Font("Arial", 9.75F);
			this.lbl_monthly.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 4.749942F);
			this.lbl_monthly.Name = "lbl_monthly";
			this.lbl_monthly.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.lbl_monthly.SizeF = new System.Drawing.SizeF(601.875F, 2.083333F);
			this.lbl_monthly.StylePriority.UseFont = false;
			this.lbl_monthly.Text = "lbl_monthly";
			this.lbl_monthly.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbl_monthly_BeforePrint);
			// 
			// lbl_quarterly
			// 
			this.lbl_quarterly.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																									new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_emp_offer_cr_details_xsd.quarterly")});
			this.lbl_quarterly.Font = new System.Drawing.Font("Arial", 9.75F);
			this.lbl_quarterly.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 6.833275F);
			this.lbl_quarterly.Name = "lbl_quarterly";
			this.lbl_quarterly.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.lbl_quarterly.SizeF = new System.Drawing.SizeF(601.875F, 2.083333F);
			this.lbl_quarterly.StylePriority.UseFont = false;
			this.lbl_quarterly.Text = "lbl_quarterly";
			this.lbl_quarterly.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbl_quarterly_BeforePrint);
			// 
			// lbl_annually
			// 
			this.lbl_annually.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																								new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_emp_offer_cr_details_xsd.annually")});
			this.lbl_annually.Font = new System.Drawing.Font("Arial", 9.75F);
			this.lbl_annually.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 8.963486F);
			this.lbl_annually.Name = "lbl_annually";
			this.lbl_annually.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.lbl_annually.SizeF = new System.Drawing.SizeF(601.875F, 2F);
			this.lbl_annually.StylePriority.UseFont = false;
			this.lbl_annually.Text = "lbl_annually";
			this.lbl_annually.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbl_annually_BeforePrint);
			// 
			// lbl_asneeded
			// 
			this.lbl_asneeded.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																								new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_emp_offer_cr_details_xsd.as_required")});
			this.lbl_asneeded.Font = new System.Drawing.Font("Arial", 9.75F);
			this.lbl_asneeded.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 11.01036F);
			this.lbl_asneeded.Name = "lbl_asneeded";
			this.lbl_asneeded.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.lbl_asneeded.SizeF = new System.Drawing.SizeF(601.875F, 2F);
			this.lbl_asneeded.StylePriority.UseFont = false;
			this.lbl_asneeded.Text = "lbl_asneeded";
			this.lbl_asneeded.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbl_asneeded_BeforePrint);
			// 
			// emp_offer_cr_details
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin});
			this.DataAdapter = this.dt_emp_offer_cr_details_xsd1;
			this.DataMember = "dt_emp_offer_cr_details_xsd";
			this.DataSource = this.dt_emp_offer_cr_details1;
			this.Margins = new System.Drawing.Printing.Margins(100, 117, 0, 0);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
																							this.moid,
																							this.crid});
			this.Version = "12.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.emp_offer_cr_details_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer_cr_details1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void lbl_asneeded_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "As Required: " + l.Text;
				}
			}

		private void lbl_annually_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Annually: " + l.Text;
				}
			}

		private void lbl_quarterly_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Quarterly: " + l.Text;
				}
			}

		private void lbl_monthly_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Monthly: " + l.Text;
				}
			}

		private void lbl_weekly_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Weekly: " + l.Text;
				}
			}

		private void lbl_daily_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Daily: " + l.Text;
				}
			}

		private void lbl_wording_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{

			}

		private void emp_offer_cr_details_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
           
			var cr_tableadapater = new Nesi.Web.Reports.emp_offer.dt_emp_offer_cr_detailsTableAdapters.dt_emp_offer_cr_details_xsd();
			cr_tableadapater.Fill(dt_emp_offer_cr_details1.dt_emp_offer_cr_details_xsd, Convert.ToInt32(moid.Value), Convert.ToInt32(crid.Value));
			}
		}
	}