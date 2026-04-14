using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Compilation;
using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Shared;
using NESI.DTO.ViewModels.Page.PurchaseOrder;
using System.Linq;

namespace NESI.BLL.Pages.PurchaseOrder
{
    public class PurchaseOrderBase : BLLBase
    {

        public int[] selected_businessUnits { get; set; }
        public int[] openned_tax_entities { get; set; }
        public OrderCompanySummary[] visible_businessUnit { get; set; }
        protected NeMember myMember;

        public PurchaseOrderBase()
        {

        }

        public PurchaseOrderBase(Employee user) : base(user)
        {

        }

        public object Profile()
        {
            visible_businessUnit = GetCompanies();
            var profile = new NESI.BLL.Base.ProfileBase(CurrentUser);
            selected_businessUnits = profile.GetHomeLayout_selected_business_units("PurchaseOrder");
            openned_tax_entities = profile.GetHomeLayout_openned_tax_entities("PurchaseOrder");
            return this;
        }

        public DataTable GetBusinessUnitSummaryDetail(int bu_id)
        {
            var o = bllToolbox.doSQL_dt(@"CALL report_po_progress(@p0)", bu_id);
            return !CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault()
                ? (new DataView(o) { RowFilter = "nesi_cut_po = false" }).ToTable()
                : o;
        }


        public OrderCompanySummary[] GetCompanies(string buids = "")
        {
            var removed_nesi_cut_po = CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault() ? "" : " AND nesi_cut_po = false";

            return bllToolbox.doSQL_List<OrderCompanySummary>(string.Format(@"
SELECT 
	a.id value, 
	a.ddl_name label, 
	b.ddl_name AS tax_entity_name,
	b.id AS tax_entity_id,
	a.old_company_id,
	a.old_div,
" + (buids != "" ?
@" 
(SELECT COUNT(*) FROM poprog_header WHERE business_unit_id = a.id AND poprog_check_sent_date IS NULL AND poprog_status NOT IN (4,6,7,8) {0} ) total, 
	(
	SELECT 
		IFNULL(SUM(poprog_total_cost), 0)
	FROM 
		poprog_header 
	WHERE 
		business_unit_id = a.id AND 
		poprog_check_sent_date IS NULL 
	AND
		poprog_status NOT IN (4,6,7,8) {0} 
	) dollars,
			" : @"
			0 total,
			0 dollars,
			")
                + @"	
	0 orderby,
	b.dsn dsn,
	POPath path,
	a.gl_div
FROM
	business_unit AS a
INNER JOIN 
	tax_entity b on a.tax_entity_id = b.id
WHERE 
	(a.istest='F' or a.id = @v2) and 
	b.is_holdco = 0 and 
	FIND_IN_SET(a.id, @v0) AND
	b.id = @v1 AND
	b.public_name NOT LIKE 'Master %' 
UNION
SELECT 
	a.id value, 
	a.ddl_name label, 
	b.ddl_name AS tax_entity_name,
	b.id AS tax_entity_id,
	a.old_company_id,
	a.old_div,
" + (buids != "" ?
@" 
	(SELECT COUNT(*) FROM poprog_header WHERE business_unit_id = a.id AND poprog_check_sent_date IS NULL AND poprog_status NOT IN (4,6,7,8) {0} ) total, 
	(
	SELECT 
		IFNULL(SUM(poprog_total_cost), 0)
	FROM 
		poprog_header 
	WHERE 
		business_unit_id = a.id AND 
		poprog_check_sent_date IS NULL 
	AND
		poprog_status NOT IN (4,6,7,8) {0} 
	) dollars, 
			" : @"
			0 total,
			0 dollars,
			")
            + @"	
	1 orderby,
	b.dsn dsn,
	POPath path,
	a.gl_div
FROM
	business_unit AS a
INNER JOIN 
	tax_entity b on a.tax_entity_id = b.id
WHERE 
	(a.istest='F' or a.id = @v2) and 
	b.is_holdco = 0 and 
	FIND_IN_SET(a.id, @v0) AND
	b.id != @v1 AND
	b.public_name NOT LIKE 'Master %' 
ORDER BY 
	orderby, tax_entity_id, gl_div, label",
    removed_nesi_cut_po),
    (buids != "" ? buids : CurrentUser.VisibleBusinessUnits),
    CurrentUser.TaxEntityId, CurrentUser.BusinessUnitId).ToArray();
        }


        public BusinessUnitSummary[] GetSummaryByBusinessUnit(int[] bu_ids)
        {
            var removed_nesi_cut_po = CurrentUser.BusinessUnit.is_backoffice.GetValueOrDefault() ? "" : " AND nesi_cut_po = false";
            var companies = GetCompanies(string.Join(",", bu_ids));
            if (companies.Length == 0) { return null; }
            var list = new List<BusinessUnitSummary>();
            for (var i = 0; i < bu_ids.Length; i++)
            {
                var bu_id = bu_ids[i];
                var company = companies[i];

                var summary = new BusinessUnitSummary();
                var bu = BLL.Common.Cache.Global.BusinessUnit.GetValue(bu_id);
                var te = BLL.Common.Cache.Global.TaxEntity.GetValue(bu.tax_entity_id);
                summary.tax_entity_id = te.id;
                summary.businessUnit_id = bu_id;
                summary.businessUnit_name = bu.ddl_Name;
                summary.total = new CountValue() { name = "Total", count = company.total, value = company.dollars };
                summary.just_cut = new CountValue() { name = "Just Cut" };
                summary.waiting_approval = new CountValue() { name = "Waiting for Approval" };
                summary.be_issued = new CountValue() { name = "Waiting to be Issued" };
                summary.waiting_confirmation = new CountValue() { name = "Waiting for Confirmation" };
                summary.waiting_parts = new CountValue() { name = "Waiting for Parts" };
                summary.questions = new CountValue() { name = "Questions" };
                summary.ap_problems = new CountValue() { count = GetPO_NEEDED_Count(bu_id), name = "AP Problems" };
                var dtTotals = bllToolbox.doSQL_dt(
                    $@"SELECT COUNT(*) n, poprog_status po_status, SUM(poprog_total_cost) d FROM poprog_header WHERE business_unit_id = @v0 
AND poprog_check_sent_date IS NULL AND poprog_status NOT IN (4,6,7,8) {removed_nesi_cut_po}  GROUP BY po_status ", bu_id);
                foreach (DataRow _subdr in dtTotals.Rows)
                {
                    var po_status = _subdr["po_status"].ToString();
                    var _n = Convert.ToInt32(_subdr["n"]);
                    var _d = Convert.ToDouble(_subdr["d"]);
                    switch (po_status)
                    {
                        case "1":
                            summary.just_cut.count = _n;
                            summary.just_cut.value = _d;
                            break;
                        case "2":
                            summary.waiting_approval.count = _n;
                            summary.waiting_approval.value = _d;
                            break;
                        case "3":
                            summary.waiting_parts.count = _n;
                            summary.waiting_parts.value = _d;
                            break;
                        case "5":
                            summary.be_issued.count = _n;
                            summary.be_issued.value = _d;
                            break;
                        case "9":
                            summary.questions.count = _n;
                            summary.questions.value = _d;
                            break;
                        case "10":
                            summary.ap_problems.count += _n;
                            summary.ap_problems.value = _d;
                            break;
                        case "11":
                            summary.waiting_confirmation.count = _n;
                            summary.waiting_confirmation.value = _d;
                            break;
                    }
                }
                list.Add(summary);
            }
            return list.ToArray();
        }


        public int GetPO_NEEDED_Count(int buid)
        {
            try
            {
                NeBusinessUnit.CheckBUProcessFolderStructure(buid);
                var popath = bllToolbox.doSQL_string(@"SELECT POPath from business_unit  WHERE ID =@v0", buid);
                var dirJust = new DirectoryInfo(popath);
                var arrFiles = dirJust.GetFiles("PO_NEEDED*.pdf");
                return arrFiles.Length;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }


        public LabelValueInt[] APStatusList()
        {
            return bllToolbox.doSQL_List<LabelValueInt>("select apstatus_id value, apstatus_name label from apstatus").ToArray();
        }

        public string UpdateAPNotes(int poprog_id, string value)
        {
            bllToolbox.doSQL_void(@"Update poprog_header set poprog_hasproblem_notes=@p1 where poprog_id =@p0", poprog_id, value);
            return "success.";
        }

        public string UpdateStatus(int poprog_id, int value)
        {
            bllToolbox.doSQL_void(@"Update poprog_header set poprog_apstatus=@p1 where poprog_id =@p0", poprog_id, value);
            return "success.";
        }

        public DataTable CreditCard()

        {
            var can_purchase_other_branches = CurrentUser.AuthenticatedForPrivilege(187);
            var is_purchaser = CurrentUser.AuthenticatedForPrivilege(71);
            var is_admin = CurrentUser.AuthenticatedForPrivilege(175);
            //            var dt_users = Toolbox.doSQL_dt(string.Format(@"SELECT a.member_id id, CONCAT(b.ddl_name, ' - ', a.member_fullname) name 
            //FROM member a inner join business_unit b ON a.business_unit_id = b.id
            //and a.member_status='Active' and b.id in (" + new Current_User().visible_business_units + ") ORDER BY b.ddl_name, a.member_fullname"), null);

            var sub_users = Toolbox.doSQL_string(@"SELECT IFNULL(REPORTS_TO(" + CurrentUser.Id + "), '')");
            if (!is_admin && !is_purchaser && sub_users != "" && !can_purchase_other_branches)
            {
                //                dt_users = Toolbox.doSQL_dt(string.Format(@"SELECT a.member_id id, CONCAT(b.ddl_name, ' - ', a.member_fullname) name 
                //FROM member a inner join business_unit b ON a.business_unit_id = b.id 
                //WHERE (a.member_id IN ({0}) OR a.member_id = @v0 ) and b.id in (" + new Current_User().visible_business_units + @") 
                //ORDER BY  b.ddl_name, a.member_fullname", sub_users), new object[] { CurrentUser.Id });

                var query = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' - ', credit_cards.type,' - ',right(number,4)) _name from credit_cards 
inner join member on member.member_id = credit_cards.member_id 
inner join business_unit b ON member.business_unit_id = b.id
where  b.id in (" + new Current_User().visible_business_units + ") and credit_cards.member_id in(" + sub_users + "," + CurrentUser.Id + ") and credit_cards.status='Active' and member.member_status='Active' ORDER BY b.ddl_name, member.member_fullname", null);
                return query;
            }

            else if (is_admin || can_purchase_other_branches)
            {
                var query = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' - ', credit_cards.type,' - ',right(number,4)) _name 
from credit_cards 
inner join member on member.member_id = credit_cards.member_id 
inner join business_unit b on b.id = member.business_unit_id 
where member.member_status='Active' and credit_cards.status='Active' and b.id in (" + new Current_User().visible_business_units + ") ORDER BY b.ddl_name, member.member_fullname ", null);
                return query;
            }
            else if (is_purchaser)
            {
                //                dt_users = Toolbox.doSQL_dt(@"SELECT a.member_id id, CONCAT(b.name, ' - ', a.member_fullname) name 
                //FROM member a 
                //inner join business_unit b ON a.business_unit_id = b.id 
                //WHERE a.business_unit_id = @v0  ORDER BY b.ddl_name, a.member_fullname", new object[] { CurrentUser.BusinessUnit.ID });
                var query = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' - ', credit_cards.type,' - ',right(number,4)) _name 
from credit_cards 
inner join member on member.member_id = credit_cards.member_id  
where member.member_status='Active' and credit_cards.status='Active' and find_in_set(member.business_unit_id, @v0) ORDER BY  member.member_fullname", new object[] { CurrentUser.VisibleBusinessUnits });
                return query;

            }
            else if (!can_purchase_other_branches)
            {
                var query = Toolbox.doSQL_dt(@"Select credit_cards.id, concat(member_fullname,' - ', credit_cards.type,' - ',right(number,4)) _name 
from credit_cards 
inner join member on member.member_id = credit_cards.member_id  
where find_in_set(credit_cards.member_id,@v0 ) and credit_cards.status='Active' and member.member_status='Active' ORDER BY  member.member_fullname", new object[] { CurrentUser.Id });
                return query;
            }
            else
            {
                return null;
            }
        }

        public List<LabelValueInt> GetCreditCardList()
        {
            List<LabelValueInt> list = new List<LabelValueInt> { };
            var dt = CreditCard();
            foreach (DataRow row in dt.Rows)
            {
                var value = Convert.ToInt32(row["id"]);
                var label = Convert.ToString(row["_name"]);

                var item = new LabelValueInt { Label = label, Value = value };

                list.Add(item);
            }

            return list;

        }

        class returnReuslt {


            public int id { get; set; }

            // addd more below

        }

        public string SaveCreditCard(int Id, Employee employee, AddCreditCard model)
        {
            NECredit_card_purchase exp = new NECredit_card_purchase();
            var _isEdit = exp.id > 0;
            var posted_file = Path.Combine(model.file_path, model.file_name);
            

            if (model.has_file && !string.IsNullOrEmpty(posted_file))
            {
                exp.has_file = true;

                exp.file_ext = Path.GetExtension(posted_file).Replace(".", "").ToLower();

                exp.file_mime = NeFiles.GetMimeType(exp.file_ext);
            }

            exp.member_id = model.member_id;
            // (From Matt) - Date purchase is defaulting to 4am... not sure why, but shouldn't matter... so setting it manually
            exp.date_purchased = model.date_purchased.Date;
            exp.date_requested = DateTime.Now;

            if (exp.seller_id == 0)
            {
                exp.seller_id = exp.new_seller(model.seller_id, CurrentUser.Id);
            }
            else
            {
                exp.seller_id = GetCreditCardSellerId(model.seller_id);
            }
            
            exp.woprog_id = model.woprog_id;
            exp.amount = model.amount;
            exp.currency = model.currency.ToString();
            exp.credit_card_id = model.credit_card_id;
            exp.master_id = model.master_id;
            exp.expense_category_id = model.expense_category_id;
            exp.item_text = model.item_text;
            exp.has_file = model.has_file;
            exp.woprog_id = int.TryParse(model.Wo_number, out int woId) ? woId : 0;
            exp.business_unit_id = CurrentUser.BusinessUnit.ID;
            exp.save();

            if (model.has_file && !string.IsNullOrEmpty(posted_file))
            {
                try
                {
                    var attachmentPath = Path.Combine(new BLL.Core.FileManager.CreditCardFile(employee).BasePath, exp.id.ToString() + "." + exp.file_ext);
                    System.IO.File.Move(posted_file, attachmentPath);
                    System.IO.File.Delete(posted_file);
                    // if directory is empty delete directory.
                    if (!Directory.EnumerateFileSystemEntries(model.file_path).Any())
                    {
                        Directory.Delete(model.file_path);
                    }
                  
                }
                catch (Exception ee)
                {
                    
                    exp.delete();
                    // Rewind the work order.
                   
       
                }
            }

            return "Credit card purchase submitted successfully";

        }

        private int GetCreditCardSellerId(string name)
        {
            
                const string sql = @"SELECT id_seller FROM expense_seller WHERE id_seller != 309 and name_seller =@p0 GROUP BY name_seller ORDER BY name_seller";

                return bllToolbox.doSQL_int(sql, name);
            
        }
    }
}

