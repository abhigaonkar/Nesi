function select_box(obj, _which, _id, _where, _whereval)
		{
		var _select							= "";
		var _sql							= "";
		switch(_which)
			{
			case "company":	_sql			= "SELECT business_unit_id id, name name from business_unit ";
			break;
			case "member":	_sql			= "SELECT member_id id, CONCAT(member_firstname, ' ', member_lastname) name FROM member";
			break;
			}
			
		if(_where != null && _where != "" && _whereval != "" && _whereval != null)
			{
			_sql		+= " WHERE "+_where+" = \""+_whereval+"\"";
			}
		$.ajax(	{
				type:		"GET",
				url:		'/_tools/select_xml/index.aspx',
				data:		'query='+_sql,
				beforeSend:	function()
								{
								$(obj).empty();
								},
				success:	function(xml)
								{
								$(xml).find('ROW').each(function()
															{
															var this_id			= $(this).children('id').text();
															var this_name		= $(this).children('name').text();
															var selected		= "";
															if(this_id == _id)
																{
																selected		= "selected";
																}
															else
																{
																selected		= "";
																}
															_select			+= "<option value='"+this_id+"' "+selected+">"+this_name+"</option>";
															});
								},
				error:		function(XMLHttpRequest, textStatus, errorThrown)
								{
								_select				+= "<option>There was an error retrieving data</option></select>";
								return _select;
								},
				complete:	function()
								{
								return _select;
								}
				});
				
		}