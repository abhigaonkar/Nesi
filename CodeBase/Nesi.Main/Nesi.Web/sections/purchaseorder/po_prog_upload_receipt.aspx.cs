using System;
using System.IO;
using System.Web.UI;
using DevExpress.Web;
using System.Net.Mail;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Data;
using nesi.core;
using NESI.Common.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;



public partial class sections_member_po_upload_receipt : Page
{
    Toolbox _tools;
    NeMember current_user;
    NameValueCollection _q;
    protected void Page_Init(object sender, EventArgs e)
    {
        _tools = new Toolbox();
        current_user = Toolbox.do_handle_authentication(1);
        _tools.page_author = new NeMember(4520);
        Session["business_unit_id"] = current_user.business_unit_id.ToString();
        _q = Request.QueryString;
        var is_mobile = !string.IsNullOrEmpty(_q["is_mobile"]) && _q["is_mobile"] == "1";

        if (!IsCallback && !IsPostBack)
        {
            if (cb_user.Items.FindByValue(current_user.id) != null)
            {
                cb_user.Value = current_user.id;
            }
            else
            {
                cb_user.SelectedIndex = 0;
            }

        }
        NeMember _user = cb_user.Value == null ? new NeMember() : new NeMember(Convert.ToInt32(cb_user.Value));

        if (is_mobile)
        {
            _tools.add_css("./mobile.css");
            var height = Unit.Pixel(35);

        }
        else
        {
            _tools.add_css("./desktop.css");
        }


    }
    protected void Page_Load(object sender, EventArgs e)
    {

        fill_cc_bu_user();


        ddlBusinessUnit.DataSource = NeBusinessUnit.units_filtered(new Current_User().visible_business_units);
        ddlBusinessUnit.DataBind();
        ddlBusinessUnit.Items[0].Text = "";
        if (!IsCallback && !IsPostBack)
        {
            ddlBusinessUnit.SelectedIndex = (current_user.business_unit_id.ToString() == null || current_user.business_unit_id == 0)
            ? ddlBusinessUnit.Items.IndexOf(ddlBusinessUnit.Items[0])
            : ddlBusinessUnit.Items.IndexOf(ddlBusinessUnit.Items.FindByValue(current_user.business_unit_id));

        }
    }
    public void fill_cc_bu_user()
    {
        cb_user.DataSource = Toolbox.doSQL_dt(@"SELECT Member_ID, CONCAT(Member_FirstName , ' ' , Member_LastName) AS FullName FROM member mm WHERE mm.Member_ID = @v0 ", new object[] { current_user.id });
        cb_user.DataBind();

    }

    private void do_error(string e, bool show_up_error)
    {
        lb_error.Text = "- " + e;
    }

    protected void b_save_Click(object sender, EventArgs e)
    {
        var businessUnitId = ddlBusinessUnit.SelectedItem != null ? Convert.ToInt32(ddlBusinessUnit.SelectedItem.Value) : 0;
        if (ddlBusinessUnit.SelectedItem == null || !int.TryParse(ddlBusinessUnit.SelectedItem.Value.ToString(), out businessUnitId) || businessUnitId == 0)
        {
            do_error("Please select business unit.", false);
            return;
        }
        if (!po_receipt.UploadedFiles[0].IsValid)
        {
            do_error("Please attach a packing slip containing supported file format.", true);
            return;
        }
        if (po_receipt.UploadedFiles.Length != 0 && po_receipt.UploadedFiles[0].FileBytes.Length > 4194304)
        {
            do_error("File size exceeds the maximum allowed size, which is 4MB.", true);

            return;
        }
        if ((po_receipt.UploadedFiles.Length == 0 || po_receipt.UploadedFiles[0].ContentLength == 0))
        {
            do_error("Please select Packing Slip file to upload.", true);

            return;
        }

        try
        {
            ddlBusinessUnit.Enabled = false;
            var posted_file = po_receipt.UploadedFiles[0];
            if (posted_file.FileName != "")
            {
                string popath = Toolbox.doSQL_string(@"SELECT POPath from business_unit WHERE ID = @v0", new object[] { businessUnitId });
                try
                {
                    var this_data = new byte[posted_file.ContentLength];
                    string fileExtension = Path.GetExtension(posted_file.FileName).ToLower();
                    posted_file.FileContent.Read(this_data, 0, Convert.ToInt32(posted_file.ContentLength));
                    string dateTimeNow = DateTime.Now.ToString("MMddyyyy_HHmmss");
                    string strPath = $@"{popath}BPSU_264_{current_user.id}_{businessUnitId}_{dateTimeNow}{fileExtension}";

                    // Check if the uploaded file is a PDF
                    if (fileExtension == ".pdf")
                    {
                        // Save the PDF file directly (no changes here)
                        using (var this_stream = new FileStream(strPath, FileMode.Create))
                        {
                            this_stream.Write(this_data, 0, this_data.Length);
                        }
                    }
                    else if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                    {
                        // Convert image (JPEG, PNG) to PDF using iTextSharp
                        string pdfPath = Path.ChangeExtension(strPath, ".pdf"); // Change extension to .pdf
                        using (var ms = new MemoryStream(this_data))
                        {
                            var document = new Document(PageSize.A4, 25, 25, 25, 25);

                            using (var fileStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
                            {
                                var writer = PdfWriter.GetInstance(document, fileStream);
                                document.Open();

                                var image = iTextSharp.text.Image.GetInstance(ms);

                                // Scale the image to fit within an A4 page while maintaining the aspect ratio
                                if (image.Width > PageSize.A4.Width || image.Height > PageSize.A4.Height)
                                {
                                    image.ScaleToFit(PageSize.A4.Width - 50, PageSize.A4.Height - 50);
                                }

                                image.Alignment = Element.ALIGN_CENTER;
                                document.Add(image);
                                document.Close();
                                writer.Close();
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Unsupported file format. Only PDF, JPG, and PNG files are allowed.");
                    }

                    // Success message
                    Page.ClientScript.RegisterStartupScript(
                        GetType(),
                        "myScript",
                        "please_wait('stop', 'Reloading'); " +
                        "alert('Packing Slip Uploaded Successfully'); " +
                        "setTimeout(function() { location.href = location.href; }, 500);",
                        true
                    );

                    ddlBusinessUnit.Enabled = true;
                    ddlBusinessUnit.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    do_error(ex.ToString(), true);
                    Toolbox.do_errorLog(ex);
                    Page.ClientScript.RegisterStartupScript(
                        GetType(),
                        "myScript",
                        "please_wait('stop', 'Reloading'); " +
                        "alert('Packing Slip file upload was not successful. Please retry file upload.'); ",
                        true
                    );
                }
            }
        }
        catch (Exception ex)
        {
            do_error(ex.ToString(), true);
            Toolbox.do_errorLog(ex);
            Page.ClientScript.RegisterStartupScript(
                GetType(),
                "myScript",
                "please_wait('stop', 'Reloading'); " +
                "alert('Packing Slip file upload was not successful. Please retry file upload.'); ",
                true
            );
        }
    }

}