using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.WorkOrder;
using System.Text.RegularExpressions;
using NESI.Common.Models;

namespace NESI.BLL.Pages.WorkOrder
{
	public class WorkOrderBuckets : BLLBase
	{

		private readonly string[] IMAGES = { ".jpg", ".png", ".gif", ".bmp", ".tiff" };

		public int business_unit_id { get; set; }
		public int pm_id { get; set; }
		public string cust_id { get; set; }
		public string order_by { get; set; }
		public bool can_viewtotaltime { get; set; }
		public bool can_approval_bm { get; set; }


		public WorkOrderBuckets(Employee user, int buid) : base(user)
		{
			business_unit_id = buid == 0 ? CurrentUser.BusinessUnitId : buid;
			can_viewtotaltime = CurrentUser.AuthenticatedForPrivilege(60);
			can_approval_bm = CurrentUser.AuthenticatedForPrivilege(16);
		}

		public WorkOrderBuckets(Employee user, int buid, int pmid, string orderby) : base(user)
		{
			business_unit_id = buid == 0 ? CurrentUser.BusinessUnitId : buid;
			can_viewtotaltime = CurrentUser.AuthenticatedForPrivilege(60);
			can_approval_bm = CurrentUser.AuthenticatedForPrivilege(16);

			pm_id = pmid;
			order_by = orderby;
			cust_id = "%";

		}

		public DataExtra UploadFiles(DataFiles model)
		{
			var _working_business_unit = new NeBusinessUnit(business_unit_id);
			try
			{
				NeBusinessUnit.CheckBUProcessFolderStructure(business_unit_id);
			}
			catch (Exception)
			{
				//
			}
			var images = new List<string>();
			var file_count = 0;
			foreach (var file in model.files)
            {
                string originalName = Path.GetFileNameWithoutExtension(file);
                string originalExtension = Path.GetExtension(file);
                string specialchar = Regex.Replace(originalName, @"[^0-9a-zA-Z_\s]+", "");
                string modifiedFileName = Regex.Replace(specialchar, @"(?m:^ +| +$|( ){2,})", " ").Replace(" ", "_");

                var fromName = Path.Combine(model.fullpath, file);
                var info = new FileInfo(fromName);
                var ext = info.Extension.ToLower();
                var toName = Path.Combine(_working_business_unit.WOPath, modifiedFileName + ext);



                if (!Directory.Exists(_working_business_unit.WOPath)) // to verify target folder exist or not
                {
                    return new DataExtra("Please check that the target path exists.");
                }
                else if (File.Exists(toName)) // to verify this file is exist or not
                {
                    return new DataExtra("This file already exists.");
                }




                if (ext == ".pdf")
				{
					File.Copy(fromName, toName);
					file_count++;
				}

				if (IMAGES.Contains(ext))
				{
					images.Add(fromName);
				}
			}

			if (images.Count > 0)
			{
				var pdffile = Path.Combine(_working_business_unit.WOPath, "doc" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
				Core.FileManager.ImageToPdf.GeneratePDF(pdffile, images.ToArray());
				file_count++;
			}

			return new DataExtra((file_count > 1 ? "Files have" : "File has") + " been uploaded successfully.", GetScannedFiles());
		}


        public DTO.ViewModels.Page.WorkOrder.WorOrderBucketScanned[] GetScannedFiles()
        {
            var _working_business_unit = new NeBusinessUnit(business_unit_id);
            var list = new List<WorOrderBucketScanned>();
            if (string.IsNullOrEmpty(_working_business_unit.WOPath))
            {
                //return null;
                return list.ToArray();
            }
            try
            {
                NeBusinessUnit.CheckBUProcessFolderStructure(business_unit_id);

                if (!string.IsNullOrEmpty(_working_business_unit.WOPath))
                {

                    var baseServer = new Uri(_working_business_unit.WOPath).Host;

                    var ispath = Directory.Exists(_working_business_unit.WOPath);

                    if (NeBusinessUnit.CheckURLPath(baseServer) || ispath)
                    {

                        var dir_just = new DirectoryInfo(_working_business_unit.WOPath);


                        var arr_files = dir_just.GetFiles();

                        foreach (var t in arr_files)
                        {
                            if (!t.Extension.Contains("pdf") && !t.Extension.Contains("PDF")) continue;
                            var item = new WorOrderBucketScanned
                            {
                                file_name = t.Name,
                                size = Convert.ToDouble(t.Length / 1000),
                                created_time = t.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                last_access_time = t.LastAccessTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                business_unit_id = business_unit_id,
                            };
                            list.Add(item);
                        }
                    }

                    //return list.ToArray();
                }

                return list.ToArray();
            }

            catch (Exception)
            {
                return null;
            }
        
		}

		protected WorkOrderBucket[] GetBMApproval(string orderby)
		{
			var sql = $@"
SELECT 
	a.*, 
	a.woprog_pm_memberid pm_id,
	MEMBER_NAME(woprog_pm_memberid) pm_name,
	IFNULL(DATEDIFF(CURDATE(),woprog_opendatetime), 0) days_since,
	c.ddl_name 
FROM 
	woprog a 
INNER JOIN 
	business_unit c ON a.business_unit_id = c.id 
WHERE 
	a.business_unit_id ={business_unit_id}  and woprog_closedatetime IS NULL AND woprog_status = 'Waiting BM Approval'
 and woprog_customer_id like '{cust_id}'
union
SELECT
	wo_child.*, 
    wo_child.woprog_pm_memberid pm_id,
	MEMBER_NAME (wo_child.woprog_pm_memberid) pm_name,
	IFNULL(DATEDIFF(CURDATE(),wo_child.woprog_opendatetime),0) days_since,
	c.ddl_name 
FROM
	woprog wo_child 
inner join 
	woprog wo_parent on wo_child.parent_woprog_id = wo_parent.WOProg_ID 
INNER JOIN 
	business_unit c ON wo_child.business_unit_id = c.id 
WHERE
	wo_parent.business_unit_id ={business_unit_id} and wo_child.woprog_status = 'Waiting Parent BM Approval'
	
UNION
	SELECT
	wo_child.*, 
    wo_child.woprog_pm_memberid pm_id,
	MEMBER_NAME (wo_child.woprog_pm_memberid) pm_name,
	IFNULL(DATEDIFF(CURDATE(),wo_child.woprog_opendatetime),0) days_since,
	c.ddl_name
FROM
	woprog wo_child 
LEFT JOIN
	internal_companyno int_co ON wo_child.woprog_customer_id = int_co.internal_companyno_intranet_custid 
LEFT JOIN 
	business_unit c ON wo_child.business_unit_id = c.id
WHERE 
	wo_child.parent_woprog_id = 1 AND
	int_co.business_unit_id = {business_unit_id} AND 
	wo_child.woprog_status = 'Questions For PM'
ORDER BY {orderby}, WOProg_id
";
			var list = bllToolbox.doSQL_Array<WorkOrderBucket>(sql);
			return GetUpdateStatus(list);
		}


		protected WorkOrderBucket[] GetUpdateStatus(WorkOrderBucket[] list)
		{
			foreach (var item in list)
			{
				var temp_dt = bllToolbox.doSQL_dt(@"
SELECT 
	datediff(curdate(),woprogstatus_datetime) dt, 
	member_name(woprogstatus_member_id) name 
FROM 
	woprogstatus 
WHERE 
	woprogstatus_woprog_id = @v0 AND 
	woprogstatus_status = (SELECT woprog_status FROM woprog WHERE woprog_id = @v0) ORDER BY woprogstatus_datetime DESC LIMIT 1",
					item.woprog_id);
				if (temp_dt.Rows.Count <= 0) continue;
				var laststatus = temp_dt.Rows[0];	
                item.last_updated_by = laststatus["name"] == null ? "--" : laststatus["name"].ToString();
                item.last_updated_dt = Toolbox.ReturnZeroIfNull_int(laststatus["dt"]).ToString();
            }
			return list;
		}


		public object Profile()
		{
			var pmList = GetPMList();
			var selected_pm = 0;
			var business_unit_name = BLL.Common.Cache.Global.BusinessUnit.GetValue(business_unit_id).Name;
			if (pmList.FirstOrDefault(x => x.Value == UserId) != null)
			{
				selected_pm = UserId;
			}
			var sortByList = new[]
			{
				new LabelValueString("Scanned Date","WOProg_OpenDateTime"),
				new LabelValueString("Customer Name","WOProg_CustomerName"),
				new LabelValueString("Work Order Number","WOProg_BVWO")
			};

			var sql = $@"
SELECT
	a.woprog_id,
	a.woprog_associate_woprog_id,
	a.woprog_pm_memberid,
	IF(a.invoice_fb_issues=1 and a.woprog_status='Waiting To Be Invoiced', 'Invoice Feedback Issues', a.woprog_status) AS woprog_status,
	a.woprog_bvwo,
	a.woprog_bvwo,
	a.woprog_custpo,
	a.woprog_customername,
	a.woprog_description,
	a.woprog_stilltobebilled,
	a.woprog_totaltandm,
	a.woprog_hold,
	a.woprog_creditcard_payment,
	a.woprog_quoteid,
	a.parent_woprog_id,
	a.business_unit_id,
	c.ddl_name,
	b.member_fullname pm_name,
	IFNULL(DATEDIFF(CURDATE(),a.woprog_opendatetime),0) days_since
FROM
	woprog a 
INNER JOIN 
	member b on b.member_id = a.woprog_pm_memberid 
INNER JOIN 
	business_unit c ON a.business_unit_id = c.id
WHERE
	a.business_unit_id = {business_unit_id} AND 
	a.woprog_pm_memberid = {(pm_id > 0 ? pm_id.ToString() : "woprog_pm_memberid")} AND 
	a.woprog_closedatetime IS NULL AND 
	a.woprog_status NOT IN ('{OpsWOStatus.Open}', '{OpsWOStatus.Deleted}', '{OpsWOStatus.Invoiced}') AND
	a.woprog_customer_id LIKE '{cust_id}' 
ORDER BY 
	{order_by}, woprog_id DESC";

			var list = bllToolbox.doSQL_Array<WorkOrderBucket>(sql);
			foreach (var wo in list)
			{
				wo.po_list = bllToolbox.doSQL_Array<WorkOrderPo>(@"
SELECT 
	b.poprog_id id, 
	b.poprog_bvpo bvpo ,
	c.vendor_name 
FROM
	po_details_current a
LEFT JOIN 
	poprog_header b 
		ON a.po_details_poprog_id = b.poprog_id
LEFT JOIN
	vendor c
		ON b.poprog_vendor_id = c.vendor_id
WHERE 
	b.poprog_status IN (1,2,5,3,9) AND 
	a.po_details_woprog_id =@v0 AND 
	a.po_details_line_active = 1 and
    a.is_gl_account=false
GROUP BY
	id
", wo.woprog_id);

				var c = bllToolbox.doSQL_int(@"
SELECT 
	COUNT(woprogstatus_id)  
FROM 
	woprogstatus 
WHERE 
	woprogstatus_woprog_id = @v0 AND 
	woprogstatus_status = (SELECT woprog_status FROM woprog WHERE woprog_id = @v0)", wo.woprog_id);
				if (c > 0)
				{
					var laststatus = bllToolbox.doSQL_dt(@"
SELECT 
	datediff(curdate(),woprogstatus_datetime) dt, 
	member_name(woprogstatus_member_id) name 
FROM 
	woprogstatus 
WHERE 
	woprogstatus_woprog_id = @v0 AND 
	woprogstatus_status = (SELECT woprog_status FROM woprog WHERE woprog_id =@v0) ORDER BY woprogstatus_datetime DESC LIMIT 1",
						wo.woprog_id).Rows[0];
                    wo.last_updated_by = laststatus["name"] == null ? "--" : laststatus["name"].ToString();
                    wo.last_updated_dt = Toolbox.ReturnZeroIfNull_int(laststatus["dt"]).ToString();

                }

			}

			return new
			{
				business_unit_name,
				pmList,
				sortByList,
				selected_pm,
				scanned_list = GetScannedFiles(),
				bm_approval_list = GetBMApproval(order_by).Where(x => pm_id == 0 || x.pm_id == this.pm_id).ToArray(),
				open_list = list
			};
		}

		protected LabelValueInt[] GetPMList()
		{
			return bllToolbox.doSQL_Array<LabelValueInt>(
				$@"
SELECT 
	0 value,
	'All Project Managers' label
UNION ALL
	(
SELECT 
	b.member_id value,
	b.member_fullname label
FROM 
	woprog a
LEFT JOIN 
	member b on a.woprog_pm_memberid = b.member_id
WHERE 
	woprog_status NOT IN ('', 'Invoiced', 'Open', 'Deleted') AND 
	woprog_pm_memberid IS NOT NULL {(business_unit_id > 0 ? " AND  a.business_unit_id =@p0" : "")}
GROUP BY 
	b.member_id
ORDER BY 
	label)
", business_unit_id);
		}
	}
}