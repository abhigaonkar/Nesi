using DevExpress.XtraReports.UI;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for SafetyForm
	/// </summary>
	public class SafetyForm : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private XRPictureBox xrPictureBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null; 

		public SafetyForm()
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
			var resources = global::Resources.SafetyForm.ResourceManager;
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrPictureBox1});
			this.Detail.HeightF = 902.0833F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrPictureBox1
			// 
			this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
			this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(4.583311F, 0F);
			this.xrPictureBox1.Name = "xrPictureBox1";
			this.xrPictureBox1.SizeF = new System.Drawing.SizeF(683.4167F, 899.9999F);
			this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.CenterImage;
			// 
			// TopMargin
			// 
			this.TopMargin.HeightF = 75F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// BottomMargin
			// 
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// SafetyForm
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin});
			this.Margins = new System.Drawing.Printing.Margins(50, 50, 75, 100);
			this.Version = "12.2";
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion
		}
	}