using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NESI.BLL.Layout.Banner
	{
	public class IdSearch : BLLBase
		{
		public IdSearch(Employee user) : base(user)
			{
			}

		public DTO.ViewModels.CurrentUser.Layout.IdSearch[] GetSearch(string searchString)
			{
			searchString = searchString.Trim().TrimStart('0');
			const int limiter = 20;
			var master = new List<DTO.ViewModels.CurrentUser.Layout.IdSearch>();
			var authorizedForWorkOrders = CurrentUser.AuthorizePage(OpsPage.WorkOrders);
			var authorizedForPurchaseOrders = CurrentUser.AuthorizePage(OpsPage.Purchases);
			var authorizedForQuotes = CurrentUser.AuthorizePage(OpsPage.QuoteModule);

			if (authorizedForWorkOrders)
				{
				var woURL = HttpUtility.UrlEncode(@"/wo_prog_frame.aspx?action=show&woprog_id=");
				var woAll = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.IdSearch>(@"
SELECT
	a.woprog_bvwo id,
    TRIM(LEADING '0' FROM a.WOProg_bvwo) displaytext,
	a.woprog_customername customervendor,
	'WO' type,
	CONCAT('/redir.aspx?url=', @p1, a.woprog_id) url,
	b.ddl_name businessunitname
FROM
	woprog a
INNER JOIN
	business_unit b ON a.business_unit_id = b.id
WHERE
	FIND_IN_SET(a.business_unit_id, @p2) AND
	a.woprog_bvwo LIKE CONCAT('%',@p0,'%')
ORDER BY 
	a.WOProg_bvwo DESC
LIMIT @p3",
					searchString, 
					woURL, 
					CurrentUser.VisibleBusinessUnits,
					limiter
					).ToList();


				master.AddRange(woAll.Take(limiter));
				}

			if (authorizedForPurchaseOrders)
				{
				var poURL = HttpUtility.UrlEncode(@"/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=");
				var poAll = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.IdSearch>(@"
SELECT
	a.poprog_id id,
    TRIM(LEADING '0' FROM poprog_bvpo) displaytext,
	c.vendor_name customervendor,
	'PO' type,
	CONCAT('/redir.aspx?url=',@p1, a.poprog_id) url,
	b.ddl_name businessunitname
FROM
	poprog_header a
INNER JOIN
	business_unit b ON a.business_unit_id = b.id
INNER JOIN
	vendor c ON a.poprog_vendor_id = c.vendor_id
WHERE
	FIND_IN_SET(a.business_unit_id, @p2) AND
	a.poprog_bvpo LIKE CONCAT('%',@p0,'%')
ORDER BY 
	a.poprog_id DESC
LIMIT @p3", 
					searchString, 
					poURL, 
					CurrentUser.VisibleBusinessUnits,
					limiter
				).ToList();

				master.AddRange(poAll.Take(limiter));
				}

			// Search Q
			if (authorizedForQuotes)
				{
				const string qoURL1 = @"/#/opens/65/quotes/";
				const string qoURL2 = @"/";
				var qoAll = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.IdSearch>(@"
SELECT
    a.quote_id id,
    a.quote_id displaytext,
    c.customer_name customervendor,
    'Q' type,
    CONCAT(@p1, a.quote_id, @p2, a.revision) url,
    b.ddl_name businessunitname
FROM
    quote_master a
LEFT JOIN
    business_unit b ON a.business_unit_id = b.id
LEFT JOIN
    customer c ON a.customer_id = c.customer_id
WHERE
    a.active_revision = 1 AND
    FIND_IN_SET(a.business_unit_id, @p3) AND
    a.quote_id LIKE CONCAT('%',@p0,'%')
ORDER BY 
	a.quote_id DESC
LIMIT @p4",
					searchString,
					qoURL1,
					qoURL2,
					CurrentUser.VisibleBusinessUnits,
					limiter
					).ToList();

				master.AddRange(qoAll.Take(limiter));
				}

			return master.ToArray();
			}
		}
	}
