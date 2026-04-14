using System;
using System.Web;
using System.Web.UI;
using System.Data;
using DevExpress.Web;
using nesi.core;

public partial class mobile_callRecord : System.Web.UI.Page
{
	NeMember current_user;
	NEContact c;
	NECustomer customer;
	NEAddress address;
	NEVendor vendor;
	NePhoneNumbers number;
	NePhoneLog phone_log;
	string cell_direct = "";
	string imei;
	string callDatenb;
	string user; //encrypted username
	string pass;
	string toNumber;
	string toName;
	string toCompany;
	string fromNumber;
                                                                                    //could be formatted like "+12804000313" or "2894000313", or "12894000313"

	string callDuration;
	string callDate;
	string callType;


    string passphrase = Toolbox.MobileEncryptionPassword(); //Should get thisfrom ToolBox



   

    protected void Page_Load(object sender, EventArgs e)
    {
	//	current_user = new Toolbox().handle_authentication("1");
        //get information
        user = HttpContext.Current.Request.QueryString["user"]; //encrypted username
        pass = HttpContext.Current.Request.QueryString["pass"]; //encrypted password
        toNumber = HttpContext.Current.Request.QueryString["toNum"]; //string call out number 
        toName = HttpContext.Current.Request.QueryString["toName"]; //string call out name IF number was in phone contacts, could be wrong or blank
        toCompany = HttpContext.Current.Request.QueryString["toCompany"]; //string company name IF was in contacts, could be wrong or blank
        fromNumber = HttpContext.Current.Request.QueryString["fromNumber"]; //string number On newelectric Phone, could be wrong should never be blank.
		//could be formatted like "+12804000313" or "2894000313", or "12894000313"

        toNumber = toNumber.Replace(" ", "");
        fromNumber = fromNumber.Replace(" ", "");
	//	user = "1";
	//	pass = "1";
		if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
		{
			current_user =  Toolbox.do_handle_authentication(1);
		
		}
		else if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
		{
		
			//decrypt username and password
			try
			{
		//		current_user = new NeMember(8);
				
				var username = Toolbox.DecryptString(user, passphrase);
			var password = Toolbox.DecryptString(pass, passphrase);
				current_user = new NeMember(username, password);
			}
			catch { }
		}
        callDuration = HttpContext.Current.Request.QueryString["callDuration"]; //Duration of call in seconds could be null
        callDate = HttpContext.Current.Request.QueryString["callDate"]; //date & time call took place Formatt like "Tue Jun 09 09:40:55 EDT 2015" could be null
        callDatenb = HttpContext.Current.Request.QueryString["callDatenb"]; //"1436533313033"
        callType = HttpContext.Current.Request.QueryString["callType"]; //Call type 1,2,3,4,5,6
        imei = HttpContext.Current.Request.QueryString["imei"]; //device imei

//	 Test variable here

	//	toNumber = "12894000313"; //string call out number 
	//	toName = ""; //string call out name IF number was in phone contacts, could be wrong or blank
	//	toCompany = ""; //string company name IF was in contacts, could be wrong or blank
	//	fromNumber = "9052080096"; //string number On newelectric Phone, could be wrong should never be blank.
		//could be formatted like "+12804000313" or "2894000313", or "12894000313"

	//	callDuration = "10"; //Duration of call in seconds could be null
	//	callDate = "Tue Jun 09 09:40:55 EDT 2015"; //date & time call took place Formatt like "Tue Jun 09 09:40:55 EDT 2015" could be null
	//	callDatenb = "9999999999997"; //"1436533313033"
	//	callType = "2"; //Call type 1,2,3,4,5,6
	//	imei = "999999999999997";
	//	imei = "352308061294499"; //device imei

	// end of test variables

        //convert all to plain text
		if (!IsPostBack && !IsCallback)
		{
			var validUser = false;
			ddl_update_company.Focus();
			if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
			{
				current_user = Toolbox.do_handle_authentication(1);
				validUser = true;
			}
			else if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
			{
				
				//decrypt username and password
				try
				{
					var username = Toolbox.DecryptString(user, passphrase);
					var password = Toolbox.DecryptString(pass, passphrase);
					current_user = new NeMember(username, password);
			//		current_user = new NeMember(8);

					//Validate User
					validUser = current_user.Authenticated;
                    if (!current_user.AuthenticatedForPrivilege(181)){
                        validUser = false;
                    }
				}
				catch
				{	}

			}
			else
			{
				//	ClientScriptManager csm = Page.ClientScript;
				//	csm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Login not authenticated, contact nesi.ca IT department.'</script>");

				Toolbox.FriendlyPopup(HttpContext.Current.Response, "User Name Not Supplied, contact "+ Toolbox.app_setting("Domain") + "IT department.", "", "Not Authorized");

				//Prompt user that an error occored
				//Account was not correct username and pasword.
				//user should reloginto their nesi account on the phone to update the password.

			}
				if (validUser)//userValid
				{

					//pre populate text fileds
					if (!string.IsNullOrWhiteSpace(toNumber))
					{
						// prep number text
						toNumber = toNumber.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
                        if (toNumber.Substring(0, 1) == "1")
                        {
                            toNumber = toNumber.Substring(1);
                        }
						if (toNumber.Length == 10)
						{
							calledNumber.Text = toNumber.Substring(0, 3) + " " + toNumber.Substring(3, 3) + " " + toNumber.Substring(6, 4);
							// first find a contact and build backwards.. 

							var dt = new NePhoneNumbers().get_contact_from_phonenumber(toNumber);
							if (dt.Rows.Count > 0)
							{
								foreach (DataRow dr in dt.Rows)
								{
									c = new NEContact(Convert.ToInt32(dr[0]));
									ddl_new_company0.Value = dr[1].ToString();
									cell_direct = dr[1].ToString();
									if (c.address_id == 0)
									{
										if (c.type.Equals("Customer"))
										{
											customer = new NECustomer(Convert.ToInt32(c.customer_id));
											address = new NEAddress(customer.id, "Customer");
										}
										else
										{
											vendor = new NEVendor(Convert.ToInt32(c.customer_id));
											address = new NEAddress(customer.id, "Vendor");
										//	ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "myEndWebViewAndApp", "if (typeof (Android) !== 'undefined') {Android.showToast('This feature is not yet working for vendors',3);Android.endWebViewAndApp();}else{alert('This feature is not yet working for vendors');}", true);
										//	up.Visible = false;
											return;
										}
									}
									else
									{
										address = new NEAddress(c.address_id);
									}

									ddl_update_company.Value = address.id;
									ddl_update_contact.DataBind();
									if (ddl_update_contact.Items.FindByValue(c.id) != null)
									{
										ddl_update_contact.Value = c.id;
									}
									else
									{
										var li = new ListEditItem(c.Contact_Name, c.id);
										ddl_update_contact.Items.Add(li);
										ddl_update_contact.Value = c.id;

									}

									txt_notes.Focus();
									break;
								}
							}
							else
							{
								number = NePhoneNumbers.get_phonenumber_record(toNumber);
								if (number.phone_numbers_id != 0)
								{
									if (number.phone_numbers_type.Equals("Address"))
									{
										address = new NEAddress(Convert.ToInt32(number.phone_numbers_table_id));
										ddl_update_company.Value = address.id;
										ddl_update_contact.DataBind();
										ddl_update_contact.CallbackPageSize = ddl_update_contact.Items.Count>20? ddl_update_contact.Items.Count: 20;
										ddl_update_contact.DropDownRows = ddl_update_contact.CallbackPageSize;
										ddl_update_contact.DataBind();
										ddl_new_company0.Value = "Company Line";
										cell_direct = "Company Line";
									}
								}
							}






						}
						else
						{
							calledNumber.Text = toNumber;
						}
						if (!string.IsNullOrWhiteSpace(fromNumber))
						{
							fromNumber = fromNumber.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
                            

						}



						//get callers information from database 
						//like, Name, job title?
						//is this needed? they know who they are and probably shouldnt be able to change it.

					}




				}
				else
				{
					Toolbox.FriendlyPopup(HttpContext.Current.Response, "not authenticated, contact " + Toolbox.app_setting("Domain")  + " IT department.", "", "Not Authorized");
					//Prompt user that an error occored
					//Account was not correct username and pasword.
					//user should reloginto their nesi account on the phone to update the password.

				}
				


			
		}
    }
    protected void submit_Click(object sender, EventArgs e)
    {
		
		NePhoneLog phone_log;

		#region save or update log
		if (ddl_update_contact.SelectedIndex < 0 && (txt_new_contact.Text=="" && txt_new_contact.Enabled==true ))
		{
			ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", @"
if (typeof (Android) !== 'undefined') {
    Android.andAlert('Who did you call?','You must either select a contact OR add a new one to save the call record.', 'noFun');
}else{
    alert('You must either select a contact OR add a new one to save the call record.');
}", true);
		
			return;
			
		}


		phone_log = new NePhoneLog().get_NePhonglog(imei, callDatenb);
		if (phone_log.phone_log_id == 0)
		{
            //new
			var pl = new NePhoneLog();
			pl.calltime_nb = Convert.ToInt32(callDatenb);
			pl.external_name = "";
			pl.imei_ds = imei;
			pl.internal_name = "";
			if (ddl_new_company0.Value.Equals("Company Line"))
			{
				pl.notes = ddl_update_contact.Text + " - " + txt_notes.Text;
			}
			else
			{
				pl.notes = txt_notes.Text;
			}
			pl.phone_log_date = System.DateTime.Today;
			pl.phone_log_direction = callType == "2" ? 2 : 1;
			pl.phone_log_duration = Convert.ToInt32(callDuration);
            var log_from_name = callType == "2" ? current_user.FullName : ddl_update_contact.Text != "" ? ddl_update_contact.Text : txt_new_contact.Text;

            if (log_from_name.Equals(string.Empty) && callType != "2")
            {
                log_from_name = txt_new_contact.Text;
            }
			pl.phone_log_from_name = log_from_name;
            var phone = (callType == "2" ? fromNumber : toNumber).Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            if (phone.Length >= 1 && phone.Substring(0, 1).Equals("1"))
            {
                phone = phone.Substring(1);
            }
            pl.phone_log_from_number = phone;
			pl.phone_log_time = Convert.ToInt32(Math.Round(Convert.ToDecimal((Convert.ToInt32(callDatenb) / 1000)), 0));

            var log_to_name = callType == "2" ? ddl_update_contact.Text : current_user.FullName;

            if (log_to_name.Equals(string.Empty) && callType == "2")
            {
                log_to_name = txt_new_contact.Text;
            }


            pl.phone_log_to_name = log_to_name;
            phone = (callType == "2" ? toNumber : fromNumber).Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
            if (phone.Substring(0, 1).Equals("1"))
            {
                phone = phone.Substring(1);
            }
            pl.phone_log_to_number = phone;
			pl.phone_log_alitgen_session_id = 0;
			pl.calltype_fg = Convert.ToInt16(callType);
			
			pl.save();
		}
		else
		{
			phone_log.phone_log_alitgen_session_id = 0;
			if (ddl_new_company0.Value.Equals("Company Line"))
			{
                var name = ddl_update_contact.Text;
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = txt_new_contact.Text;
                }
                phone_log.notes = name + " - " + txt_notes.Text;
			}
			else
			{
				phone_log.notes = txt_notes.Text;
			}
			phone_log.save();
		}


		#endregion

		#region connect phone number to contact (if its a cell or a direct line)

	
	    // check to see if the number is already linked to someone else
		if (!string.IsNullOrWhiteSpace(toNumber))
		{
			toNumber = toNumber.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();
			if (toNumber.Length == 10)
			{

				if ((ddl_new_company0.Value.Equals("Cell") || ddl_new_company0.Value.Equals("Direct Line"))&&(ddl_update_contact.SelectedIndex>=0))
				{
					var dt = new NePhoneNumbers().get_contact_from_phonenumber(toNumber);
					if (dt.Rows.Count > 0)
					{
						foreach (DataRow dr in dt.Rows)
						{
							if (!ddl_update_contact.Value.ToString().Equals(dr[0].ToString()))// if so, unlink it.
							{
								var c = new NEContact(Convert.ToInt32(dr[0]));
								if (c.cellphone.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim().Equals(toNumber))
								{
									c.cellphone = "";
								}
								if (c.direct_line.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim().Equals(toNumber))
								{
									c.direct_line = "";
								}
								c.save();
							}
							else
							{
								var update_contact1 = new NEContact(Convert.ToInt32(ddl_update_contact.Value));
								if (ddl_new_company0.Value.Equals("Cell"))
								{
									if (update_contact1.direct_line.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim().Equals(toNumber))
									{
										update_contact1.direct_line = "";
									}
									update_contact1.cellphone = toNumber.Substring(0, 3) + "-" + toNumber.Substring(3, 3) + "-" + toNumber.Substring(6, 4);
								}
								else
								{
									if (update_contact1.cellphone.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim().Equals(toNumber))
									{
										update_contact1.cellphone = "";
									}
									update_contact1.direct_line = toNumber.Substring(0, 3) + "-" + toNumber.Substring(3, 3) + "-" + toNumber.Substring(6, 4);
								}
								update_contact1.save();
							}
						}
					}
					// now set up the new links
					var update_contact = new NEContact(Convert.ToInt32(ddl_update_contact.Value));
					if (ddl_new_company0.Value.Equals("Cell"))
					{
						update_contact.cellphone = toNumber.Substring(0, 3) + "-" + toNumber.Substring(3, 3) + "-" + toNumber.Substring(6, 4);
					}
					else
					{
						update_contact.direct_line = toNumber.Substring(0, 3) + "-" + toNumber.Substring(3, 3) + "-" + toNumber.Substring(6, 4);
					}
					update_contact.save();
				}
				else if (ddl_new_company0.Value.Equals("Company Line"))  // if its a company  line add it to the phonenumber table.
				{
					var pn = new NePhoneNumbers();
					pn.comm_type = "LandLine";
					pn.is_active = true;
					pn.is_default = false;
					pn.number = toNumber.Substring(0, 3) + " " + toNumber.Substring(3, 3) + " " + toNumber.Substring(6, 4);
					pn.type = "Address";
					pn.table_id = Convert.ToInt32(ddl_update_company.Value);
                    try
                    {
                        pn.Save();
                    }
                    catch
                    {

                    }

				}
			}

		}

		


		#endregion




		
		#region new contact
		var _tools = new Toolbox();

        if (ddl_new_company0.Value == "Cell" || ddl_new_company0.Value == "Direct Line" || ddl_new_company0.Value.Equals("Company Line"))  // if its a personal number
		{
			#region entering a new contact
			if ((txt_new_contact.Text != "")&&(ddl_update_contact.SelectedIndex<0))  // if something has been entered into the new contact box
			{
				var new_contact_address = new NEAddress(Convert.ToInt32(ddl_update_company.Value));  // load the address
				if (new_contact_address.Table == "Customer")  // if its a customer...
				{
					if (_tools.getSQL_int(@"Select count(contact_id) from contact  where contact_name =@v0 and address_id =@v1  limit 1 ", new object[] { txt_new_contact.Text,new_contact_address.id })==0)
					{
						var nc = new NEContact();
						nc.address_id = new_contact_address.id;
						nc.customer_id = new_contact_address.Table_ID;
						nc.type = "Customer";
						nc.Contact_Type = "Customer";
						nc.Contact_Status = "Active";
						nc.status = "Active";
						nc.name = txt_new_contact.Text;
						if (ddl_new_company0.Value == "Cell")
						{
							nc.cellphone = calledNumber.Text.Replace(" ", "-");
						}
						else if (ddl_new_company0.Value == "Direct Line")
						{
							nc.direct_line = calledNumber.Text.Replace(" ", "-");
						}
						nc.AddNEContact(nc);

					}
					else  // if it already exists
					{
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "andAlert", @" alert('The name you entered for the new contact, already exists at this address.  Please select them from the drop down list.');", true);
						return;
					}

				}
				else  // else its a vendor
				{
					if (_tools.getSQL_int(@"Select count(contact_id) from contact  where contact_name =@v0 and Address_id =@v1  limit 1 ", new object[] { txt_new_contact.Text,new_contact_address.id })==0)
					{
						var nc = new NEContact();
						nc.address_id = new_contact_address.id;
						nc.customer_id = new_contact_address.Table_ID;
						nc.type = "Vendor";
						nc.Contact_Type = "Vendor";
						nc.Contact_Status = "Active";
						nc.status = "Active";
						nc.name = txt_new_contact.Text;
						if (ddl_new_company0.Value == "Cell")
						{
							nc.cellphone = calledNumber.Text.Replace(" ", "-");
						}
						else if (ddl_new_company0.Value == "Direct Line")
						{
							nc.direct_line = calledNumber.Text.Replace(" ", "-");
                        }
                        else if (ddl_new_company0.Value.Equals("Company Line"))
                        {

                        }
						nc.AddNEContact(nc);

					}
					else
					{
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "andAlert", @"
if (typeof (Android) !== 'undefined') {
    Android.andAlert('Name exists','The name you entered for the new contact, already exists at this address.  Please select them from the drop down list.','noFun')
}else{
    alert('The name you entered for the new contact, already exists at this address.  Please select them from the drop down list.');
}", true);
						return;
					}
				}
			}
			#endregion



        }
		#endregion

        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "andAlert", "Android.andAlert('Call record has been saved.', 'noFun')", true);
//        ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "myEndWebViewAndApp", @"
//Android.showToast('Call record has been saved.');
//Android.endWebViewAndApp();", true);
//		up.Visible = false;
//		Response.Redirect("#/home/1/1/homepage/default");
//		return;

//	}
//    protected void END_Click(object sender, EventArgs e)
//    {
//        //this kills the webview on the phone
//        ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), "myEndWebViewAndApp", @"
//Android.andDialog('Close it down?','Your comments wont be saved.', 'endPageFun');

//function endPageFun(answer){

//if (answer == 'yes'){

//    Android.showToast('Comments NOT saved');
//    Android.endWebViewAndApp();
//}

//}
//", true);
        //	up.Visible = false;

        Response.Redirect("#/home/1/1/homepage/default");
    }
	
	protected void ddl_update_contact_Callback1(object sender, DevExpress.Web.CallbackEventArgsBase e)
	{
		ddl_update_contact.DataBind();
		ddl_update_contact.Focus();
	}

}