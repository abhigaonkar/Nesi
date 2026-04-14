		function preview(id)
			{
			if(id != null)
				{
				boing("/_tools/get_file/index.aspx?file_id="+id+"&iframe=true", preview, 768, 480); 
				}
			else
				{
				alert("Please select a file");
				}
			}
		var is_changing_focus_te		= false;
		var is_changing_focus_bu		= false;
		function switch_focus_te(s,e)
			{
			if(window['gv_bus'] !== undefined && gv_bus.GetFocusedRowIndex() > -1)
				{
				gv_bus.SetFocusedRowIndex(-1);
				}
			s.GetRowValues(e.visibleIndex, 'id', switch_taxentity);
			}
		function switch_focus_bu(s,e)
			{	
			if(gv_taxentity.GetFocusedRowIndex() > -1)
				{
				gv_taxentity.SetFocusedRowIndex(-1);
				}
			s.GetRowValues(e.visibleIndex, 'id', switch_businessunit);
			}
		function buentity_savesettings(s,e)
			{
			var cbtype	= working_taxentity_id > 0
							? 'TE'
							: 'BU'; 
			var cbid	= working_taxentity_id > 0
							? working_taxentity_id
							: working_businessunit_id; 
			var layoutid = lb_layouts.GetSelectedValues().toString();
			if(layoutid != null && (!lb_layouts.GetIsItemSelected(0) || (lb_layouts.GetIsItemSelected(0) && layoutid.indexOf(",") === -1)))
				{
				cbp_taxentity.PerformCallback('save|'+cbtype+'|'+cbid+'|'+lb_layouts.GetSelectedValues()+'|'+html_emaillayout.GetHtml());
				}
			else if(layoutid != null && lb_layouts.GetIsItemSelected(0) && layoutid.indexOf(",") > -1)
				{
				alert("You cannot include the 'Please Select' item when multiple layouts are selected.");
				lb_layouts.UnselectAll();
				}
			else
				{
				alert("Please select a default FVR layout before saving.")
				}
			}
		var working_taxentity_id		= 0;
		var working_businessunit_id		= 0;
		function switch_businessunit(vals)
			{
			working_taxentity_id		= 0;
			working_businessunit_id		= vals/1;
			cbp_taxentity.PerformCallback("load|BU|"+working_businessunit_id);
			}
		function switch_taxentity(vals)
			{
			working_businessunit_id		= 0;
			working_taxentity_id		= vals/1;
			cbp_taxentity.PerformCallback("load|TE|"+working_taxentity_id);
			}
		function htmlEncode(value)
			{
			return $('<div/>').text(value).html();
			}
		function toggle_active_file(id)
			{
			if(id != null)
				{
				if(combo_ed_default_file.GetValue() == id)
					{
					alert("Before this file's active status can be toggled, you must first remove this file as the default file for this category.");
					return;
					}
				callback_toggleactive.PerformCallback(id); 
				}
			else
				{
				alert("Please select a file");
				}
			}
		function remove_file(id, is_edit, is_edit_category)
			{
			if(id != null)
				{
				 if(is_edit_category && combo_ed_default_file.GetValue() == id)
						{
						alert("Before this can be removed, you must first remove this file as the default file for this category.");
						return;
						}
				if(confirm("Are you sure you want to remove this file?"))
					{
					if(is_edit && !is_edit_category)
						{
						callback_remove_edit.PerformCallback(id); 
						}
					else if(!is_edit_category)
						{
						callback_remove.PerformCallback(id); 
						}
					else if(is_edit_category)
						{
						if(combo_ed_default_file.GetValue() == id)
							{
							alert("Before this can be removed, you must first remove this file as the default file for this category.");
							return;
							}
						callback_remove_category.PerformCallback(id); 
						}
					}
				}
			else
				{
				alert("Please select a file");
				}
			}
		function setdefault(id, cat_id, fvr_type, is_edit)
			{
			if(id != null)
				{
				callback_setdefault.PerformCallback(id+"|"+cat_id+"|"+fvr_type);
				}
			else
				{
				alert("Please select a file");
				}
			}
		function handle_url(s,e)
			{
			var raw_id			= s.uniqueID.split("$");
			var CID				= raw_id[raw_id.length - 1];
			if(CID.match(/tb_url/g))
				{
				var suffix			= CID.replace("tb_url", "");
				var file_list		= eval("existing_files"+suffix);
				if(file_list != null)
					{
					file_list.UnselectAll();
					}
				}
			else
				{
				var suffix			= CID.replace("existing_files", "");
				var tb_url			= eval("tb_url"+suffix);
				if(tb_url != null)
					{
					tb_url.SetText("");
					}
				}
			}
	$("document").ready(function()
		{
		if(window["Sys"] !== undefined)
			{
			page_obj.update_panel_progress.bind();
			}
		$(".ttip").tip();
		});