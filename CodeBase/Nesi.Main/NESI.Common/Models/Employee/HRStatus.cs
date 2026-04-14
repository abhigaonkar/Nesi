namespace NESI.Common.Models
{
	public struct HRStatus
	{
		public struct ByName
		{
			public const string New = "New";
			public const string WaitingforInitialSetup = "Waiting for Initial Setup";
			public const string Approved = "Approved";
			public const string PendingClosure = "Pending Closure";
			public const string Past = "Past";
			public const string Probation = "Probation";
		}
		public struct ByID
		{
			public const int New = 1;
			public const int WaitingforInitialSetup = 2;
			public const int Approved = 3;
			public const int PendingClosure = 4;
			public const int Past = 5;
			public const int Probation = 6;
		}
	}
}