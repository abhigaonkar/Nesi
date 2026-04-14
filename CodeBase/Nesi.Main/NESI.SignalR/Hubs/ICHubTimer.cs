using System.Web.Hosting;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using Microsoft.AspNet.SignalR;
using System.Web.Script.Serialization;


[assembly: OwinStartup(typeof(NESI.SignalR.Startup))]

namespace NESI.SignalR.Hubs
{
    public class IcHubTimer : IRegisteredObject
    {
        private readonly IHubContext _hub;
        private Timer _timer;
        private readonly int _timerInterval;
        private int _count;

        public IcHubTimer()
        {
            //System.Diagnostics.Debug.WriteLine("HUBTIMER: CONSTRUCTOR");

            // GET HUB CONTEXT
            _hub = GlobalHost.ConnectionManager.GetHubContext("Clock"); // .GetHubContext();

            // IN MILLISECONDS (15000 = 15 SECONDS, 30000 = 30 SECONDS, 60000 = 60 SECONDS)
            _timerInterval = 1000;

            // START THE TIMER
            StartTimer();
        }

        private void StartTimer()
        {
            //System.Diagnostics.Debug.WriteLine("HUBTIMER: STARTTIMER");

            // SET TIMER UP WITH INTERVAL
            _timer = new Timer(_timerInterval);

            // ADD HANDLER TO TIMER.ELAPSED EVENT
            _timer.Elapsed += OnTimerElapsed;
            _count = 0;
            // START THE TIMER GOING
            _timer.Start();
        }

        private void OnTimerElapsed(object source, ElapsedEventArgs e)
        {
            var msg = "test" + _count.ToString();
            _count++;
            var json = new JavaScriptSerializer().Serialize(msg);
            _hub.Clients.All.Message(json);
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }
    }

}
