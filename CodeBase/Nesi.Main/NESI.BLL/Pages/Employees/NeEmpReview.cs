using System;
using System.Data;
using nesi.core;

namespace NESI.BLL.Pages.Employees
{
	public class NeEmpReview
	{
		private int _id = 0;
		private DateTime _date;
		private NeMember _member;
		private NeMember _reviewed_by;
		private int _locked;
		private NeMemberType _mt;
		private NeMemberOffer _offer;
		private int _was_printed;
		private string _status;
		private string _notes;


		public int id { get => _id;
			set => _id = value;
		}
		public DateTime date { get => _date;
			set => _date = value;
		}
		public int member_id { get; set; }
		public NeMember reviewed_by { get => _reviewed_by;
			set => _reviewed_by = value;
		}
		public int locked { get => _locked;
			set => _locked = value;
		}
		public NeMemberType mt { get => _mt;
			set => _mt = value;
		}
		public NeMemberOffer offer { get => _offer;
			set => _offer = value;
		}
		public int was_printed { get => _was_printed;
			set => _was_printed = value;
		}
		public string status { get => _status;
			set => _status = value;
		}
		public string notes { get => _notes;
			set => _notes = value;
		}
		public NeMember member { get => _member;
			set => _member = value;
		}
		public int reviewed_by_id { get; set; }

		//TODO: This needs to be moved to not using dt.Rows[0] crap...
		public NeEmpReview(int id)
		{
			var _tools = new Toolbox();
			var dt = _tools.getSQL_datatable(@"Select * from emp_review  where id = @v0", new object[] { id });
			if (dt.Rows.Count > 0)
			{
				_id = id;
				_date = dt.Rows[0]["date"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(dt.Rows[0]["date"]);
				member_id = dt.Rows[0]["member_id"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["member_id"]);
				reviewed_by_id = dt.Rows[0]["reviewed_by_id"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["reviewed_by_id"]);
				_reviewed_by = dt.Rows[0]["reviewed_by_id"] == DBNull.Value ? new NeMember() : new NeMember((int)dt.Rows[0]["reviewed_by_id"]);
				_locked = dt.Rows[0]["locked"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["locked"]);
				_mt = dt.Rows[0]["mt"] == DBNull.Value ? new NeMemberType() : new NeMemberType(dt.Rows[0]["mt"]);
				_offer = dt.Rows[0]["offerid"] == DBNull.Value ? new NeMemberOffer() : new NeMemberOffer(Convert.ToInt32(dt.Rows[0]["offerid"]));
				_was_printed = Convert.ToInt32(dt.Rows[0]["was_printed"]);
				_status = dt.Rows[0]["status"] == DBNull.Value ? "Waiting to be Completed" : dt.Rows[0]["status"].ToString();
				_notes = dt.Rows[0]["notes"] == DBNull.Value ? "" : dt.Rows[0]["notes"].ToString();
				_member = dt.Rows[0]["member_id"] == DBNull.Value ? new NeMember() : new NeMember((int)dt.Rows[0]["member_id"]);

			}
		}

		public void delete(int review_id)
		{

			Toolbox.doSQL_void("delete from emp_review where id =@v0 ", review_id);
			Toolbox.doSQL_void("delete from emp_review_history where emp_review_id =@v0 ", review_id);

		}

		public void save()
		{
			if (_id == 0)  // if adding a new row
			{
				_id = Toolbox.doSQL_return_id(@"Insert into emp_review 
(date,member_id,reviewed_by_id,locked,mt,offerid,status,notes) 
values (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7)",
new object[] {
_date.ToString("yyyy-MM-dd"), //0
member_id, //1
reviewed_by_id, //2
0, //3
_mt.MemberTypeID, //4
_offer.id, //5
"In Development", //6
_notes //7
});
			}
			else // if updating
			{
				Toolbox.doSQL_void(@"update emp_review 
set date=@v0 ,
reviewed_by_id =@v1 ,
locked=@v2 ,
mt=@v3 ,
offerid=@v4 ,
status = @v5 ,
notes = @v6  
where id = @v7",
new object[] {
	_date.ToString("yyyy-MM-dd"),
	reviewed_by_id,
	_locked,
	_mt.id,
	_offer.id,
	_status,
	_notes,
 _id});
			}
		}


		public NeEmpReview()
		{
			//
			//  
			//
		}

		public static int create_review(int offer_id, DateTime dtereview, int reviewed_by)
		{
			var _tools = new Toolbox();
			var mo = new NeMemberOffer(offer_id);

			// Create Header
			_tools.getSQL_void(@"insert into emp_review (date,member_id,reviewed_by_id,locked,mt,offerid,status) 
values(@v0,@v1,@v2,@v3,@v4,@v5,@v6)",
new object[] { dtereview.ToString("yyyy-MM-dd"), mo.memberid, reviewed_by, 0, mo.membertypeid, mo.id, "In Development" });
			// pull up the review ID that we just added
			var review_id = _tools.getSQL_int(@"Select id from emp_review  where emp_review.offerid = @v0  order by id desc limit 1", new object[] { offer_id });

			// Grab all the generic review items
			var dt = _tools.getSQL_datatable(@"Select * from emp_review_items  where status = 'Active' order by `group`", null);
			foreach (DataRow dr in dt.Rows)
			{
				_tools.getSQL_void(@"insert into emp_review_history (member_id, date, emp_review_item_id,emp_review_id,score,emp_review_question) 
values (@v0,curdate(),@v1,@v2,@v3,@v4)",
new object[] { mo.memberid, dr["id"], review_id, 0, dr["item"] });
			}
			// NOw grab all the items related to the members offer
			dt = _tools.getSQL_datatable(@"SELECT cr_review.cr_review_id,cr_review.cr_review_question FROM cr_review,memberoffer_cr,core_responsibilities  WHERE memberoffer_cr.memberoffer_crid = cr_review.cr_review_cr_id and core_responsibilities.id = memberoffer_cr.memberoffer_crid and core_responsibilities.`status` = 'Active' and memberoffer_cr.memberoffer_moid = @v0  and cr_review.cr_review_status = 'Active'", new object[] { offer_id });
			// Now loop through and insert these
			foreach (DataRow dr in dt.Rows)
			{
				_tools.getSQL_void(@"insert into emp_review_history (member_id, date, cr_review_item_id,emp_review_id,score,cr_review_question) 
values (@v0,curdate(),@v1,@v2,@v3,@v4)",
					new object[] {
mo.memberid ,dr["cr_review_id"], review_id ,0, dr["cr_review_question"] });
			}
			return review_id;




		}
	}
}
