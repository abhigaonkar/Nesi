using nesi.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nesi.Web.sections.workorder
{
    public partial class comments_fixup : System.Web.UI.Page
    {
        private const int page_id = 1; // from Page table in DB
        NeMember _currentUser = new NeMember();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            var wo = LoadData();

            if (wo != null)
            {
                var content = "";
                var commentRecordList = LoadCommentRecords(wo.woprog_id, out content);
                this.workCommentIdList.Value = this.GetCommentIDs(commentRecordList);
                this.HiddenFieldOrderNumber.Value = wo.OrderNumber;
                this.HiddenFieldBuinessUnit.Value = wo.business_unit_id.ToString();
                RenderPage(wo.woprog_id, commentRecordList, content);
            }
        }

        protected void btnUpdateTS_Click(object sender, EventArgs e)
        {
            //
            // Get workorder id, content.
            //
            var workID = this.workOrderID.Value;
            var content = this.txtComments.Text;
            var workorderID = Convert.ToInt32(workID);
            var idList = this.workCommentIdList.Value;
            string ordernumber = this.HiddenFieldOrderNumber.Value;
            string buID = this.HiddenFieldBuinessUnit.Value;
            if (string.IsNullOrEmpty(idList) || string.IsNullOrEmpty(ordernumber) || string.IsNullOrEmpty(buID))
            {
                return;
            }

            if (workorderID <= 0)
            {
                return;
            }

            //
            // Do the update
            //
            CommentRecordFixup(workorderID, content, idList, ordernumber, buID);

            //
            // Rerender
            //
            var content2 = "";
            var commentRecordList = LoadCommentRecords(workorderID, out content2);
            RenderPage(workorderID, commentRecordList, content2);
        }

        private void RenderPage(int woId, List<CommentRecord> list, string content)
        {
            //
            // Bind it.
            //
            this.gdComments.DataSource = list;
            this.gdComments.DataBind();

            //
            // Check the health of comments records.
            //
            if (list.Count > 1)
            {
                ImageWarning.Visible = true;
                ImageOkay.Visible = false;
                lblWarningInfo.Text = "Found more than ONE comment records. Please review them.";
                btnUpdateTS.Enabled = true;
                this.txtComments.Text = content;
                this.txtComments.Enabled = true;
                this.lblFix.Text = "You can edit then save below content to fix above problem.";
            }
            else
            {
                ImageWarning.Visible = false;
                ImageOkay.Visible = true;
                lblWarningInfo.Text = "Timesheet Comments on Invoice Looks good now.";
                btnUpdateTS.Enabled = false;
                this.txtComments.Text = "";
                this.txtComments.Enabled = false;
                this.txtComments.Visible = false;
                this.lblFix.Text = "";
            }
        }

        private void CommentRecordFixup(int workID, string content, string commentIDList, string ordernumber, string buID)
        {
            //
            // SOFT delete by setting woprog_id to NULL.
            //
            var sql = @"
DELETE FROM wocomment
WHERE woprog_id = {0} AND wocomment_member_id = 0 AND WoComment_ID IN ({1})";

            var sqlToRun = string.Format(sql, workID, commentIDList);
            Toolbox.doSQL_void(sqlToRun);

            //
            // Added the new one share record. 
            //

            Toolbox.doSQL_void(@"
INSERT INTO wocomment 
	(
	workorder_id, 
	comments, 
	personal, 
	printcomments, 
	member_id_audit,
	created_date, 
	modified_date, 
	wocomment_member_id, 
	business_unit_id,
	woprog_id
	) 
VALUES 
	(
	@v0 , 
	@v1 ,
	0,
	@v2 , 
	0 , 
	NOW(),
	NOW(), 
	0, 
	@v3 ,
	@v4 
	)",

    new object[] { ordernumber, content, "1", buID, workID }
    );

        }

        private List<CommentRecord> LoadCommentRecords(int woId, out string content)
        {
            List<CommentRecord> data = new List<CommentRecord>() { };
            var seperator = "************************ ({0}) - ({1}) ************************\r\n";
            content = "";

            var dt = Toolbox.doSQL_dt(@"SELECT * FROM wocomment WHERE woprog_id = @v0 AND wocomment_member_id = 0 ORDER BY WoComment_ID DESC ", new object[] { woId });
            var count = dt.Rows.Count;
            foreach (DataRow row in dt.Rows)
            {
                var WoComment_ID = int.Parse(row["WoComment_ID"].ToString());
                var comments = row["comments"].ToString();
                var PrintComments = ((byte[])(row["PrintComments"]))[0] == 49 ? true: false;
                var Created_Date = Convert.ToDateTime(row["Created_Date"]);
                var Modified_Date = Convert.ToDateTime(row["Modified_Date"]);

                CommentRecord w = new CommentRecord
                {
                    WoComment_ID = WoComment_ID,
                    comments = comments,
                    PrintComments = PrintComments,
                    Created_Date = Created_Date,
                    Modified_Date = Modified_Date,
                };
                data.Add(w);

                if (count > 1)
                {
                    // Put all records together for users to edit it.
                    if (!string.IsNullOrWhiteSpace(comments))
                    {
                        var start = string.Format(seperator, WoComment_ID, Modified_Date);
                        if (string.IsNullOrEmpty(content))
                        {
                            content += start + comments;
                        }
                        else
                        {
                            content += "\r\n" + start + comments;
                        }
                    }
                }
            }

            return data;
        }

        private NeWOProg LoadData()
        {
            _currentUser = Toolbox.do_handle_authentication(page_id);
            var woprogid = Request.QueryString["woid"];
            var wo = this.LoadWorkOrder(Convert.ToInt32(woprogid));
            if (wo == null)
            {
                return null;
            }

            this.workOrderID.Value = wo.woprog_id.ToString();
            lblHeading.Text = "Work Order: " + wo.OrderNumber + " - " + wo.CustomerName + "<br/>" + wo.Description.Replace("\n", "<br/>");

            return wo;
        }

        private NeWOProg LoadWorkOrder(int workorderID)
        {
            NeWOProg wo = null;
            try
            {
                wo = new NeWOProg(Convert.ToInt32(workorderID));
            }
            catch (Exception ex)
            {
                wo = null;
            }
            finally
            {
            }

            return wo;
        }

        private string GetCommentIDs(List<CommentRecord> list)
        {
            if (list == null || list.Count <= 1)
            {
                return "";
            }

            var ids = "";
            foreach (var r in list)
            {
                if (ids == "")
                {
                    ids = r.WoComment_ID.ToString();
                }
                else
                {
                    ids = ids + "," + r.WoComment_ID.ToString();
                }
            }

            return ids;
        }
    }

    public class CommentRecord
    {
        public long WoComment_ID { get; set; }
        public string comments { get; set; }
        public bool PrintComments { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> WOComment_Company_ID { get; set; }
    }
}