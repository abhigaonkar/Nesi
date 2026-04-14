using System;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.DTO.ViewModels.Core.Enums;

namespace NESI.BLL.Common.Shared
{
	public class NeCurrency 
	{
		public double get_exchange_rate_at_date(Employee user, DateTime dt, string toCurrency)
		{
			var c = (Currency) Enum.Parse(typeof(Currency), toCurrency);
			return get_exchange_rate_at_date(user, dt, c);
		}

		public double get_exchange_rate_at_date(Employee user, DateTime dt, Currency toCurrency)
		{
			var db = new Data.Entities.NESIMySQL();
			double _exchange_rate = 1;
			var format_date = "";
			format_date = dt.ToString("yyyy-MM-dd");
			var _home_currency = user.Currency;
			if (toCurrency != _home_currency)
			{
				switch (toCurrency)
				{
					case Currency.USA:
						switch (_home_currency)
						{
							case Currency.CAN:
								_exchange_rate = db.Database.SqlQuery<double>(
									@"Select per_usd from currency_history  where currency = 'CAD' 
									and currency_history.date<=@p0  order by currency_history.date 
									desc limit 1", format_date).First();
								break;
							case Currency.EUR:
								_exchange_rate = db.Database.SqlQuery<double>(
									@"Select per_usd from currency_history  where currency = 'EUR' 
									and currency_history.date<=@p0  order by currency_history.date desc limit 1"
									,format_date ).First();
								break;
						}
						break;
					case Currency.CAN:
						switch (_home_currency)
						{
							case Currency.USA:
								_exchange_rate = db.Database.SqlQuery<double>(
									@"Select 1/per_usd from currency_history  where currency = 'CAD' 
								and currency_history.date<=@p0  order by currency_history.date desc limit 1",
									format_date).First();
								break;
							case Currency.EUR:
								_exchange_rate = db.Database.SqlQuery<double>(
										@"1/((Select per_usd from currency_history  where currency = 'CAD' 
										and currency_history.date<=@p0 order by currency_history.date desc 
										limit 1)/(Select per_usd from currency_history where currency = 'EUR' 
										and currency_history.date<=@p1 order by currency_history.date desc limit 1))", 
										format_date).First();
								break;
						}
						break;
					case Currency.EUR:
						switch (_home_currency)
						{
							case Currency.USA:
								_exchange_rate = db.Database.SqlQuery<double>(
									@"Select 1/per_usd from currency_history  where currency = 'EUR' 
									and currency_history.date<=@v0  order by currency_history.date desc limit 1",
									format_date).First();
								break;
							case Currency.CAN:
								_exchange_rate = db.Database.SqlQuery<double>(
									@"Select per_usd/(Select per_usd from currency_history  where currency = 'EUR' 
									and currency_history.date<=@p0 order by currency_history.date desc limit 1) 
									from currency_history where currency = 'CAD' and currency_history.date<=@p0 
									order by currency_history.date desc limit 1", 
									format_date).First();
								break;
						}
						break;
				}

			}



			return _exchange_rate;


		}
	}
}