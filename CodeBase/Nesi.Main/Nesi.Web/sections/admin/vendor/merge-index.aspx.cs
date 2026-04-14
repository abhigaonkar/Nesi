using System;
using System.Data;
using DevExpress.Web;
using nesi.core;

public partial class sections_admin_vendor_index : System.Web.UI.Page
{
    Toolbox _Tools = new Toolbox();
    NeMember myMember;
    int _PageAccess = 86;

    protected void Page_Load(object sender, EventArgs e)
    {

        
        myMember = Toolbox.do_handle_authentication(_PageAccess);


       var sqlVendorFix = @"
SELECT 
	A.address_id AS AID, 
	b.address_id AS BID,
	CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast,A.address_type) AS PhoneA,
	CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast,B.address_type) AS PhoneB,
	C.vendor_number AS ANum, 
	D.vendor_number AS BNum,
	C.Vendor_ID AS AVID,
	D.Vendor_ID AS BVID,
	C.Vendor_Name AS ANAME, 
	D.Vendor_Name AS BNAME
FROM 
	address as A, 
	address as B, 
	Vendor as C, 
	Vendor as D
WHERE 
	A.address_phonearea = B.address_phonearea AND 
	A.address_phonefirst = B.address_phonefirst AND 
	A.address_phonelast = B.address_phonelast AND 
	A.address_type = B.address_type AND
	CONCAT(A.address_phonearea,A.address_phonefirst,A.address_phonelast) NOT IN ('','0000000000','9058272555' ,'9056891845','8002044153','9058274255') AND
	CONCAT(B.address_phonearea,B.address_phonefirst,B.address_phonelast) NOT IN ('','0000000000','9058272555' ,'9056891845','8002044153','9058274255') AND
	A.address_id != b.address_id AND
	A.Address_Table_ID = C.Vendor_ID AND
	B.Address_Table_ID = D.Vendor_ID AND
	A.Address_Table = 'Vendor' AND
	B.Address_Table = 'Vendor' AND
	C.Vendor_Active = TRUE AND
	D.Vendor_Active = TRUE
GROUP BY 
	A.address_id
ORDER BY 
	C.Vendor_Name";

        var VendorTable = _Tools.getSQL_datatable(sqlVendorFix,null);
        ASPxGridView1.DataSource = VendorTable;
        ASPxGridView1.DataBind();

       var sqlVendorFix2 = @"
SELECT 
	b.vendor_id, 
	a.vendor_name, 
	a.vendor_number, 
	b.vendor_name, 
	b.vendor_number 
FROM 
	vendor AS a, vendor as b 
WHERE 
	a.vendor_name LIKE CONCAT (b.vendor_name,'%') AND 
	a.vendor_id != b.vendor_id AND 
	a.Vendor_Active = 1 AND 
	b.Vendor_Active = 1
ORDER BY A.Vendor_Name";

        var VendorTable2 = _Tools.getSQL_datatable(sqlVendorFix2  , null);
        ASPxGridView2.DataSource = VendorTable2;
        ASPxGridView2.DataBind();
    }

    protected void ASPxGridView1_SelectionChanged(object sender, EventArgs e)
    {
        var vendorid = "";

        var gridView = (ASPxGridView)sender;
        var gridIndex = gridView.FocusedRowIndex;
        if (gridView.FocusedRowIndex != -1)
        {
            var row = gridView.GetDataRow(gridIndex);
            vendorid = row["AVID"].ToString();
            Response.Redirect("DisplayVendInfo.aspx?vendid=" + vendorid);
        }
    }

    protected void ASPxGridView2_SelectionChanged(object sender, EventArgs e)
    {

        var vendorid = "";

        var gridView = (ASPxGridView)sender;
        var gridIndex = gridView.FocusedRowIndex;
        if (gridView.FocusedRowIndex != -1)
        {
            var row = gridView.GetDataRow(gridIndex);
            vendorid = row["vendor_id"].ToString();
            Response.Redirect("DisplayVendInfo.aspx?namevendid=" + vendorid);
        }
    }
    protected void ASPxGridView3_SelectionChanged(object sender, EventArgs e)
    {
        var vendorid = "";

        var gridView = (ASPxGridView)sender;
        var gridIndex = gridView.FocusedRowIndex;
        if (gridView.FocusedRowIndex != -1)
        {
            var row = gridView.GetDataRow(gridIndex);
            vendorid = row["AVID"].ToString();
            Response.Redirect("DisplayVendInfo.aspx?addressvendid=" + vendorid);
        }
    }
}
