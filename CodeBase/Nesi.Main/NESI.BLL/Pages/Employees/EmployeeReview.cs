using System;
using System.Collections.Generic;
using System.Linq;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Page.Employees;

namespace NESI.BLL.Pages.Employees
{
	public class EmployeeReview : EmployeeEdit
	{
		public int review_id { get; set; }
		protected NeEmpReview review;
		protected readonly NeMember current_user;

		public EmployeeReview(Employee user, int mid) : base(user, mid)
		{
			can_access = tab_enabled[9];
		}

		public EmployeeReview(Employee user, int mid, int rid) : base(user, mid)
		{
			can_access = tab_enabled[9];
			review_id = rid;
			review = review_id > 0 ? new NeEmpReview(review_id) : new NeEmpReview();
			current_user = new NeMember(UserId);
			issupervisor = (NeMember.is_supervisor(review.member_id, current_user.id));
			can_access = ((review.reviewed_by_id == current_user.id) ||
						  (current_user.AuthenticatedForPrivilege(144)) ||
						  issupervisor ||
						  ((review.member_id == current_user.id) && (review.status == "Delivered")));
		}

		private EmployeeReviewListItem[] GetReviewList()
		{
			return bllToolbox.doSQL_Array<EmployeeReviewListItem>(@"
SELECT
	a.id,
	a.date,
	a.member_id memberid,
	a.locked,
	b.member_fullname as reviewedby,
	a.offerid,
	a.status,
	a.was_printed,
  IFNULL(
    (SELECT
      AVG(emp_review_history.score)
    FROM
      emp_review_history
    WHERE IFNULL(emp_review_history.score, 0) > 0
      AND emp_review_id = a.id),
    ''
  ) `AVG`
FROM
	emp_review a 
LEFT JOIN
	member b ON a.reviewed_by_id = b.member_id 
WHERE 
	a.member_id =@p0 order by a.id desc", member_id);
		}

		public object ListProfile()
		{

			return new
			{
				member_id,
				fullName = n1_member.FullName,
				list = GetReviewList()
			};
		}


		public DataExtra DeleteList(int reviewId)
		{
			var rv = new NeEmpReview(reviewId);
			if (rv.locked == 1)
			{
				return new DataExtra("The review could not be deleted.", ListProfile());
			}
			rv.delete(reviewId);
			return new DataExtra("The review has been deleted successfully.", ListProfile());
		}


		private List<LabelValueInt> GetOfferList()
		{
			return review_id == 0 || review.offer == null || review.offer.id == 0
				? bllToolbox.doSQL_List<LabelValueInt>(@"
Select id value,concat(DATE_FORMAT(date,'%d %b %Y'),' - ',status) label from member_offers  where memberid =@v0 and (status = 'Accepted') order by id desc
", member_id)
				: bllToolbox.doSQL_List<LabelValueInt>(@"
Select id value,concat(DATE_FORMAT(date,'%d %b %Y'),' - ',status) label from member_offers  where id =@v0 order by id desc
", review.offer.id);
		}

		private List<LabelValueInt> GetReviewedByList()
		{
			return bllToolbox.doSQL_List<LabelValueInt>(@"
 call get_possible_reviewers_n2(@v0,0)", member_id);
		}

		public object GetScores()
		{
			var scoreList = GetScoreList();
			return new
			{
				score1 = Math.Round(scoreList[0].Value, 2).ToString("N2"),
				score2 = Math.Round(scoreList[1].Value, 2).ToString("N2")
			};
		}


		public new object Profile()
		{
			review = new NeEmpReview(review_id);
			var offerList = GetOfferList();
			var reviewedByList = GetReviewedByList();

			EmployeeReviewProfile entity;
			if (review_id > 0)
			{
				entity = new EmployeeReviewProfile
				{
					id = review.id,
					fullName = n1_member.FullName,
					date = review.date,
					locked = review.locked == 1,
					offerid = review.offer.id,
					reviewed_by_id = review.reviewed_by_id,
					status = review.status,
					membertype_name = review.mt.name,
					is_supervisor = issupervisor,
					was_printed = review.was_printed == 1

				};

				if (reviewedByList.FirstOrDefault(x => x.Value == review.reviewed_by_id) == null)
				{
					reviewedByList.Add(new LabelValueInt(review.reviewed_by.FullName, review.reviewed_by.id));
				}
				if (reviewedByList.FirstOrDefault(x => x.Value == review.offer.reports_to) == null)
				{
					reviewedByList.Add(new LabelValueInt(new NeMember(review.offer.reports_to).FullName, review.offer.reports_to));
				}
				if (reviewedByList.FirstOrDefault(x => x.Value == n1_member.reports_to) == null)
				{
					reviewedByList.Add(new LabelValueInt(new NeMember(n1_member.reports_to).FullName, n1_member.reports_to));
				}
			}
			else
			{
				entity = new EmployeeReviewProfile()
				{
					id = 0,
					fullName = n1_member.FullName,
					date = DateTime.Today,
					locked = false,
					offerid = offerList.Count > 0 ? offerList[0].Value : 0,
					reviewed_by_id = n1_member.reports_to,
					status = "Not Started Yet...",
					was_printed = false,
					is_supervisor = issupervisor
				};
				entity.membertype_name = entity.offerid > 0 ? new NeMemberType(new NeMemberOffer(entity.offerid).membertypeid).name : n1_member.membertype.name;
			}
			var scoreList = GetScoreList();

			return new
			{
				reports_to_name = review_id > 0 ? review.reviewed_by.FullName : "",
				offerList,
				reviewedByList,
				score1 = Math.Round(scoreList[0].Value, 2).ToString("N2"),
				score2 = Math.Round(scoreList[1].Value, 2).ToString("N2"),
				reviewList = GetReviewItemList(),
				core_reviewList = GetCoreReviewItemList(),
				milestoneList = GetMileStoneList(),
				entity,
			};
		}

		public DataExtra SaveProfile(EmployeeReviewProfile model)
		{
			if (review_id == 0)
			{
				model.id = NeEmpReview.create_review(model.offerid, model.date, model.reviewed_by_id);
				review_id = model.id;
			}
			else
			{
				review.date = model.date;
				review.locked = model.locked ? 1 : 0;
				if (model.locked)
				{
					review.status = review.was_printed == 1 ? "Delivered" : "Ready to Deliver";
				}
				else
				{
					review.status = "In Development";
				}
				review.reviewed_by_id = model.reviewed_by_id;
				review.save();
			}
			return new DataExtra("Review has been saved successfully.", Profile());
		}


		public DataExtra PrintCopy()
		{
			bllToolbox.doSQL_void(@"update emp_review set locked = 1, was_printed=1, status = 'Delivered' where id = @v0", review_id);
			return new DataExtra("Review has been saved successfully.", Profile());
		}


		private List<EmployeeReviewItem> GetReviewItemList()
		{
			return bllToolbox.doSQL_List<EmployeeReviewItem>(@"
SELECT
  emp_review_history.id,
  emp_review_group.group `group`,
  emp_review_history.emp_review_question question,
  emp_review_history.notes notes,
  emp_review_history.score score
FROM
  emp_review_history
  INNER JOIN emp_review_items
    ON emp_review_history.emp_review_item_id = emp_review_items.id
  INNER JOIN emp_review_group
    ON emp_review_items.group = emp_review_group.id
WHERE emp_review_history.emp_review_id = @p0", review_id);
		}


		private List<EmployeeReviewItem> GetCoreReviewItemList()
		{
			return bllToolbox.doSQL_List<EmployeeReviewItem>(@"
SELECT
  emp_review_history.id id,
  emp_review_history.cr_review_question question,
  emp_review_history.notes notes,
  emp_review_history.score score,
  core_responsibilities.core_responsibility AS `group`
FROM
  emp_review_history
  INNER JOIN cr_review
    ON emp_review_history.cr_review_item_id = cr_review.cr_review_id
  INNER JOIN core_responsibilities
    ON cr_review.cr_review_cr_id = core_responsibilities.id
WHERE emp_review_history.emp_review_id = @p0", review_id);
		}


		private List<EmployeeReviewMileStone> GetMileStoneList()
		{
			return bllToolbox.doSQL_List<EmployeeReviewMileStone>(@"
SELECT
  memberoffer_milestones.milestone,
  memberoffer_milestones.due,
  memberoffer_milestones.id,
  memberoffer_milestones.ticketid,
  memberoffer_milestones.notes,
  ifnull(memberoffer_milestones.completed,0) as completed
FROM
  memberoffer_milestones
  INNER JOIN emp_review
    ON emp_review.offerid = memberoffer_milestones.offerid
WHERE emp_review.id = @p0", review_id);
		}


		private List<LabelValueDouble> GetScoreList()
		{

			return bllToolbox.doSQL_List<LabelValueDouble>(
				@"
(SELECT
  'general' AS label,
  IFNULL(
    AVG(
      IF(
        emp_review_history.score > 0,
        emp_review_history.score,
        NULL
      )
    ),
    0
  ) AS `value`
FROM
  emp_review_history
  INNER JOIN emp_review_items
    ON emp_review_history.emp_review_item_id = emp_review_items.id
  INNER JOIN emp_review_group
    ON emp_review_items.group = emp_review_group.id
WHERE emp_review_history.emp_review_id = @p0
)
UNION 
(
SELECT
  'core'  AS label,
  IFNULL(
    AVG(
      IF(
        emp_review_history.score > 0,
        emp_review_history.score,
        NULL
      )
    ),
    0
  )  AS `value`
FROM
  emp_review_history
  INNER JOIN cr_review
    ON emp_review_history.cr_review_item_id = cr_review.cr_review_id
  INNER JOIN core_responsibilities
    ON cr_review.cr_review_cr_id = core_responsibilities.id
WHERE emp_review_history.emp_review_id = @p0
)
", review_id);
		}

		public object SaveReviewItem(EmployeeReviewItem model)
		{
			bllToolbox.doSQL_void(@"update emp_review_history 
set member_id=@v0,date = curdate(),score =@v1, notes=@v2 where id =@v3", UserId, model.score, model.notes, model.id);
			return new DataExtra("success",
				GetScores());
		}


		public object SaveReviewMileStone(EmployeeReviewMileStone model)
		{
			bllToolbox.doSQL_void(@"update memberoffer_milestones 
set completed=@v0, notes=@v1 where id =@v2", model.completed ? 1 : 0, model.notes, model.id);
			return new DataExtra("success", null);
		}
	}
}