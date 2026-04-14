using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.Membertype;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for print_qualifications
	/// </summary>
	public class print_qualifications : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private dt_qualifications dt_qualifications1;
		private Nesi.Web.Reports.Membertype.dt_qualificationsTableAdapters.dt_qualificationsTableAdapter dt_qualificationsTableAdapter1;
		private XRLabel xrLabel1;
		private DevExpress.XtraReports.Parameters.Parameter mtid;
		private DevExpress.XtraReports.Parameters.Parameter prov;
		private XRLabel xrLabel2;
		private XRLabel xrLabel4;
		private XRLabel xrLabel3;
		private ReportHeaderBand ReportHeader;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public print_qualifications()
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
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.dt_qualifications1 = new dt_qualifications();
			this.dt_qualificationsTableAdapter1 = new Nesi.Web.Reports.Membertype.dt_qualificationsTableAdapters.dt_qualificationsTableAdapter();
			this.mtid = new DevExpress.XtraReports.Parameters.Parameter();
			this.prov = new DevExpress.XtraReports.Parameters.Parameter();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			((System.ComponentModel.ISupportInitialize)(this.dt_qualifications1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrLabel2,
																						this.xrLabel1});
			this.Detail.HeightF = 18.83334F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrLabel1
			// 
			this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_qualifications.qualification")});
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(408.3333F, 18.83334F);
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.Text = "xrLabel1";
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
			this.BottomMargin.HeightF = 33.33333F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// dt_qualifications1
			// 
			this.dt_qualifications1.DataSetName = "dt_qualifications";
			this.dt_qualifications1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// dt_qualificationsTableAdapter1
			// 
			this.dt_qualificationsTableAdapter1.ClearBeforeFill = true;
			// 
			// mtid
			// 
			this.mtid.Name = "mtid";
			this.mtid.Type = typeof(short);
			this.mtid.ValueInfo = "0";
			// 
			// prov
			// 
			this.prov.Name = "prov";
			this.prov.ValueInfo = "ON";
			// 
			// xrLabel2
			// 
			this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_qualifications.credential")});
			this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(437.5F, 0F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(212.4999F, 18.83334F);
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.Text = "xrLabel2";
			// 
			// xrLabel3
			// 
			this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 16.75F);
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.Text = "Qualification";
			// 
			// xrLabel4
			// 
			this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(437.5F, 0F);
			this.xrLabel4.Name = "xrLabel4";
			this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel4.SizeF = new System.Drawing.SizeF(100F, 16.75F);
			this.xrLabel4.StylePriority.UseFont = false;
			this.xrLabel4.Text = "Credential";
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																							this.xrLabel3,
																							this.xrLabel4});
			this.ReportHeader.HeightF = 18.75F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// print_qualifications
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin,
																		this.ReportHeader});
			this.DataAdapter = this.dt_qualificationsTableAdapter1;
			this.DataMember = "dt_qualifications";
			this.DataSource = this.dt_qualifications1;
			this.Margins = new System.Drawing.Printing.Margins(100, 100, 0, 33);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
																							this.mtid,
																							this.prov});
			this.Version = "12.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.print_qualifications_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.dt_qualifications1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void print_qualifications_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{

			var ta = new Nesi.Web.Reports.Membertype.dt_qualificationsTableAdapters.dt_qualificationsTableAdapter();
			ta.Fill(dt_qualifications1._dt_qualifications, Convert.ToInt32(mtid.Value),prov.Value.ToString());

//		print_membertype_dataTableAdapters.core_responsibilitiesTableAdapter cr_tableadapater = new print_membertype_dataTableAdapters.core_responsibilitiesTableAdapter();
//		cr_tableadapater.Fill(this.print_membertype_data1.core_responsibilities, Convert.ToInt32(this.mtid.Value));

			}
		}
	}