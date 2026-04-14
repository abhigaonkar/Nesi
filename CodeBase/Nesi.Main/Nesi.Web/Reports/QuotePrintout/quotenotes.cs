using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.QuotePrintout;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for quotenotes
	/// </summary>
	public class quotenotes : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin; 
		private BottomMarginBand BottomMargin;
		private XRLabel linetext;
		private DevExpress.XtraReports.Parameters.Parameter quoteid;
		private DevExpress.XtraReports.Parameters.Parameter rev;
		private XRLabel line_number;
		private dsQuotenotes dsQuotenotes1;
		private Nesi.Web.Reports.QuotePrintout.dsQuotenotesTableAdapters.quote_notesTableAdapter quote_notesTableAdapter1;
		private ReportHeaderBand ReportHeader;
		private XRLabel xrLabel31;
		private XRControlStyle xrControlStyle1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public quotenotes()
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
			this.line_number = new DevExpress.XtraReports.UI.XRLabel();
			this.linetext = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.quoteid = new DevExpress.XtraReports.Parameters.Parameter();
			this.rev = new DevExpress.XtraReports.Parameters.Parameter();
			this.dsQuotenotes1 = new Nesi.Web.Reports.QuotePrintout.dsQuotenotes();
			this.quote_notesTableAdapter1 = new Nesi.Web.Reports.QuotePrintout.dsQuotenotesTableAdapters.quote_notesTableAdapter();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
			((System.ComponentModel.ISupportInitialize)(this.dsQuotenotes1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.line_number,
            this.linetext});
			this.Detail.FillEmptySpace = true;
			this.Detail.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Detail.ForeColor = System.Drawing.Color.MidnightBlue;
			this.Detail.HeightF = 608.7916F;
			this.Detail.Name = "Detail";
			this.Detail.StyleName = "xrControlStyle1";
			this.Detail.StylePriority.UseFont = false;
			this.Detail.StylePriority.UseForeColor = false;
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// line_number
			// 
			this.line_number.CanShrink = true;
			this.line_number.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[line_number]")});
			this.line_number.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.line_number.ForeColor = System.Drawing.Color.Black;
			this.line_number.KeepTogether = true;
			this.line_number.LocationFloat = new DevExpress.Utils.PointFloat(32F, 6F);
			this.line_number.Name = "line_number";
			this.line_number.SizeF = new System.Drawing.SizeF(59F, 40F);
			this.line_number.StylePriority.UseFont = false;
			this.line_number.StylePriority.UseForeColor = false;
			this.line_number.StylePriority.UseTextAlignment = false;
			this.line_number.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// linetext
			// 
			this.linetext.AllowMarkupText = true;
			this.linetext.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.linetext.CanShrink = true;
			this.linetext.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[linetext]")});
			this.linetext.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.linetext.ForeColor = System.Drawing.Color.Black;
			this.linetext.LocationFloat = new DevExpress.Utils.PointFloat(96F, 5.000019F);
			this.linetext.Multiline = true;
			this.linetext.Name = "linetext";
			this.linetext.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
			this.linetext.SizeF = new System.Drawing.SizeF(590.0001F, 593.7916F);
			this.linetext.StylePriority.UseBorders = false;
			this.linetext.StylePriority.UseFont = false;
			this.linetext.StylePriority.UseForeColor = false;
			this.linetext.StylePriority.UsePadding = false;
			this.linetext.StylePriority.UseTextAlignment = false;
			this.linetext.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			this.linetext.TextTrimming = System.Drawing.StringTrimming.None;
			// 
			// TopMargin
			// 
			this.TopMargin.HeightF = 5F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// BottomMargin
			// 
			this.BottomMargin.HeightF = 10F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
			// dsQuotenotes1
			// 
			this.dsQuotenotes1.DataSetName = "dsQuotenotes";
			this.dsQuotenotes1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// quote_notesTableAdapter1
			// 
			this.quote_notesTableAdapter1.ClearBeforeFill = true;
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel31});
			this.ReportHeader.HeightF = 32F;
			this.ReportHeader.KeepTogether = true;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// xrLabel31
			// 
			this.xrLabel31.BackColor = System.Drawing.Color.Transparent;
			this.xrLabel31.CanShrink = true;
			this.xrLabel31.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(30.20833F, 2.70834F);
			this.xrLabel31.Name = "xrLabel31";
			this.xrLabel31.SizeF = new System.Drawing.SizeF(590.625F, 25F);
			this.xrLabel31.StylePriority.UseBackColor = false;
			this.xrLabel31.StylePriority.UseFont = false;
			this.xrLabel31.StylePriority.UseTextAlignment = false;
			this.xrLabel31.Text = "Notes and Adders:";
			this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrControlStyle1
			// 
			this.xrControlStyle1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrControlStyle1.ForeColor = System.Drawing.Color.MidnightBlue;
			this.xrControlStyle1.Name = "xrControlStyle1";
			// 
			// quotenotes
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
			this.DataAdapter = this.quote_notesTableAdapter1;
			this.DataMember = "quote_notes";
			this.DataSource = this.dsQuotenotes1;
			this.Margins = new System.Drawing.Printing.Margins(0, 0, 5, 10);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.quoteid,
            this.rev});
			this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1});
			this.Version = "19.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.quotenotes_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.dsQuotenotes1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		protected void quotenotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var quotenoteadapter = new Nesi.Web.Reports.QuotePrintout.dsQuotenotesTableAdapters.quote_notesTableAdapter();
			quotenoteadapter.Fill(dsQuotenotes1.quote_notes, Convert.ToInt32(quoteid.Value), Convert.ToInt32(rev.Value));
    // Cast the sender to the DetailBand type.
    DetailBand band = sender as DetailBand;
if(band == null) return;
    // Find the XRLabel control named 'linetext' in the DetailBand.
    XRLabel label = band.FindControl("linetext", true) as XRLabel;

    if(label != null)
    {
        // Set the label's CanGrow property to true to allow for dynamic height adjustment.
        label.CanGrow = true;

        // Optionally, you can also set the label's WordWrap property to true to ensure
        // that the text is wrapped correctly within the control.
        label.WordWrap = true;

        // Measure the height required for the label text.
        // Note: The Graphics.MeasureString method could be used for more precise measurement,
        // but it requires a graphics context from the printing page, which is not available here.
        // This simplistic approach uses the TextRenderer.MeasureText method as a workaround.
        SizeF textSize = TextRenderer.MeasureText(label.Text, label.Font, 
                                  new Size((int)label.WidthF, int.MaxValue), 
                                  TextFormatFlags.WordBreak);

        // Set the height of the label to the calculated text height, if needed.
        if (textSize.Height > label.HeightF)
        {
            label.HeightF = textSize.Height;
        }

        // If the label height is more than the band's height, adjust the band's height.
        if (textSize.Height > band.HeightF)
        {
            band.HeightF = textSize.Height;
        }
    }


			}
		}
	}