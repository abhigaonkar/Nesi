using System;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web;
using nesi.core;

public partial class sections_er_index : System.Web.UI.Page
{
    NeMember myMember;
    private const int _page_id = 100; // from Page table in DB
    Toolbox _tools = new Toolbox();

  

    protected void Page_Init()
    {
        myMember = Toolbox.do_handle_authentication(_page_id);
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
        divMenu.InnerHtml = menu.MenuHTML;
        divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
 
        if (!IsPostBack)  
        {

            fill_jobs_grid();
            fill_product_grid();
       
        }
        else
        {
            
            
        }
        
        
    }


    protected void fill_product_grid()
    {
        grid_products.DataSource = _tools.getSQL_datatable(@"SELECT urldecode(eritem.eritem_model) as eritem_model, urldecode(eritem.eritem_make)as eritem_make, urldecode(eritem.eritem_description) as eritem_decription, er_branch_data.er_branch_data_repairprice as repairprice, er_branch_data.er_branch_data_newprice as newprice, er_branch_data.er_branch_data_qtyinstock as qtyinstock, erjob.erjob_DateEntered as datelastrepaired, eritem.eritem_id, er_branch_data.er_branch_data_id FROM eritem Left Join er_branch_data ON er_branch_data.er_branch_data_itemid = eritem.eritem_id AND er_branch_data.business_unit_id =@v0 Left Join erjob ON erjob.erjob_item_id = eritem.eritem_id",new object[] { myMember.business_unit_id } );
        grid_products.DataBind();

    }


    
   



    protected void ef_btnSave_Click(object sender, EventArgs e)
    {
        double newprice=0;
        double repairprice=0;
        double qtyinstock=0;

        var item = new NeERItem();

        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var lbl_error = (Label)grid_products.FindEditFormTemplateControl("ef_lblerror");
        var txteritem_id = (ASPxTextBox)pc.FindControl("ef_txteritem_id");
        var txtmake = (ASPxTextBox)pc.FindControl("ef_txtmake");
        var txtmodel = (ASPxTextBox)pc.FindControl("ef_txtmodel");
        var txtqtyinstock = (ASPxTextBox)pc.FindControl("ef_txtqtyinstock");
        var txtrepairprice = (ASPxTextBox)pc.FindControl("ef_txtrepairprice");
        var txtnewprice = (ASPxTextBox)pc.FindControl("ef_txtnewprice");
        if (txteritem_id.Text != "")
        {
            try { item = new NeERItem(Convert.ToInt32(txteritem_id.Text),Convert.ToInt32(myMember.business_unit_id)); }
            catch
            {
                lbl_error.Text = "Problem loading er item";
                return;
            }
        }
        lbl_error.Text = "";
        if (txtmake.Text == "")
        {
            lbl_error.Text = "Invalid Make Name";
            return;
        }
        if (txtmodel.Text == "")
        {
            lbl_error.Text = "Invalid Model name";
            return;
        }
        try {newprice = Convert.ToDouble(txtnewprice.Text);}
        catch 
        {
            lbl_error.Text = "Invalid New Price";
            return;
        }
        try {repairprice = Convert.ToDouble(txtrepairprice.Text);}
        catch 
        {
            lbl_error.Text = "Invalid Repair Price";
            return;
        }
        if (repairprice==0 && newprice == 0)
        {
            lbl_error.Text = "Either new price or repair price must be greater than 0";
            return;
        }
        try { qtyinstock = Convert.ToDouble(txtqtyinstock.Text); }
        catch
        {
            lbl_error.Text = "Invalid Stock Qty";
            return;
        }

        item.branch_newprice = newprice;
        item.branch_qtyinstock = qtyinstock;
        item.branch_repairprice = repairprice;

        item.eritem_makename = _tools.value_to(txtmake.Text);
        item.eritem_modelname = _tools.value_to(txtmodel.Text);
        item.eritem_addedby = myMember.id;
        item.Save(myMember.business_unit_id);
        txteritem_id.Text = item.eritem_id.ToString();

        pc.TabPages[1].ClientEnabled = true;
        pc.TabPages[2].ClientEnabled = true;
        pc.TabPages[3].ClientEnabled = true;
        pc.TabPages[4].ClientEnabled = true;
        
        lbl_error.Text = "Item saved";


    }

    protected void clear_product_page()
    {
        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var lbl_error = (Label)grid_products.FindEditFormTemplateControl("ef_lblerror");
        var txteritem_id = (ASPxTextBox)pc.FindControl("ef_txteritem_id");
        var txtmake = (ASPxTextBox)pc.FindControl("ef_txtmake");
        var txtmodel = (ASPxTextBox)pc.FindControl("ef_txtmodel");
        var txtqtyinstock = (ASPxTextBox)pc.FindControl("ef_txtqtyinstock");
        var txtrepairprice = (ASPxTextBox)pc.FindControl("ef_txtrepairprice");
        var txtnewprice = (ASPxTextBox)pc.FindControl("ef_txtnewprice");
        txteritem_id.Text = "";
        txtmake.Text = "";
        txtmodel.Text = "";
        txtqtyinstock.Text = "0";
        txtrepairprice.Text = "0";
        txtnewprice.Text = "0";

    }

    protected void ef_btnCancel_Click(object sender, EventArgs e)
    {
        grid_products.CancelEdit();
        fill_product_grid();
    }
    protected void ef_btnPrintQuote_Click(object sender, EventArgs e)
    {

    }

    protected void grid_products_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        fill_product_grid();
    }
    protected void grid_products_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
    {
        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var lbl_error = (Label)grid_products.FindEditFormTemplateControl("ef_lblerror");
        var txteritem_id = (ASPxTextBox)pc.FindControl("ef_txteritem_id");
        var txtmake = (ASPxTextBox)pc.FindControl("ef_txtmake");
        var txtmodel = (ASPxTextBox)pc.FindControl("ef_txtmodel");
        var txtqtyinstock = (ASPxTextBox)pc.FindControl("ef_txtqtyinstock");
        var txtrepairprice = (ASPxTextBox)pc.FindControl("ef_txtrepairprice");
        var txtnewprice = (ASPxTextBox)pc.FindControl("ef_txtnewprice");
        txtnewprice.Text = "0";
        txtrepairprice.Text = "0";
        txtqtyinstock.Text = "0";
        pc.TabPages[1].ClientEnabled = false;
        pc.TabPages[2].ClientEnabled = false;
        pc.TabPages[3].ClientEnabled = false;
        pc.TabPages[4].ClientEnabled = false;
    }


    protected void grid_products_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        toplblerror.Text = "";
        if (_tools.getSQL_int(@"Select ifnull(count(erjob_id),0) from erjob  where erjob_item_id =@v0", new object[] { e.Keys[0] }) > 0)
        {
            toplblerror.Text = @"This product is / has been used on job, therefore it can't be deleted";
            return;
        }


        _tools.getSQL_void("Delete from eritem where eritem_id =@v0", new object[] { e.Keys[0]});
        _tools.getSQL_void("Delete from er_branch_data where er_branch_data_itemid =@v0", new object[] { e.Keys[0] });
		_tools.getSQL_void("Delete from er_vendor_data where er_vendor_data_itemid =@v0", new object[] { e.Keys[0] });
		e.Cancel = true;
        grid_products.CancelEdit();
        fill_product_grid();
    }
   
    
   
   
    
   
    protected void grid_products_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
    {

        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var gv = (ASPxGridView)pc.FindControl("grid_vendor_data");
        var lbl_error = (Label)grid_products.FindEditFormTemplateControl("ef_lblerror");
        var txteritem_id = (ASPxTextBox)pc.FindControl("ef_txteritem_id");
        var txtmake = (ASPxTextBox)pc.FindControl("ef_txtmake");
        var txtmodel = (ASPxTextBox)pc.FindControl("ef_txtmodel");
        var txtqtyinstock = (ASPxTextBox)pc.FindControl("ef_txtqtyinstock");
        var txtrepairprice = (ASPxTextBox)pc.FindControl("ef_txtrepairprice");
        var txtnewprice = (ASPxTextBox)pc.FindControl("ef_txtnewprice");
       

        var rowIndex = grid_products.EditingRowVisibleIndex;
        if (rowIndex >= 0)
        {
            var value1 = grid_products.GetRowValues(rowIndex, new string[] { "eritem_id" });
            if (value1 != null)
            {
               
                Session["stuff"] = value1.ToString();
                var item = new NeERItem(Convert.ToInt32(Session["stuff"]),Convert.ToInt32(myMember.business_unit_id));
                lbl_error.Text = "";
                txteritem_id.Text = item.eritem_id.ToString();
                txtmake.Text = item.eritem_makename;
                txtmodel.Text = item.eritem_modelname;
                txtnewprice.Text = item.branch_newprice.ToString("C2");
                txtqtyinstock.Text = item.branch_qtyinstock.ToString();
                txtrepairprice.Text = item.branch_repairprice.ToString("C2");
            }
        }

   //     if (!gv.IsCallback)
    //    {
            fill_vendor_grid();
     //   }
       
        
    }
    protected void fill_vendor_grid()
    {
        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var gv = (ASPxGridView)pc.FindControl("grid_vendor_data");
        if (Session["stuff"] == null)
        {
            Session["stuff"] = 0;
        }

        var dt = _tools.getSQL_datatable(@"SELECT vendor.Vendor_Name, er_vendor_data.er_vendor_data_date, er_vendor_data.er_vendor_data_vendor_id, er_vendor_data.er_vendor_data_vendorpart, er_vendor_data.er_vendor_data_cost, er_vendor_data.er_vendor_data_id, Concat('(',address.Address_PhoneArea,') ',address.Address_PhoneFirst,' ',address.Address_PhoneLast) as vendorphone FROM eritem Inner Join er_vendor_data ON er_vendor_data.er_vendor_data_itemid = eritem.eritem_id Inner Join vendor ON vendor.Vendor_ID = er_vendor_data.er_vendor_data_vendor_id Inner Join address ON address.Address_Table_ID = vendor.Vendor_ID  where address_table = 'Vendor' and er_vendor_data_itemid =@v0", new object[] { Session["stuff"] });
        gv.DataSource = dt;
  //      if (!gv.IsCallback)
  //      {
            gv.DataBind();
  //      }
    }


    protected void btnCancelVendor_Click(object sender, EventArgs e)
    {
        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var gv = (ASPxGridView)pc.FindControl("grid_vendor_data");
        gv.CancelEdit();
        fill_vendor_grid();

    }
    protected void btnSaveVendor_Click(object sender, EventArgs e)
    {
        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var eritem_id = (ASPxTextBox)pc.FindControl("ef_txteritem_id");
        var gv = (ASPxGridView)pc.FindControl("grid_vendor_data");
        var lbl = (Label)pc.FindControl("lbl_vendorerror");
        var txt_vendor_edit_vendor_id = (ASPxTextBox)gv.FindEditFormTemplateControl("txt_vendor_edit_vendor_id");
        var txt_vendor_edit_vendor_partnumber = (ASPxTextBox)gv.FindEditFormTemplateControl("txt_vendor_edit_partnumber");
        var txt_vendor_edit_vendor_cost = (ASPxTextBox)gv.FindEditFormTemplateControl("txt_vendor_edit_cost");
        var ddl_vendor_edit_vendor = (ASPxComboBox)gv.FindEditFormTemplateControl("ddl_vendor_edit_vendor");
        var item = new NeERItem(Convert.ToInt32(Session["stuff"]), Convert.ToInt32(myMember.business_unit_id));
        double cost;
        if (ddl_vendor_edit_vendor.Text == "")
        {
            lbl.Text = "Invalid vendor selected";
            return;
        }
        try
        {
            cost = Convert.ToDouble(txt_vendor_edit_vendor_cost.Text);

        }
        catch
        {
            lbl.Text = "Invalid Cost Price";
            return;
        }
        try
        {
            if (txt_vendor_edit_vendor_id.Text == "")
            {
                txt_vendor_edit_vendor_id.Text = item.add_vendor_row(item.eritem_id, Convert.ToInt32(ddl_vendor_edit_vendor.Value), _tools.value_to(txt_vendor_edit_vendor_partnumber.Text), Convert.ToDouble(txt_vendor_edit_vendor_cost.Text)).ToString();
            }

        }
        catch
        {
            lbl.Text = "Error saving vendor row";
            return;
        }
        lbl.Text = "Vendor Row Saved";
        
        gv.CancelEdit();
        fill_vendor_grid();
        
    }
   
    protected void grid_vendor_data_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
    {
        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var gv = (ASPxGridView)pc.FindControl("grid_vendor_data");

        var txt_vendor_edit_vendor_id = (ASPxTextBox)gv.FindEditFormTemplateControl("txt_vendor_edit_vendor_id");
        var txt_vendor_edit_vendor_partnumber = (ASPxTextBox)gv.FindEditFormTemplateControl("txt_vendor_edit_partnumber");
        var txt_vendor_edit_vendor_cost = (ASPxTextBox)gv.FindEditFormTemplateControl("txt_vendor_edit_cost");
        var ddl_vendor_edit_vendor = (ASPxComboBox)gv.FindEditFormTemplateControl("ddl_vendor_edit_vendor");

        txt_vendor_edit_vendor_cost.Text = "0";
        txt_vendor_edit_vendor_partnumber.Text = "";
        txt_vendor_edit_vendor_id.Text = "";
        ddl_vendor_edit_vendor.Text = "";

    }
    
    protected void grid_vendor_data_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var gv = (ASPxGridView)pc.FindControl("grid_vendor_data");
        _tools.getSQL_void(@"Delete from er_vendor_data where er_vendor_data_id =@v0", new object[] { e.Keys[0] });
		e.Cancel = true;
        gv.CancelEdit();
    }




    protected void grid_vendor_data_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {

    }
   
    protected void grid_vendor_data_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        var pc = grid_products.FindEditFormTemplateControl("ASPxPageControl1") as ASPxPageControl;
        var gv = (ASPxGridView)pc.FindControl("grid_vendor_data");
        var value1 = gv.GetRowValues(e.VisibleIndex, new string[] { "er_vendor_data_id" }).ToString();
        _tools.getSQL_void("Delete from er_vendor_data where er_vendor_data_id = @v0" , new object[] { value1});
        fill_vendor_grid();
        
    }

    protected void btnJobSave_Click(object sender, EventArgs e)
    {
        double repairprice;
        double newprice;


        var ddlAddress = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobAddress");
        var ddlCustomer = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobCustomer");
         var ddlContact = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobContact");
        var ddl_jobERitem = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobERitem");
        var ddl_jobstatus = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobstatus");
        var ddl_JobShippedVia = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_JobShippedVia");
        var dte_jobDateEntered = (ASPxDateEdit)grid_currentjobs.FindEditFormTemplateControl("dte_jobDateEntered");
        var dte_jobShipped = (ASPxDateEdit)grid_currentjobs.FindEditFormTemplateControl("dte_jobShipped");
        var mem_jobnotes = (ASPxMemo)grid_currentjobs.FindEditFormTemplateControl("mem_jobnotes");
        var chk_jobwarranty = (ASPxCheckBox)grid_currentjobs.FindEditFormTemplateControl("chk_jobwarranty");
        var txtjobMake = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txt_JobMake");
        var txtjobModel = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txt_JobModel");
        var txtjobRepairPrice = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtJobRepairPrice");
        var txtjobNewPrice = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjobnewprice");
        var txtjob_description = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjob_description");
        var txtSerialNo = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtSerialNo");
        var txt_jobDateQuoted = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txt_jobDateQuoted");
        var btnJobSave = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobSave");
        var btnJobPrint = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobPrint");
        var btnJobCancel = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobCancel");
        var btnJobCutWO = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobCutWO");
        var lbl_joberror = (ASPxLabel)grid_currentjobs.FindEditFormTemplateControl("lbl_joberror");
        var txt_jobid = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjob_id");

        if (txt_jobid.Text != null)
        {
            txt_jobid.Text = "0";
        }
        

        if (ddlCustomer.Value==null)
        {
        lbl_joberror.Text = "Invalid Customer Selected";
        return;
        }
        if (ddlAddress.Value == null)
        {
            lbl_joberror.Text = "Invalid Address Selected";
        return;
        }
        if (ddl_jobERitem.Value == null)
        {
            lbl_joberror.Text = "You must select a product from the product list";
            return;
        }
        if (ddlContact.Value == null)
        {
            lbl_joberror.Text = "You must select a contact";
            return;
        }
        try
        {
            repairprice = Convert.ToDouble(decimal.Parse(txtjobRepairPrice.Text, System.Globalization.NumberStyles.Currency));
            newprice = Convert.ToDouble(decimal.Parse(txtjobNewPrice.Text, System.Globalization.NumberStyles.Currency));
 
        }
        catch
        {
            lbl_joberror.Text = "Invalid repair or new price";
            return;
        }

        try
        {
            var erjob = new NeERdb();
            erjob.erjob_id = Convert.ToInt32(txt_jobid.Text);
            erjob.erjob_address_id = Convert.ToInt32(ddlAddress.Value);
            erjob.erjob_ContactID = Convert.ToInt32(ddlContact.Value);
            erjob.erjob_customer_id = Convert.ToInt32(ddlCustomer.Value);
            erjob.erjob_product_id = Convert.ToInt32(ddl_jobERitem.Value);
            erjob.erjob_status = ddl_jobstatus.Text;
            erjob.erjob_ShippedVia = ddl_JobShippedVia.Text;
            erjob.erjob_SerialNumber = _tools.value_to(txtSerialNo.Text);
            erjob.erjob_Warranty = Convert.ToInt16(chk_jobwarranty.Checked);
            erjob.erjob_QuotePrice = repairprice;
            erjob.erjob_NewPrice = newprice;
            erjob.erjob_JobNotes = _tools.value_to(mem_jobnotes.Text);
            erjob.erjob_DateEntered = dte_jobDateEntered.Date;

            var erjobid = erjob.Save_job();
            

        }
        catch
        {
            lbl_joberror.Text = "Error creating ER Job";
            return;
        }

        grid_currentjobs.CancelEdit();

        fill_jobs_grid();

    }
    protected void btnJobCancel_Click(object sender, EventArgs e)
    {

        grid_currentjobs.CancelEdit();

        fill_jobs_grid();
    }
    protected void btnJobCutWO_Click(object sender, EventArgs e)
    {


        pop_cutwo.ShowOnPageLoad = true;
    }
    protected void btnJobPrint_Click(object sender, EventArgs e)
    {

    }
    public void populate_ddlCustomer(int business_unit_id, int custid)
    {
        var ddlCustomer = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobCustomer");
        ddlCustomer.DataSource = _tools.getSQL_datatable(@"Select customer_id,urldecode(customer_name) customer_name from vw_activecustomers  where business_unit_id =@v0", new object[] { business_unit_id });
        ddlCustomer.DataBind();

        if (custid != 0)
            ddlCustomer.Value = Convert.ToInt32(custid);
        else
            ddlCustomer.Text = "";

    }
    public void populate_ddlContact(int cust_id, int Contactid)
    {
        var ddlContact = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobContact");
  
        ddlContact.DataSource = _tools.getSQL_datatable(@"Select Contact_ID,Contact_Name from contact  where Contact_Type = 'Customer' AND Contact_cust_id=@v0", new object[] { cust_id });
        ddlContact.DataBind();

        if (Contactid != 0)
        {
            ddlContact.Value = Convert.ToInt32(Contactid);
        }
        else
            ddlContact.SelectedIndex = 0;

    }
    public void populate_ddlAddress(int Custid, int Addressid)
    {
        var ddlAddress = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobAddress");
        ddlAddress.DataSource = _tools.getSQL_datatable(@"Select address_id,urldecode(`address`)as address from vw_address  where cust_id =@v0 and Active=true", new object[] { Custid });
        ddlAddress.DataBind();

        if (Addressid != 0)
            ddlAddress.Value = Convert.ToInt32(Addressid);
        else
            ddlAddress.SelectedIndex = 0;
    }
    public void populate_ddlERItem(int eritemid)
    {
        var ddlEritem = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobERitem");
        ddlEritem.DataSource = _tools.getSQL_datatable(@"Select eritem_id,concat(urldecode(eritem_make),' ',urldecode(eritem_model)) as eritem from eritem order by concat(urldecode(eritem_make),' ',urldecode(eritem_model))"  , null);
        ddlEritem.DataBind();

        if (eritemid != 0)
            ddlEritem.Value = Convert.ToInt32(eritemid);
        else
            ddlEritem.Text = "";

    }
    protected void grid_currentjobs_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
    {
        var ddlAddress = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobAddress");
        var ddlCustomer = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobCustomer");
        var ddlContact = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobContact");
        var ddl_jobERitem = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobERitem");
        var ddl_jobstatus = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobstatus");
        var ddl_JobShippedVia = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_JobShippedVia");
        var dte_jobDateEntered = (ASPxDateEdit)grid_currentjobs.FindEditFormTemplateControl("dte_jobDateEntered");
        var dte_jobShipped = (ASPxDateEdit)grid_currentjobs.FindEditFormTemplateControl("dte_jobShipped");
        var mem_jobnotes = (ASPxMemo)grid_currentjobs.FindEditFormTemplateControl("mem_jobnotes");
        var chk_jobwarranty = (ASPxCheckBox)grid_currentjobs.FindEditFormTemplateControl("chk_jobwarranty");
        var txtjobMake = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txt_JobMake");
        var txtjobModel = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txt_JobModel");
        var txtjobRepairPrice = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtJobRepairPrice");
        var txtjobNewPrice = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjobnewprice");
        var txtjob_description = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjob_description");
        var txtSerialNo = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtSerialNo");
        var txt_jobDateQuoted = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txt_jobDateQuoted");
        var btnJobSave = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobSave");
        var btnJobPrint = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobPrint");
        var btnJobCancel = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobCancel");
        var btnJobCutWO = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobCutWO");
        var lbl_joberror = (ASPxLabel)grid_currentjobs.FindEditFormTemplateControl("lbl_joberror");
        var txt_jobid = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjob_id");
  
        var erjob = new NeERdb();
        var rowIndex = grid_currentjobs.EditingRowVisibleIndex;
        if (rowIndex >= 0)
        {
            var value1 = grid_currentjobs.GetRowValues(rowIndex, new string[] { "erjob_id" });
            erjob = new NeERdb(Convert.ToInt32(value1));
            var item = new NeERItem(myMember.business_unit_id, erjob.erjob_product_id);
            populate_ddlAddress(Convert.ToInt32(ddlCustomer.Value), erjob.erjob_address_id);
            populate_ddlContact(Convert.ToInt32(ddlCustomer.Value), erjob.erjob_ContactID);


            txt_jobid.Text = erjob.erjob_id.ToString();
            ddl_jobstatus.Text = erjob.erjob_status;
            ddl_JobShippedVia.Text = erjob.erjob_ShippedVia;

            dte_jobDateEntered.Text = erjob.broughtinDate;

            dte_jobShipped.Date = erjob.erjob_DateShipped;
            mem_jobnotes.Text = erjob.erjob_JobNotes;
            if (erjob.erjob_Warranty == 1)
                chk_jobwarranty.Checked = true;
            else
                chk_jobwarranty.Checked = false;

            txtjobMake.Text = item.eritem_makename;
            txtjobModel.Text = item.eritem_modelname;
            txtjobNewPrice.Text = erjob.erjob_NewPrice.ToString();
            txtjobRepairPrice.Text = erjob.erjob_QuotePrice.ToString();
            txtjob_description.Text = item.eritem_description;
            txtSerialNo.Text = erjob.erjob_SerialNumber;
            txt_jobDateQuoted.Text = erjob.erjob_DateQuoted.Date.ToShortDateString();


            btnJobSave.ClientEnabled = true;

            btnJobPrint.ClientEnabled = true;
            btnJobCancel.ClientEnabled = true;
            btnJobCutWO.ClientEnabled = true;
            lbl_joberror.Text = "";




        }
        populate_ddlERItem(erjob.erjob_product_id);
        populate_ddlCustomer(myMember.business_unit_id, erjob.erjob_customer_id);
        populate_ddlContact(Convert.ToInt32(ddlCustomer.Value), erjob.erjob_ContactID);
        populate_ddlAddress(Convert.ToInt32(ddlCustomer.Value), erjob.erjob_address_id);
        fill_jobs_grid();
    }
    protected void ddl_jobAddress_Callback(object sender, CallbackEventArgsBase e)
    {
        populate_ddlAddress(Convert.ToInt32(e.Parameter), 0);
    }

    protected void ddl_jobERitem_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ddl_jobERitem = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobERitem");
        var txtjobMake = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txt_JobMake");
        var txtjobModel = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txt_JobModel");
        var txtjobRepairPrice = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtJobRepairPrice");
        var txtjobNewPrice = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjobnewprice");
        var txtjob_description = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjob_description");
     

        var item = new NeERItem(Convert.ToInt32(ddl_jobERitem.Value), Convert.ToInt32(myMember.business_unit_id));
        txtjobMake.Text = item.eritem_makename;
        txtjobModel.Text = item.eritem_modelname;
        txtjobNewPrice.Text = item.branch_newprice.ToString("C2");
        txtjobRepairPrice.Text = item.branch_repairprice.ToString("C2");
        txtjob_description.Text = item.eritem_description;
        
    }
    protected void fill_jobs_grid()
    {
        var dt = _tools.getSQL_datatable(@"Select * from vw_erjobs"  , null);
        grid_currentjobs.DataSource = dt;
        grid_currentjobs.DataBind();


    }




    protected void ddl_jobContact_Callback(object sender, CallbackEventArgsBase e)
    {
     
        populate_ddlContact(Convert.ToInt32(e.Parameter), 0);
   
    }

    protected void grid_currentjobs_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
    {
        var dte_jobDateEntered = (ASPxDateEdit)grid_currentjobs.FindEditFormTemplateControl("dte_jobDateEntered");
        var ddl_jobstatus = (ASPxComboBox)grid_currentjobs.FindEditFormTemplateControl("ddl_jobstatus");
        var btnJobSave = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobSave");
        var btnJobPrint = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobPrint");
        var btnJobCancel = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobCancel");
        var btnJobCutWO = (ASPxButton)grid_currentjobs.FindEditFormTemplateControl("btnJobCutWO");
 
        ddl_jobstatus.SelectedIndex = 0;
        dte_jobDateEntered.Date = System.DateTime.Today;
        var txt_jobid = (ASPxTextBox)grid_currentjobs.FindEditFormTemplateControl("txtjob_id");
        txt_jobid.Text = "0";
        btnJobCutWO.ClientEnabled = false;
        btnJobPrint.ClientEnabled = false;
        
    }

    protected void grid_currentjobs_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
      
        _tools.getSQL_void(@"Delete from erjob where erjob_id =@v0 ", new object[] { e.Keys[0]});
        e.Cancel = true;
        grid_currentjobs.CancelEdit();
    }
}
