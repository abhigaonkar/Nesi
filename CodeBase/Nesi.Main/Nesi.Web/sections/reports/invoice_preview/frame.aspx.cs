using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using nesi.core;
using NESI.Common.Models;

public partial class sections_reports_invoice_preview_frame : System.Web.UI.Page
	{
	NeMember _current_user;
	NameValueCollection _q;
	bool _approve_pm, _approve_bm, _approve_dm, _is_signoff, _do_signature, _is_child_approval, _del_signature;
	int _id = 0;
	protected void Page_Init(object _sender, EventArgs _e)
		{
		_q                      = Request.QueryString;
		_current_user           = Toolbox.do_handle_authentication(OpsPage.Home);
		_approve_pm             = _current_user.AuthenticatedForPrivilege(OpsPrivilege.ApprovalPm);
		_approve_bm             = _current_user.AuthenticatedForPrivilege(OpsPrivilege.ApprovalBm);
		_approve_dm				= _current_user.AuthenticatedForPrivilege(OpsPrivilege.ApprovalDm);
		_is_signoff             = !string.IsNullOrEmpty(_q["is_signoff"]) && _q["is_signoff"].ToLower() == "true";
		_do_signature           = !string.IsNullOrEmpty(_q["do_signature"]) && _q["do_signature"].ToLower() == "true";
		_del_signature          = !string.IsNullOrEmpty(_q["del_signature"]) && _q["del_signature"].ToLower() == "true";
		_is_child_approval      = !string.IsNullOrEmpty(_q["is_child_approval"]) && _q["is_child_approval"].ToLower() == "true";
		_id                     = Convert.ToInt32(_q["id"]);
		hid_id.Value            = _q["id"];
		hid_from.Value          = string.IsNullOrEmpty(_q["from"]) ? "" : _q["from"];
		var wo                  = new NeWOProg(_id);
		btn_open_parent.Visible = false;
		Title                   = "Invoice Preview for WO " + wo.OrderNumber.TrimStart('0');
		if (wo.QuoteID == "0" && wo.woprog_associate_woprog_id == 0)
			{
			ts_comments.Visible = true;
			ts_comments.Text = wo.Description;
			btn_edit_ts_comments.Visible = true;
			}
		else
			{
			btn_edit_ts_comments.Visible = false;
			ts_comments.Visible = false;

			}
		if(_del_signature)
			{
			// delete sig
			var s		= new shared.signature(_id, "woprog");
			s.delete();
			    try
			        {
			        NeWOProg.check_before_move(wo, OpsWOStatus.Open);
			        }
			    catch (Exception check_move_error)
			        {
			        Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
			        return;
			        }
            NeWOProg.move_status(wo, _current_user, "false", OpsWOStatus.Enums.Open);
			// redirect to mobile
			Response.Redirect(string.Format("/mobile/index.aspx?a=signoff&woprog_id={0}&from=wo", _id));
			}
		if (!_do_signature)
			{
			panel_frame.Visible = true;

			if_invoice.Attributes["src"] = _is_signoff ? "./index.aspx?id=" + _q["id"] + "&html=1&is_signoff=true" : "./index.aspx?id=" + _q["id"] + "&html=1";
			btn_bm_approve.Visible = (_approve_bm || _approve_dm) && !_is_signoff && (wo.Status != OpsWOStatus.WaitingToBeInvoiced && wo.Status != "Invoiced");
			btn_pm_approve.Visible = _approve_pm && !_approve_bm && !_approve_dm && !_is_signoff && (wo.Status != OpsWOStatus.WaitingToBeInvoiced && wo.Status != "Invoiced");

			if (_is_signoff)
				{
				var c = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM signature WHERE `table` = 'woprog' AND table_id = @v0 ", new object[] {  _id } );
				if (c == 0)
					{
					btn_signature.Visible = true;
					btn_signature.Attributes["onclick"] = string.Format("location.href='frame.aspx?id={0}&do_signature=true&from={1}';", _id, hid_from.Value);

					}
				else
					{
					var s = new shared.signature(_id, "woprog");
					signature_graphic.Src = "data:image/png;base64," + Convert.ToBase64String(s.graphic);
					signature_graphic.Visible = true;
					signature_printed.Visible = true;
					signature_printed.InnerText = s.printed_name;
					if(wo.Status == "Initial Prep")
						{
						btn_del_signature.Visible = true;
						btn_del_signature.Attributes["onclick"] = string.Format("if(confirm('Are you sure you want to delete this signature?')){{location.href='frame.aspx?id={0}&del_signature=true';}}", _id);
						btn_signature.InnerText = "Replace Signature";
						btn_signature.Visible = true;
						btn_signature.Attributes["onclick"] = string.Format("location.href='frame.aspx?id={0}&do_signature=true&from={1}';", _id, hid_from.Value);
						}
					}
				}
			if (_is_child_approval)
				{
				btn_back.Visible = false;
				btn_edit_ts_comments.Visible = false;
				btn_edit_ts_comments.Visible = false;
				ts_comments.Visible = true;
				var child_co = new NeBusinessUnit(wo.business_unit_id);
				if (wo.parent_woprog_id > 1)
					{
					btn_open_parent.Visible = true;
					btn_open_parent.Attributes["onclick"] = "javascript:boing('/sections/workorder/index.aspx?woprog_id=" + wo.parent_woprog_id + "','wo',800,800);";
					if (wo.QuoteID=="0")
					{
						ts_comments.Text +=  Environment.NewLine;
						var dt = Toolbox.doSQL_dt(@"SELECT distinct concat(membertime.Date,' ',round(membertime.NumberOfHours,1),' hrs ',member.member_fullname, wocomment.comments) entry FROM membertime INNER JOIN member ON membertime.membertime_memberid = member.Member_ID AND membertime.business_unit_id = member.Member_ID INNER JOIN wocomment ON membertime.MemberTime_WoComment_ID = wocomment.WoComment_ID  WHERE membertime.MemberTime_WOProg_id =@v0 order by membertime.date", new object[] { wo.woprog_id });
						foreach(DataRow dr in dt.Rows)
						{
							ts_comments.Text += dr[0] + Environment.NewLine;
						}
						
					}
					var wo_parent = new NeWOProg(wo.parent_woprog_id);
					btn_bm_approve.Visible = (_approve_bm || (_approve_dm && wo_parent.business_unit_id==_current_user.business_unit_id)) && !_is_signoff && (wo.Status != OpsWOStatus.WaitingToBeInvoiced && wo.Status != "Invoiced");
					btn_pm_approve.Visible = _approve_pm && !_approve_bm && !_approve_dm && !_is_signoff && (wo.Status != OpsWOStatus.WaitingToBeInvoiced && wo.Status != "Invoiced");

					btn_open_parent.InnerText = "Open Parent:" + wo_parent.OrderNumber;
				//	this.Title = "Invoice Preview for WO " + wo.OrderNumber.TrimStart('0') + " from " + child_co.name + " - Parent WO is " ;
					btn_bm_approve.Attributes["onclick"] = string.Format("if(confirm('The amount of {0} (for materials) will be added to your work order {1} for {2} as a subcontract cost.  The resulting sellprice will be {3}.  Labor has already been added via timesheets. ')){{cbp.PerformCallback('bm_approve|0');}}", wo.woprog_materialtotalSell.ToString("C2"), wo_parent.OrderNumber, wo_parent.CustomerName, wo_parent.use_fixed_material_markup? (wo.woprog_materialtotalSell*wo_parent.fixed_material_markup).ToString("C2") : shared.GetSellPrice(wo.woprog_materialtotalSell, 0, true, 1, wo_parent.business_unit_id).ToString("C2"));
					}
				else
					{
					btn_bm_approve.Attributes["onclick"] = string.Format("if(confirm('Please confirm you want to approve {0} WO #: {1}')){{cbp.PerformCallback('bm_approve|0');}}", child_co.name, wo.OrderNumber);
					}

				}
			else
			{
				btn_open_parent.Visible = false;
			}
			}
		else
			{
			panel_signature.Visible = true;
			uc_signpad.customer_id = wo.WOProg_Customer_ID;
			uc_signpad.address_id = wo.woprog_Address_ID;
			uc_signpad.show_contact_ddl = true;
			uc_signpad.show_po = true;
			uc_signpad.woprog_id = wo.woprog_id;
			}
		if (Page.Request.Browser.IsMobileDevice)
			{
			if_invoice.Style["height"] = "450px";
			}
		else
			{
			if_invoice.Style["height"] = "700px";
			}
		}
	protected void Page_Load(object _sender, EventArgs _e)
		{
		}
	protected void cbp_Callback(object _sender, DevExpress.Web.CallbackEventArgsBase _e)
		{
		var wo = new NeWOProg();
		var paras = _e.Parameter.Contains("|") ? _e.Parameter.Split('|') : null;
		if (paras != null)
			{
			var action	= paras[0];
			if (action == "bm_approve")
				{
				try
					{
					wo		= new NeWOProg(_id);
					var temp_parent_wo		= wo.parent_woprog_id > 1 ? new NeWOProg(wo.parent_woprog_id) : new NeWOProg();
					if (_approve_bm || _approve_dm && (wo.business_unit_id == _current_user.business_unit_id || temp_parent_wo.business_unit_id == _current_user.business_unit_id))
						{
						var child_wo = new NeWOProg(_id);
						if (_is_child_approval && child_wo.parent_woprog_id > 1)
							// if it's a child approval.. make sure the subcontract gets to the parent
							{
							var parent_wo      = new NeWOProg(child_wo.parent_woprog_id);
							var wo_dc          = new NeWODetailCurrent();
							var parent_company = new NeBusinessUnit(parent_wo.business_unit_id);
							var child_company  = new NeBusinessUnit(child_wo.business_unit_id);
							var child_cust     = new NECustomer((int) child_wo.WOProg_Customer_ID);
							var parent_cust    = new NECustomer((int) parent_wo.WOProg_Customer_ID);
							//child_cust.sync_bv(parent_company.DSN, _current_user);
							//child_cust.sync_bv(child_company.DSN, _current_user);
							//parent_cust.sync_bv(parent_company.DSN, _current_user);
							//parent_cust.sync_bv(child_company.DSN, _current_user);

							var sales           = new NeSalesOrder();
							var inv             = new inventory();
							var dsn             = parent_company.GetBUDSN(Convert.ToInt32(child_wo.business_unit_id));

							wo_dc.added_by      = _current_user.id;
							wo_dc.date_added    = Toolbox.MySQLNow_long();
							wo_dc.date_modified = Toolbox.MySQLNow_long();
							wo_dc.description   = $"WO-{child_wo.OrderNumber}({child_company.name}) materials";
							wo_dc.master_id     = OpsSpecialPart.NewChildWO;
							if (child_wo.woprog_associate_woprog_id != 0)
								{
								wo_dc.description = $"WO-{child_wo.OrderNumber}({child_company.name}) progress bill on Q{child_wo.QuoteID}";
								wo_dc.cost = child_wo.woprog_StillToBeBilled;
								}
							else if (child_wo.QuoteID != "0") // if its a quote, grab the whole amount
								{
								wo_dc.description = $"WO-{child_wo.OrderNumber}({child_company.name}) Quote Q{child_wo.QuoteID}";
								wo_dc.cost = Convert.ToDouble(child_wo.QuotedPrice);
								}
							else // otherwise just grab the material
								{
								wo_dc.description = $"WO-{child_wo.OrderNumber}({child_company.name}) materials";
								wo_dc.cost = child_wo.woprog_materialtotalSell;
								}
							if(wo_dc.cost < 0)
								{
								wo_dc.cost			= Math.Abs(wo_dc.cost);
								wo_dc.qty_ordered   = -1;
								wo_dc.qty_committed = -1;
								wo_dc.qty_invoiced  = -1;
								}
							else
								{
								wo_dc.qty_ordered   = 1;
								wo_dc.qty_committed = 1;
								wo_dc.qty_invoiced  = 1;
								}
							wo_dc.sell					= wo.use_fixed_material_markup 
															? wo_dc.cost*wo.fixed_material_markup 
															: shared.GetSellPrice(wo_dc.cost, 0, false, 1, wo_dc.business_unit_id);
							wo_dc.unit					= wo.use_fixed_material_markup 
															? wo_dc.cost * wo.fixed_material_markup 
															: shared.GetSellPrice(wo_dc.cost, 0, false, 1, wo_dc.business_unit_id);
							wo_dc.billtypeid			= parent_wo.QuoteID == "0" ? 0 : 1;
							wo_dc.tax1					= parent_wo.woprog_tax1;
							wo_dc.tax2					= parent_wo.woprog_tax2;
							wo_dc.tax3					= parent_wo.woprog_tax3;
							wo_dc.tax4					= parent_wo.woprog_tax4;
							wo_dc.woprog_id				= parent_wo.woprog_id;
							wo_dc.bvwo					= Convert.ToInt32(parent_wo.OrderNumber);
							wo_dc.business_unit_id		= parent_wo.business_unit_id;
							wo_dc.type					= OpsWOLineType.Material;
							wo_dc.code					= OpsSpecialPart.NewChildWO.ToString();
							wo_dc.origin				= OpsWOLineOrigin.ChildWO;
							wo_dc.issues				= "";
							wo_dc.memberid				= _current_user.id;
							wo_dc.paytypeid				= 0;
							wo_dc.child_woprog_id		= child_wo.woprog_id;
							if (wo_dc.cost != 0)
								{
								wo_dc.save(_current_user, "/sections/reports/invoice/preview/frame.aspx", false);
								}


							NeWOProg.update_header_totals(child_wo.woprog_id.ToString(), child_wo.business_unit_id, child_wo.OrderNumber);
							NeWOProg.update_header_totals(parent_wo.woprog_id.ToString(), parent_wo.business_unit_id, parent_wo.OrderNumber);
							// send an email to the originating pm and bm that their work order has been approved by the parent BM
							}
						NeWOProg.move_status(new NeWOProg(_id), _current_user, "", OpsWOStatus.Enums.WaitingToBeInvoiced);
						}
					cbp.JSProperties["cpSUCCESS"] = true;
					cbp.JSProperties["cpCLOSE"] = true;
					}
				catch (Exception ee)
					{
					cbp.JSProperties["cpERROR"] = ee.Message;
					cbp.JSProperties["cpSUCCESS"] = false;
					cbp.JSProperties["cpCLOSE"] = false;
					}
				}
			else if (action == "pm_approve")
				{
				if (_approve_pm)
					{
					    try
					        {
					        NeWOProg.check_before_move(new NeWOProg(_id), OpsWOStatus.WaitingBMApproval);
					        }
					    catch (Exception check_move_error)
					        {
					        Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
					        return;
					        }
                    NeWOProg.move_status(new NeWOProg(_id), _current_user, "", OpsWOStatus.Enums.WaitingBMApproval);
					}
				cbp.JSProperties["cpSUCCESS"] = true;
				cbp.JSProperties["cpCLOSE"] = true;
				}
			else if (action == "usesignature")
				{
				if (paras[1] != "" && paras[2] != "" && paras[3] != "")
					{
					try 
						{
					// Detect multiple recipients - Paras[2] will be comma delimited
					var do_multiple_recipients	= paras[2].Contains(",");
					var contact_id = do_multiple_recipients ? 0 : Convert.ToInt32(paras[2]);
					var contact = do_multiple_recipients ? new NEContact() : new NEContact(contact_id);
					if(do_multiple_recipients)
						{
						var blank_names		= new List<string>();
						var bad_names		= new List<string>();
						foreach(var c_id in paras[2].Split(','))
							{
							int sub_contact_id;
							int.TryParse(c_id, out sub_contact_id);
							if(sub_contact_id == 0)
								{
								throw new Exception("There were errors with your submission, a contact's id is malformed - Stopping process");
								}
							contact		= new NEContact(sub_contact_id);
							if (contact.email.Trim() == "")
								{
								blank_names.Add(contact.name);
								}
							else if(!Toolbox.CheckEmail(contact.email))
								{
								bad_names.Add(contact.name);
								}
							}
						if(blank_names.Count > 0 || bad_names.Count > 0)
							{
							// No bueno... throw an exception.
							var ex		= new StringBuilder();
							if(blank_names.Count > 0)
								{
								ex.Append("These contacts have a blank email address on file:\n");
								foreach(var n in blank_names)
									{
									ex.AppendLine(n);
									}
								}
							if(bad_names.Count > 0)
								{
								ex.Append("These contacts have an invalid email address on file:\n");
								foreach(var n in blank_names)
									{
									ex.AppendLine(n);
									}
								}
							throw new Exception(ex.ToString());
							}
						}
					else if(contact_id > 0)
						{
						if (contact.email.Trim() == "")
							{
							throw new Exception("Contact doesn't have an email on file");
							}
						}
					var exists = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM signature WHERE `table` = 'woprog' AND table_id = @v0 ", new object[] {  _id } );
					if (exists > 0)
						{
						Toolbox.doSQL_void(@"DELETE FROM signature WHERE `table` = 'woprog' AND table_id = @v0 ", new object[] {  _id } );
						}
					//Save signature
					var s = new shared.signature
								{
									created_dt		= DateTime.Now,
									member_id		= (int) _current_user.id,
									printed_name	= paras[1],
									contact_id		= contact_id,
									send_to_contact_ids = paras[2],
									graphic			= Convert.FromBase64String(paras[3].Split(',')[1]),
									table			= "woprog",
									table_id		= _id
								};
					s.save();
					// Save signoff sheet to branches folder
					if (!string.IsNullOrEmpty(paras[4]) && paras[4] != "0" && s.table == "woprog")
						{
						wo = new NeWOProg(_id) {PONumber = paras[4]};
						wo.SaveWorkOrder();
						}
					cbp.JSProperties["cpDoUpdate"] = true;
					}
					catch (Exception ee)
						{
						if(ee.Message.Contains("email on file"))
							{
							throw new Exception("Please check that this contact has an email on file");
							}
						else
							{
							shared.alert_debug("Problem attaching signature to work order", string.Format("WO: {0}<br/>User: {1}<br/>Exception:<br/>{2}", _id, _current_user.FullName, ee));
							throw new Exception("There was an issue attaching the signature to the work order. Our support team has been notified.");
							}
						}
					}
				}
			else if (action == "updateworkorder")
				{
				// Update workorder status
				var sig = new shared.signature(_id, "woprog");
				var send_to_multiple_addresses		= sig.contact_id == 0 && sig.send_to_contact_ids.Contains(",");
				var blank_contact					= sig.contact_id == 0 && !sig.send_to_contact_ids.Contains(",");
				var email_address					= "";
				var cc_email_addresses				= _current_user.NEEmail;
				wo = new NeWOProg(_id);
				    try
				        {
				        NeWOProg.check_before_move(wo, OpsWOStatus.InitialPrep);
				        }
				    catch (Exception check_move_error)
				        {
				        Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
				        return;
				        }
                if (!blank_contact && !send_to_multiple_addresses)
					{
					var co = new NEContact(sig.contact_id);
					email_address = co.email;
					cc_email_addresses				= _current_user.id == wo.intProjectManager ? _current_user.NEEmail : cc_email_addresses+";"+new NeMember((int) wo.intProjectManager).NEEmail;
					}
				if(send_to_multiple_addresses)
					{
					foreach(var c_id in sig.send_to_contact_ids.Split(','))
						{
						int contact_id;
						int.TryParse(c_id, out contact_id);
						var contact		= new NEContact(contact_id);
						cc_email_addresses	+= contact.email+";";
						}
					email_address					= _current_user.id == wo.intProjectManager ? _current_user.NEEmail : new NeMember((int) wo.intProjectManager).NEEmail;
					}
				NeWOProg.move_status(wo, _current_user, "", OpsWOStatus.Enums.InitialPrep);
				// Send email
				var fileServer		= NeTaxEntity.BaseFolder(wo.business_unit_id, false);
				if(email_address != "")
					{
					var em = new NeEMail
								{
									To = email_address,
									From = _current_user.NEEmail,
									CC = cc_email_addresses,
									Subject = string.Format(new NeBusinessUnit(wo.business_unit_id).name + " WO# {0} - Packing Slip", wo.OrderNumber),
									Body = "Please find your packing slip attached.",
									Attachment = new System.Net.Mail.Attachment(string.Format(fileServer + @"\wos\{0}.pdf", wo.woprog_id)),
									isHTML = true
								};
					em.Send();
					}

				cbp.JSProperties["cpEmail"] = email_address;
				cbp.JSProperties["cpComplete"] = true;
				}
			else if (action == "save_comments")
				{
				Toolbox.doSQL_void(@"update woprog set woprog_description = @v0  where woprog_id = @v1 ", new object[] {  ts_comments.Text, hid_id.Value } );
				cbp.JSProperties["cpcomments_updated"] = true;
				}
			}
		}
	}