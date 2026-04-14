#region

using System;
using System.Data;
using System.IO;
using System.Web.UI;

using MySql.Data.MySqlClient;
using NESI.Common.Models;
using nesi.core;

#endregion

public partial class WoProgRename : Page
	{
	private NeMember _currentUser;
	private int _woprogIdQuerystring;

	protected void Page_Init(object sender, EventArgs e)
		{
		_currentUser = Toolbox.do_handle_authentication(12);
		}

	protected void Page_Load(object sender, EventArgs e)
		{
		using (var conn = Toolbox.connect())
			{
			int.TryParse(Request.QueryString["woprog_id"], out _woprogIdQuerystring);
			if (_woprogIdQuerystring == 0)
				{
				hidCompanyID.Value = Request.QueryString["business_unit_id"];
				hidScanPath.Value = Request.QueryString["scanfile"];
				panName.Visible = true;                
				divCoName.InnerText = "Business Unit: " + Toolbox.doSQL_string(conn,@"SELECT ddl_name from business_unit  WHERE id =@v0", new object[] { hidCompanyID.Value });
				}
			else
				{
				var dtWoprog = Toolbox.doSQL_dt(conn,@" SELECT woprog_id, woprog_bvwo, business_unit_id, dsn, name, pathtimesheet, (SELECT woprogstatus_status FROM woprogstatus WHERE woprogstatus_woprog_id = woprog_id ORDER BY woprogstatus_id DESC LIMIT 0,1) thestatus FROM woprog a, business_unit b, tax_entity c WHERE a.woprog_id=@v0  AND a.business_unit_id=b.id AND b.tax_entity_id = c.id", new object[] {  _woprogIdQuerystring } );

				if (dtWoprog.Rows.Count != 1)
					{
					throw new Exception("Invalid work order ID supplied.");
					}
				var drWoProg = dtWoprog.Rows[0];
				hidCompanyID.Value = drWoProg["business_unit_id"].ToString();
				panName.Visible = true;
				divCoName.InnerText = "Business Unit: " + Toolbox.doSQL_string(conn,@"SELECT ddl_name from business_unit  WHERE ID =@v0", new object[] { drWoProg["business_unit_id"] });
				}

			int.TryParse(hidCompanyID.Value, out var businessUnitId);
			if(!string.IsNullOrEmpty(hidScanPath.Value))
				{
				pdfDocument.Src = NeTaxEntity.BaseFolder(businessUnitId, true)+"/WOs/"+hidScanPath.Value;
				}
			else
				{
				pdfDocument.Src = NeTaxEntity.BaseFolder(businessUnitId, true)+"/WOs/"+_woprogIdQuerystring+".pdf";
				}
			if (!IsPostBack)
				{
				var dt = Toolbox.doSQL_dt(conn,@"SELECT woprog_bvwo wo, CONCAT(woprog_bvwo,' - ', woprog_customername) customer FROM woprog WHERE woprog_closedatetime IS NULL AND woprog_status='Open' AND woprog_bvwo != 'Not Entered' AND woprog_hold = 0 AND business_unit_id=@v0  ORDER BY WOProg_CustomerName, WOProg_BVWO", new object[] {  hidCompanyID.Value } );
				ddlWOrename.DataSource = dt;
				ddlWOrename.DataValueField = "wo";
				ddlWOrename.DataTextField = "customer";
				ddlWOrename.DataBind();
				}
            CheckIfScanExists();

            divButtons.InnerHtml = string.Format("</br><a href='#' onclick='navigateToAngularWorld({0}); return false;' >Back</a>", hidCompanyID.Value);
			}
		}
    protected void CheckIfScanExists()
    {
        using (var conn = Toolbox.connect())
        {
            var bvWoNumber = ddlWOrename.SelectedValue.Trim();
            var businessUnitId = Convert.ToInt32(hidCompanyID.Value);
            var businessUnit = new NeBusinessUnit(businessUnitId);
           // string woPath = businessUnit.WOPath;
            var fileServer = NeTaxEntity.BaseFolder(businessUnit.id, false);
            var woprogIdSelected = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(woprog_id), 0) FROM woprog WHERE woprog_bvwo =@v0  AND woprog_status='Open' AND business_unit_id=@v1 ", new object[] { bvWoNumber, businessUnitId });
            var fileName = woprogIdSelected + ".pdf";
            var fileExists = File.Exists(fileServer + @"\WOs\" + fileName);
            if (fileExists)
            {
                btnReplaceScan.Visible = true;
                btnWOrename.Visible = false;
                lblWarning.Text = "The selected work order already has a scan attached, which will be lost on replacing";
                lblWarning.Visible = true;
            }
            else
            {
                btnReplaceScan.Visible = false;
                btnWOrename.Visible = true;
                lblWarning.Text = "";
                lblWarning.Visible = false;
            }
           
        }
    }
    protected void OnWoSelectionChanged(object sender, EventArgs e)
    {
        if (ddlWOrename.SelectedIndex < 0)
        {
            return;
        }
        CheckIfScanExists();
    }
    protected void btnReplaceScan_Click(object sender, EventArgs e)
    {
        // overriding here as the query string changes 
        scanAttachment(NeWOProg.MoveScanType.Replace);

    }
    protected void scanAttachment(NeWOProg.MoveScanType moveScanType)
    {
        using (var conn = Toolbox.connect())
        {
            var bvWoNumber = ddlWOrename.SelectedValue.Trim();
            var businessUnitId = Convert.ToInt32(hidCompanyID.Value);
            var businessUnit = new NeBusinessUnit(businessUnitId);
            var woPath = businessUnit.WOPath;
            var fileServer = NeTaxEntity.BaseFolder(businessUnit.id, false);
            hidScanPath.Value = Request.QueryString["scanfile"]; // overriding here as the query string changes 
            if (ddlWOrename.SelectedIndex < 0 || businessUnitId == 0)
            {
                return;
            }

            try
            {
                var woprogIdSelected = Toolbox.doSQL_int(conn, @"SELECT IFNULL(MAX(woprog_id), 0) FROM woprog WHERE woprog_bvwo =@v0  AND woprog_status='Open' AND business_unit_id=@v1 ", new object[] { bvWoNumber, businessUnitId });
                if ((woprogIdSelected > 0) && (hidScanPath.Value != ""))
                {
                    var woSelected = new NeWOProg(woprogIdSelected);
                    try
                    {
                        NeWOProg.check_before_move(woSelected, OpsWOStatus.InitialPrep);
                    }
                    catch (Exception check_move_error)
                    {

                        Toolbox.FriendlyException(this.Response, check_move_error.Message, this.Page.Request.Url.AbsoluteUri);
                    }
                    var fileName = woprogIdSelected + ".pdf";
                    var fileExists = File.Exists(fileServer + @"\WOs\" + fileName);

                    var scanfile = hidScanPath.Value.Replace("&", "_");
                    //New Work Order is Being Associated with a Scan
                  //  if (_woprogIdQuerystring == 0)
                   // {
                        NeWOProg.move_scans(fileServer, woPath, scanfile, fileName, moveScanType);
                  //  }
                
                    woSelected = new NeWOProg(woprogIdSelected);

                    foreach (DataRow drChildWos in NeWOProg.get_child_workorders(woprogIdSelected).Rows)
                    {
                        var status = drChildWos["woprog_status"].ToString();
                        var childWoprogId = Convert.ToInt32(drChildWos["woprog_id"]);
                        if (status == "Invoiced" || status == OpsWOStatus.WaitingToBeInvoiced || status == "Deleted")
                        {
                            continue;
                        }
                        var childWo = new NeWOProg(childWoprogId);
                        var childBusinessUnit = new NeBusinessUnit(woSelected.business_unit_id);
                        var emailChildPm = new NeEMail
                        {
                            To = new NeMember(childWo.intProjectManager).NEEmail,
                            CC = childBusinessUnit.branch_manager.NEEmail,
                            Subject = string.Format("WO {0} from {1} is being invoiced.. your WO {2} needs to be finalized ASAP.", woSelected.OrderNumber, childBusinessUnit.name, childWo.OrderNumber),
                            From = "noreply@" + Toolbox.app_setting("DomainForEmail")
                        };
                        emailChildPm.Send();
                    }

                    // Move the selected WO to initial prep
                    NeWOProg.move_status(woSelected, _currentUser, "", OpsWOStatus.Enums.InitialPrep);

                    //Set old work order back to open and delete related records.
                    if (_woprogIdQuerystring > 0)
                    {
                        Toolbox.doSQL_void(conn, @"UPDATE woprog SET woprog_status = 'Open'  WHERE woprog_id =@v0", new object[] { _woprogIdQuerystring });
                        Toolbox.doSQL_void(conn, @"DELETE FROM woprogstatus  WHERE woprogstatus_woprog_id =@v0", new object[] { _woprogIdQuerystring });
                        Toolbox.doSQL_void(conn, @"DELETE FROM woprogcomment  WHERE woprogcomment_woprog_id =@v0", new object[] { _woprogIdQuerystring });
                    }

                    ScriptManager.RegisterStartupScript(this, GetType(), "open_", string.Format("navigateToAngularWorld({0})", businessUnitId), true);
                    //		ScriptManager.RegisterStartupScript(this, GetType(), "open_", string.Format("parent.location.href='wo_prog_frame.aspx?action=show&woprog_id={0}&business_unit_id={1}'", woprogIdSelected, businessUnitId), true);
                }
            }
            catch (Exception ex)
            {
                Toolbox.do_errorLog_errorStack(ex);
                throw;
            }
        }
    }
    protected void btnWOrename_Click(object sender, EventArgs e)
		{
        scanAttachment(NeWOProg.MoveScanType.New);
		}

	protected void btnDeleteScan_Click(object sender, EventArgs e)
		{
		pdfDocument.Visible = false;
		var error = "";
		var businessUnitId = Convert.ToInt32(hidCompanyID.Value);
		if (_woprogIdQuerystring == 0)
			{
			try
				{
				var businessUnit = new NeBusinessUnit(businessUnitId);
				var path = businessUnit.WOPath;
				var scanFile = hidScanPath.Value.Replace("&", "_");
				var fileServer				= NeTaxEntity.BaseFolder(businessUnit.id, false);
				File.Delete(path + scanFile);
				File.Delete($@"{fileServer}\WOs\{scanFile}");
				}
			catch (Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				error = ee.ToString();
				}
			}
		else
			{
			try
				{
				var wo = new NeWOProg(_woprogIdQuerystring);
				var businessUnit = new NeBusinessUnit(wo.business_unit_id);
				var fileName = wo.woprog_id + ".pdf";
				var fileServer				= NeTaxEntity.BaseFolder(businessUnit.id, false);
				var pathWo = $"{fileServer}\\WOs\\{fileName}";
				var sig = new shared.signature(wo.woprog_id, "woprog");
				if (sig.id > 0)
					{
					sig.delete();
					}
				NeWOProg.reopen_workorder(wo);
				if (File.Exists(pathWo))
					{
					File.Delete(pathWo);
					}

				// panName.Visible = false;
				ddlWOrename.Visible = false;
				btnWOrename.Visible = false;
				btnDeleteScan.Visible = false;
				selectinfo.Visible = false;
				divCoName.Visible = false;
				}
			catch(Exception ee)
				{
				Toolbox.do_errorLog_errorStack(ee);
				error = ee.ToString();
				}

            }
			lblMessage.Visible = true;
			lblMessage.Text = error != "" ? error : "";
			if(error == "")
				{ 
				ScriptManager.RegisterStartupScript(this, GetType(), "open_", $"alert('Scan has been deleted');navigateToAngularWorld({businessUnitId});", true);
				}
		}
	}
