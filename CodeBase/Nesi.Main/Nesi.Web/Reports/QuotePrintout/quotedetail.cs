using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.QuotePrintout;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for quotedetail
	/// </summary>
	public class quotedetail : XtraReport
		{
		private DetailBand Detail; 
		private TopMarginBand TopMargin;
		private XRLabel xrLabel1;
		private dsQuoteDetails dsQuoteDetails1;
		private Nesi.Web.Reports.QuotePrintout.dsQuoteDetailsTableAdapters.quote_detailsTableAdapter quote_detailsTableAdapter1;
		private DevExpress.XtraReports.Parameters.Parameter quoteid;
		private DevExpress.XtraReports.Parameters.Parameter rev;
		private ReportHeaderBand ReportHeader;
		private XRLabel xrLabel31;
		private XRControlStyle xrControlStyle1;
		private DevExpress.XtraReports.Parameters.Parameter signoff;
		private XRLabel xrLabel2;
		private BottomMarginBand BottomMargin;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public quotedetail()
			{
			InitializeComponent();
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
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.dsQuoteDetails1 = new Nesi.Web.Reports.QuotePrintout.dsQuoteDetails();
			this.quote_detailsTableAdapter1 = new Nesi.Web.Reports.QuotePrintout.dsQuoteDetailsTableAdapters.quote_detailsTableAdapter();
			this.quoteid = new DevExpress.XtraReports.Parameters.Parameter();
			this.rev = new DevExpress.XtraReports.Parameters.Parameter();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
			this.signoff = new DevExpress.XtraReports.Parameters.Parameter();
			((System.ComponentModel.ISupportInitialize)(this.dsQuoteDetails1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel1});
			this.Detail.FillEmptySpace = true;
			this.Detail.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Detail.ForeColor = System.Drawing.Color.MidnightBlue;
			this.Detail.HeightF = 621.6666F;
			this.Detail.Name = "Detail";
			this.Detail.StyleName = "xrControlStyle1";
			this.Detail.StylePriority.UseFont = false;
			this.Detail.StylePriority.UseForeColor = false;
			this.Detail.StylePriority.UsePadding = false;
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
			// 
			// xrLabel2
			// 
			this.xrLabel2.AllowMarkupText = true;
			this.xrLabel2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
			this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrLabel2.CanShrink = true;
			this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[linetext]")});
			this.xrLabel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel2.ForeColor = System.Drawing.Color.Black;
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(96F, 5.000003F);
			this.xrLabel2.Multiline = true;
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(628.5417F, 606.6666F);
			this.xrLabel2.StyleName = "xrControlStyle1";
			this.xrLabel2.StylePriority.UseBorders = false;
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.StylePriority.UseForeColor = false;
			this.xrLabel2.StylePriority.UsePadding = false;
			this.xrLabel2.StylePriority.UseTextAlignment = false;
			this.xrLabel2.Text = "xrLabel2";
			this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			this.xrLabel2.TextTrimming = System.Drawing.StringTrimming.Word;
			// 
			// xrLabel1
			// 
			this.xrLabel1.CanShrink = true;
			this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[line_number]")});
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel1.ForeColor = System.Drawing.Color.Black;
			this.xrLabel1.KeepTogether = true;
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(32F, 6F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.SizeF = new System.Drawing.SizeF(59F, 40F);
			this.xrLabel1.StyleName = "xrControlStyle1";
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.StylePriority.UseForeColor = false;
			this.xrLabel1.StylePriority.UseTextAlignment = false;
			this.xrLabel1.Text = "xrLabel1";
			this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			this.xrLabel1.TextFormatString = "{0}.";
			this.xrLabel1.TextTrimming = System.Drawing.StringTrimming.None;
			// 
			// TopMargin
			// 
			this.TopMargin.HeightF = 5F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 5, 0, 100F);
			this.TopMargin.StylePriority.UsePadding = false;
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// BottomMargin
			// 
			this.BottomMargin.HeightF = 10F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// dsQuoteDetails1
			// 
			this.dsQuoteDetails1.DataSetName = "dsQuoteDetails";
			this.dsQuoteDetails1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// quote_detailsTableAdapter1
			// 
			this.quote_detailsTableAdapter1.ClearBeforeFill = true;
			// 
			// quoteid
			// 
			this.quoteid.Name = "quoteid";
			this.quoteid.Type = typeof(int);
			this.quoteid.ValueInfo = "0";
			// 
			// rev
			// 
			this.rev.Name = "rev";
			this.rev.Type = typeof(int);
			this.rev.ValueInfo = "0";
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel31});
			this.ReportHeader.HeightF = 31.00001F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// xrLabel31
			// 
			this.xrLabel31.BackColor = System.Drawing.Color.Transparent;
			this.xrLabel31.CanShrink = true;
			this.xrLabel31.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(30.25002F, 5.000003F);
			this.xrLabel31.Name = "xrLabel31";
			this.xrLabel31.SizeF = new System.Drawing.SizeF(593.2363F, 23F);
			this.xrLabel31.StylePriority.UseBackColor = false;
			this.xrLabel31.StylePriority.UseFont = false;
			this.xrLabel31.StylePriority.UseTextAlignment = false;
			this.xrLabel31.Text = "Scope of Work:";
			this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrControlStyle1
			// 
			this.xrControlStyle1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrControlStyle1.ForeColor = System.Drawing.Color.MidnightBlue;
			this.xrControlStyle1.Name = "xrControlStyle1";
			// 
			// signoff
			// 
			this.signoff.Name = "signoff";
			this.signoff.Type = typeof(short);
			this.signoff.ValueInfo = "0";
			// 
			// quotedetail
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
			this.DataAdapter = this.quote_detailsTableAdapter1;
			this.DataMember = "quote_details";
			this.DataSource = this.dsQuoteDetails1;
			this.Margins = new System.Drawing.Printing.Margins(0, 0, 5, 10);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.quoteid,
            this.rev,
            this.signoff});
			this.SnapGridSize = 13.02083F;
			this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
			this.Version = "19.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.quotedetail_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.dsQuoteDetails1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void quotedetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var quotedetailadapter = new Nesi.Web.Reports.QuotePrintout.dsQuoteDetailsTableAdapters.quote_detailsTableAdapter();
			quotedetailadapter.Fill(dsQuoteDetails1.quote_details, Convert.ToInt32(quoteid.Value), Convert.ToInt32(rev.Value));
			if (Convert.ToInt16(signoff.Value) == 1)
				{
				//lblsignoff.Text = "Notes:______________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________ Customer Initials: ___________";
            

				}
			else
				{
				//lblsignoff.Text = "";
           
				}


			}

private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
{
    DetailBand band = sender as DetailBand;

    // Ensure that the band is not null
    if (band == null) return;

    XRLabel label = band.FindControl("xrLabel2", true) as XRLabel;

    // Ensure that the label is not null
    if (label != null)
    {
        // Allow the label to grow and wrap text
        label.CanGrow = true;
        label.WordWrap = true;

        // Calculate the required height of the text in the label
        SizeF requiredSize;
        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
        {
            requiredSize = g.MeasureString(label.Text, label.Font, new SizeF(label.WidthF, float.MaxValue), new StringFormat { Trimming = StringTrimming.Word });
        }

        // Check if the calculated height is greater than the label's current height
        if (requiredSize.Height > label.HeightF)
        {
            // Set the label's height to the calculated required height
            label.HeightF = requiredSize.Height;
        }

        // Check if the calculated height is greater than the band's current height
        if (requiredSize.Height > band.HeightF)
        {
            // Set the band's height to accommodate the label's new height
            band.HeightF = requiredSize.Height;
        }
    }
}


		}
	}