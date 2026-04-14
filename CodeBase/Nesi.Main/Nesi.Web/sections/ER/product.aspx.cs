using System;
using System.Collections.Specialized;
using System.Web.UI;
using nesi.core;

public partial class product : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 100; // from Page table in DB
    NeERItem item;
    Toolbox _tools = new Toolbox();
    protected void Page_Init()
    {
        var _q = Request.QueryString;
        if (_q["from"] == "index")
            pnl_select.ClientVisible = false;
        else
            pnl_select.ClientVisible = true;
       
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        var _tools = new Toolbox();
        myMember = Toolbox.do_handle_authentication(_page_id);
        var _q = Request.QueryString;
        var eritem_id = Convert.ToInt32(_q["eritem_id"]);
        
        if (eritem_id != 0)
        {
            item = new NeERItem(Convert.ToInt32(eritem_id),Convert.ToInt32(myMember.business_unit_id));
          
        }
        else
        {
            item = new NeERItem();
            ASPxPageControl1.TabPages[1].ClientEnabled = false;
            ASPxPageControl1.TabPages[2].ClientEnabled = false;
            ASPxPageControl1.TabPages[3].ClientEnabled = false;
            ASPxPageControl1.TabPages[4].ClientEnabled = false;
        }
        

        if (!IsPostBack)  // first time
        {
            if (eritem_id != 0)
            {
                txt_Make.Text = item.eritem_makename;
                txt_Model.Text = item.eritem_modelname;
                txt_newprice.Text = item.branch_newprice.ToString("C2");
                txtrepairprice.Text = item.branch_repairprice.ToString("C2");
                txtQtyinStock.Text = item.branch_qtyinstock.ToString();
                txtDescription.Text = item.eritem_description;
                txteritemid.Text = item.eritem_id.ToString();

            }
            else
            {

                txtQtyinStock.Text = "0";
                
            }
        }
        else
        {

        }
        
        
    }




    
    protected void btnSave_Click(object sender, EventArgs e)
    {
       // see if make model combo already exists.

        var eritemid = _tools.getSQL_int(@"Select if (count(eritem_id) = 0,0,eritem_id) from eritem  where eritem_make like @v0 and eritem_model like @v1", new object[] { txt_Make.Text,txt_Model.Text });
        double repairprice = 0;
        double newprice = 0;

        if (eritemid != 0)  // if this make and model already exist...
        {
            if ((txteritemid.Text != eritemid.ToString()) && (txteritemid.Text != ""))
            {
                lblerror.Text = "A combination of this make and model already exists under ER Item # " + txteritemid.Text;
                return;
            }
        }
        else
        {
            try
            {
                repairprice = Convert.ToDouble(txtrepairprice.Text);
            }
            catch
            {
                lblerror.Text = "Invalid repair price";
                return;
            }

            try
            {
                newprice = Convert.ToDouble(txt_newprice.Text);
            }
            catch
            {
                lblerror.Text = "Invalid new price";
                return;
            }
            try
            {
                item.branch_qtyinstock = Convert.ToDouble(txtQtyinStock.Text);
            }
            catch
            {
                lblerror.Text = "Invalid Qty";
                return;
            }
            try
            {
                item.eritem_addedby = myMember.id;
                item.eritem_dateadded = System.DateTime.Today;
                item.branch_newprice = newprice;
                item.branch_repairprice = repairprice;
                
                item.eritem_description = txtDescription.Text;
                item.eritem_makename = txt_Make.Text;
                item.eritem_modelname = txt_Model.Text;
                item.Save(myMember.business_unit_id);

            }
            catch
            {

            }
            lblerror.Text = "";
            txt_Make.Text = "";
            txt_Model.Text = "";
            txt_newprice.Text = "";
            txtQtyinStock.Text = "0";
            txtrepairprice.Text = "";
            txtDescription.Text = "";
            var javaScript = "parent.opener.location.href = parent.opener.location.href;";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", javaScript, true);

        }

   // validate


    }
    protected void btnCreateQuote_Click(object sender, EventArgs e)
    {

    }
    protected void btnsavenotes_Click(object sender, EventArgs e)
    {

    }
    protected void btnCreatePath_Click(object sender, EventArgs e)
    {

        if (lblFolderPath.Text == "")
        {
            item.CreateFolder((int) item.eritem_id);
         
        }
        lblFolderPath.Text = item.GetProjectFolder((int) item.eritem_id);
        ASPxFileManager1.Visible = true;
        
        ASPxFileManager1.Settings.RootFolder = lblFolderPath.Text;
    }
}
