using nesi.core;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Common;
using NESI.DTO.ViewModels.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{
    public class MasterQuoteGrid : BLLGridBase<DTO.ViewModels.Page.Reports.MasterQuoteGrid>
    {
        public MasterQuoteGrid()
        {
        }

        public MasterQuoteGrid(Employee user) : base(user)
        {
        }

        public MasterQuoteGrid(Employee user, BodyParams param) : base(user, param, new object[] { })
        {
            this.query_params = new object[] { user.Id };
            this.query = "CALL report_quote_master_n2(@p0,'{bu_ids}')";
        }

        public override object ProcessExtra(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return null;
            }

            List<NESI.DTO.ViewModels.Page.Reports.MasterQuoteGrid> list = new List<DTO.ViewModels.Page.Reports.MasterQuoteGrid>();

            this.ConvertToList(table.AsEnumerable().ToList(), list, false, null);

            int won = 0;
            foreach (var item in list)
            {
                if (item.status.ToLower() == "P.O Received".ToLower())
                {
                    won++;
                }
            }


            var rate = Math.Round(Convert.ToDouble(won) / Convert.ToDouble(list.Count) * 100, 0);
            this.CaculationsOnAllColumns = string.Format("{0} %", rate);

            return null;
        }

        public LabelValueInt[] GetChances()
        {
            var chances = bllToolbox.doSQL_Array<LabelValueInt>(@"SELECT quote_chance_id value, ifnull(quote_chance_name, ' ') label FROM quote_chance ORDER BY quote_chance_id");
            foreach (var item in chances)
            {
                if (item.Label.Trim() == "")
                {
                    item.Label = "　";
                }
            }

            return chances;
        }

        public DataExtra UpdateSingleQuote(DTO.ViewModels.Page.Reports.MasterQuoteGrid quote)
        {
            DataExtra result = new DataExtra()
            {
                Data = "Information has been saved successfully",
                Extra = quote
            };

            if (quote.status_id == 9 || quote.status_id == 8 || quote.status_id == 7 || quote.status_id == 6 || quote.status_id == 5 ||
                quote.status_id == 13 || quote.status_id == 4)
            {
                try
                {
                    bllToolbox.doSQL_void(@"
                UPDATE quote_master 
                SET 
                completion_date =@v0,
                pct_chance =@v1, 
                pct_chance_reason =@v2,
                date_expected_start =@v3,
                follow_up =@v4,
                parallel_bid =@v5,
                exp_podate =@v6
                WHERE quote_id =@v7  AND revision =@v8 ",
                        new object[]
                        {
                        quote.completion,
                        quote.pct_chance,
                        quote.pct_chance_reason,
                        quote.qO_startdate,
                        quote.follow_up,
                        quote.parallel_bid,
                        quote.exp_podate,
                        quote.quote,
                        quote.rev
                        });
                }
                catch (Exception ex)
                {
                    result.Data = string.Format("[{0}] quote can't be updated.", quote.quote);
                    result.Extra = ex.Message;
                }

            }
            else
            {
                try
                {
                    bllToolbox.doSQL_void(@"
                UPDATE quote_master 
                SET 
                date_due =@v0, 
                completion_date =@v1,
                pct_chance =@v2, 
                pct_chance_reason =@v3,
                date_expected_start =@v4,
                follow_up =@v5,
                parallel_bid =@v6,
                exp_podate =@v7
                WHERE quote_id =@v8  AND revision =@v9 ",
                        new object[]
                        {
                        quote.date_due,
                        quote.completion,
                        quote.pct_chance,
                        quote.pct_chance_reason,
                        quote.qO_startdate,
                        quote.follow_up,
                        quote.parallel_bid,
                        quote.exp_podate,
                        quote.quote,
                        quote.rev
                        });
                }
                catch (Exception ex)
                {
                    result.Data = string.Format("[{0}] quote can't be updated.", quote.quote);
                    result.Extra = ex.Message;
                }
            }
        
            return result;
        }

        public List<KeyValuePair<string, bool>> UpdateMultipleQuotes(DTO.ViewModels.Page.Reports.MasterQuoteGrid[] quotes)
        {
            List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();

            if (quotes == null || quotes.Length == 0)
            {
                return list;
            }

            foreach (var quote in quotes)
            {
                var result = this.UpdateSingleQuote(quote);
                var r = result.Data as string;
                if (r == "Information has been saved successfully")
                {
                    list.Add(new KeyValuePair<string, bool>(quote.quote.ToString(), true));
                }
                else
                {
                    list.Add(new KeyValuePair<string, bool>(quote.quote.ToString(), false));
                }
            }

            return list;
        }

        public DataExtra AddNote(DTO.ViewModels.Page.Reports.MasterQuoteGrid quote)
        {
            DataExtra result = new DataExtra()
            {
                Data = "New note has been saved successfully",
                Extra = ""
            };

            try
            {
                int quoteid = (int)quote.quote;
                var notes = new NeWoProgNotes(quoteid, "Q");
                notes.woprog_project_notes_woprogid = quoteid;

                var temp_note = Toolbox.MySQLNow_long() + ": " + this.CurrentUser.FullName + " - " + quote.note + "\n" + quote.notes + "\n";

                notes.woprog_project_notes_notes = temp_note;
                notes.woprog_project_notes_memberid = this.CurrentUser.Id;
                notes.woprog_project_notes_Type = "Q";
                notes.SaveWOProgProjectNote();
                result.Extra = temp_note;
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public DataExtra UpdateChance(DTO.ViewModels.Page.Reports.MasterQuoteGrid quote)
        {
            DataExtra result = new DataExtra()
            {
                Data = "Chance reason has been updated successfully",
                Extra = ""
            };

            int id = int.Parse(quote.pct_chance_reason);
            bllToolbox.doSQL_void(@"
                UPDATE quote_master 
                SET 
                pct_chance_reason =@v0
                WHERE quote_id =@v1  AND revision =@v2",
                   new object[]
                   {
                        id,
                        quote.quote,
                        quote.rev
                   });

            return result;
        }
     }
}
