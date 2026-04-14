using System;
using System.Collections.Generic;
using System.Web.UI;
using System.DirectoryServices;
using nesi.core;

public partial class sections_hr_member_modules_it : System.Web.UI.UserControl
	{
	Toolbox	_tools		= new Toolbox();
	public int _id {get;set;}
	public int _comp_id {get;set;}
	public NeMember user {get;set;}
	protected string ldap_path			= Toolbox.app_setting("ldap_path");
	protected void Page_Load(object sender, EventArgs e)
		{

		}
	public void populate()
		{
		tb_old_employee_login.Text			= user.emplogon;
		
		tb_old_employee_id.Text		= user.id.ToString();
		tb_nesi_user.Text				= user.Username;
		var decrypted_password		= user.Password;
		var c_ldap						= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM member 
WHERE member_user = @v0 AND TRIM(IFNULL(member_ldap_user, '')) != ''", user.Username);
		if(c_ldap > 0)
			{
			// Decrypt password
			var encrypted_pass		= Toolbox.doSQL_string(@"SELECT member_pass 
FROM member WHERE member_user = @v0 AND TRIM(IFNULL(member_ldap_user, '')) != ''", user.Username);
			decrypted_password			= encrypted_pass == decrypted_password ? Toolbox.DecryptString(decrypted_password, Toolbox.app_setting("encryption_pass")) : decrypted_password;
			var de_user					= new DirectoryEntry(ldap_path, user.LDAP_user, decrypted_password, AuthenticationTypes.Secure);
			try
				{
				var nativeObject			= de_user.NativeObject;
				img_pass_sync_error.Visible	= false;
				}
			catch
				{
				// User needs to enter new password
				img_pass_sync_error.Visible	= true;
				img_pass_sync_error.Attributes["title"]	= "Password & RDP password don't match";
				tb_nesi_pass.Enabled		= false;
				}
			}
		else if(user.LDAP_user != "")
			{
			var de_user					= new DirectoryEntry(ldap_path, user.LDAP_user, user.Password, AuthenticationTypes.Secure);
			try
				{
				var nativeObject			= de_user.NativeObject;
				}
			catch
				{
				img_pass_sync_error.Visible	= true;
				img_pass_sync_error.Attributes["title"]	= "Password & RDP password don't match";
				}
			}
		tb_nesi_pass.Text				= decrypted_password;
		tb_email_address.Text			= user.NEEmail;
		tb_ldap.Text					= user.LDAP_user;
	//	tb_ldap_pass.Text = user.windows_password;
		tb_ext.Text						= user.PhoneExtension;
		hdncompid.Value = user.business_unit_id.ToString();
		hdnid.Value = user.id.ToString();
		ASPxComboBox1.DataBind();
		ASPxComboBox2.DataBind();
		ASPxComboBox1.Value = Convert.ToInt32(user.cellphone_id);
		ASPxComboBox2.Value = Convert.ToInt32(user.cellphone_number_id);
		chkbc.Checked = user.gets_barcodescanner;
		chkcell.Checked = user.gets_phone;
		chkemail.Checked = user.gets_neemail;
		chkext.Checked = user.gets_phoneext;
		chklaptop.Checked = user.gets_laptop;
		chk_include_in_mobile.Checked = user.include_in_mobile_contactlist;
	
		lbl_last_mobile_login.Text = _tools.getSQL_string(@"select ifnull((Select dt from log_page_asax  where dt>'2014-01-01' and 
log_page_asax.member_id =@v0 and log_page_asax.url='/mobile/index.aspx' order by log_page_asax.id desc limit 1),'Unknown')", new object[] { user.id });
	
		}
	protected void bt_savepanel_Click(object sender, EventArgs e)
		{
		try
		{
			#region Validating
			if (chkemail.Checked)
			{
/*				if (tb_email_address.Text != "" && tb_ldap.Text == "")
				{
					lbl_error.Text = "If an email address is given, you must have an LDAP username and password set.";
					return;
				}
				if (tb_ldap.Text != "" && tb_ldap_pass.Text == "")
				{
					lbl_error.Text = "If an LDAP account is given, you must have an LDAP password set.";
					return;
				}
				if (tb_ldap.Text == "" && tb_ldap_pass.Text != "")
				{
					lbl_error.Text = "If an LDAP password is given, you must have an LDAP account set.";
					return;
				}
 */
				if (tb_ldap.Text == "")
				{
					lbl_error.Text = "If the person is supposed to have email, they need ldap data.";
					return;
				}
			}
			if (chkcell.Checked && (ASPxComboBox1.SelectedIndex<0 || ASPxComboBox2.SelectedIndex<0))
			{
				lbl_error.Text = "This user must have a cell phone set to them, and a SIM card.";
				return;
			}
			if (chkext.Checked && tb_ext.Text=="")
			{
				lbl_error.Text = "This user must have an extension set.";
				return;
			}

			#endregion


			var mymember = new NeMember(Session["session"].ToString());
			user.emplogon					= tb_old_employee_login.Text.Trim();
			user.id					= Convert.ToInt32(tb_old_employee_id.Text.Trim());
			user.Username					= tb_nesi_user.Text.Trim();
			user.LDAP_user					= tb_ldap.Text.Trim();
			user.gets_barcodescanner = chkbc.Checked;
			user.gets_laptop = chklaptop.Checked;
			user.gets_neemail = chkemail.Checked;
			user.gets_phone = chkcell.Checked;
			user.gets_phoneext = chkext.Checked;
			user.include_in_mobile_contactlist = chk_include_in_mobile.Checked;
		

			var do_change_ldap				= false;
			var old_password				= user.Password;
			if(!string.IsNullOrEmpty(user.windows_password)) // User hasn't been hooked up to a consolidated pass
				{
				if(user.LDAP_user != "" && tb_ldap.Text.Trim() == "" &&  NeMember.is_encrypted_password(user.Password)) // Removing LDAP need to decrypt pass.
					{
					user.Password				= Toolbox.DecryptString(user.Password, Toolbox.app_setting("encryption_pass"));
					}
				old_password				=  NeMember.is_encrypted_password(user.Password)  // Never, EVER, allowed to have equals sign in password.
													? Toolbox.DecryptString(user.Password, Toolbox.app_setting("encryption_pass"))
													: user.Password;
				if(tb_nesi_pass.Enabled && tb_nesi_pass.Text != old_password && user.LDAP_user != "")
					{
					do_change_ldap				= true;
					}
				}
			else if(!string.IsNullOrEmpty(user.LDAP_user) && ! NeMember.is_encrypted_password(user.Password)) 
				// User is hooked up to an LDAP account... need to check if their existing/new pass 
				// can authenticate, if so, their current pass should be encrypted and the windows 
				// pass should be populated
				{
				user.Password				= tb_nesi_pass.Text.Trim();
				var de_user					= new DirectoryEntry(ldap_path, user.LDAP_user, user.Password, AuthenticationTypes.Secure);
				try
					{
					var nativeObject			= de_user.NativeObject;
					// Matches, encrypt password, add windows password entry.
					user.Password				= Toolbox.EncryptString(user.Password, Toolbox.app_setting("encryption_pass"));
					user.windows_password		= user.Password;
					}
				catch
					{
					// Doesn't match, move on.
					}
				}
			else
				{
				user.Password				= tb_nesi_pass.Text.Trim();
				}
			user.NEEmail					= tb_email_address.Text.Trim();
			user.PhoneExtension				= tb_ext.Text;
			var tmp_cellphone_id			= 0;
			var tmp_cellphone_number_id		= 0;
			var errors				= new List<string>();
			var is_error					= false;
			if(ASPxComboBox1.Value != null)
				{
				int.TryParse(ASPxComboBox1.Value.ToString(), out tmp_cellphone_id);
				}
			if(ASPxComboBox2.Value != null)
				{
				int.TryParse(ASPxComboBox2.Value.ToString(), out tmp_cellphone_number_id);
				}
			user.cellphone_id				= tmp_cellphone_id;
			user.cellphone_number_id		= tmp_cellphone_number_id;
			if (!is_error)
				{
				if (user.cellphone_id != 0)
					{
					var c = new NECellphone(user.cellphone_id);
					c.last_update_member_id = Convert.ToInt32(mymember.id);
					if(c.activedate.Year < 2000)
						{
						c.activedate		= DateTime.Now;
						}
					c.save();
					}
				if (user.cellphone_number_id != 0)
					{
					var c = new NECellphone_Number(user.cellphone_number_id);
					c.last_update_member_id = Convert.ToInt32(mymember.id);
					c.save();
					}
			

			user.save();
			if(do_change_ldap)
				{
				#region Change AD Password
				var ad_admin					= new DirectoryEntry(ldap_path, user.LDAP_user, old_password, AuthenticationTypes.Secure);
				if(ad_admin != null)
					{
					try
						{
						var ad_search	= new DirectorySearcher(ad_admin);
						ad_search.Filter			= "(SAMAccountName="+user.LDAP_user+")";
						var ad_result		= ad_search.FindOne();
						if(ad_result != null)
							{
							var ad_user	= ad_result.GetDirectoryEntry();
							if(ad_user != null)
								{
								ad_user.Invoke("ChangePassword", new object[] { old_password, tb_nesi_pass.Text});
								ad_user.CommitChanges();
								}
							}
						else // Roll back, can't find user in AD
							{
							lbl_error.Text		= "Couldn't update Windows password.";
							return;
							}
						}
					catch(Exception ee)
						{
						tb_nesi_pass.Text	= old_password;
						user.Password		= old_password;
						user.save();
                        _tools.catch_error(ee);
						throw;
						}
					}
				#endregion
				}
			lbl_error.Text		= "";
			}
		else
			{
			foreach(var s in errors)
				{
				lbl_error.Text		+= s+"<br/>";
				}
			}
			}
		catch (Exception ee)
			{
			lbl_error.Text		= ee.ToString();
			}
		ScriptManager.RegisterStartupScript(this, this.GetType(), "remove", @"please_wait('stop');", true);

		}
	}