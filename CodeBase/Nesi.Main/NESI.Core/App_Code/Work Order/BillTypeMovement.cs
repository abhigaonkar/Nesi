using System.Collections.Generic;
using NESI.Common.Models;

namespace nesi.core
	{
	public class BillTypeMovement
		{
		public static List<int> AllowedBillTypes(int fromBillType)
			{
			var AllowedBillTypeMovement = new Dictionary<int, List<int>>
												{	
													{OpsBillType.Regular, 
														new List<int>
															{
															OpsBillType.Regular,
															OpsBillType.DoNotInclude, 
															OpsBillType.VisibleNoCharge, 
															}
													},
													{OpsBillType.JobcostForQuote, 
														new List<int>
															{
															OpsBillType.JobcostForQuote,
															OpsBillType.DoNotInclude
															}
													},
													{OpsBillType.InvisibleCredit, 
														new List<int>
															{
															OpsBillType.InvisibleCredit,
															OpsBillType.DoNotInclude
															}
													},
													{OpsBillType.DoNotInclude,
														new List<int>
															{
															OpsBillType.Regular,			// T&M Job
															OpsBillType.JobcostForQuote,	// Quoted WO
															OpsBillType.InvisibleCredit,	// Credit WO
															OpsBillType.DoNotInclude,		// Itself
															OpsBillType.VisibleNoCharge,	// T&M Job
															OpsBillType.VisibleCredit		// Credit WO
															}
													},
													{OpsBillType.VisibleNoCharge,
														new List<int>
															{
															OpsBillType.Regular,
															OpsBillType.DoNotInclude,
															OpsBillType.VisibleNoCharge
															}
													},
													{OpsBillType.VisibleCredit,
														new List<int>
															{
															OpsBillType.InvisibleCredit,
															OpsBillType.DoNotInclude,
															OpsBillType.VisibleCredit
															}
													},
													// No Movement
													{OpsBillType.QuotedPrice, new List<int>()},
													// No Movement
													{OpsBillType.ProgressBilling, new List<int>()},
													// No Movement
													{OpsBillType.BalanceForward, new List<int>()}
												};
			return AllowedBillTypeMovement[fromBillType];
			}
		public static List<int> AllowedBillTypes(int fromBillType, NeWOProg wo)
			{
			List<int> listOfAllowedMovements;
			if(fromBillType == OpsBillType.DoNotInclude)
				{
				listOfAllowedMovements = wo.IsQuoted 
											? new List<int> { OpsBillType.JobcostForQuote }
											: wo.IsCredit 
												? new List<int> { OpsBillType.InvisibleCredit, OpsBillType.VisibleCredit}
												: wo.IsTM
													? new List<int> { OpsBillType.Regular }
													: new List<int>();
				}
			else
				{
				listOfAllowedMovements = AllowedBillTypes(fromBillType);
				}
			return listOfAllowedMovements;
			}
		}
	}
