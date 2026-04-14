using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using DevExpress.XtraScheduler;
using nesi.core;

namespace nesi.core
{
	[Serializable]
	public class NeAppointment
	{
		object id;
		DateTime start;
		DateTime end;
		string subject;
		int status;
		string description;
		int label;
		string location;
		bool allday;
		int eventType;
		string recurrenceInfo;
		string reminderInfo;
		object ownerId;
		int _woprog_id = 0;
		int _quote_id = 0;
		int _assetid = 0;
		int _ResourceId = 0;
		int _membertype_id = 0;
		int _confirmed = 0;
		string _notes = "";
		bool _meet_at_shop = true;
		DateTime _date_of_entry;

		public NeAppointment()
		{
		}

		public DateTime StartTime { get { return start; } set { start = value; } }
		public DateTime EndTime { get { return end; } set { end = value; } }
		public string Subject { get { return subject; } set { subject = value; } }
		public int Status { get { return status; } set { status = value; } }
		public string Description { get { return description; } set { description = value; } }
		public int Label { get { return label; } set { label = value; } }
		public string Location { get { return location; } set { location = value; } }
		public bool AllDay { get { return allday; } set { allday = value; } }
		public int EventType { get { return eventType; } set { eventType = value; } }
		public string RecurrenceInfo { get { return recurrenceInfo; } set { recurrenceInfo = value; } }
		public string ReminderInfo { get { return reminderInfo; } set { reminderInfo = value; } }
		public object OwnerId { get { return ownerId; } set { ownerId = value; } }
		public object Id { get { return id; } set { id = value; } }
		public int business_unit_id { get; set; }
		public int woprog_id { get { return _woprog_id; } set { _woprog_id = value; } }
		public int quote_id { get { return _quote_id; } set { _quote_id = value; } }
		public int ResourceId { get { return _ResourceId; } set { _ResourceId = value; } }
		public int setby { get; set; }
		public int member_id { get; set; }
		public int membertype_id { get { return _membertype_id; } set { _membertype_id = value; } }
		public int confirmed { get { return _confirmed; } set { _confirmed = value; } }
		public string notes { get { return _notes; } set { _notes = value; } }
		public bool meet_at_shop { get { return _meet_at_shop; } set { _meet_at_shop = value; } }
		public DateTime date_of_entry { get { return _date_of_entry; } set { _date_of_entry = value; } }

		public int assetid { get { return _assetid; } set { _assetid = value; } }

		public NeAppointment(int id)
		{
			var dt = Toolbox.doSQL_dt(@"select * from appointments  where id = @v0", new object[] { id });
			{
				if (dt.Rows.Count > 0)
				{
					start = Convert.ToDateTime(dt.Rows[0]["StartDate"]);
					end = Convert.ToDateTime(dt.Rows[0]["EndDate"]);
					subject = dt.Rows[0]["Subject"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Subject"]) : "";
					status = dt.Rows[0]["Status"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Status"]) : 0;
					description = dt.Rows[0]["Description"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Description"]) : "";
					label = dt.Rows[0]["Label"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Label"]) : 0;
					location = dt.Rows[0]["Location"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["Location"]) : "";
					allday = dt.Rows[0]["Allday"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["Allday"]) : false;
					eventType = dt.Rows[0]["eventtype"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["eventtype"]) : 0;
					recurrenceInfo = dt.Rows[0]["RecurrenceInfo"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["RecurrenceInfo"]) : "";
					reminderInfo = dt.Rows[0]["ReminderInfo"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["ReminderInfo"]) : "";
					_ResourceId = dt.Rows[0]["ResourceId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["ResourceId"]) : 0;
					id = dt.Rows[0]["ID"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["ID"]) : 0;
					business_unit_id = dt.Rows[0]["business_unit_id"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["business_unit_id"]) : 0;
					_woprog_id = dt.Rows[0]["woprog_id"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["woprog_id"]) : 0;
					_quote_id = dt.Rows[0]["quote_id"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["quote_id"]) : 0;
					setby = dt.Rows[0]["setby"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["setby"]) : 0;
					_assetid = dt.Rows[0]["assetid"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["assetid"]) : 0;
					member_id = dt.Rows[0]["member_id"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["member_id"]) : 0;
					_membertype_id = dt.Rows[0]["membertype_id"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["membertype_id"]) : 0;
					_confirmed = dt.Rows[0]["confirmed"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["confirmed"]) : 0;
					_notes = dt.Rows[0]["notes"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["notes"]) : "";
					_meet_at_shop = dt.Rows[0]["meet_at_shop"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["meet_at_shop"]) : true;
					_date_of_entry = dt.Rows[0]["date_of_entry"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["date_of_entry"]) : new DateTime(1, 1, 1);
				}
			}

		}
		public void send_email_for_approval(DataTable dt)
		{
			var m = new NeEMail();
			m.passport_array = new ArrayList();

			if (dt.Rows.Count > 1)
			{
				var id_list = "";
				foreach (DataRow dr in dt.Rows)
				{
					id_list += dr[0].ToString() + "|";
				}
				id_list = id_list.TrimEnd('|');
				if (dt.Rows.Count > 1)
				{
					var pao = new passport.array_object();
					pao.url_yes = string.Format("/home.aspx?a=schedule_approve_all&all_apt_id={0}", id_list);
					pao.url_no = string.Format("/home.aspx?a=schedule_delete_all&all_apt_id={0}", id_list);
					pao.text_yes = "<div style='font-size:11px;'>Confirm ALL</div>";
					pao.text_no = "<div style='font-size:11px;'>Delete ALL</div> ";
					m.passport_array.Add(pao);
				}

			}

			foreach (DataRow dr in dt.Rows)
			{
				var apt = new NeAppointment(Convert.ToInt32(dr[0]));

				m.From = new NeMember(apt.setby).NEEmail;
				m.To = new NeMember(new NeMember(apt.member_id).reports_to).NEEmail;
				m.Subject = "Schedule needs to be confirmed";
				m.Body = "Schedule needs to be confirmed";
				m.to_member_id = new NeMember(new NeMember(apt.member_id).reports_to).id;

				if (apt.status == 97)
				{
					var pao = new passport.array_object();
					var whatever = " " + new NeMember(apt.member_id).FullName + " at " + new NeWOProg(apt.woprog_id).CustomerName + " on " + apt.start.ToString("yyyy-MM-dd") + " WO: " + new NeWOProg(apt.woprog_id).OrderNumber + " - set by : " + new NeMember(apt.setby).FullName;
					pao.url_yes = string.Format("/home.aspx?a=schedule_approve&apt_id={0}", Convert.ToInt32(dr[0]));
					pao.url_no = string.Format("/home.aspx?a=schedule_delete&apt_id={0}", Convert.ToInt32(dr[0]));

					pao.text_yes = "<div style='font-size:11px;'>Confirm" + whatever + "</div>";
					pao.text_no = "<div style='font-size:11px;'>Delete" + whatever + "</div>";

					m.passport_array.Add(pao);
				}

			}
			if (dt.Rows.Count > 0)
			{
				m.file_passport();
			}


		}

		public void Save()
		{
			if (id.ToString() == "0")
			{
				Toolbox.doSQL_void(@"Insert into appointments
 (Subject, Status, Description, StartDate,EndDate,ResourceId,business_unit_id,woprog_id,quote_id,setby,assetid,member_id,notes,meet_at_shop,date_of_entry) 
values(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14)",
new object[] {
	subject, //0
	status, //1
	description, //2
	start.ToString("yyyy-MM-dd HH:mm:ss"), //3
	end.ToString("yyyy-MM-dd HH:mm:ss"), //4
	_ResourceId, //5
	business_unit_id, //6
	woprog_id, //7
	quote_id, //8
	setby, //9
	_assetid, //10
	member_id, //11
	_notes, //12
	Convert.ToInt16(_meet_at_shop), //13
	_date_of_entry.ToString("yyyy-MM-dd HH:mm:ss") //14
});
			}
			else
			{
				Toolbox.doSQL_void(@"update appointments 
set Subject = @v0 ,
Status =@v1 ,
Description =@v2  ,
StartDate = @v3 ,
EndDate = @v4 ,
ResourceId = @v5 ,
business_unit_id =@v6 ,
woprog_id = @v7 ,
quote_id = @v8 ,
setby = @v9 ,
assetid = @v10 ,
member_id=@v11  ,
notes=@v12 ,
meet_at_shop =@v13
where id = @v14 
limit 1",
					new object[]
					{
						subject,
						status,
						description,
						start.ToString("yyyy-MM-dd HH:mm:ss"),
						end.ToString("yyyy-MM-dd HH:mm:ss"),
						_ResourceId,
						business_unit_id,
						_woprog_id,
						_quote_id,
						setby,
						_assetid,
						member_id,
						_notes,
						_meet_at_shop,
						id
					});
			}

		}
	}
}

#region #customobject

public class NeAppointments : CollectionBase
{
	// public NeAppointment this[int index]
	//{
	//   get
	//  {
	//     return (NeAppointment)List[index];
	//       }
	//  }

	//  public void Add(NeAppointment NeAppointmentToAdd)
	// {
	//    List.Add(NeAppointmentToAdd);
	// }

	//  public void Remove(int index)
	//  {
	//     List.RemoveAt(index);
	//}


}




[Serializable]
public class AppointmentList : BindingList<NeAppointment>
{
	public void AddRange(AppointmentList events)
	{
		foreach (var appointmentEvent in events)
			Add(appointmentEvent);
	}
	public int GetEventIndex(object eventId)
	{
		for (var i = 0; i < Count; i++)
			if (this[i].Id == eventId)
				return i;
		return -1;
	}
}




public class AppointmentDataSource
{
	AppointmentList events;
	public AppointmentDataSource(AppointmentList events)
	{
		if (events == null)
			DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("events");
		this.events = events;
	}
	public AppointmentDataSource()
		: this(new AppointmentList())
	{
	}
	public AppointmentList Events { get { return events; } set { events = value; } }



	#region ObjectDataSource methods
	public object InsertMethodHandler(NeAppointment customEvent)
	{


		object id = customEvent.GetHashCode();
		customEvent.Id = id;
		Events.Add(customEvent);
		return id;
	}
	public void DeleteMethodHandler(NeAppointment customEvent)
	{
		var eventIndex = Events.GetEventIndex(customEvent.Id);
		if (eventIndex >= 0)
			Events.RemoveAt(eventIndex);
	}
	public void UpdateMethodHandler(NeAppointment customEvent)
	{
		var eventIndex = Events.GetEventIndex(customEvent.Id);
		if (eventIndex >= 0)
		{
			Events.RemoveAt(eventIndex);  // remove the old one,
			Events.Insert(eventIndex, customEvent); // add the new one..  we want to do the same.

		}
	}
	public int Count { get { return Events.Count; } }
	public IEnumerable SelectMethodHandler()
	{
		var result = new AppointmentList();
		result.AddRange(Events);
		return result;
	}


	public object ObtainLastInsertedId()
	{
		if (Count < 1)
			return null;
		return Events[Count - 1].Id;
	}
	#endregion

}

public class CustomTimeScaleDay : TimeScaleDay
{
	private const double StartHour = 6.5d;

	public override DateTime Floor(DateTime date)
	{
		if (date == DateTime.MinValue)
			return date.AddHours(6).AddMinutes(30);

		var start = base.Floor(date);

		if (GetCorrectedTime(date) < StartHour)
			return RoundToHour(date.AddDays(-1), (int)StartHour);
		return start.AddHours(6).AddMinutes(30);
	}

	double GetCorrectedTime(DateTime someDate)
	{
		return someDate.Hour + someDate.Minute / 60d;
	}

	protected DateTime RoundToHour(DateTime date, int hour)
	{
		return new DateTime(date.Year, date.Month, date.Day, hour, 30, 0);
	}

}

public class CustomTimeScaleHalfHour : TimeScaleFixedInterval
{
	public CustomTimeScaleHalfHour() : base(TimeSpan.FromMinutes(30)) { }

	protected override string DefaultDisplayFormat { get { return "HH:mm"; } }
	protected override string DefaultMenuCaption { get { return "6:30-21:30"; } }

	private const double StartHour = 6.5d;
	private const double FinishHour = 21.5d;

	public override DateTime Floor(DateTime date)
	{
		var returnedValue = base.Floor(date);

		if (date == DateTime.MinValue || date == DateTime.MaxValue) return returnedValue;

		if (GetCorrectedTime(date) < StartHour)
			// Round down to the end of the previous working day.
			returnedValue = RoundToHour(date.AddDays(-1), (int)FinishHour);

		if (GetCorrectedTime(date) > FinishHour)
		{
			// Round down to the end of the current working day.
			returnedValue = RoundToHour(date.AddDays(1), (int)StartHour);
		}
		return returnedValue;
	}

	double GetCorrectedTime(DateTime someDate)
	{
		return someDate.Hour + someDate.Minute / 60d;
	}

	protected DateTime RoundToHour(DateTime date, int hour)
	{
		return new DateTime(date.Year, date.Month, date.Day, hour, 30, 0);
	}

	public override DateTime GetNextDate(DateTime date)
	{
		var returnedValue = base.GetNextDate(date);
		if (GetCorrectedTime(date) > FinishHour - 0.5)
		{
			// Round down to the end of the current working day.
			returnedValue = RoundToHour(date.AddDays(1), (int)StartHour);
		}
		return returnedValue;
	}

}



#endregion #customobject
