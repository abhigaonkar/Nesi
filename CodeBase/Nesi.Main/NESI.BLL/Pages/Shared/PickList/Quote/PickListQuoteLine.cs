using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using nesi.core;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.Models.Quote;
using NESI.DTO.ViewModels.Core;
using NESI.DTO.ViewModels.Shared.PickList;
using NESI.DTO.ViewModels.Shared.PickList.Quote;
using inventory = nesi.core.inventory;
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable All

namespace NESI.BLL.Pages.Shared.PickList.Quote
{
	public class PickListQuoteLine : PickListQuoteBase
	{
		/*
		 ALTER TABLE `neintranet`.`quote_worksheet`   
  ADD COLUMN `line_number` INT(11) DEFAULT 0 NOT NULL AFTER `is_checked`;
		 */

		private NeMember neUser;

		public PickListQuoteLine(Employee user) : base(user)
		{
			neUser = new NeMember(UserId);
		}

		public PickListQuoteLine(Employee user, int quote_id, int revision) : base(user, quote_id, revision)
		{
			neUser = new NeMember(UserId);
		}

		public string UpdateQuoteLineChecked(int id, bool is_checked)
		{
			bllToolbox.doSQL_void(@"update quote_worksheet set is_checked=@p1 where id=@p0 and quote_id=@p2 and revision=@p3",
				id, is_checked ? 1 : 0, _quote.QuoteID, _quote.Revision);
			return "Success.";
		}

		public DataExtra applyDiscount(bool applied)
		{
			double quotediscountamt = 0;
			var quotediscountflag = "0";
			if (applied)
			{
				var woline =
					bllToolbox.doSQL_dt(
						@"SELECT customer_id c_id, address_id a_id FROM quote_master WHERE quote_id = @v0  AND revision = @v1 ",
						_quote.QuoteID, _quote.Revision).Rows[0];
				var c_id = Convert.ToInt32(woline["c_id"]);
				var a_id = Convert.ToInt32(woline["a_id"]);
				if (a_id == 0)
				{
					a_id = (int)NECustomer.Get_Default_Billing_Address_Id(c_id);
				}

				DiscountAmount = 0;
				quotediscountamt = Convert.ToDouble(DiscountAmount);
				quotediscountflag = "1";
			}
			else
			{
				quotediscountamt = 0;
				quotediscountflag = "0";
			}
			// All material parts
			bllToolbox.doSQL_void(@"UPDATE quote_worksheet AS a, inventory_item_master AS b,
inventory_tag AS c SET quote_worksheet_discount = @v0 
WHERE a.part_no = b.master_id AND b.tag_id = c.tag_id AND
CAST(if(a.part_no > 0,a.part_no,0) AS SIGNED) < 990000 AND a.part_no != '0'
AND a.Part_no != '' AND is_exclude = 0 AND a.quote_id = @v1  AND a.revision = @v2 ",
				quotediscountamt, _quote.QuoteID, _quote.Revision);
			// All kits
			bllToolbox.doSQL_void(@"UPDATE quote_worksheet SET quote_worksheet_discount = @v0  
WHERE CAST(if(part_no > 0,part_no,0) AS SIGNED) >= 2000000 AND quote_id = @v1 
AND revision = @v2 ", quotediscountamt, _quote.QuoteID, _quote.Revision);
			// For handling addline stuff

			bllToolbox.doSQL_void(
				@"UPDATE quote_master SET quote_master_apply_discount = @v0  WHERE quote_id = @v1  AND revision = @v2 ",
				quotediscountflag, _quote.QuoteID, _quote.Revision);

			return new DataExtra()
			{
				Data = "Success.",
				Extra = new PickListQuote(CurrentUser, _quote.QuoteID, _quote.Revision)
			};
		}

		public DataExtra CopyLines(DTO.ViewModels.Shared.PickList.Quote.PickListQuoteCopyMoveLine model)
		{
			var count = 0;
			foreach (var line in model.items)
			{
				// same section do not copy line
				foreach (var d in model.destination)
				{
					if (line.section_id != d.Value)
					{
						var entity = new Data.Entities.quote_worksheet();
						entity.quote_id = _quote.QuoteID;
						entity.revision = _quote.Revision;
						entity.section_id = d.Value;
						entity.part_no = line.master_id;
						entity.qty = line.qty;
						entity.description = line.label;
						entity.sell = line.sell;
						entity.cost = line.cost;
						entity.extended_per = line.extd2;
						entity.original_sell = line.sell;
						entity.member_id = UserId;
						entity.quote_worksheet_discount = line.discount;
						entity.consignment_id = 0;
						entity.cost_level = line.cost_level;
						entity.code = "";
						entity.is_checked = line.is_checked;

						_db.quote_worksheet.AddOrUpdate(entity);
						count++;
					}
				}

			}

			if (count > 0)
			{
				_db.SaveChanges();
				return new DataExtra()
				{
					Data = count == 1 ? $"One item has been copied successfully." : $"{count} items have been copied successfully.",
					Extra = null
				};
			}
			else
			{
				return new DataExtra()
				{
					Data = "No item has been copied.",
					Extra = null
				};
			}
		}


		public DataExtra MoveLines(DTO.ViewModels.Shared.PickList.Quote.PickListQuoteCopyMoveLine model)
		{
			var ids = model.items.Where(x => x.section_id != model.destination[0].Value).Select(x => x.id.ToString()).ToArray();
			if (ids.Length > 0)
			{
				bllToolbox.doSQL_void(@"Update quote_worksheet set section_id=@p0 where find_in_set(id,@p1) and quote_id=@p2 and revision=@p3",
					model.destination[0].Value, string.Join(",", ids), _quote.QuoteID, _quote.Revision);

				return new DataExtra()
				{
					Data = ids.Length == 1 ? "One item has been moved successfully." : $"{ids.Length} items have been moved successfully.",
					Extra = null
				};
			}
			else
			{
				return new DataExtra()
				{
					Data = "No item has been moved.",
					Extra = null
				};
			}
		}

		public DataExtra UpdateLine(PickListQuotePartDetailEdit model, string field)
		{

			if (!int.TryParse(model.master_id, out var master_id))
			{
				master_id = 0;
			}

			var entity = _db.quote_worksheet.FirstOrDefault(x => x.id == model.id);
			if (entity == null)
			{
				return new DataExtra()
				{
					Data = "The worksheet line was not found.",
					Extra = null
				};
			}
			if (field == "qty")
			{
				var obj = master_id != 0 ? GetInventoryByMasterId(master_id, model.qty, model.id) : null;
				send_cost_warning_email(obj);
				var costhandler = CostHandler(model.qty, model.cost);
				model.extd = costhandler.extd;
				model.extd2 = costhandler.extd2;
				entity.original_sell = costhandler.sell;
				entity.cost = model.cost;
				entity.qty = model.qty;
				entity.extended_per = model.extd2;
			}
			else if (field == "extd2")
			{
				entity.extended_per = model.extd2;
			}
			else if (field == "description")
			{
				entity.description = model.label;
			}
			else if (field == "cost")
			{
				entity.cost = model.cost;
				entity.extended_per = model.extd2;
				entity.original_sell = model.qty != 0 ? model.extd / model.qty : model.extd;
			}
			else
			{
				return new DataExtra()
				{
					Data = "Bad Request.",
					Extra = null
				};
			}
			_db.quote_worksheet.AddOrUpdate(entity);
			_db.SaveChanges();

			return new DataExtra()
			{
				Data = "Saved successfully.",
				Extra = new
				{
					cost = model.cost,
					extd = model.extd,
					extd2 = model.extd2
				}
			};

		}
		public string AddNewQuoteMaterialLines(PickListQuotePartDetailInsert[] models)
		{
			var count = 0;
			foreach (var model in models)
			{
				if (model.qty != 0)
				{
					//	model.extd2 = 0;
					AddNewQuoteMaterialLine(model);
					count++;
				}
			}
			return count > 1 ? $"{count} items have been added successfully." : $"One item has been added successfully.";
		}



		public string AddNewQuoteMaterialLine(PickListQuotePartDetailInsert model)
		{
			var entity = new Data.Entities.quote_worksheet();

			var obj = model.master_id == "" ? null : GetInventoryByMasterId(Convert.ToInt32(model.master_id), model.qty, 0);
			var blankLine = model.master_id == "" || obj.cost == 0 || obj.exclude_part;
			entity.quote_id = _quote.QuoteID;
			entity.revision = _quote.Revision;
			entity.section_id = model.section_id;
			entity.part_no = model.master_id;
			entity.qty = model.qty;
			entity.description = model.description;
			entity.sell = blankLine ? model.sell : obj.sell_price; //  model.sell; //GetPartNewSell(model.master_id,model.qty);
			entity.cost = blankLine ? model.cost : obj.cost_price; // model.cost; // GetPartNewCost(model.master_id);
			entity.extended_per = model.sell > 0 ? entity.sell * model.qty : (blankLine ? model.extd2 : obj.extd2);
			entity.original_sell = entity.sell;
			entity.member_id = UserId;
			entity.quote_worksheet_discount = blankLine ? 0 : obj.discount;
			entity.consignment_id = 0;
			entity.cost_level = blankLine ? 0 : obj.cost_level; // GetCost_Level(model.master_id);
			entity.code = "";
			entity.is_checked = true;

			_db.quote_worksheet.AddOrUpdate(entity);
			_db.SaveChanges();

			return "One item has been added successfully.";
		}

		public string AddNewQuoteLaborLine(PickListQuoteWorksheetLabor model)
		{
			var entity = new Data.Entities.quote_worksheet();

			entity.quote_id = _quote.QuoteID;
			entity.revision = _quote.Revision;
			entity.section_id = model.section_id;
			entity.part_no = model.master_id.ToString();
			entity.qty = model.qty.GetValueOrDefault();
			entity.description = GetLaborDescription(model.membertype_name, model.paytype_name); // $"{model.membertype_name} {model.paytype_name} {this.WorkingBusinessUnit.labour_labor}";
			entity.sell = GetLaborSellPrice(model.master_id.ToString());
			entity.cost = GetLaborCostPrice(model.master_id.ToString());
			entity.extended_per = entity.sell * model.qty;
			entity.original_sell = entity.sell;
			entity.member_id = UserId;
			entity.quote_worksheet_discount = 0;
			entity.consignment_id = 0;
			entity.cost_level = 0;
			entity.code = "";
			entity.is_checked = true;

			_db.quote_worksheet.AddOrUpdate(entity);
			_db.SaveChanges();



			return "Success.";
		}

		public string AddQuoteLabor(PickListQuoteWorksheetLabor[] models)
		{
			var count = 0;
			foreach (var model in models)
			{
				if (model.qty.GetValueOrDefault() > 0)
				{
					AddNewQuoteLaborLine(model);
					count++;
				}
			}
			return count > 1 ? $"{count} items have been added successfully." : $"{count} item has been added successfully.";
		}


		public PickListQuotePartDetail[] GetInventoryByMasterIdList(int[] master_id_list)
		{
			var list = new List<PickListQuotePartDetail>();
			foreach (var id in master_id_list)
			{
				if (id > 0)
				{
					list.Add(GetInventoryByMasterId(id));
				}
			}
			return list.ToArray();
		}

		public PickListQuotePartDetail GetInventoryByMasterId(int master_id, double qty = 0, int line_id = 0)
		{

			var _inventory = new inventory();
			_inventory.current_user = neUser;
			_inventory.Load(master_id.ToString(), WarehouseBusinessUnit.id);
			var obj = new PickListQuotePartDetail();
			if (qty == 0) qty = 1;

			if (master_id < 990000 && _inventory.part_exists(master_id))
			{
				obj.description_full = _inventory.description_full;
				obj.description = _inventory.description;
				obj.exclude_part = _inventory.is_exclude;
				obj.tag_id = _inventory.tag_id;

				obj.cost_price = GetPartNewCost(master_id.ToString(), qty);
				obj.sell_price = shared.GetSellPrice(obj.cost_price, 0, true, qty, WarehouseBusinessUnit.id);

				obj.used_sell_price = obj.sell_price;
				obj.used_cost_price = obj.cost_price;
				obj.discount = this.member_can_discount && this.QuoteDiscount ? DiscountAmount : 0;
				obj.cost_level = GetCost_Level(master_id.ToString());
			}
			else if (master_id >= 990000 && master_id < 2000000) // labor
			{

				var labor = bllToolbox.doSQL_Object<PickListMemberTypeLaborHour>(MEMBERTYPE_HOURS2 + " where c.`id`=@v0", master_id);
				if (labor != null)
				{
					obj.description = GetLaborDescription(labor.membertype_name, labor.paytype_name);
					obj.sell_price = GetLaborSellPrice(master_id.ToString());
					obj.cost_price = GetLaborCostPrice(master_id.ToString());
					obj.used_sell_price = obj.sell_price;
					obj.used_cost_price = obj.cost_price;
					obj.discount = 0;
					obj.cost_level = 0;
					//if (!this.member_can_see_labour_cost)
					//{
					//	obj.cost_price = 0;
					//	obj.used_cost_price = 0;
					//}

				}
			}
			else if (master_id >= 2000000) // kitted
			{
				obj.cost_price = GetKittedCost(master_id);
				obj.sell_price = GetKittedSell(master_id, qty);
				obj.used_sell_price = obj.sell_price;
				obj.used_cost_price = obj.cost_price;
				obj.discount = this.member_can_discount && this.QuoteDiscount ? DiscountAmount : 0;
				obj.cost_level = 0;
			}
			if (!this.member_can_discount || obj.is_exclude || !QuoteDiscount)
			{
				obj.discount = 0;
			}
			//if (!this.member_can_see_cost)
			//{
			//	obj.cost_price = 0;
			////	obj.used_cost_price = 0;
			//}
			//if (!this.member_can_see_sell)
			//{
			//	obj.sell_price = 0;
			////	obj.used_sell_price = 0;
			//}
			obj.master_id = master_id > 0 ? master_id.ToString() : "";
			obj.cost = Math.Round(obj.cost_price, 2);
			obj.sell = Math.Round(obj.sell_price, 2);
			obj.allowed_to_stock = _inventory.allowed_to_stock;
			obj.extd = Math.Round(obj.sell * qty * (1 - obj.discount / 100), 2);
			obj.extd2 = obj.extd;
			obj.quote_id = _quote.QuoteID;
			obj.revision = _quote.Revision;
			obj.line_id = line_id;
			obj.qty = qty;
			obj.onhand = _inventory.onhand_qty;
			obj.int_onhand_qty = _inventory.int_onhand_qty;
			obj.ext_onhand_qty = _inventory.ext_onhand_qty;
			obj.has_pic = _inventory.has_pic;
			obj.has_minmax = _inventory.n_minmax > 0;
			obj.is_stocked = _inventory.is_stocked;
			obj.wo_usage = _inventory.wo_usage;
			obj.po_usage = _inventory.po_usage;
			obj.is_exclude = _inventory.is_exclude;
			obj.ttl_ms = _inventory.ttl_ms;


			var shown_vendor_date = "";
			try
			{
				if (_inventory.vendor_price_last_dt != "--" && _inventory.vendor_price_last_dt != "")
				{
					shown_vendor_date = Convert.ToDateTime(_inventory.vendor_price_last_dt).ToString("yyyy-MM-dd");
				}
			}
			catch
			{
				shown_vendor_date = "--";
			}
			obj.last_purchased = shown_vendor_date;

			bllToolbox.doSQL_void("Call update_inventory_cost(@p0,@p1)", master_id, WarehouseBusinessUnit.id);

			return obj;
		}

		public void send_cost_warning_email(PickListQuotePartDetail obj)
		{
			if (obj == null || obj.sell_price >= obj.cost_price)
			{
				return;
			}
			var mail = new NeEMail();
			mail.To = BLL.Common.Shared.Configuration.DebugRedirectEmail;
			mail.From = BLL.Common.Shared.Configuration.EmailFrom;
			mail.Subject = $@"The labour {obj.master_id} sell price is less than cost price.";
			mail.Body = $@"Master Id: {obj.master_id} <br/>
Description: {obj.description} <br/>
Cost Price: {obj.cost_price} <br/>
Sell Price: {obj.sell} <br/>
Quote Id: {obj.quote_id} <br/>
";
			mail.Send_Background();
		}

		public double GetPartNewSell(string masterId, double qty)
		{
			return bllToolbox.doSQL_double(@"Select GetSellPrice(get_current_cost(@v0 ,@v1 ),proc_GetInvSellPrice(@v0 ,@v1 ),true,@v2,@v1 )", masterId, buId, qty);
		}

		public double GetPartNewCost(string masterId, double qty)
		{
			return bllToolbox.doSQL_double(@"Select get_cost_at_qty(@v0 ,@v1,0,@v2 )", masterId, buId, qty);
		}

		public double GetLaborSellPrice(string masterId)
		{
			return masterId == "1000000" ? 0 : bllToolbox.doSQL_double(@"call CUSTOMER_CHARGEOUT(@v0,@v1)", _quote.cust_id, masterId);
		}

		public string GetLaborDescription(string membertype_name, string paytype_name)
		{
			return $"{membertype_name} {paytype_name} {this.WorkingBusinessUnit.labour_labor}";
		}
		public double GetLaborCostPrice(string masterId)
		{
			return bllToolbox.doSQL_double(@"SELECT GET_CURRENT_LABOUR_COST(@v0 ,@v1 )", masterId, buId);
		}

		public int GetCost_Level(string masterId)
		{
			return bllToolbox.doSQL_int(@"SELECT GET_COST(@v0 ,@v1 ,true)", masterId, buId);
		}

		public double GetKittedSell(int masterId, double quantity)
		{
			return bllToolbox.doSQL_double(@"SELECT ifnull(GETKITTEDSELL(@v0 ,@v1 ,@v2 ),0)", quantity, buId, masterId);
		}

		public double GetKittedCost(int masterId)
		{
			return bllToolbox.doSQL_double(@"select get_cost(@v0 ,@v1 , false)", masterId, buId);
		}

		public string UpdateNote(PickListNote model)
		{
			string sql;
			if (model.is_new)
			{
				sql =
					@"UPDATE quote_worksheet SET notes =   CONCAT(IFNULL(notes, ''),'\n--\n [',@v2,' ] - ', NOW(),'\n ', @v0 ) WHERE id = @v1";
			}
			else
			{
				sql =
					@"UPDATE quote_worksheet SET notes = @v0 WHERE id = @v1";
			}
			bllToolbox.doSQL_void(sql,
				model.note, model.id, CurrentUser.FullName);

			return "Note has been updated successfully.";
		}


		public string DeleteWorkSheet(int id)
		{
			if (_quote.worksheet_consignment_id != 0)
			{
				var _consign = new consignment(_quote.worksheet_consignment_id);
				_consign.load();
				_consign.status = consignment.StatusType.WaitingToBeQuoted;
				_consign.save();
			}
			bllToolbox.doSQL_void(@"DELETE FROM quote_worksheet  WHERE id =@v0 limit 1 ", id);

			return "Item has been deleted successfully.";
		}


		public PickListQuoteCostHandler CostHandler(double qty, double cost)
		{

			var sell = Math.Round(shared.GetSellPrice(Math.Abs(cost), 0, true, qty, WorkingBusinessUnit.id32), 2);
			var extd_sell = cost < 0 ? Math.Round(qty * sell, 2) * -1 : Math.Round(qty * sell, 2);
			var extd_cost = Math.Round(qty * cost, 2);

			var v = new PickListQuoteCostHandler
			{
				qty = qty,
				sell = sell,
				cost = cost,
				extd2 = extd_sell,
				extd = extd_sell
			};
			return v;
		}

		public string AddSection(string sectionName)
		{
			bllToolbox.doSQL_void(@"INSERT INTO quote_section ( quote_id, revision, section, picklist_controlled ) VALUES ( @v0 , @v1 , @v2 , 1 )", _quote.QuoteID, _quote.Revision, sectionName);
			return "Success.";
		}

		public string EditSection(int section_id, string sectionName)
		{
			bllToolbox.doSQL_void(@" UPDATE quote_section SET section = @v1 , picklist_controlled = 1 WHERE id = @v0  LIMIT 1", section_id, sectionName);
			return "Success.";
		}

		public string DeleteSection(int section_id)
		{
			bllToolbox.doSQL_void(@"DELETE FROM quote_section WHERE id = @v0  LIMIT 1", section_id);
			return "Success.";
		}

		public DataTable LoadSections()
		{
			return bllToolbox.doSQL_dt(@"
					SELECT a.id, 
					a.section, 
					a.detail_id, 
					(SELECT COUNT(*) FROM quote_worksheet b WHERE b.section_id = a.id) dependants 
					FROM quote_section a 
					WHERE a.quote_id = @v0 
					AND a.revision = @v1  
					AND LENGTH(TRIM(a.section)) > 0 
					and IFNULL(a.detail_id,0)=0
					ORDER BY a.section", _quote.QuoteID, _quote.Revision);
		}

		public DataExtra ReOrder(ReOrderItem[] models)
		{
			foreach (var model in models)
			{
				bllToolbox.doSQL_void(@"UPDATE quote_worksheet SET line_number = @v1 WHERE id = @v0 LIMIT 1", model.id, model.order);
			}

			return new DataExtra
			{
				Data = "Success",
				Extra = new PickListQuote(CurrentUser, _quote.QuoteID, _quote.Revision).items
			};
		}
	}
}