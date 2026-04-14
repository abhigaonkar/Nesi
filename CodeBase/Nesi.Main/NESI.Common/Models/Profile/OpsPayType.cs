namespace NESI.Common.Models
{
    public struct OpsPayType
    {
        public const int Regular                        = 1;
        public const int OverTime                       = 2;
        public const int DoubleTime                     = 3;
        public const int RegularTimeShiftPremium        = 4;
        public const int OverTimeShiftPremium           = 5;
        public const int DoubleTimeShiftPremium         = 6;
        public const int Travel                         = 7;
        public const int Mileage                        = 8;
		public struct Multipliers
			{
			public const double RegularTime             = 1;
			public const double OverTime                = 1.5;
			public const double DoubleTime              = 2;
			public const double RegularTimeShiftPremium = 1.1;
			public const double OverTimeShiftPremium    = 1.65;
			public const double DoubleTimeShiftPremium  = 2.2;
			}
    }
}