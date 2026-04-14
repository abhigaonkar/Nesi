namespace NESI.Common.Models
	{
	public struct OpsPayroll
		{
		public struct Vacation
			{
			public struct PaymentMethod
				{
				/// <summary>
				/// 1
				/// </summary>
				public const int WithoutPay     = 1;
				/// <summary>
				/// 2
				/// </summary>
				public const int VacationPay    = 2;
				/// <summary>
				/// 3
				/// </summary>
				public const int AllOutstanding = 3;
				/// <summary>
				/// 4
				/// </summary>
				public const int PelPay         = 4;
				}
			public struct Status
				{
				/// <summary>
				/// 1
				/// </summary>
				public const int Pending        = 1;
				/// <summary>
				/// 2
				/// </summary>
				public const int Denied         = 2;
				/// <summary>
				/// 3
				/// </summary>
				public const int Approved       = 3;
				/// <summary>
				/// 4
				/// </summary>
				public const int Cancelled      = 4;
				/// <summary>
				/// 5
				/// </summary>
				public const int PaidOut        = 5;
				}
			public struct Type
				{
				/// <summary>
				/// 1
				/// </summary>
				public const int UnpaidTimeOff			= 1;
				/// <summary>
				/// 2
				/// </summary>
				public const int SickDayUnpaid			= 2;
				/// <summary>
				/// 3
				/// </summary>
				public const int LateAppearance			= 3;
				/// <summary>
				/// 4
				/// </summary>
				public const int ScheduledVacation		= 4;
				/// <summary>
				/// 5
				/// </summary>
				public const int AppSchool				= 5;
				/// <summary>
				/// 6
				/// </summary>
				public const int DisciplineDayOff		= 6;
				/// <summary>
				/// 7
				/// </summary>
				public const int ShortageOfWork			= 7;
				/// <summary>
				/// 8
				/// </summary>
				public const int LieuDay				= 8;
				/// <summary>
				/// 9
				/// </summary>
				public const int PEL					= 9;
				/// <summary>
				/// 10
				/// </summary>
				public const int SickDayPaid			= 10;
				/// <summary>
				/// 11
				/// </summary>
				public const int SickDayOther			= 11;
				/// <summary>
				/// 12
				/// </summary>
				public const int Birthday				= 12;
				/// <summary>
				/// 13
				/// </summary>
				public const int Bereavement			= 13;
				/// <summary>
				/// 14
				/// </summary>
				public const int AutoVacationPayout		= 14;
				}
			}
		public struct DaysOffType
			{
			public const int UnpaidTimeOff		= 1;
			public const int SickDayUnpaid		= 2;
			public const int LateAppearance		= 3;
			public const int Vacation			= 4;
			public const int AppSchool			= 5;
			public const int DisciplineDayOff	= 6;
			public const int ShortageOfWork		= 7;
			public const int LieuDay			= 8;
			public const int PEL				= 9;
			public const int SickDayPaid		= 10;
			public const int SickDayOther		= 11;
			public const int Birthday			= 12;
			public const int Bereavement		= 13;
			}
		public struct TransactionType
			{
			public struct ExtraPayment
				{
				public const string Commission = "C";
				public const string Bonus = "B";
				public const string Miscellaneous = "M";
				}

			public struct BankedPay
				{
				public const string Deposited = "D";
				public const string Withdrawn = "W";
				public const string PaidOut = "P";
				public const string Deducted = "E";
				}
			}

		public struct PayrollExport
		{
			public struct PayrollEntryType
			{ 
				public const string RT = "01";

				public const string OT = "02";

				public const string DT = "04";

				public const string RTSP = "18";

				public const string OTSP = "19";

				public const string DTSP = "20";

				public const string VAC = "14";
				public const string PAIDSICKDAY = "13";
				public const string PAIDOTHER = "12";
				public const string BON = "26";

				public const string PerDiem = "78";

				public const string EXP = "79";

				public const string BANK = "91";

				public const string BDAY = "31";
				public const string BEREAVEMENT = "16";
				public const string COMM = "28";
				public const string STAT = "08";
			}
			public struct ExportDetCodes
			{
				public const string REG							= "REG";
				public const string OT							= "OT";
				public const string DT							= "DT";
				public const string RTShiftPrem					= "RTShiftPrem";
				public const string DTShiftPrem					= "DTShiftPrem";
				public const string OTShiftPrem					= "OTShiftPrem";
				public const string VAC							= "VAC";
				public const string VACWITH						= "VACWITH";
				public const string ExpenseReimbursement		= "ExpenseReimbursement";
				public const string ONCAL						= "ONCAL";
				public const string SICK						= "SICK";
				public const string BONUS						= "BONUS";
				public const string Commission					= "Commission";
				public const string PerDiem						= "PerDiem";
				public const string BDAY						= "BDAY";
				public const string BEREAVE						= "BEREAVE";
				public const string STAT = "STAT";

			}	
        public struct Fields
            {
            public struct PayWorks
                {
                /// <summary>
                /// Number
                /// </summary>
                public static string Number            = "Number";
                /// <summary>
                /// Rate
                /// </summary>
                public static string Rate              = "Rate";
                /// <summary>
                /// Reg. Hrs
                /// </summary>
                public static string RTHours           = "Reg. Hrs";
                /// <summary>
                /// Overtime (E)
                /// </summary>
                public static string OTHours           = "Overtime (E)";
                /// <summary>
                /// Double Time (E)
                /// </summary>
                public static string DTHours           = "Double Time (E)";
                /// <summary>
                /// Premium 10% RT(E)
                /// </summary>
                public static string RTSPHours         = "Premium 10% RT(E)";
                /// <summary>
                /// Premium 10% OT(E)
                /// </summary>
                public static string OTSPHours         = "Premium 10% OT(E)";
                /// <summary>
                /// Premium 10% DT(E)
                /// </summary>
                public static string DTSPHours         = "Premium 10% DT(E)";
                /// <summary>
                /// Banked Hours IN (E)
                /// </summary>
                public static string BankedIn          = "Banked Hours IN (E)";
                /// <summary>
                /// Bank Time Paid (E)
                /// </summary>
                public static string BankedOut         = "Bank Time Paid (E)";
                /// <summary>
                /// Vac. Taken
                /// </summary>
                public static string VacationTaken     = "Vac. Taken";
                /// <summary>
                /// Vac. Withd.
                /// </summary>
                public static string VacationWithdrawn = "Vac. Withd.";
                /// <summary>
                /// Reimbursement (E)
                /// </summary>
                public static string Reimbursement     = "Reimbursement (E)";
                /// <summary>
                /// On Call (E)
                /// </summary>
                public static string OnCall            = "On Call (E)";
                /// <summary>
                /// Sick Pay(E)
                /// </summary>
                public static string SickPay           = "Sick Pay(E)";
                /// <summary>
                /// Sick Pay Other (E)
                /// </summary>
                public static string SickPayOther      = "Sick Pay Other (E)";
                /// <summary>
                /// Bonus (E)
                /// </summary>
                public static string Bonus             = "Bonus (E)";
                /// <summary>
                /// Sales Commission (E)
                /// </summary>
                public static string Commission        = "Sales Commission (E)";
                /// <summary>
                /// Per Diem (E)
                /// </summary>
                public static string PerDiem           = "Per Diem (E)";
                /// <summary>
                /// Happy Birthday(E)
                /// </summary>
                public static string Birthday          = "Happy Birthday(E)";
                /// <summary>
                /// Bereavement pay(E)
                /// </summary>
                public static string Bereavement       = "Bereavement pay(E)";
                /// <summary>
                /// Stat Pay
                /// </summary>
                public static string StatPay           = "Stat Pay";
                }
            public struct Paylocity
                {

                }
            }
		}
		}
	}