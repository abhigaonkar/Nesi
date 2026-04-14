using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using nesi.core;

namespace HeartBeat
	{
	public partial class Service1 : ServiceBase
		{
		public Service1()
			{
			InitializeComponent();
			}
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

		protected override void OnStart(string[] args)
			{
			var timer = new Timer {Interval = Convert.ToInt32(Toolbox.app_setting("ServiceTimer"))};
			timer.Elapsed  += OnTimer;
			timer.Start();
			}

		private static void OnTimer(object sender, ElapsedEventArgs args)
			{
			Toolbox.doSQL_void(@"INSERT INTO matt (dt, jumble) VALUES (NOW(), 'Timer Elapsed')", new object[]{});
			}
		protected override void OnStop()
			{
			var serviceStatus = new ServiceStatus
											{
											dwCurrentState = ServiceState.SERVICE_STOP_PENDING, 
											dwWaitHint = 100000
											};
			SetServiceStatus(ServiceHandle, ref serviceStatus);

			// Update the service state to Stopped.
			serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
			SetServiceStatus(ServiceHandle, ref serviceStatus);
			}
		
		}


	internal enum ServiceState
		{
		SERVICE_STOPPED          = 0x00000001,
		SERVICE_START_PENDING    = 0x00000002,
		SERVICE_STOP_PENDING     = 0x00000003,
		SERVICE_RUNNING          = 0x00000004,
		SERVICE_CONTINUE_PENDING = 0x00000005,
		SERVICE_PAUSE_PENDING    = 0x00000006,
		SERVICE_PAUSED           = 0x00000007,
		}
	[StructLayout(LayoutKind.Sequential)]
	internal struct ServiceStatus
		{
		public int          dwServiceType;
		public ServiceState dwCurrentState;
		public int          dwControlsAccepted;
		public int          dwWin32ExitCode;
		public int          dwServiceSpecificExitCode;
		public int          dwCheckPoint;
		public int          dwWaitHint;
		}
	}
