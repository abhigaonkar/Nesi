using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.emp_offer;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for rpt_milestones
	/// </summary>
	public class rpt_milestones : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		
		private DevExpress.XtraReports.Parameters.Parameter moid; 
		private XRLabel xrLabel2;
		private XRLabel xrLabel1;
		private ReportHeaderBand ReportHeader;
        private ds_milestone ds_milestone1;
        private Nesi.Web.Reports.emp_offer.ds_milestoneTableAdapters.memberoffer_milestonesTableAdapter memberoffer_milestonesTableAdapter1;
        private ds_milestone ds_milestone2;
        private XRLabel xrLabel4;
        private XRLabel xrLabel3;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		public rpt_milestones()
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
            string resourceFileName = "rpt_milestones.resx";
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.moid = new DevExpress.XtraReports.Parameters.Parameter();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.ds_milestone1 = new ds_milestone();
            this.memberoffer_milestonesTableAdapter1 = new Nesi.Web.Reports.emp_offer.ds_milestoneTableAdapters.memberoffer_milestonesTableAdapter();
            this.ds_milestone2 = new ds_milestone();
            ((System.ComponentModel.ISupportInitialize)(this.ds_milestone1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds_milestone2)).BeginInit();
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
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "memberoffer_milestones.milestone")});
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(120.8333F, 0F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(498.3333F, 18.83334F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "xrLabel2";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "memberoffer_milestones.due")});
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(89.99999F, 18.83334F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel1_BeforePrint);
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
            // moid
            // 
            this.moid.Name = "moid";
            this.moid.Type = typeof(short);
            this.moid.ValueInfo = "0";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel3});
            this.ReportHeader.HeightF = 23F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(120.8333F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(90.00002F, 16.75F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Milestone";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(90.00002F, 16.75F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Milestone Date";
            // 
            // ds_milestone1
            // 
            this.ds_milestone1.DataSetName = "ds_milestone";
            this.ds_milestone1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // memberoffer_milestonesTableAdapter1
            // 
            this.memberoffer_milestonesTableAdapter1.ClearBeforeFill = true;
            // 
            // ds_milestone2
            // 
            this.ds_milestone2.DataSetName = "ds_milestone";
            this.ds_milestone2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // rpt_milestones
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.ds_milestone1,
            this.ds_milestone2});
            this.DataMember = "memberoffer_milestones";
            this.DataSource = this.ds_milestone1;
            this.Margins = new System.Drawing.Printing.Margins(100, 106, 0, 0);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.moid});
            this.Version = "17.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.rpt_milestones_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.ds_milestone1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds_milestone2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
		
		
		

			}

		private void xrLabel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = Convert.ToDateTime(l.Text).ToString("yyyy-MM-dd");
				}
			}

        private void rpt_milestones_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
	var ms_tableadapter = new Nesi.Web.Reports.emp_offer.ds_milestoneTableAdapters.memberoffer_milestonesTableAdapter();
			ms_tableadapter.Fill(ds_milestone1.memberoffer_milestones, Convert.ToInt32(moid.Value));
        }
    }
	}