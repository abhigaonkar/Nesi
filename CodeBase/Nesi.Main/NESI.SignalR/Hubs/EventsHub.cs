using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
// ReSharper disable ArrangeAccessorOwnerBody

namespace NESI.SignalR.Hubs
{

	public static class Constants
	{
		public const string AdminChannel = "admin";
		public const string TaskChannel = "tasks";
	}

	/// <summary>
	/// A generic object to represent a broadcasted event in our SignalR hubs
	/// </summary>
	public class ChannelEvent
	{
		/// <summary>
		/// The name of the event
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The name of the channel the event is associated with
		/// </summary>
		public string ChannelName { get; set; }

		/// <summary>
		/// The date/time that the event was created
		/// </summary>
		public DateTimeOffset Timestamp { get; set; }

		/// <summary>
		/// The data associated with the event
		/// </summary>
		public object Data
		{
			get { return _data; }
			set
			{
				_data = value;
				this.Json = JsonConvert.SerializeObject(_data);
			}
		}
		private object _data;

		/// <summary>
		/// A JSON representation of the event data. This is set automatically
		/// when the Data property is assigned.
		/// </summary>
		public string Json { get; private set; }

		public ChannelEvent()
		{
			Timestamp = DateTimeOffset.Now;
		}
	}


	public class EventsHub : Hub
	{
		public async Task Subscribe(string channel)
		{
			await Groups.Add(Context.ConnectionId, channel);

			var ev = new ChannelEvent
			{
				ChannelName = Constants.AdminChannel,
				Name = "user.subscribed",
				Data = new
				{
					Context.ConnectionId,
					ChannelName = channel
				}
			};

			await Publish(ev);
		}

		public async Task Unsubscribe(string channel)
		{
			await Groups.Remove(Context.ConnectionId, channel);

			var ev = new ChannelEvent
			{
				ChannelName = Constants.AdminChannel,
				Name = "user.unsubscribed",
				Data = new
				{
					Context.ConnectionId,
					ChannelName = channel
				}
			};

			await Publish(ev);
		}


		public Task Publish(ChannelEvent channelEvent)
		{
			Clients.Group(channelEvent.ChannelName).OnEvent(channelEvent.ChannelName, channelEvent);

			if (channelEvent.ChannelName != Constants.AdminChannel)
			{
				// Push this out on the admin channel
				//
				Clients.Group(Constants.AdminChannel).OnEvent(Constants.AdminChannel, channelEvent);
			}

			return Task.FromResult(0);
		}


		public override Task OnConnected()
		{
			var ev = new ChannelEvent
			{
				ChannelName = Constants.AdminChannel,
				Name = "user.connected",
				Data = new
				{
					Context.ConnectionId,
				}
			};

			Publish(ev);

			return base.OnConnected();
		}


		public override Task OnDisconnected(bool stopCalled)
		{
			var ev = new ChannelEvent
			{
				ChannelName = Constants.AdminChannel,
				Name = "user.disconnected",
				Data = new
				{
					Context.ConnectionId,
				}
			};

			Publish(ev);

			return base.OnDisconnected(stopCalled);
		}

	


	public string SendToOthers(string sender, string message)
	{
		Clients.Others.Fire(new Event()
		{
			Name = sender,
			Message = message,
			Time = DateTime.Now
		});
		return "Fired Message send to Others Successed.";
	}

	public string SendToAll(string sender, string message)
	{
		Clients.All.Fire(new Event()
		{
			Name = sender,
			Message = message,
			Time = DateTime.Now
		});
		return "Fired Message send to All Successed.";
	}

	public string SendToAllAdvance(AdvanceEvent model)
	{
		Clients.All.Fire(model);
		return "Fired Advance event send to All Successed.";
	}

	//public void Connect()
	//{
	//	System.Web.Hosting.HostingEnvironment.RegisterObject(new IcHubTimer());
	//}


}

public class AdvanceEvent
{
	public int UserId { get; set; }
	public string From { get; set; }
	public string To { get; set; }
	public string Message { get; set; }
	public string Function { get; set; }
	public string Params { get; set; }
	public string Data { get; set; }
	public string Value { get; set; }
	public DateTime Time { get; set; }
}

public class Event
{
	public string Name { get; set; }
	public string Message { get; set; }
	public DateTime Time { get; set; }
}
}