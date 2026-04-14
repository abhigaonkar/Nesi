using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.employment_record;

/// <summary>
/// Summary description for employment_record
/// </summary>
public class employment_record : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRLabel lbl_company; 
    private XRLabel lbl_email;
    private XRLabel lbl_title;
    private XRLabel lbl_payroll_admin;
    private XRLabel xrLabel3;
    private XRLabel lbl_conclusion;
    private XRLabel lbl_body;
    private XRLabel xrLabel2;
    private XRLabel xrLabel1;
    private XRLabel lbl_date;
    private XRPictureBox pb_logo;
    private XRPageInfo xrPageInfo1;
    private XRLabel lbl_address;
    private ReportFooterBand ReportFooter;
    private Nesi.Web.Reports.employment_record.ds_emp_historyTableAdapters.ds_emp_history_daTableAdapter dt_emp_history_da1;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRLabel xrLabel6;
    private XRLabel xrLabel7;
    private DevExpress.XtraReports.Parameters.Parameter m_id;
    private XRSubreport xrSubreport1;
    private XRLabel xrLabel8;
    private ds_emp_history ds_emp_history1;
  





    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;


 

    public employment_record(int m_id, bool include_details)
    {
        InitializeComponent();
        if (include_details)
        {
            dt_emp_history_da1.Fill(ds_emp_history1.ds_emp_history_da, m_id);
        }
        else
        {
           
            
            Detail.Visible = false;
        }

    
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            string resourceFileName = "employment_record.resx";
            System.Resources.ResourceManager resources = global::Resources.employment_record.ResourceManager;
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.lbl_company = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_email = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_title = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_payroll_admin = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_conclusion = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_body = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_date = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.pb_logo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.lbl_address = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.dt_emp_history_da1 = new Nesi.Web.Reports.employment_record.ds_emp_historyTableAdapters.ds_emp_history_daTableAdapter();
            this.m_id = new DevExpress.XtraReports.Parameters.Parameter();
            this.ds_emp_history1 = new ds_emp_history();
            ((System.ComponentModel.ISupportInitialize)(this.ds_emp_history1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrSubreport1});
            this.Detail.HeightF = 50.2917F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrLabel8
            // 
            this.xrLabel8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[hours]")});
            this.xrLabel8.Font = new System.Drawing.Font("Calibri", 10F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(177.4999F, 4.166698F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(89.58339F, 18.83332F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "xrLabel5";
            this.xrLabel8.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel8_BeforePrint);
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Calibri", 10F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(81.25F, 4.166698F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(23.3333F, 18.83334F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "To ";
            // 
            // xrLabel6
            // 
            this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[membertype_name]")});
            this.xrLabel6.Font = new System.Drawing.Font("Calibri", 10F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(277.0834F, 4.166698F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(383.75F, 18.83332F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "xrLabel6";
            // 
            // xrLabel5
            // 
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[enddate]")});
            this.xrLabel5.Font = new System.Drawing.Font("Calibri", 10F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(104.5833F, 4.166698F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(72.91665F, 18.83332F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLabel4
            // 
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[startdate]")});
            this.xrLabel4.Font = new System.Drawing.Font("Calibri", 10F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.166698F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(81.25F, 18.83332F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "xrLabel4";
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(7.916673F, 26.74999F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(652.9167F, 17.2917F);
            this.xrSubreport1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrSubreport1_BeforePrint);
            // 
            // lbl_company
            // 
            this.lbl_company.Font = new System.Drawing.Font("Calibri", 10F);
            this.lbl_company.LocationFloat = new DevExpress.Utils.PointFloat(0F, 217.0001F);
            this.lbl_company.Name = "lbl_company";
            this.lbl_company.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_company.SizeF = new System.Drawing.SizeF(650F, 20.91666F);
            this.lbl_company.StylePriority.UseFont = false;
            this.lbl_company.Text = "HR Company";
            // 
            // lbl_email
            // 
            this.lbl_email.Font = new System.Drawing.Font("Calibri", 10F);
            this.lbl_email.LocationFloat = new DevExpress.Utils.PointFloat(0F, 196.0835F);
            this.lbl_email.Name = "lbl_email";
            this.lbl_email.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_email.SizeF = new System.Drawing.SizeF(650F, 20.91666F);
            this.lbl_email.StylePriority.UseFont = false;
            this.lbl_email.Text = "HR Email";
            // 
            // lbl_title
            // 
            this.lbl_title.Font = new System.Drawing.Font("Calibri", 10F);
            this.lbl_title.LocationFloat = new DevExpress.Utils.PointFloat(0F, 175.1667F);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_title.SizeF = new System.Drawing.SizeF(650F, 20.91666F);
            this.lbl_title.StylePriority.UseFont = false;
            this.lbl_title.Text = "HR Title";
            // 
            // lbl_payroll_admin
            // 
            this.lbl_payroll_admin.Font = new System.Drawing.Font("Calibri", 10F);
            this.lbl_payroll_admin.LocationFloat = new DevExpress.Utils.PointFloat(0F, 154.25F);
            this.lbl_payroll_admin.Name = "lbl_payroll_admin";
            this.lbl_payroll_admin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_payroll_admin.SizeF = new System.Drawing.SizeF(650F, 20.91666F);
            this.lbl_payroll_admin.StylePriority.UseFont = false;
            this.lbl_payroll_admin.Text = "HR Name";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Calibri", 10F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 114.6666F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(650F, 20.91666F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Regards,";
            // 
            // lbl_conclusion
            // 
            this.lbl_conclusion.Font = new System.Drawing.Font("Calibri", 10F);
            this.lbl_conclusion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 22.91667F);
            this.lbl_conclusion.Name = "lbl_conclusion";
            this.lbl_conclusion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_conclusion.SizeF = new System.Drawing.SizeF(660.8333F, 20.91666F);
            this.lbl_conclusion.StylePriority.UseFont = false;
            this.lbl_conclusion.Text = "conclusion";
            // 
            // lbl_body
            // 
            this.lbl_body.Font = new System.Drawing.Font("Calibri", 10F);
            this.lbl_body.LocationFloat = new DevExpress.Utils.PointFloat(1.589457E-05F, 298.2507F);
            this.lbl_body.Name = "lbl_body";
            this.lbl_body.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_body.SizeF = new System.Drawing.SizeF(650F, 20.91666F);
            this.lbl_body.StylePriority.UseFont = false;
            this.lbl_body.Text = "Body";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Calibri", 10F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 256.2083F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(500F, 20.91665F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "To Whom It May Concern:";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 224.9584F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(500F, 20.91665F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Re: Confirmation of Employment";
            // 
            // lbl_date
            // 
            this.lbl_date.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Italic);
            this.lbl_date.LocationFloat = new DevExpress.Utils.PointFloat(0F, 182.9167F);
            this.lbl_date.Name = "lbl_date";
            this.lbl_date.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_date.SizeF = new System.Drawing.SizeF(500F, 20.91665F);
            this.lbl_date.StylePriority.UseFont = false;
            this.lbl_date.Text = "lbl_date";
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbl_body,
            this.xrLabel2,
            this.xrLabel1,
            this.lbl_date,
            this.pb_logo});
            this.TopMargin.HeightF = 354F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // pb_logo
            // 
            this.pb_logo.Image = ((System.Drawing.Image)(resources.GetObject("pb_logo.Image")));
            this.pb_logo.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleLeft;
            this.pb_logo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 20.83333F);
            this.pb_logo.Name = "pb_logo";
            this.pb_logo.SizeF = new System.Drawing.SizeF(345.8342F, 106.2506F);
            this.pb_logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbl_address,
            this.xrPageInfo1});
            this.BottomMargin.HeightF = 162.9167F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lbl_address
            // 
            this.lbl_address.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.lbl_address.Font = new System.Drawing.Font("Calibri", 9F);
            this.lbl_address.LocationFloat = new DevExpress.Utils.PointFloat(0.0001589457F, 26.6667F);
            this.lbl_address.Multiline = true;
            this.lbl_address.Name = "lbl_address";
            this.lbl_address.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_address.SizeF = new System.Drawing.SizeF(660.8332F, 33.41662F);
            this.lbl_address.StylePriority.UseBorders = false;
            this.lbl_address.StylePriority.UseFont = false;
            this.lbl_address.StylePriority.UseTextAlignment = false;
            this.lbl_address.Text = "To Whom It May Concern:";
            this.lbl_address.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(277.0834F, 101.6666F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrPageInfo1.TextFormatString = "{0} of {1}";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbl_conclusion,
            this.lbl_email,
            this.lbl_title,
            this.lbl_payroll_admin,
            this.xrLabel3,
            this.lbl_company});
            this.ReportFooter.HeightF = 256.6667F;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.PrintAtBottom = true;
            this.ReportFooter.StylePriority.UseTextAlignment = false;
            this.ReportFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // dt_emp_history_da1
            // 
            this.dt_emp_history_da1.ClearBeforeFill = true;
            // 
            // m_id
            // 
            this.m_id.Name = "m_id";
            this.m_id.Type = typeof(int);
            this.m_id.ValueInfo = "0";
            // 
            // ds_emp_history1
            // 
            this.ds_emp_history1.DataSetName = "ds_emp_history";
            this.ds_emp_history1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // employment_record
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportFooter});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.ds_emp_history1});
            this.DataAdapter = this.dt_emp_history_da1;
            this.DataMember = "dt_emp_history";
            this.DataSource = this.ds_emp_history1;
            this.Margins = new System.Drawing.Printing.Margins(83, 83, 354, 163);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.m_id});
            this.Version = "17.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.employment_record_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.ds_emp_history1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    

    private void employment_record_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        

    //    var q = new ds_emp_history();
        
  //      q.Parameters[0].Value = Convert.ToInt32(mid.Value);
       
    //    xrSubreport1.ReportSource = q;
    }

    private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        
    }

    private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (GetCurrentColumnValue("mt_id")!= null && (int)GetCurrentColumnValue("mt_id") != 0)
        {
            var q = new rpt_emp_record_mt_details();
            q.Parameters[0].Value = (int)GetCurrentColumnValue("mt_id");
            xrSubreport1.ReportSource = q;
        }

     
    }

    private void xrLabel8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRLabel x = (XRLabel)sender;
      if (x.Text != null && x.Text!="" && x.Text!="0")
        {
            x.Text = x.Text + " Hours";
        }
      else
        {
            x.Text = "";
        }
    }

    private void xrLabel8_HtmlItemCreated(object sender, HtmlEventArgs e)
    {
  //     if (e.ContentCell.InnerText != null || e.ContentCell.InnerText=="0" || e.ContentCell.InnerText=="")
   //     {
    //        e.ContentCell.InnerText = "";
     //   }
     //   else
      //  {
       //     e.ContentCell.InnerText = e.ContentCell.InnerText + " hours";
      // }
    }
}
