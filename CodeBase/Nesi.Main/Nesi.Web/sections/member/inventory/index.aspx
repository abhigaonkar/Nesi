<%@
	Page	Language		= 'C#'
			MasterPageFile	= '../../../IntraDefault.master'
			AutoEventWireup	= 'true'
			Inherits		= 'branch_inventory' Culture="en-US"
			Title			= 'Inventory' 
			Validaterequest	= 'false'
			EnableTheming  ='true'
 Codebehind="index.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
	Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Src="~/modules/layout_control.ascx" TagName="LayoutControl" TagPrefix="lc" %>
<%@ Register Src="user_controls/inv_rfq.ascx" TagName="RFQ_Tab" TagPrefix="uc" %>
<%@ Register Src="user_controls/inv_master_locations.ascx" TagName="master_locations" TagPrefix="uc" %>
<%@ Register src="user_controls/orders_tab.ascx" tagname="orders_tab" tagprefix="uc" %>
<%@ Register src="user_controls/home_tab.ascx" tagname="home_tab" tagprefix="uc" %>
<%@ Register src="user_controls/vendor_data_import.ascx" tagname="vdi" tagprefix="uc" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx1" %>

<%@ Register src="user_controls/part_transfer.ascx" tagname="part_transfer" tagprefix="uc1" %>

<asp:Content ID='Content1' ContentPlaceHolderID='cphMasterLeft' Runat='Server'>
				<div id='divSide' runat='server'>
					<asp:SqlDataSource ID='Users' runat='server'></asp:SqlDataSource>
				</div>
</asp:Content>

<asp:Content ID='Content2' ContentPlaceHolderID='cphMasterMenu' Runat='Server'>
				<div id='divMenu' runat='server'></div>
</asp:Content>
<asp:Content ID='Content3' ContentPlaceHolderID='cphMasterBody' Runat='Server'>
	<script type='text/javascript' src='/js/jquery.form.js'></script>
<script type='text/javascript' src='/js/jquery.paste.js'></script>
	<script type='text/javascript' src='/js/inventory/_branch.js'></script>
	<script type="text/javascript">
		function bc(master_id)
        {
            
            createPopupWin("/_tools/print_barcode/index.aspx?master_id=" + master_id, "Print Barcode", 600, 200);
        
		//popbcprint.SetHeaderText("Print Labels for: " + master_id);
		//hidbcmastertid.Set("id",master_id);
		//popbcprint.Show();

        }
        function createPopupWin(pageURL, pageTitle, 
                    popupWinWidth, popupWinHeight) { 
            var left = (screen.width-popupWinWidth)/2; 
            var top = (screen.height-popupWinHeight)/4;
            var myWindow = window.open(pageURL, pageTitle,  
                    'resizable=yes, width=' + popupWinWidth 
                    + ', height=' + popupWinHeight + ', top=' 
                    + top + ', left=' + left); 
        } 
		function apply_part(obj, c_id, r_id)
			{
			var o		=	{
							a:				"wo_switch_part",
							master_id:		$(obj).parents("td:first").find(".part_n").val(),
							business_unit_id:		c_id,
							row_id:			r_id
							};
			
			if(o.master_id === "" || o.master_id == null || o.master_id == undefined || isNaN(o.master_id))
				{
				alert("Invalid Part Selection");
				return false;
				}
			else
				{
			$.ajax( 
				{
				type:		"GET",
				cached:		false,
				url:		"./index.aspx",
				data:		o,
				dataType:	"text",
				beforeSend:	function()
								{
								please_wait("start", "Saving...");
								},
				success:	function(ret)
								{
								if(ret != "SUCCESS")
									{
									alert(ret);
									}
								else
									{
									alert("Row Updated");
									}
								},
				complete:	function()
								{
								please_wait("stop");
								gv_tocreate.Refresh();
								}
				});
				}
			}
		function sw_def_ven(obj)
			{
			var _parent	= $(obj).parents("tr[class*=dxgvDataRow]:first");
			var _v	=	{
						name:	_parent.find('.vendor_name'),
						code:	_parent.find('.vendor_code'),
						cost:	_parent.find('.vendor_price'),
						qty:	_parent.find('.vendor_qty'),
						lead: _parent.find('.vendor_lead'),
						_date:	_parent.find('.vendor_date')
						};
			_v.name.attr("data-default_vendor_id", $(obj).val());
			_v.name.attr("data-default_vendor_name", $("option:selected", obj).text());
			_v.name.attr("data-cost", $("option:selected", obj).attr("data-cost"));
			_v.name.attr("data-lead", $("option:selected", obj).attr("data-lead"));
			_v.name.attr("data-_date", $("option:selected", obj).attr("data-_date"));
		
			_v.name.attr("data-qty-per", $("option:selected", obj).attr("data-qty"));
			_v.name.attr("data-vendor_code", decodeURIComponent($("option:selected", obj).attr("data-code")));
			_v.name.attr("data-id", $(obj).val());
			_v.name.val($("option:selected", obj).text());
			_v.code.val(decodeURIComponent($("option:selected", obj).attr("data-code")));
			_v.cost.val($("option:selected", obj).attr("data-cost"));
			_v.qty.val($("option:selected", obj).attr("data-qty"));
			_v.lead.val($("option:selected", obj).attr("data-lead"));
			_v._date.val($("option:selected", obj).attr("data-_date"));
			
			}
		function save_mmo(s,e)
			{
			var _parent	= $(s.mainElement).parents("tr[class*=dxgvDataRow]:first");
			var server_vals		= _parent.find('.server_vals');
			if(server_vals.size() == 0) 
				{
				alert("Please make sure the master ID field is in this grid.");
				return;
				}
			var o		= 	{
							master_id:				gv_minmax.cpid[s.cpIndex],
							min:					_parent.find('.min').val(),
							max:					_parent.find('.max').val(),
							qty:					_parent.find('.onhand').val(),
							location_master_id:		server_vals.attr("data-location_id"),
							business_unit_id:		server_vals.attr("data-business_unit_id")
							};
			if(o.min == undefined || o.min == null)
				{
				alert("Please make sure the min column exists in the grid");
				return;
				}
			if(o.max == undefined || o.max == null)
				{
				alert("Please make sure the max column exists in the grid");
				return;
				}
			if(o.qty == undefined || o.qty == null)
				{
				alert("Please make sure the qty column exists in the grid");
				return;
				}
			cb_saveminmax.PerformCallback(JSON.stringify(o));
			}
		function mmo_rover(s,e){$(s.mainElement).css({"border": "solid 1px #f99"});}
		function mmo_rout(s,e){$(s.mainElement).css({"border": "solid 1px transparent"});}
		function handle_dept_filter(obj)
			{
			obj					= obj == null ? $("#dept_filter") : $(obj);
			var dept_id			= $(obj).val();
			$(obj).attr('disabled', true);
			please_wait('begin', "Setting Department Option");
			var vars			=	{
									a:				"orders_dept",
									requested_id:	dept_id
									}
			$.get("./index.aspx", vars,
					function(returned)
						{
						if(returned == "Success")
							{
							gv_orders.Refresh();
							$(obj).attr('disabled', false);
							}
						else
							{
							alert(returned);
							$(obj).attr('disabled', false);
							}
						please_wait('stop');
						});
			}
		function handle_wo_filter(obj)
			{
			obj					= obj == null ? $("#wo_filter") : $(obj);
			var wo_id			= obj.val();
			var gv_id			= eval(h.Get("gridview_id"));
			var should_exclude = $("#exclude_wo").is(":checked") ? "NOT" : "";
		//	confirm(should_exclude);
		//	confirm(gv_id.cpFil);
			var find = "AND [wo_parts] NOT LIKE '%" + wo_id + "%'";
		//	var re = new RegExp(find);
		//	confirm(find);
			var current_filter = gv_id.cpFil.toString();
		
			current_filter = current_filter.replace(find, "");
			var findd = "AND [wo_parts]  LIKE '%" + wo_id + "%'";
	//		re = new RegExp(find);
		//	confirm(findd);
			current_filter = current_filter.replace(findd, "");
	//	confirm(current_filter);
			// remove current [wo_parts] NOT LIKE '%55605%'
			var proposed_filter	= current_filter == "" ? "[wo_parts] "+should_exclude+" LIKE '%"+wo_id+"%'" : current_filter + " AND [wo_parts] "+should_exclude+" LIKE '%"+wo_id+"%'";
		//	confirm(proposed_filter);
			if(wo_id == "0" && should_exclude == "NOT")
				{
				$("#exclude_wo").attr("checked", false)
				}
			else
				{
				if(wo_id == "0")
					{
					gv_id.PerformCallback("reload");
					}			
				gv_id.ApplyFilter(proposed_filter);
				}
			}
		function handle_rfq_filter(obj)
			{
			obj					= obj == null ? $("#rfq_filter") : $(obj);
			var rfq_id			= $(obj).val();
			$(obj).attr('disabled', true);
			please_wait('begin', "Setting RFQ Option");
			var vars			=	{
									a:				"orders_rfq",
									requested_id:	rfq_id
									}
			$.get("./index.aspx", vars,
					function(returned)
						{
						if(returned == "Success")
							{
							gv_orders.Refresh();
							$(obj).attr('disabled', false);
							}
						else
							{
							alert(returned);
							$(obj).attr('disabled', false);
							}
						please_wait('stop');
						});

			}
		function handle_cust_filter(obj)
			{
			obj					= obj == null ? $("#cust_filter") : $(obj);
			var should_exclude	= $("#exclude_cust").is(":checked");
			var cust_id			= should_exclude ? "1_"+$(obj).val() : $(obj).val();
			$(obj).attr('disabled', true);
			please_wait('begin', "Setting Customer Option");
			var vars			=	{
									a:				"orders_customer",
									requested_id:	cust_id
									}
			$.get("./index.aspx", vars,
					function(returned)
						{
						if(returned == "Success")
							{
							gv_orders.Refresh();
							$(obj).attr('disabled', false);
							}
						else
							{
							alert(returned);
							$(obj).attr('disabled', false);
							}
						please_wait('stop');
						});

			}
		function handle_extloc_filter(obj)
			{
			obj					= obj == null ? $("#extloc_filter") : $(obj);
			var cust_id			= obj.val();
			$(obj).attr('disabled', true);
			please_wait('begin', "Setting Ext. Location");
			var vars			=	{
									a:				"orders_extloc",
									requested_id:	$(obj).val()
									}
			$.get("./index.aspx", vars,
					function(returned)
						{
						if(returned == "Success")
							{
							if(vars.requested_id != "0")
								{
								$("#include_external").attr("checked", true);
								}
							gv_orders.Refresh();
							$(obj).attr('disabled', false);
							}
						else
							{
							alert(returned);
							$(obj).attr('disabled', false);
							}
						please_wait('stop');
						});

			}
		function save_visible(s,e)
			{
			var max		= gv_minmax.GetVisibleRowsOnPage();
			var o		= [];
			for(var i = 0;i < max ;i++)
				{
				var _parent		= $("#ctl00_cphMasterBody_gv_minmax_DXMainTable tbody").find('tr[class*=dxgvSelectedRow]:eq('+i+')');
				var server_vals		= _parent.find('.server_vals');
				if(server_vals.size() == 0) 
					{
					continue;
					}
				var vals		=	{
									master_id:				_parent.find('.master_id').text(),
									min:					$.trim(_parent.find('.min').val()),
									max:					$.trim(_parent.find('.max').val()),
									qty:					$.trim(_parent.find('.onhand').val()),
									location_master_id:		server_vals.attr("data-location_id"),
									business_unit_id:		server_vals.attr("data-business_unit_id")
									};
                if (vals.min == undefined || vals.min == null || vals.min === '')
					{
					alert("Please make sure the min column exists in the grid");
					return;
					}
                if (vals.max == undefined || vals.max == null || vals.max === '')
					{
					alert("Please make sure the max column exists in the grid");
					return;
					}
                if (vals.qty == undefined || vals.qty == null || vals.qty === '')
					{
					alert("Please make sure the qty column exists in the grid");
					return;
					}
                if (vals.master_id !== "")
					{
					vals.min			= vals.min == "" ? 0 : vals.min;
					vals.max			= vals.max == "" ? 0 : vals.max;
					vals.qty			= vals.qty == "" ? 0 : vals.qty;
					//vals.location       = vals.location == "" ? "": vals.location;
					o.push(vals);
					}
				}
			if(o.length == 0)
				{
				alert("Please make sure you have some rows selected and the master ID column exists in the grid");
				return;
				}
			var query_string		=	{
										a:		"mass_save_mmo", 
										mmos:	JSON.stringify(o)
										};
			if(o.length == 0)
				{
				alert("Please select some rows to edit.");
				return;
				}
			else
				{
				$.ajax( 
					{
					type:		"POST",
					cached:		false,
					url:		"./index.aspx",
					data:		query_string,
					dataType:	"text",
					beforeSend:	function()
									{
										if ("activeElement" in document)
											{
											document.activeElement.blur();
											}
									please_wait("start");
									},
					success:	function(ret)
									{
									if(ret != "SUCCESS")
										{
										alert(ret);
										}
									else
										{
										alert("Rows Updated");
										}
									},
					complete:	function()
									{
									please_wait("stop");
									}
					});
				}
			}
		function cut_visible_pos(s,e)
			{
			if(!confirm("Are you sure you want to cut these PO's?"))
				{
				return;
				}
			var max		= gv_orders.GetVisibleRowsOnPage();
			var o		= [];
			var mc		= 0;
			for(var i = 0;i < max ;i++)
				{
				var _parent		= $("#ctl00_cphMasterBody_uc_orders_tab_gv_orders tbody").find('tr[class*=dxgvDataRow]:eq('+i+')');
				if(_parent.find('.cb_inc').is(":checked"))
					{
					var c			= 0;
					var _qtyper		= $.trim(_parent.find('.vendor_qty').val()) / 1;
					var _cost		= $.trim(_parent.find('.vendor_price').val()) / 1;
					var _to_order	= Math.ceil($.trim(_parent.find('.qty_to_order').val()) / _qtyper);
					_cost			= (_cost * _qtyper);
					var vendor_id	= _parent.find('.vendor_name').attr("data-id");
					var vals		=	{
										master_id:		_parent.find('.vendor_name').attr("data-master_id"),
										vendor_code:	$.trim(_parent.find('.vendor_code').val()),
										lead:			$.trim(_parent.find('.vendor_lead').val()),
										bu_id:			_parent.find('.vendor_name').attr("data-business_unit_id"),
										cost:			_cost,
										to_order:		_to_order,
										qty:			$.trim(_parent.find('.vendor_qty').val()),
										_date:$.trim(_parent.find('.vendor_date').val())
										};
					var v			=	{
										id:				vendor_id,
										cut_new:		$("#cut_new").is(":checked"),
										data:			[vals]
										};
                    if (vendor_id == null || vendor_id === "" || vendor_id == undefined)
						{
						_parent.find('.vendor_name').css({"border":"solid 1px #f00"});
						c++;
						}
					else
						{
						_parent.find('.vendor_name').css({"border":"solid 1px #abadb3"});
						}
                    if (vals.vendor_code == null || vals.vendor_code === "" || vals.vendor_code == undefined)
						{
						_parent.find('.vendor_code').css({"border":"solid 1px #f00"});
						c++;
						}
					else
						{
						_parent.find('.vendor_code').css({"border":"solid 1px #abadb3"});
						}
                    if (vals.cost === "" || isNaN(vals.cost))
						{
						_parent.find('.vendor_price').css({"border":"solid 1px #f00"});
						c++;
						}
					else
						{
						_parent.find('.vendor_price').css({"border":"solid 1px #abadb3"});
						}
                    if (vals.qty == null || vals.qty === "" || vals.qty == undefined || isNaN(vals.qty))
						{
						_parent.find('.vendor_qty').css({"border":"solid 1px #f00"});
						c++;
						}
					else
						{
						_parent.find('.vendor_qty').css({"border":"solid 1px #abadb3"});
						}
                    if (vals.lead == null || vals.lead === "" || vals.lead == undefined || isNaN(vals.lead))
						{
						_parent.find('.vendor_lead').css({"border":"solid 1px #f00"});
						c++;
						}
					else
						{
						_parent.find('.vendor_lead').css({"border":"solid 1px #abadb3"});
						}
					if(c==0)
						{
						// array has entities
						if(o.length > 0)
							{
							var caught		= false;
							for(var j = 0;j<o.length;j++)
								{
								// if vendor exists
								if(o[j].id != undefined && o[j].id == vendor_id)
									{
									//o=>vendor_id=>data[]
									o[j].data.push(vals);
									caught	= true;
									}
								// if vendor doesn't exist
								else
									{
									}
								}
							if(!caught)
								{
								o.push(v);
								}
							}
						// array doesn't have entities
						else
							{
							o.push(v);
							}
						}
					else
						{
						mc++;
						}
					}
				}
				if(mc > 0)
					{
					alert("There are errors with your submission, please fix.");
					}
				else if(o.length == 0)
					{
					alert("No parts selected");
					for(var i = 0;i < max ;i++)
						{
						$("#ctl00_cphMasterBody_uc_orders_tab_gv_orders tbody").find('tr[class*=dxgvDataRow]:eq('+i+')').find('input:text').each(function()
							{
							if($(this).attr('class') in {vendor_lead:1, vendor_qty:1, vendor_price:1, vendor_code:1, vendor_name:1,vendor_date:1})
								{
								$(this).css({"border":"solid 1px #abadb3"});
								}
							});
						}
					}
				else
					{
					var query_string		=	{
												a:		"mass_po_cut", 
												parts:	JSON.stringify(o)
												};
					var should_refresh		= true;
					$.ajax( 
						{
						type:		"POST",
						cached:		false,
						url:		"./index.aspx",
						data:		query_string,
						dataType:	"text",
						beforeSend:	function()
										{
										if ("activeElement" in document)
											{
											document.activeElement.blur();
											}
										please_wait("start");
										},
						success:	function(ret)
										{
										if(ret.indexOf("SUCCESS") == -1)
											{
											alert(ret);
											should_refresh			= false;
											}
										else
											{
											var arrs	= ret.split("|");
											var po_s	= arrs[1].split(",");
											var str		= "";
											for(var k = 0;k<po_s.length;k++)
												{
											    str += po_s[k] + "</br>";
											//    if (po_s[k].indexOf('PO #') > -1) {
											 //       var po_n = po_s[k].replace("PO #", "").replace(" Cut", "").replace("Using ","").replace(" - didn't cut a new one","");
											   //     str += "<a href='/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=" + po_n + "' target='_blank'>" + po_n + "</a></br>";
											  //      window.open("/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid=" + po_n);
											//    }
												}
											alert("PO's:\n-------------</br>" + str);
											}
										},
						complete:	function()
										{
										please_wait("stop");
										if(should_refresh)
											{
											gv_orders.Refresh();
											}
										}
						});
					}
			}
		function save_orders_visible(s,e)
			{
			var max		= gv_orders.GetVisibleRowsOnPage();
			var o		= [];
			for(var i = 0;i < max ;i++)
				{
				var _parent		= $("#ctl00_cphMasterBody_uc_orders_tab_gv_orders tbody").find('tr[class*=dxgvDataRow]:eq('+i+')');
				var server_vals		= _parent.find('.server_vals');
				if(server_vals.size() == 0) 
					{
					alert("Please make sure the master ID field is in this grid.");
					return;
					}
				var vals		=	{
									master_id:			_parent.find('.master_id').text(),
									min:				$.trim(_parent.find('.qty_min').val()),
									max:				$.trim(_parent.find('.qty_max').val()),
									location_master_id:	server_vals.attr("data-location_id"),
									business_unit_id:	server_vals.attr("data-business_unit_id"),
									qty:				$.trim(_parent.find('.onhand').val())
									};
				vals.min			= vals.min == "" ? 0 : vals.min;
				vals.max			= vals.max == "" ? 0 : vals.max;
				vals.qty			= vals.qty == "" ? 0 : vals.qty;
				o.push(vals);
				}
			var query_string		=	{
										a:		"mass_save_mmo", 
										mmos:	JSON.stringify(o)
										};
			$.ajax( 
				{
				type:		"POST",
				cached:		false,
				url:		"./index.aspx",
				data:		query_string,
				dataType:	"text",
				beforeSend:	function()
								{
								if ("activeElement" in document)
									{
									document.activeElement.blur();
									}
								please_wait("start");
								},
				success:	function(ret)
								{
								if(ret != "SUCCESS")
									{
									alert(ret);
									}
								else
									{
									alert("Rows Updated");
									}
								},
				complete:	function()
								{
								please_wait("stop");
								}
				});
			}
			function print_barcode(s,e)
				{
				cb_print_barcode.PerformCallback(gv_minmax.cpid[s.cpIndex]);
				}
			function gv_orders_init(s,e)
				{
				$(document).find(".wo_note").each(function()
					{
					$(this).tip({title:'Work Order Note'});
					});
				up_rfq_panel();
				}
			function consignment_save(obj)
				{
				var _parent			= gv_consignment.IsEditing() ? $('#consignment_edit') : $('#consignment_new');
				
				var _paras			=	{
										a:				"new_consignment",
										customer_id:	_parent.find('.t_customer').attr('data-id'),
										serial:			_parent.find('.t_serial').val(),
										master_id:		$("#MASTER_ID").val(),
										status:			_parent.find('.s_status').val()
										};
				if(gv_consignment.IsEditing())
					{
					_paras.a		= "edit_consignment";
					_paras.id		= _parent.attr('data-id');
					}
				if(_paras.customer_id == null || _paras.customer_id == undefined || _paras.customer_id == "")
					{
					alert("Please select a customer");
					_parent.find('.t_customer').val("").focus();
					return false;
					}
                if (_paras.serial == "" || _paras.serial == undefined || _paras.serial == null)
					{
					alert("Please enter a serial #");
					_parent.find('.t_serial').focus();
					return false;
					}
			
				
				please_wait("start");
				$.get("./index.aspx", _paras, 
					function(ret)
						{
						if(ret != "SUCCESS")
							{
							alert(ret);
							}
						else
							{
							if(gv_consignment.IsEditing())
								{
								gv_consignment.CancelEdit();
								}
							else
								{
								var _customer	= _parent.find('.t_customer');
								$(_customer).val("");
								$(_customer).unbind();
								$(_customer).focus(function()
												{
												attach_ac(_customer, 'customer');
												});
								_customer.attr('data-isac', 'false');
								_customer.attr('data-id', '');
								_customer.attr('title', '');
								_parent.find('.t_serial').val("");
								//_parent.find('.t_price').val(_parent.find('.t_price').attr('data-default_price'));
								gv_consignment.Refresh();
								_customer.focus();
								}
							}
						please_wait("stop");
						});				
				}
		function toggle_include_external(obj)
			{
			var include_ext			= $(obj).is(":checked") ? 1 : 0;
			var query_string		=	{
										a:			"toggle_include_external",
										include:	include_ext
										};
			$.ajax( 
				{
				type:		"GET",
				cached:		false,
				url:		"./index.aspx",
				data:		query_string,
				dataType:	"text",
				beforeSend:	function()
								{
								please_wait("start", "Setting Option");
								},
				success:	function(txt)
								{
								if(txt != "SUCCESS")
									{
									alert(txt);
									}
								else
									{
									gv_orders.Refresh();
									}
								},
				error:		function()
								{
								alert("Error toggling 'include external'.");
								please_wait("end");
								},
				complete:	function()
								{
								please_wait("end");
								}
				});

			}
		function bind_tooltips()
			{
			$(".note").each(function()
				{
				$(this).tip();
				});
			}
			$(document).ready(function()
				{
				bind_tooltips();
				$('#partsearch').focus();
				});
			function refresh_contacts()
				{
				var query_string		=	{
											a:			"xml_vendor_contacts",
											vendor_id:	$(".t_vendor").attr("data-id")
											};
				var ops					= "<option value='0'>Pick a Contact</option>";
				$.ajax( 
					{
					type:		"GET",
					cached:		false,
					url:		"./index.aspx",
					data:		query_string,
					dataType:	"text",
					beforeSend:	function()
									{
									please_wait("start");
									},
					success:	function(xml)
									{
									if($(xml).find("contact").size() > 0)
										{
										$(".s_contact").removeAttr("disabled").html("");
										$(xml).find("contact").each(function()
											{
											var id			= $(this).find("id").text();
											var name		= $(this).find("name").text();
											ops				+= "<option value='"+id+"'>"+name+"</option>";
											if(id == 0)
												{
												$(".s_contact").attr("disabled", true);
												}
											});
										}
									},
					error:		function()
									{
									alert("error");
									please_wait("end");
									},
					complete:	function()
									{
									$(".s_contact").html(ops);
									please_wait("end");
									}
					});
				}

function load_file_popup ()
{
	var id = $("#master_id").val();
	$(".popup_files_iframe").attr("src", "/filemanager.aspx?parent_page=inventory_files&id="+id);
	pop_files.Show();

}

    </script>
	<div id='inventory_branch' style='width:100%'>
			<asp:HiddenField ID="temp_master_id" runat="server" /><div class='_legend'>

														<div id='line_description'></div>
														<table cellspacing='0' cellpadding='2'>
															<tr style='background-color:#000;color:#fff;text-align:center;'><td colspan='2'>Cost Price Level Legend</td></tr>
															<tr class='L0' style='background-color:#fff;color:#000;'>		<td class='_l'>L0</td>	<td class='_r'>Unknown</td></tr>
															<tr class='L1' style='background-color:#009900;color:#fff;'>	<td class='_l'>L1</td>	<td class='_r'>Current On Hand Stock Cost</td></tr>
															<tr class='L2' style='background-color:#1d7373;color:#fff;'>	<td class='_l'>L2</td>	<td class='_r'>From Last Cut PO for Stock (Within 2 years)</td></tr>
															<tr class='L3' style='background-color:#009999;color:#fff;'>	<td class='_l'>L3</td>	<td class='_r'>From Saved Vendor Pricing (Newer than 2 years)</td></tr>
															<tr class='L4' style='background-color:#5ccccc;color:#007;'>	<td class='_l'>L4</td>	<td class='_r'>From Last Cut PO for a Work Order (Within 2 years)</td></tr>
															<tr class='L5' style='background-color:#a66f00;color:#fff;'>	<td class='_l'>L5</td>	<td class='_r'>From Last Cut PO for Stock at another business unit in your region (Within 2 years)</td></tr>
															<tr class='L6' style='background-color:#bf8f30;color:#fff;'>	<td class='_l'>L6</td>	<td class='_r'>From Last Cut PO for Stock at another business unit in your country (Within 2 years)</td></tr>
															<tr class='L7' style='background-color:#ff9966;color:#000;'>	<td class='_l'>L7</td>	<td class='_r'>From Saved Vendor Pricing at another business unit in your region (Newer than 2 years)</td></tr>
															<tr class='L8' style='background-color:#bf3030;color:#fff;'>	<td class='_l'>L8</td>	<td class='_r'>From Saved Vendor Pricing at another business unit in your country (Newer than 2 years)</td></tr>
															<tr class='L9' style='background-color:#a60000;color:#fff;'>	<td class='_l'>L9</td>	<td class='_r'>From Last Cut PO for Stock at another business unit in another country (Within 2 years)</td></tr>
															<tr class='L10' style='background-color:#f00;color:#fff;'>	<td class='_l'>L10</td>	<td class='_r'>From Saved Vendor Pricing at another business unit in another country</td></tr>
														</table>
													</div>
		<div align="left" id="toggle_stock_adj" runat="server">&nbsp;</div>
		<table class='framework' border='0' cellpadding='0' cellspacing='0' width="100%">
			<tr class='menu'>
				<td align="left" id="branch_box" runat="server">&nbsp;</td>
				<td align='right' valign='bottom'>
					<button	id="button_home" onclick="location.href='./index.aspx'" class="available" runat="server" type="button">home</button><button 
					id="button_orders" onclick="window.parent.location.href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Dorders'" class="available" runat="server" type="button">orders</button><button 
					id="button_rfqs" onclick="window.parent.location.href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Drfqs'" class="available" runat="server" type="button">RFQ's</button><button
					id="button_location" onclick="window.parent.location.href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Dlocations'" class="available" runat="server" type="button">locations</button><button 
					id="button_kitted" onclick="window.parent.location.href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Dkitted'" class="available" runat="server" type="button">kitted parts</button><button 
					id="button_groups" onclick="window.parent.location.href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Dgroups'" class="available" runat="server" type="button">groups</button><button
					id="button_newitem" runat="server" class="available" onclick="window.parent.location.href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Dstart_part'" type="button">new item</button><button
					id="button_qty" onclick="window.parent.location.href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Dqty'" class="available" runat="server" type="button">qty</button><button
					id="button_manageprices" runat="server" class="available" onclick="window.parent.location.href='/redir.aspx?url=%2Fsections%2Fmember%2Finventory%2Findex.aspx%3Fa%3Dmanage_prices'" type="button">pricing</button>
					<button
					id="button_move_parts" runat="server" class="available" onclick="location.href='./index.aspx?a=move_parts'" type="button">Move Parts</button>
				</td>
				<td align="center" width="15%" valign="middle">Search: <input type="text" id="partsearch" name="partsearch" onfocus="$(this).inventory(
																						{
																						force_clickable: true,
																						business_unit_id: <%= Session["working_warehouse_bu_id"] %>,
																						click: function(row)
																								{
																								location.href		= './index.aspx?a=get&tab=G&id='+row.master_id;
																								}
																						});" /></td>
			</tr>
			<tr>
				<td id="detail" runat="server" align="center" colspan="3" valign="middle">&nbsp;</td>
			</tr>
		</table></div>
	<asp:ScriptManager ID="ScriptManager1" runat="server">
	</asp:ScriptManager>
	<uc:vdi ID="uc_vdi" runat="server" visible="False" />
	<uc:rfq_tab ID="rfq_tab" runat="server" Visible="false" />
	<asp:UpdateProgress ID="UpdateProgress1" runat="server">
		<ProgressTemplate>
			<img id="Img1" alt="progressing" src="/images/loading_panel.gif" style="left: 50%;
				position: absolute; top: 50%" />
		</ProgressTemplate>
	</asp:UpdateProgress>
	<asp:UpdatePanel ID="group_panel" runat="server" RenderMode="Inline" Visible="False">
		<ContentTemplate>
		<table>
			<tr>
				<td align="center" style="width: 221px; height: 41px;" valign="middle">
					<dx:ASPxTextBox ID="group_part_search" runat="server" Width="170px" AutoPostBack="True">
						<ClientSideEvents Init="function(s, e) {
	var _obj		= s.GetMainElement();
	$(_obj).inventory({
						force_clickable:true
						});
}" />
					</dx:ASPxTextBox>
					<br />
				</td>
				<td style="width: 100px; height: 41px;" valign="top">
					<dx:ASPxButton ID="groups_search" runat="server" Text="Search Groups" OnClick="groups_search_Click" Height="25px">
					</dx:ASPxButton>
				</td>
				<td style="width: 100px; height: 41px" valign="top">
				<dx:ASPxButton ID="Button1" runat="server" Text="New Group" 
						Height="25px" AutoPostBack="False" >
					<ClientSideEvents Click="function(s, e) {
	boing('/sections/member/picklist/pikclist.aspx?origin=groupings&amp;id=', 'groupings',950, 800);

}" />
					</dx:ASPxButton>
					</td>
			</tr>
		</table>
		<dx:ASPxGridView id="group_hdrs" runat="server" Visible="False" 
				AutoGenerateColumns="False" KeyFieldName="id" 
				OnRowDeleting="group_hdrs_CustomButtonCallback" Cursor="pointer" Width="100%" 
				Theme="NETheme01">
            <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
			<Columns>

				<dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="5%" Caption="Action" AllowDragDrop="False" ShowDeleteButton="true" >
					
					
				</dx:GridViewCommandColumn>
				<dx:GridViewDataTextColumn Caption="Group ID" FieldName="id" VisibleIndex="1" Width="5%">
					
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Group Name" FieldName="name" VisibleIndex="2"
					Width="15%">
					
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Created By" FieldName="created_by" VisibleIndex="3"
					Width="10%">
				
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Last Updated" FieldName="edited_dt" VisibleIndex="4"
					Width="15%">
					
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="# of Parts" FieldName="n_parts" VisibleIndex="5"
					Width="5%">
				
					<CellStyle HorizontalAlign="Center">
					</CellStyle>
				</dx:GridViewDataTextColumn>
				<dx:GridViewDataTextColumn Caption="Notes" FieldName="Notes" VisibleIndex="6" Width="50%" CellStyle-Wrap="True">
					
				</dx:GridViewDataTextColumn>
			</Columns>
			<ClientSideEvents RowClick="function(s, e) {
var keyValue = s.GetRowKey(e.visibleIndex);
	boing('/sections/member/picklist/pikclist.aspx?origin=groupings&id='+keyValue, 'groupings', 950, 800);
}" />
			<Settings ShowFilterRow="True" />
		</dx:ASPxGridView>
		</ContentTemplate>
	</asp:UpdatePanel>
	<lc:LayoutControl runat="server" id="layout" ShowExcelExport="true" ShowToggle="true" />
	<asp:UpdatePanel ID="kitted_panel" runat="server" RenderMode="Inline" Visible="False">
		<ContentTemplate>
			<table>
				<tr>
					<td align="center" style="width: 221px; height: 41px;" valign="middle">
						<dx:ASPxTextBox ID="kitted_part_search" runat="server" Width="170px" AutoPostBack="True" Theme="NETheme01">
							<ClientSideEvents Init="function(s, e) {
	var _obj		= s.GetMainElement();
	$(_obj).inventory({
						force_clickable:true
						});
}" />
						</dx:ASPxTextBox>
						<br />
					</td>
					<td style="width: 100px; height: 41px;" valign="top">
						<dx:ASPxButton ID="kitted_search" runat="server" Text="Search Kits" OnClick="kitted_search_Click">
						</dx:ASPxButton>
					</td>
					<td style="width: 100px; height: 41px" valign="top">
					<dx:ASPxButton ID="Button2" runat="server" Text="New Kit" 
						Height="25px" AutoPostBack="False" >
					<ClientSideEvents Click="function(s, e) {
	boing('/sections/member/picklist/pikclist.aspx?origin=kitted&id=', 'kitted',950, 800);

}" /></dx:ASPxButton>
					</td>
				</tr>
			</table>
			<dx:ASPxGridView id="kitted_hdrs" runat="server" Visible="False" 
				AutoGenerateColumns="False" KeyFieldName="id" 
				OnRowDeleting="kitted_hdrs_CustomButtonCallback" Cursor="pointer" Width="100%" 
				Theme="NETheme01">
				<SettingsBehavior EnableRowHotTrack="True" />
				
				<ClientSideEvents RowClick="function(s, e) {
var keyValue = s.GetRowKey(e.visibleIndex);
	boing('/sections/member/picklist/pikclist.aspx?origin=kitted&id='+keyValue, 'kitted', 950, 800);
}" />
                <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
				<Columns>
					<dx:GridViewCommandColumn AllowDragDrop="False" ButtonType="Image" Caption="Action" ShowDeleteButton="true"
						VisibleIndex="0" Width="5%">
						
						
					</dx:GridViewCommandColumn>
					<dx:GridViewDataTextColumn Caption="Kit ID" FieldName="id" VisibleIndex="1" Width="5%">
					
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Kit Name" FieldName="name" VisibleIndex="2" Width="15%">
					
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Created By" FieldName="created_by" VisibleIndex="3"
						Width="10%">
					
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="Last Updated" FieldName="edited_dt" VisibleIndex="4"
						Width="15%">
					
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<dx:GridViewDataTextColumn Caption="# of Parts" FieldName="n_parts" VisibleIndex="5"
						Width="5%">
						
						<CellStyle HorizontalAlign="Center">
						</CellStyle>
					</dx:GridViewDataTextColumn>
					<%--
					<dx:GridViewDataTextColumn Caption="Sell Price" FieldName="sell" VisibleIndex="6">
						<HeaderStyle BackColor="#3399ff" ForeColor="White" />
					</dx:GridViewDataTextColumn>
					--%>
					<dx:GridViewDataTextColumn Caption="Notes" FieldName="Notes" VisibleIndex="7" Width="50%">
					
					</dx:GridViewDataTextColumn>
				</Columns>
				<Settings ShowFilterRow="True" />
			</dx:ASPxGridView>
		</ContentTemplate>
	</asp:UpdatePanel>
	<asp:Panel ID="orders_panel" runat="server" Visible="False">
		<uc:orders_tab ID="uc_orders_tab" runat="server" />
	</asp:Panel>
	<asp:Panel ID="pnl_home" runat="server" Visible="false">
		<uc:home_tab ID="uc_home_tab" runat="server" />
	</asp:Panel>
	<script type="text/javascript">
		var gv_minmax_obj		= 
			{
			chk_handler:
				function(s,e)
					{
					if(s.GetChecked())
						{
						gv_minmax_obj.select_all();
						}
					else
						{
						gv_minmax_obj.unselect_all();
						}
					},
			select_all: 
				function()
					{
					gv_minmax.SelectAllRowsOnPage();
					},
			unselect_all: 
				function()
					{
					gv_minmax.UnselectAllRowsOnPage();
					}
			};
	</script>
	<dx:ASPxGridView ID="gv_minmax" runat="server" Visible="False" 
		AutoGenerateColumns="False" 
		Theme="NETheme01"
		OnCustomButtonCallback="gv_minmax_CustomButtonCallback" 
		ClientInstanceName="gv_minmax" Width="100%" 
		OnCustomJSProperties="gv_minmax_CustomJSProperties" 
		OnCustomCallback="Load_Layout" KeyFieldName="id"
		onhtmldatacellprepared="gv_minmax_HtmlDataCellPrepared" OnHtmlFooterCellPrepared="gv_minmax_FooterCellPrepared" oncommandbuttoninitialize="gv_minmax_CommandButtonInitialize" onhtmlcommandcellprepared="gv_minmax_HtmlCommandCellPrepared">
		<Styles>
			
		
			<SelectedRow ForeColor="Black">
			</SelectedRow>
			<FilterRow BackColor="White">
			</FilterRow>
			<Cell Font-Size="11px" Wrap="False">
			</Cell>
			<Table>
				<BorderBottom BorderColor="#3355AA" BorderStyle="Solid" BorderWidth="3px" />
			</Table>
			<Footer Font-Size="11px">
			</Footer>
		</Styles>
		
		<SettingsPager PageSize="50">
		</SettingsPager>
		<TotalSummary>
			<dx:ASPxSummaryItem DisplayFormat="{0:C2}" FieldName="dollar_balance" ShowInColumn="$ Balance" SummaryType="Sum" />
			<dx:ASPxSummaryItem DisplayFormat="C2" FieldName="ob_prov" ShowInColumn="Ob. Prov." SummaryType="Sum" ValueDisplayFormat="C2" />
		</TotalSummary>
		<Columns>
			<dx:GridViewCommandColumn ShowSelectCheckbox="True" Caption="#" VisibleIndex="0" Width="15px">
				<headertemplate>
					<dx1:aspxcheckbox id="chk_all" runat="server" text="All"><clientsideevents checkedchanged="gv_minmax_obj.chk_handler" /></dx1:aspxcheckbox>
				</headertemplate>
			</dx:GridViewCommandColumn>
			<dx:GridViewDataTextColumn Caption="Part #" FieldName="master_id" 
				Name="master_id" Visible="false"
				VisibleIndex="1" Width="75px">
				<DataItemTemplate>
					<input type="hidden" id="server_vals" runat="server" class="server_vals" value='<%# Eval("master_id") %>' data-location_id='<%# Eval("location_master_id") %>' data-business_unit_id='<%# Eval("business_unit_id") %>'  />
					<a id="masterlink" href='<%# "/redir.aspx?url=" + HttpUtility.UrlEncode(string.Format("/sections/member/inventory/index.aspx?a=get&tab=G&id={0}",Eval("master_id")) )%>' tabindex="9999" class="master_id" target="_blank"><%# Eval("master_id") %></a><img src='/images/icon/icon[print_barcode].gif' title='Print Barcode' alt='Print Barcode' onclick='bc(<%# Eval("master_id") %>)' style="cursor:pointer;margin-left:5px;" width='16' height='16' align='absmiddle' />
				</DataItemTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Tag" FieldName="tag" VisibleIndex="2" 
				Width="150px" Visible="False">
				<PropertiesTextEdit DisplayFormatString="HttpUtility.UrlDecode(Eval(&quot;tag&quot;).ToString())">
				</PropertiesTextEdit>
				<Settings FilterMode="DisplayText" />
				<DataItemTemplate>
					<dx:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl="javascript:void();"
						Text='<%# Eval("tag") %>' TabIndex="9999">
						<ClientSideEvents Click="function(s, e) {
	var st	= s.GetText();
	st = st.replace(/'/g, &quot;''&quot;);
	current_filter = &quot;[tag] = '&quot;+st+&quot;'&quot;;
	gv_minmax.ApplyFilter(current_filter);
}" />
					</dx:ASPxHyperLink>
				</DataItemTemplate>
				<CellStyle Font-Bold="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Description" FieldName="description" 
				VisibleIndex="3">
				<Settings AllowAutoFilter="True" AllowHeaderFilter="True" AutoFilterCondition="Contains"
					ShowFilterRowMenu="True" FilterMode="DisplayText" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Location" FieldName="location" VisibleIndex="4"
				Width="10%">
                <Settings FilterMode="DisplayText" />
                <DataItemTemplate>
                    <dx:ASPxTextBox ID="loc" runat="server" HorizontalAlign="Center" ClientEnabled="false" Text='<%# bind("location") %>'
						Width="100%" CssClass="location">
                        <NullTextStyle ForeColor="#CCCCCC">
                        </NullTextStyle>
                    </dx:ASPxTextBox>
                </DataItemTemplate>
				<FooterTemplate>
					<dx:ASPxButton ID="bt_selected" runat="server" AutoPostBack="False" Text="Delete Selected Locations">
						<ClientSideEvents Click="delete_multiple_locations" />
						<Image Url="~/images/icon/icon[deletemultiple].gif">
						</Image>
					</dx:ASPxButton>
				</FooterTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Min" FieldName="min_qty" VisibleIndex="5" 
				Width="65px">
				<PropertiesTextEdit DisplayFormatInEditMode="True">
				</PropertiesTextEdit>
				<DataItemTemplate>
					<dx:ASPxTextBox ID="qty_min" runat="server" Native="true" HorizontalAlign="Center" Text='<%# bind("min_qty") %>'
						Width="50px" CssClass="min">
						<NullTextStyle ForeColor="#CCCCCC">
						</NullTextStyle>
					</dx:ASPxTextBox>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" BackColor="#E0E0E0">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Max" FieldName="max_qty" VisibleIndex="6" 
				Width="65px">
				<DataItemTemplate>
					<dx:ASPxTextBox ID="qty_max" runat="server" Native="true" HorizontalAlign="Center" Text='<%# bind("max_qty") %>'
						Width="50px" CssClass="max">
						<NullTextStyle ForeColor="#CCCCCC">
						</NullTextStyle>
					</dx:ASPxTextBox>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" BackColor="#FFC0C0">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Qty " FieldName="qty" VisibleIndex="7"
				Width="65px">
				<Settings AutoFilterCondition="Greater" />
				<DataItemTemplate>
					<dx:ASPxTextBox ID="onhand_qty" runat="server" Native="true" HorizontalAlign="Center" Text='<%# bind("qty") %>'
						Width="35px" CssClass="onhand">
						<NullTextStyle ForeColor="#CCCCCC">
						</NullTextStyle>
					</dx:ASPxTextBox>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center" Font-Bold="True">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Save" ShowInCustomizationForm="true" 
				ExportWidth="75" Name="save" VisibleIndex="8"
				Width="40px">
				<Settings AllowDragDrop="True" AllowSort="False" />
				<DataItemTemplate>
					<dx:ASPxButton ID="bt_save" runat="server" AutoPostBack="False" Height="25px" OnCustomJSProperties="bt_save_CustomJSProperties"
						Width="29px" BackColor="Transparent"  EnableTheming="False" Cursor="pointer">
						<Image Url="~/images/icon/icon[save].gif">
						</Image>
						<ClientSideEvents Click="save_mmo" GotFocus="mmo_rover" LostFocus="mmo_rout" />
						<Paddings Padding="0px" />
						<Border BorderWidth="0px" />
					</dx:ASPxButton>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<FooterTemplate>
					<dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Save All">
						<ClientSideEvents Click="save_visible" /><Image Url="~/images/icon/Icon[save-green].gif">
						</Image>
					</dx:ASPxButton>
				</FooterTemplate>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Cmtd" FieldName="committed" 
				VisibleIndex="9" Width="5%">
				<Settings AllowHeaderFilter="False" AutoFilterCondition="Greater" />
				<CellStyle HorizontalAlign="Center" BackColor="#C0FFC0">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="On Order" FieldName="on_order" 
				VisibleIndex="10" Width="5%">
				<Settings AutoFilterCondition="Greater" />
				<CellStyle HorizontalAlign="Center" BackColor="#FFC0C0">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Qty Sold (12 Mnth)" FieldName="wo_usage" 
				VisibleIndex="11" Width="50px">
				<Settings AllowHeaderFilter="True" />
				<HeaderStyle Wrap="True" />
				<CellStyle HorizontalAlign="Center" BackColor="#C0FFFF">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="$$ Sold (12 Mnth)" 
				FieldName="wo_usage_extd" VisibleIndex="12" Width="60px">
				<PropertiesTextEdit DisplayFormatString="{0:C2}">
				</PropertiesTextEdit>
				<Settings AllowHeaderFilter="True" />
				<HeaderStyle Wrap="True" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Qty Bought (12 Mnth - qty ordered)" 
				FieldName="po_usage" VisibleIndex="13" Width="50px">
				<HeaderStyle Wrap="True" />
				<CellStyle BackColor="#FFFFC0">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="$ Balance" FieldName="dollar_balance" 
				VisibleIndex="14" Width="60px">
				<PropertiesTextEdit DisplayFormatString="{0:C2}">
				</PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="$$ Bought (12 Mnth)" 
				FieldName="po_usage_extd" VisibleIndex="15" Width="60px">
				<PropertiesTextEdit DisplayFormatString="{0:C2}">
				</PropertiesTextEdit>
				<HeaderStyle Wrap="True" />
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Qty POs (12 Mnth - qty recvd)" 
				FieldName="annual_po_qty" VisibleIndex="16"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Qty WOs (12 Mnth)" 
				FieldName="annual_wo_qty" VisibleIndex="17"></dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Int/Ext" FieldName="intext" 
				VisibleIndex="18">
			</dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="# WOs w/ Comm (12 Mnth)" FieldName="n_wos" 
				Visible="False" VisibleIndex="19"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="# POs w/ Recv (12 Mnth)" FieldName="n_pos" 
				VisibleIndex="20"></dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Ob. Prov." FieldName="ob_prov" 
				UnboundType="Decimal" Visible="False" VisibleIndex="21">
				<PropertiesTextEdit DisplayFormatString="C2">
				</PropertiesTextEdit>
			</dx:GridViewDataTextColumn>
			<dx1:GridViewDataTextColumn Caption="Cost" FieldName="cost" VisibleIndex="23" 
				Width="40px">
				<PropertiesTextEdit DisplayFormatString="c2">
				</PropertiesTextEdit>
			</dx1:GridViewDataTextColumn>
			<dx1:gridviewdatadatecolumn Caption="Last Used" FieldName="last_used" 
				UnboundType="DateTime" Visible="False" VisibleIndex="59">
			</dx1:gridviewdatadatecolumn>
			<dx1:GridViewDataTextColumn Caption="Is Consumable" 
				FieldName="is_consumable"
				VisibleIndex="60" Width="60px">
				<HeaderStyle Wrap="True" />
			</dx1:GridViewDataTextColumn>
		</Columns>
		<Settings EnableFilterControlPopupMenuScrolling="True" ShowFilterRow="True" ShowFilterRowMenu="True"
			ShowHeaderFilterButton="True" ShowPreview="True" ShowFooter="True" 
			ShowFilterBar="Visible" ShowGroupPanel="True" ColumnMinWidth="20" />
		<SettingsBehavior EnableRowHotTrack="True" ColumnResizeMode="Control"
			AutoFilterRowInputDelay="3000" EnableCustomizationWindow="True" />
		<BorderBottom BorderWidth="0px" />
		<SettingsEditing Mode="Inline" />
		<BorderLeft BorderWidth="0px" />

        <SettingsPopup CustomizationWindow-HorizontalAlign="LeftSides" CustomizationWindow-VerticalAlign="TopSides">

<CustomizationWindow HorizontalAlign="LeftSides" VerticalAlign="TopSides"></CustomizationWindow>
		</SettingsPopup>

	</dx:ASPxGridView>
					<dx:ASPxCallback ID="cb_saveminmax" runat="server" ClientInstanceName="cb_saveminmax"
						OnCallback="cb_saveminmax_Callback">
						<ClientSideEvents BeginCallback="function(s, e) {
	please_wait('start');
}" CallbackComplete="function(s, e) {
	please_wait('stop');
}"
 CallbackError="function(s, e) {
	please_wait('stop');
}" />
					</dx:ASPxCallback>
	<dx:ASPxCallback ID="cb_print_barcode" runat="server" ClientInstanceName="cb_print_barcode" OnCallback="cb_print_barcode_Callback">
		<ClientSideEvents BeginCallback="function(s, e) {
	gv_minmax.ShowLoadingPanel();

}" CallbackComplete="function(s, e) {
	gv_minmax.HideLoadingPanel();

	gv_minmax.HideLoadingDiv();

}" CallbackError="function(s, e) {
	gv_minmax.HideLoadingPanel();

	gv_minmax.HideLoadingDiv();
}" />
	</dx:ASPxCallback>
	<dx:ASPxGridView ID="gv_partlocations" runat="server" DataSourceID="sds_partlocations" on Visible="False" Width="100%" OnHtmlDataCellPrepared="gv_partlocations_HtmlDataCellPrepared" AutoGenerateColumns="False" KeyFieldName="id" CssClass="stock_detail" Theme="NETheme01" ClientInstanceName="gv_partlocations">
		<ClientSideEvents EndCallback="bind_xfer" Init="bind_xfer" />
		<Columns>
			<dx:GridViewCommandColumn VisibleIndex="0" Caption=" " Visible="False" ShowClearFilterButton="true">
				
			</dx:GridViewCommandColumn>
			<dx:GridViewDataComboBoxColumn Caption="Name" FieldName="location_master_id" VisibleIndex="1">
				<PropertiesComboBox DataSourceID="sds_masterlocations" TextField="name" ValueField="id">
				</PropertiesComboBox>
				<Settings AutoFilterCondition="Contains" FilterMode="DisplayText" />
				<DataItemTemplate>
					<asp:DropDownList ID="ddl_loc" Enabled='False' runat="server" CssClass="name" DataSourceID="sds_masterlocations3" DataTextField="name" DataValueField="id" SelectedValue='<%# Bind("location_master_id") %>' Width="500px">
					</asp:DropDownList>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Left">
				</CellStyle>
			</dx:GridViewDataComboBoxColumn>
			<dx:GridViewDataTextColumn Caption="Quantity" FieldName="qty" VisibleIndex="2" Width="100px">
				<DataItemTemplate>
					<table cellpadding='0' cellspacing='0' data-draggable="yes" class='qty' id='qty_box' data-original_qty='<%# Eval("qty") %>' data-id='<%# Eval("id") %>' runat="server">
						<tr>
							<td>
					<asp:TextBox ID="TextBox1" runat="server" CssClass="qty" Text='<%# Bind("qty") %>' data-original_qty='<%# Eval("qty") %>' Width="100px"></asp:TextBox></td>
							<td><img alt="" src="../../../images/icon/icon[arrows].png" class="xfer" title="Drag me to another quantity" /></td>
						</tr>
					</table>
				</DataItemTemplate>
				<HeaderStyle HorizontalAlign="Center">
				</HeaderStyle>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Min" FieldName="min" VisibleIndex="3" Width="100px">
				<DataItemTemplate>
					<asp:TextBox ID="TextBox1" runat="server" CssClass="min" Text='<%# Bind("min") %>' Width="100px"></asp:TextBox>
				</DataItemTemplate>
				<HeaderStyle HorizontalAlign="Center">
				</HeaderStyle>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Max" FieldName="max" VisibleIndex="4" Width="100px">
				<DataItemTemplate>
					<asp:TextBox ID="TextBox1" runat="server" CssClass="max" Text='<%# Bind("max") %>' Width="100px"></asp:TextBox>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<HeaderStyle HorizontalAlign="Center">
				</HeaderStyle>
			</dx:GridViewDataTextColumn>
			<dx:GridViewDataTextColumn Caption="Actions" VisibleIndex="5" Width="100px" Name="actions">
				<DataItemTemplate>
					<button type="button" onclick="save_location(this, false);"><img src="/images/icon/icon[save].gif" align="absmiddle"></button><button type="button" onclick="delete_location(this, false);"><img src="/images/icon/icon[delete].gif" align="absmiddle"></button>
				</DataItemTemplate>
				<CellStyle HorizontalAlign="Center">
				</CellStyle>
				<HeaderStyle HorizontalAlign="Center">
				</HeaderStyle>
			</dx:GridViewDataTextColumn>
		</Columns>
		<SettingsPager PageSize="50">
		</SettingsPager>
		<Settings ShowTitlePanel="True" ShowFilterRow="True" ShowFilterRowMenu="True" ShowHeaderFilterButton="True" />
		<Styles>
			<Cell CssClass="td">
			</Cell>
			<Row CssClass="tr">
			</Row>
			<DetailRow CssClass="tr">
			</DetailRow>
			<DetailCell CssClass="td">
			</DetailCell>
			<HeaderPanel  CssClass="thead">
			</HeaderPanel>
			<TitlePanel CssClass="thead"  HorizontalAlign="Center" Font-Bold="True">
			</TitlePanel>
		</Styles>
		<Styles>
			<Row CssClass="tr">
			</Row>
		</Styles>

		<Templates>
			<TitlePanel>
				<table cellpadding="0" class="thead" cellspacing="0" style="width:100%;">
					<thead style='background-color:#27c;color:#fff;font-size:12px;padding:3px;''>
						<tr>
							<th align='left' style='padding:5px;padding-left:5px;font-weight:normal'>Name</th>
							<th style='padding:5px;font-weight:normal'>Quantity</th>
							<th style='padding:5px;font-weight:normal'>Min</th>
							<th style='padding:5px;font-weight:normal'>Max</th>
							<th style='padding:5px;font-weight:normal'>&nbsp;</th>
						</tr>
					</thead>
					<tr class="tr">
						<td align="left" class="th" style="padding: 0px 5px 4px;">
							<asp:DropDownList ID="add_name" runat="server" CssClass="name" DataSourceID="sds_masterlocations2" DataTextField="name" DataValueField="id" Width="500px">
							</asp:DropDownList>
						</td>
						<td align="center" class="th" style="padding: 3px 6px 4px;" width="100">
							<table cellpadding='0' cellspacing='0' class="qty" data-draggable="no" data-original_qty='0' data-id='' runat="server">
								<tr>
									<th><input type="text" id="qty" oninit="gv_partlocations_hdr_qty_init" runat="server" onkeydown="only_numeric(event)" style="width:100px"/></th>
									<th><b style="display:block;width:16px">&nbsp;</b></th>
								</tr>
							</table>
						</td>
						<td align="center" class="th" style="padding: 3px 6px 4px;" width="100">
							<input type="text" class="min" onkeydown="only_numeric(event)" style="width:100px"/>
						</td>
						<td align="center" class="th" style="padding: 3px 6px 4px;" width="100">
							<input type="text" class="max" onkeydown="only_numeric(event)" style="width:100px"/>
						</td>
						<td class="th" style="padding: 3px 6px 4px;" width="100">
							<button type="button" onclick="save_location(this);"><img src="/images/icon/icon[save].gif" align="absmiddle"> Save</button></td>
					</tr>
				</table>
			</TitlePanel>
		</Templates>
		<Border BorderWidth="0px" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="sds_partlocations" 
		ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" 
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" runat="server" SelectCommand="(SELECT 
	a.id,
	a.location_master_id,
	a.qty,
	b.name,
	IFNULL(a.min, 0) min,
	IFNULL(a.max, 0) max,
IF(b.type_id = 1, &quot;Internal&quot;, &quot;External&quot;) type
FROM 
	inventory_location a
LEFT JOIN
	inventory_location_master b ON a.location_master_id = b.id
WHERE 
	a.master_id = @master_id AND 
	a.business_unit_id = @warehouse_bu_id and b.type_id=1 
ORDER BY b.type_id ASC, a.qty DESC limit 500) 
UNION
(
SELECT 
	c.id,
	c.location_master_id,
	c.qty,
	d.name,
	IFNULL(c.min, 0) min,
	IFNULL(c.max, 0) max,
IF(d.type_id = 1, &quot;Internal&quot;, &quot;External&quot;) type
FROM 
	inventory_location c
LEFT JOIN
	inventory_location_master d ON c.location_master_id = d.id
WHERE 
	c.master_id = @master_id AND 
	c.business_unit_id = @warehouse_bu_id and d.type_id=2 
ORDER BY d.name ASC limit 500)">
		<SelectParameters>
			<asp:SessionParameter Name="@master_id" SessionField="working_master_id" />
			<asp:SessionParameter Name="@warehouse_bu_id" SessionField="working_warehouse_bu_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sds_masterlocations"  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"  runat="server" SelectCommand="SELECT a.id,CONCAT(IF(a.type_id = 1, 'Inl - ', 'Exl - '), a.name) name,a.type_id FROM inventory_location_master a WHERE a.business_unit_id = @warehouse_bu_id AND a.id NOT IN (SELECT location_master_id FROM inventory_location WHERE company_Id = @business_unit_id AND location_master_id  IS NOT NULL) ORDER BY name">
		<SelectParameters>
			<asp:SessionParameter Name="@warehouse_bu_id" SessionField="working_warehouse_bu_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sds_masterlocations2"  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"  runat="server" SelectCommand="SELECT a.id,CONCAT(IF(a.type_id = 1, 'Inl - ', 'Exl - '), a.name) name,a.name barename,a.type_id FROM inventory_location_master a WHERE a.business_unit_id = @warehouse_bu_id AND a.id not in (select location_master_id from inventory_location where masteR_id = @master_id AND location_master_id  IS NOT NULL) ORDER BY a.type_id ASC, a.name ASC">
		<SelectParameters>
			<asp:SessionParameter Name="@warehouse_bu_id" SessionField="working_warehouse_bu_id" />
			<asp:SessionParameter Name="@master_id" SessionField="working_master_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sds_masterlocations3"  ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>" ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>"  runat="server" SelectCommand="SELECT a.id,CONCAT(IF(a.type_id = 1, 'Inl - ', 'Exl - '), a.name) name,a.name barename,a.type_id FROM inventory_location_master a WHERE a.business_unit_id = @warehouse_bu_id ORDER BY name">
		<SelectParameters>
			<asp:SessionParameter Name="@warehouse_bu_id" SessionField="working_warehouse_bu_id" />
		</SelectParameters>
	</asp:SqlDataSource>
	<uc:master_locations ID="uc_master_locations" runat="server" Visible="false" />
	<dx:ASPxCallbackPanel id="cbp_rfq" runat="server" Visible="False" Width="100%">
		<panelcollection>
<dx:PanelContent runat="server">
			<asp:UpdatePanel ID="line_card" runat="server" RenderMode="Inline" 
		Visible="False">
				<ContentTemplate>
					<table>
						<tr>
							<td align="center" style="width: 221px; height: 41px;" valign="middle">
								<dx:ASPxTextBox ID="kitted_part_search0" runat="server" AutoPostBack="True" 
									Width="170px">
									<ClientSideEvents Init="function(s, e) {
	var _obj		= s.GetMainElement();
	$(_obj).inventory({
						force_clickable:true
						});
}" />
								</dx:ASPxTextBox>
								<br />
							</td>
							<td style="width: 100px; height: 41px;" valign="top">
								<dx:ASPxButton ID="Update" runat="server" OnClick="kitted_search_Click" 
									Text="Search Kits">
								</dx:ASPxButton>
							</td>
							<td style="width: 100px; height: 41px" valign="top">
								<input id="Button3" type="button" value="New Kit" onclick="boing('/sections/member/picklist/pikclist.aspx?origin=kitted&id=', 'kitted',950, 800);" />
							</td>
						</tr>
					</table>
					<dx:ASPxGridView ID="gv_linecard" runat="server" AutoGenerateColumns="False" 
						Cursor="pointer" KeyFieldName="id" 
						OnRowDeleting="kitted_hdrs_CustomButtonCallback" Visible="False" Width="100%">
						<SettingsBehavior EnableRowHotTrack="True" />
						<Border BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
						<ClientSideEvents RowClick="function(s, e) {
var keyValue = s.GetRowKey(e.visibleIndex);
	boing('/sections/member/picklist/pikclist.aspx?origin=kitted&amp;id='+keyValue, 'kitted'+keyValue, 950, 800);
}" />
                        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
						<Columns>
							<dx:GridViewCommandColumn AllowDragDrop="False" ButtonType="Image"  ShowDeleteButton="true"
								Caption="Action" VisibleIndex="0" Width="5%">
								
								<HeaderStyle BackColor="#3399ff" ForeColor="White" HorizontalAlign="Center" />
							</dx:GridViewCommandColumn>
							<dx:GridViewDataTextColumn Caption="Kit ID" FieldName="id" VisibleIndex="1" 
								Width="5%">
								<HeaderStyle BackColor="#3399ff" ForeColor="White" HorizontalAlign="Center" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Kit Name" FieldName="name" VisibleIndex="2" 
								Width="15%">
								<HeaderStyle BackColor="#3399ff" ForeColor="White" />
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Created By" FieldName="created_by" 
								VisibleIndex="3" Width="10%">
								<HeaderStyle BackColor="#3399ff" ForeColor="White" HorizontalAlign="Center" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Last Updated" FieldName="edited_dt" 
								VisibleIndex="4" Width="15%">
								<HeaderStyle BackColor="#3399ff" ForeColor="White" HorizontalAlign="Center" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="# of Parts" FieldName="n_parts" 
								VisibleIndex="5" Width="5%">
								<HeaderStyle BackColor="#3399ff" ForeColor="White" HorizontalAlign="Center" />
								<CellStyle HorizontalAlign="Center">
								</CellStyle>
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Sell Price" FieldName="sell" 
								VisibleIndex="6">
								<HeaderStyle BackColor="#3399ff" ForeColor="White" />
							</dx:GridViewDataTextColumn>
							<dx:GridViewDataTextColumn Caption="Notes" FieldName="Notes" VisibleIndex="7" 
								Width="50%">
								<HeaderStyle BackColor="#3399ff" ForeColor="White" />
							</dx:GridViewDataTextColumn>
						</Columns>
						<Settings ShowFilterRow="True" />
					</dx:ASPxGridView>
				</ContentTemplate>
	</asp:UpdatePanel>
	</dx:PanelContent>
</panelcollection>
	</dx:ASPxCallbackPanel>
	<br />
	<asp:UpdatePanel ID="up_consignment" runat="server" Visible="False">
		<ContentTemplate>
	<dx:ASPxGridView ID="gv_consignment" runat="server" AutoGenerateColumns="False" 
				ClientInstanceName="gv_consignment" DataSourceID="sds_consignment"
		KeyFieldName="id" Width="100%" 
				OnHtmlEditFormCreated="gv_consignment_HtmlEditFormCreated" 
				OnHtmlDataCellPrepared="gv_consignment_HtmlDataCellPrepared" 
				 Theme="NETheme01">
        <SettingsCommandButton>
	<EditButton Text="Edit" Image-Url="~/images/icon/icon[edit].gif" Image-Width="16px" Image-Height="16px"></EditButton>
	<NewButton Text="Add New" Image-Url="~/images/icon/icon[add].gif" Image-Width="16px" Image-Height="16px"></NewButton>
	<DeleteButton Text="Delete" Image-Url="~/images/icon/icon[delete].gif" Image-Width="16px" Image-Height="16px"></DeleteButton>
</SettingsCommandButton>
		<Columns>
            <dx:GridViewCommandColumn ButtonType="Image" Caption="Edit" VisibleIndex="10" Width="35px" ShowEditButton="true">
                
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="id" ReadOnly="True" VisibleIndex="0">
                <EditFormSettings Visible="False" />
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Customer Name" FieldName="customer_name" VisibleIndex="1">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="2" Width="150px">
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn Caption="Date Entered" FieldName="date_entered" VisibleIndex="4"
                Width="100px">
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn Caption="Date Returned" FieldName="date_returned" VisibleIndex="5"
                Width="100px">
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn Caption="Serial #" FieldName="serial" VisibleIndex="6">
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Quoted Price" ReadOnly="True" VisibleIndex="7"
                Width="75px">
                <PropertiesTextEdit DisplayFormatString="{0:C2}">
                </PropertiesTextEdit>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Note" FieldName="note" VisibleIndex="3" Width="35px">
                <DataItemTemplate>
                    <%# note_handler(Container)%>
                </DataItemTemplate>
                <CellStyle HorizontalAlign="Center">
                </CellStyle>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Work Order" VisibleIndex="9">
                <PropertiesTextEdit DisplayFormatString="{0}">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Quote" VisibleIndex="8">
                <PropertiesTextEdit DisplayFormatString="{0}">
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
		</Columns>
	
	
		
		<StylesEditors>
			<CalendarHeader Spacing="1px">
			</CalendarHeader>
			<ProgressBar Height="25px">
			</ProgressBar>
		</StylesEditors>
		<Templates>
			<EditForm>
				<table cellpadding="5" cellspacing="0" width="100%" id="consignment_edit" data-id="<%# Eval("id") %>">
					<tr>
						<td style="font-weight: bold; width: 100px">
							Customer:</td>
						<td>
							<asp:HiddenField ID="h_id" runat="server" Value='<%# Eval("id") %>' />
							<asp:TextBox ID="t_customer" runat="server" CssClass="t_customer" Text='<%# Eval("customer_name") %>'
								Width="250px"></asp:TextBox></td>
					</tr>
					<tr>
						<td style="font-weight: bold; width: 100px">
							Serial #:</td>
						<td>
							<asp:TextBox ID="t_serial" runat="server" CssClass="t_serial" Text='<%# Eval("serial") %>'
								Width="200px"></asp:TextBox></td>
					</tr>
					<tr>
						<td style="font-weight: bold; width: 100px">
							Status:</td>
						<td>
							<asp:DropDownList ID="DropDownList1" runat="server" CssClass="s_status" SelectedValue='<%# Bind("status") %>'>
								<asp:ListItem>Waiting to be Quoted</asp:ListItem>
								<asp:ListItem>Quoted</asp:ListItem>
								<asp:ListItem>In Process</asp:ListItem>
								<asp:ListItem>Testing</asp:ListItem>
								<asp:ListItem>Returned - Repaired</asp:ListItem>
								<asp:ListItem>Returned - Not Repaired</asp:ListItem>
								<asp:ListItem>Mothballed</asp:ListItem>
							</asp:DropDownList></td>
					</tr>
					<tr>
						<td style="font-weight: bold; width: 100px">
						</td>
						<td>
							<table cellpadding="2" cellspacing="0">
								<tr>
									<td align="center" valign="top" style="width: 100px">
										<dx:ASPxButton ID="b_cancel" runat="server" AutoPostBack="False" Text="Cancel" OnClick="b_cancel_Click" TabIndex="56" UseSubmitBehavior="False">
											<Image Url="~/images/icon/icon[undo].gif">
											</Image>
										</dx:ASPxButton>
										&nbsp;</td>
									<td valign="top">
										<dx:ASPxButton ID="b_save" runat="server" Text="Save" TabIndex="55" AutoPostBack="False" UseSubmitBehavior="False">
											<Image Url="~/images/icon/icon[save].gif">
											</Image>
											<ClientSideEvents Click="function(s, e) {
	consignment_save(s)
}" />
										</dx:ASPxButton>
									</td>
									<td class="style2" valign="top">
										<dx:ASPxButton ID="b_delete" runat="server" onclick="b_delete_Click" Text="Delete">
											<ClientSideEvents Click="function(s, e) {
	if(!confirm(&quot;Are you sure you want to delete this repair?&quot;))
		{
		e.ProcessOnServer = false;
		}
}" />
											<Image Url="~/images/icon/icon[delete].gif">
											</Image>
										</dx:ASPxButton>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</EditForm>
		</Templates>
		<SettingsEditing Mode="EditForm" />
		<SettingsBehavior EnableRowHotTrack="True" />
		<ClientSideEvents EndCallback="function(s, e) {
	bind_tooltips()
}" />
	</dx:ASPxGridView>
	<asp:SqlDataSource ID="sds_consignment" runat="server" ConnectionString="<%$ ConnectionStrings:MySQLdotnet %>"
		ProviderName="<%$ ConnectionStrings:MySQLdotnet.ProviderName %>" SelectCommand="SELECT a.id,a.customer_id, URLDECODE(b.customer_name) customer_name, a.status,a.date_entered,a.date_returned,a.serial serial,a.repair_price,a.note note FROM inventory_consignment a LEFT JOIN customer b ON a.customer_id = b.customer_id WHERE a.business_unit_id = @business_unit_id AND a.master_id = @master_id&#13;&#10; order by a.id desc">
		<SelectParameters>
			<asp:SessionParameter Name="@business_unit_id" SessionField="working_business_unit_id" />
			<asp:QueryStringParameter Name="@master_id" QueryStringField="id" />
		</SelectParameters>
	</asp:SqlDataSource>
			
			<br />
			<br />
			<br />
		</ContentTemplate>
	</asp:UpdatePanel>
		<dx:ASPxPopupControl ID="popbcprint" runat="server" AllowDragging="True" Modal="True" popupanimationtype="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popbcprint" HeaderText=" ">
			<ContentStyle>
				<Paddings Padding="10px" />
			</ContentStyle>
			<ContentCollection>
				<dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
					<dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="200px">
						<PanelCollection>
							<dx:PanelContent runat="server" SupportsDisabledAttribute="True">
								<dx:ASPxButtonEdit ID="qty_to_print" runat="server" ClientInstanceName="qty_to_print" OnButtonClick="qty_to_print_ButtonClick" HorizontalAlign="Right" NullText="Enter Qty" Spacing="3" Text="1">
									<Buttons>
										<dx:EditButton Text="Print">
											<Image Url="~/images/icon/icon[print_barcode].GIF">
											</Image>
										</dx:EditButton>
									</Buttons>
									<NullTextStyle BackColor="#FFFFCC">
									</NullTextStyle>
								</dx:ASPxButtonEdit>
								<dx:ASPxHiddenField ID="hidbcmastertid" runat="server" 
									ClientInstanceName="hidbcmastertid">
								</dx:ASPxHiddenField>
							</dx:PanelContent>
						</PanelCollection>
					</dx:ASPxCallbackPanel>
				</dx:PopupControlContentControl>
			</ContentCollection>
		</dx:ASPxPopupControl>
	<dx:ASPxPopupControl ID="pop_files" runat="server" 
		ClientInstanceName="pop_files" CloseAction="CloseButton" HeaderText="Files" 
		Height="500px" Modal="True" Width="800px" 
		onclientlayout="pop_files_ClientLayout" PopupHorizontalAlign="WindowCenter" 
		PopupVerticalAlign="WindowCenter">
		<ContentCollection>
<dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
	<iframe ID="I1" runat="server" frameborder="0" height="450" name="I1" 
		scrolling="no"  class="popup_files_iframe"
		style="border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none" 
		width="100%"></iframe>
			</dx:PopupControlContentControl>
</ContentCollection>
	</dx:ASPxPopupControl>
	<br />
	<uc1:part_transfer ID="uc_part_transfer" runat="server" Visible="False" />
	<br />
</asp:Content>

<asp:Content ID="Content4" runat="server" contentplaceholderid="header_placeholder">
	<link type="text/css" rel="Stylesheet" href="/css/inventory_branch.css" />
	<style type="text/css">
		.style2 {
			width: 127px;
		}
	</style>
</asp:Content>


