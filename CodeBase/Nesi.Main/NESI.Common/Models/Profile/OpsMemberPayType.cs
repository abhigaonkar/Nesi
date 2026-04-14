namespace NESI.Common.Models
    {
    public struct OpsMemberPayType
        {
        /// <summary>
        /// 0
        /// </summary>
        public const int NotSet                          = 0;
        /// <summary>
        /// 1
        /// </summary>
        public const int Hourly                          = 1;
        /// <summary>
        /// 2
        /// </summary>
        public const int SalaryHourlyWithTimeSheet       = 2;
        /// <summary>
        /// 3
        /// </summary>
        public const int SalaryHourlyWithoutTimeSheet    = 3;
        /// <summary>
        /// 4
        /// </summary>
        public const int Subcontract                     = 4;
        /// <summary>
        /// 5 - Due to naming constraints, this is labeled as f1099 (US IRS [F]orm 1099).
        /// </summary>
        public const int f1099                           = 5;
        /// <summary>
        /// 6
        /// </summary>
        public const int CoOp                            = 6;
        /// <summary>
        /// 7
        /// </summary>
        public const int Owner                           = 7;
        }
    }