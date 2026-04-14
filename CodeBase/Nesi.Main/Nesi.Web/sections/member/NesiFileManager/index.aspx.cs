using System;
using System.IO;
using System.Linq;
using nesi.core;

public partial class NesiFileManager : System.Web.UI.Page
{
	private static int _page_id = 19;  //Download Page
	private string rootFolder = "";
	NeMember myMember;

	protected void Page_Init()
	{
		myMember = Toolbox.do_handle_authentication(_page_id);
		var list = new Current_User().visible_tax_entities;
		if (string.IsNullOrEmpty(list))
		{
			list = "0";
		}
		ddl_te.DataSource = Toolbox.doSQL_dt("Select id,ddl_name from tax_entity where id in (" + list + ") order by ddl_name", null);
		ddl_te.DataBind();


	}
	protected void Page_Load(object sender, EventArgs e)
	{

		var menu = new NeMenu(myMember, Convert.ToInt32(_page_id));
		divMenu.InnerHtml = menu.MenuHTML;
		divSide.InnerHtml = shared.PrintSidePanelHTML(myMember);
		lblError.Text = "";
		var working_customer = new NECustomer();
		fm.Settings.UseAppRelativePath = false;

		if (!IsPostBack)
		{
			ddl_te.Value = myMember.business_unit.tax_entity_id;
			rootFolder = NeTaxEntity.BaseFolder(myMember.business_unit_id, false) + @"\_protected";
		}
		else
		{
			rootFolder = NeTaxEntity.BaseFolder(Toolbox.doSQL_int("Select id from business_unit where tax_entity_id = @v0 limit 1", new object[] { ddl_te.Value }), false) + @"\_protected";
		}

		if (myMember.isContact)
		{
			working_customer = new NECustomer((int)myMember.customerID);
			ASPxComboBox1.Value = "2";  // set the page to public mode on first load
			fm.Settings.RootFolder = rootFolder + @"\NesiFileManager-WideOpenPublic\";
			ASPxComboBox1.Items.Clear();
			ASPxComboBox1.Items.Add("Wide Open Public", "2");
		}
		else
		{
			if ((!myMember.AuthenticatedForPrivilege(19) && !NeMember.is_owner(myMember.id32, Convert.ToInt32(ddl_te.Value))))
			{
				ASPxComboBox1.Items.Clear();
				ASPxComboBox1.Items.Add("Internal Public", "0");
				ASPxComboBox1.Items.Add("Wide Open Public", "2");
			}
			if (!IsPostBack)
			{
				ASPxComboBox1.Value = "0";  // set the page to public mode on first load
				fm.Settings.RootFolder = rootFolder + @"\NesiFileManager-InternalPublic\";
			}
		}
		Session["errortext"] = "";



		if ((ASPxComboBox1.Value.ToString() == "0") && !myMember.isContact)  // if its an employee and 
		{
			fm.Settings.RootFolder = rootFolder + @"\NesiFileManager-InternalPublic\";
		}
		else if ((ASPxComboBox1.Value.ToString() == "1") && !myMember.isContact)
		{
			if (myMember.AuthenticatedForPrivilege(19)) // if they have access to the prvis area
			{
				fm.Settings.RootFolder = rootFolder + @"\NesiFileManager-Confidential\";
			}
			else
			{
				Session["errortext"] = "You are not authorized to access confidential files";
				ASPxComboBox1.Value = "0";
				fm.Settings.RootFolder = rootFolder + @"\NesiFileManager-InternalPublic\";
			}
		}
		else if (ASPxComboBox1.Value.ToString() == "2")
		{
			if (myMember.AuthenticatedForPrivilege(114)) // if they have access to the prvis area
			{
				if (myMember.isContact)
				{
					fm.Settings.RootFolder = customer_folder(working_customer.id);
					if (!customer_folder_exists(myMember.customerID))
					{
						customer_folder_create(myMember.customerID);
					}
				}
				else
				{
					fm.Settings.RootFolder = rootFolder + @"\NesiFileManager-WideOpenPublic\";
				}
			}
			else if (!myMember.isContact)
			{
				Session["errortext"] = "You are not authorized to access the wide open public file system.";
				ASPxComboBox1.Value = "0";

				fm.Settings.RootFolder = rootFolder + @"\NesiFileManager-InternalPublic\";
			}
			else
			{
				fm.ClientVisible = false;
				lblError.Text = "You do not have access to the wide open public file system";
				lblError.Visible = true;
			}

		}
		if ((myMember.AuthenticatedForPrivilege(20)) || (ASPxComboBox1.Value.ToString() == "2"))
		{
			fm.SettingsEditing.AllowDelete = true;
			fm.SettingsEditing.AllowMove = true;
			fm.SettingsEditing.AllowRename = true;
		}
		else
		{
			fm.SettingsEditing.AllowDelete = false;
			fm.SettingsEditing.AllowMove = false;
			fm.SettingsEditing.AllowRename = false;
		}
		if ((Convert.ToString(ASPxComboBox1.Value) == "0") && (myMember.isContact))
		{
			fm.ClientVisible = false;
			lblError.Text = "You do not have access to the internal public file system";
			lblError.Visible = true;
		}
		if ((Convert.ToString(ASPxComboBox1.Value) == "1") && (myMember.isContact))
		{
			fm.ClientVisible = false;
			lblError.Text = "You do not have access to the internal private file system";
			lblError.Visible = true;
		}

	}
	private string customer_folder(int _id)
	{
		var c = new NECustomer((int)_id);
		var should_be = rootFolder + @"\nesifilemanager-wideopenpublic\C" + c.id + "-" + c.Customer_Name;
		if (customer_folder_exists(_id))
		{
			return should_be;
		}
		var base_folder = rootFolder + @"\nesifilemanager-wideopenpublic";
		var by_id = Directory.EnumerateDirectories(base_folder, string.Format(@"C{0}-*", _id));
		var by_id_arr = by_id as string[] ?? by_id.ToArray();
		if (by_id_arr.Length == 1)
		{// Simple rename
			Directory.Move(by_id_arr[0], should_be);
			return should_be;
		}
		if (by_id_arr.Length == 0)
		{
			Directory.CreateDirectory(should_be);
			return should_be;
		}
		if (by_id_arr.Length > 1)
		{
			foreach (var f in by_id_arr)
			{
				Directory.Move(f, should_be);
			}
		}
		return "";
	}
	private bool customer_folder_exists(int _id)
	{
		var base_folder = rootFolder + @"\nesifilemanager-wideopenpublic";
		var by_id = Directory.EnumerateDirectories(base_folder, string.Format(@"C{0}-*", _id));
		var by_id_arr = by_id as string[] ?? by_id.ToArray();
		return by_id_arr.Length == 1;
	}
	private void customer_folder_create(int _id)
	{
		var base_folder = rootFolder + @"\nesifilemanager-wideopenpublic";
		var by_id = Directory.EnumerateDirectories(base_folder, string.Format(@"C{0}-*", _id));
		var by_id_arr = by_id as string[] ?? by_id.ToArray();
		if (by_id_arr.Length != 1)
		{
			var path = customer_folder(_id);
			//Directory.CreateDirectory(path);
		}
	}
	protected void ASPxFileManager1_FileUploading(object source, DevExpress.Web.FileManagerFileUploadEventArgs e)
	{
		try
		{
			if (e.File.FullName != "")
			{
				var di = new DirectoryInfo(e.File.Folder.FullName);
				var rgFiles = di.GetFiles();
				Session["errortext"] = "";
				foreach (var fi in rgFiles)
				{
					if (fi.FullName == e.File.FullName)
					{
						fi.Delete();
						lblError.Text = e.File.Name + " has been overwritten";
					}
				}
			}
		}
		catch (Exception ee)
		{
			Toolbox.do_errorLog_errorStack(ee);
			throw;
		}

	}

	protected void ASPxFileManager1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
	{

		e.Properties["cplblError"] = Session["errortext"].ToString();
	}

	protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		fill_can_see();
	}

	protected void ddl_te_SelectedIndexChanged(object sender, EventArgs e)
	{
		fill_can_see();
	}

	protected void fill_can_see()
	{
		if (ASPxComboBox1.Value == "1")
		{
			lb.DataSource = Toolbox.doSQL_dt(@"Select distinct member.member_id id,member_fullname name,b.ddl_name bu
from member
inner join business_unit b on b.id = member.business_unit_id
inner join tax_entity te on te.id = b.tax_entity_id 
inner join memberpageprivilege mp on mp.MemberPagePrivilege_Member_ID = member.member_id and mp.MemberPagePrivilege_Privilege_ID = 19 


where 
 member_status = 'Active' and member.member_id != @v1 and
 FIND_IN_SET(@v0,get_visible_tax_entities_group_concat(member.member_id)) 
order by member_fullname", new object[] { ddl_te.Value, myMember.id32 });


		}
		lb.DataBind();
		lb.ClientVisible = (lb.Items.Count > 0) ? true : false;
	}
}
