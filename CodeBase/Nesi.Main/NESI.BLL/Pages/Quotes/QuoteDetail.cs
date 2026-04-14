using System;
using System.Collections.Generic;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core;

namespace NESI.BLL.Pages.Quotes
{
	public class QuoteDetail : QuoteBLLBase
	{
		protected int quote_id { get; set; }
		protected int revision { get; set; }

		public QuoteDetail(Employee user, int quoteId, int revision) : base(user)
		{
			_quote.quote_id = quoteId.ToString();
			_quote.revision = revision.ToString();
			this.quote_id = quoteId;
			this.revision = revision;
		}

		public QuoteDetail(Employee user) : base(user)
		{

		}

		public DTO.ViewModels.Page.Quotes.QuoteDetail[] GetNewDetailId(int type)
		{
			_quote.new_extratext(type);
			return _quote.divs(type);
		}

		public DTO.ViewModels.Page.Quotes.QuoteDetail CreateSectionForNoteAdder(int id)
		{
			return _quote.createSectionForNoteAdder(id);
		}


		public DataExtra del_extratext_detail(int typeId, int id, bool del_assoc = false)
		{
			var msg = "";
			bllToolbox.doSQL_void(@"DELETE FROM quote_extratext WHERE id = @v0  LIMIT 1", id);
			var c = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM quote_section WHERE detail_id = @v0 ", id);
			if (c == 1)
			{
				var temp_section_id = bllToolbox.doSQL_int(@"SELECT id FROM quote_section WHERE detail_id = @v0 ", id);
				var _c = bllToolbox.doSQL_int(@"SELECT COUNT(*) FROM quote_worksheet WHERE section_id = @v0 ", temp_section_id);
				if (_c > 0 && del_assoc)
				{
					bllToolbox.doSQL_void(@"DELETE FROM quote_section WHERE detail_id = @v0  LIMIT 1", id);
					bllToolbox.doSQL_void(@"DELETE FROM quote_worksheet WHERE section_id = @v0 ", temp_section_id);
				}
				else if (_c == 0 && del_assoc)
				{
					// Delete section if user wanted to delete associate and there is no lines attached to it.
					bllToolbox.doSQL_void(@"DELETE FROM quote_section WHERE detail_id = @v0  LIMIT 1", id);
				}
				else if (!del_assoc)
				{
					// Changed section to same text just without id
					bllToolbox.doSQL_void(@"UPDATE quote_section 
		SET detail_id = NULL, section = case when substring(section, 1, locate('.', section, 1)-1)>0 
		then substring(section, locate('.', section, 1)+1, length(section)) else id end, picklist_controlled = TRUE WHERE detail_id = @v0  LIMIT 1", id);
				}
			}
			else if (c > 1)
			{
				msg = ("There were multiple sections with this section ID assigned to it... this shouldn't happen.");
			}
			msg = "Success";
			var bll=new QuoteEdit(CurrentUser,quote_id,revision);

			return new DataExtra
			{
				Data = msg,
				Extra = bll.re_order(null, typeId).Extra
			};
		}

	}


}

