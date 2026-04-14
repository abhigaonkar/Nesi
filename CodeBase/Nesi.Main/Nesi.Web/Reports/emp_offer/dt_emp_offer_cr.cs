using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.emp_offer;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for dt_emp_offer_cr
	/// </summary>
	public class dt_emp_offer_cr : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private XRLabel xrLabel4;
		private DevExpress.XtraReports.Parameters.Parameter moid;
		private DevExpress.XtraReports.Parameters.Parameter jd;
      
        private DataSet_cr dataSet_cr1;
        private Nesi.Web.Reports.emp_offer.DataSet_crTableAdapters.dt_crTableAdapter dt_crTableAdapter1;
        private XRLabel xrLabel1;
		private XRSubreport xrSubreport1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		public dt_emp_offer_cr()
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
            string resourceFileName = "dt_emp_offer_cr.resx";
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.moid = new DevExpress.XtraReports.Parameters.Parameter();
            this.jd = new DevExpress.XtraReports.Parameters.Parameter();
            this.dataSet_cr1 = new DataSet_cr();
            this.dt_crTableAdapter1 = new Nesi.Web.Reports.emp_offer.DataSet_crTableAdapters.dt_crTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_cr1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSubreport1,
            this.xrLabel1,
            this.xrLabel4});
            this.Detail.HeightF = 16.68759F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(30.48911F, 10.00001F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(599.0942F, 6.687581F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1.645887F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(25.1771F, 15.0417F);
            this.xrLabel1.StylePriority.UseFont = false;
            xrSummary1.FormatString = "{0:#,#}";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel1.Summary = xrSummary1;
            this.xrLabel1.WordWrap = false;
            this.xrLabel1.SummaryCalculated += new DevExpress.XtraReports.UI.TextFormatEventHandler(this.xrLabel1_SummaryCalculated);
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "dt_cr.core_responsibility")});
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.KeepTogether = true;
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(30.48911F, 1.645883F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(599.0942F, 7.374986F);
            this.xrLabel4.StylePriority.UseFont = false;
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
            this.moid.Type = typeof(int);
            this.moid.ValueInfo = "0";
            // 
            // jd
            // 
            this.jd.Name = "jd";
            this.jd.Type = typeof(bool);
            this.jd.ValueInfo = "False";
            // 
            // dataSet_cr1
            // 
            this.dataSet_cr1.DataSetName = "DataSet_cr";
            this.dataSet_cr1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dt_crTableAdapter1
            // 
            this.dt_crTableAdapter1.ClearBeforeFill = true;
            // 
            // dt_emp_offer_cr
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.dataSet_cr1});
            this.DataAdapter = this.dt_crTableAdapter1;
            this.DataMember = "dt_cr";
            this.DataSource = this.dataSet_cr1;
            this.Margins = new System.Drawing.Printing.Margins(100, 104, 0, 0);
            this.PageColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.moid,
            this.jd});
            this.Version = "17.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.dt_emp_offer_cr_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_cr1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
//		print_membertype_dataTableAdapters.core_responsibilitiesTableAdapter cr_tableadapater = new print_membertype_dataTableAdapters.core_responsibilitiesTableAdapter();
//		cr_tableadapater.Fill(this.print_membertype_data1.core_responsibilities, Convert.ToInt32(this.mtid.Value));
//
//		print_qualifications q = new print_qualifications();
//		q.Parameters[0].Value = Convert.ToInt32(this.mtid.Value);
//		this.xrSubreport1.ReportSource = q;

		//	var cr_tableadapater = new dt_emp_offer_cr_dtTableAdapters.emp_offer_crTableAdapter();
		//	cr_tableadapater.Fill(dt_emp_offer_cr_dt1.emp_offer_cr, Convert.ToInt32(moid.Value));


           

        
		
			//	this.GroupHeader1.GroupFields.Add(new GroupField("name"));
		

			}





		private void xrLabel1_SummaryCalculated(object sender, TextFormatEventArgs e)
			{
			e.Text = ((Convert.ToInt16(e.Value) ) + ".");
			}

        private void dt_emp_offer_cr_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var cr_tableadapater1 = new Nesi.Web.Reports.emp_offer.DataSet_crTableAdapters.dt_crTableAdapter();
            cr_tableadapater1.Fill(dataSet_cr1.dt_cr, Convert.ToInt32(moid.Value));



            if (Convert.ToBoolean(jd.Value))
            {
                var cr = new emp_offer_cr_details();
                if (RowCount > 0)
                {
                    cr.Parameters[0].Value = Convert.ToInt32(moid.Value);
                    cr.Parameters[1].Value = Convert.ToInt32(GetCurrentColumnValue("crid").ToString());
                    xrSubreport1.ReportSource = cr;
                }
            }
        }
    }
	}