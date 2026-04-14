using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csvProcessor
{
	class Program
	{
		static Dictionary<string, int> dict = new Dictionary<string, int>();
		static string sandbox = "PROD";
		static void Main(string[] args)
		{
			populateSubs();
			process("customer");
			process("vendor");
		}
		public static void process(string _type)
		{
			var i = 0;
			const int BufferSize = 128;
			var format = "HH:mm:ss";
			var sb = new List<string>();
			var BUDsb = new List<string>();
			BUDsb.Add($"DELETE FROM {_type}_business_unit WHERE {_type}_id IN (");

			var BUsb = new List<string>();
			BUsb.Add($"INSERT INTO {_type}_business_unit ({_type}_id, business_unit_id) VALUES \n");

			var Asb = new List<string>();

			var ActivateCustomers = "UPDATE customer SET active = 1, customer_ts = customer_ts WHERE customer_id IN (";
			var DeactivateCustomers = "UPDATE customer SET customer_status = 8, active = 0, customer_ts = customer_ts WHERE customer_id NOT IN (";
			var ActivateVendors = "UPDATE vendor SET vendor_active = 1, vendor_ts = vendor_ts WHERE vendor_id IN (";
			var DeactivateVendors = "UPDATE vendor SET vendor_active = 0, vendor_ts = vendor_ts WHERE vendor_id NOT IN (";
			var DeactivateAddressesCustomers = "UPDATE address SET active = 0, ts=ts WHERE address_table = 'customer' AND address_id NOT IN (";
			var DeactivateAddressesVendors = "UPDATE address SET active = 0, ts=ts WHERE address_table = 'vendor' AND address_id NOT IN (";

			var file = $@"C:\temp\NS_{_type}_list.csv";
			if(!File.Exists(file))
			{ 
				throw new Exception($"File doesn't exist - {file} - halting processor.");
			}
			var lines = File.ReadAllLines(file).Length;
			Console.WriteLine(lines+" lines");
			var processed = new List<string>();
			var processedAddresses = new List<string>();
			using (var f = File.OpenRead(file))
			{
				using (var streamReader = new StreamReader(f, Encoding.UTF8, true, BufferSize))
				{
					Console.WriteLine(DateTime.Now.ToString(format) + " - " + _type +": Start!");
					string line;
					var processedDeletions = new List<string>();
					var processedBUs = new Dictionary<string, List<int>>();
					while ((line = streamReader.ReadLine()) != null)
					{
						if (i != 0 && line.Trim() != "")
							{
							// Format has to be: NS Cust ID,NESI Cust ID,NS Address ID,NESI Address ID,Subsidiary ID
							var split = line.Split(',');
							var NSTypeId = split[0]; // Type = Customer/Vendor
							var NESITypeId = split[1];
							var NSAddressId = split[2];
							var NESIAddressId = split[3];
							var SubsidiaryId = split[4];
							//var InActive = split[5];
							if(!dict.ContainsKey(SubsidiaryId)) continue;
							var bu = dict[SubsidiaryId];
							if(!processed.Contains(NESITypeId))
								{ 
								sb.Add($"UPDATE {_type} SET netsuite_internal_id = {NSTypeId}, {_type}_ts = {_type}_ts WHERE {_type}_id = {NESITypeId};");
								processed.Add(NESITypeId);
								}
							if(!processedAddresses.Contains(NESIAddressId) && NESIAddressId != "" && NSAddressId != "")
								{ 
								Asb.Add($"UPDATE address SET active = 1, netsuite_addressbook_internal_id = {NSAddressId}, ts = ts WHERE address_table = '{_type}' AND address_id = {NESIAddressId};");
								processedAddresses.Add(NESIAddressId);
								}
							if(!processedDeletions.Contains(NESITypeId))
								{ 
								BUDsb.Add($"{NESITypeId},");
								processedDeletions.Add(NESITypeId);
								}
							if((!processedBUs.ContainsKey(NESITypeId) || processedBUs.ContainsKey(NESITypeId) && !processedBUs[NESITypeId].Contains(bu)) && i < lines-1 && NESITypeId != "")
								{ 
								BUsb.Add($"({NESITypeId}, {bu}),");
								if(!processedBUs.ContainsKey(NESITypeId))
									{
									processedBUs.Add(NESITypeId, new List<int> { bu });
									}
								else
									{
									processedBUs[NESITypeId].Add(bu);
									}
								}
							}
						i++;
					}
					
				}
			}
			if (!Directory.Exists($@"c:\temp\"))
			{
				Directory.CreateDirectory($@"c:\temp\");
			}
			if (!Directory.Exists($@"c:\temp\{sandbox}\"))
			{
				Directory.CreateDirectory($@"c:\temp\{sandbox}\");
			}
			Console.WriteLine(DateTime.Now.ToString(format) + " - " + _type +": 100%");
			Console.WriteLine($"Step 1/6: Writing {_type} link export - Start");
			using (var sw = new StreamWriter($@"c:\temp\{sandbox}\{sandbox}_{_type}_1_link_export.sql"))
			{
				foreach (var l in sb)
				{
					sw.WriteLine(l);
				}
			}
			Console.WriteLine($"Step 1/6: Writing {_type} link export - Done");

			Console.WriteLine("Step 2.0/6: Writing address link export - Start");
			using (var sw = new StreamWriter($@"c:\temp\{sandbox}\{sandbox}_{_type}_2.0_address_export.sql"))
			{
				foreach (var l in Asb)
				{
					sw.WriteLine(l);
				}
			}
			Console.WriteLine("Step 2.0/6: Writing address link export - Done");

			Console.WriteLine("Step 2.5/6: Writing address disabler - Start");
			using (var sw = new StreamWriter($@"c:\temp\{sandbox}\{sandbox}_{_type}_2.5_address_disabler.sql"))
			{
					sw.WriteLine((_type == "customer" ? DeactivateAddressesCustomers : DeactivateAddressesVendors) + string.Join(",", processedAddresses) + ");");
			}
			Console.WriteLine("Step 2.5/6: Writing address disabler- Done");

			Console.WriteLine("Step 3/6: Writing bu delete existing link export - Start");
			using(var sw = new StreamWriter($@"c:\temp\{sandbox}\{sandbox}_{_type}_3_bu_delete_export.sql"))
			{
				var ai = 1;
				foreach(var l in BUDsb)
				{
				var li = ai == BUDsb.Count
						? l.TrimEnd(',')+");"
						: l;
				sw.WriteLine(li);
				ai++;
				}
			}
			Console.WriteLine("Step 3/6: Writing bu delete existing link export - Done");

			Console.WriteLine("Step 4/6: Writing bu link export - Start");
			using (var sw = new StreamWriter($@"c:\temp\{sandbox}\{sandbox}_{_type}_4_bu_link_export.sql"))
			{
				var ai = 1;
				foreach (var l in BUsb)
				{
					var li = ai == BUsb.Count
							? l.TrimEnd(',') + ";"
							: l;
					sw.WriteLine(li);
					ai++;
				}
			}
			Console.WriteLine("Step 4/6: Writing bu link export - Done");

			Console.WriteLine("Step 5/6: Writing customer/vendor inactive script - Start");
			using (var sw = new StreamWriter($@"c:\temp\{sandbox}\{sandbox}_{_type}_5_inactivations.sql"))
			{
				sw.WriteLine((_type == "customer" ? DeactivateCustomers : DeactivateVendors) +string.Join(",", processed)+");"); 
			}

			Console.WriteLine("Step 5/6: Writing customer/vendor inactive script - Done");

			Console.WriteLine("Step 6/6: Writing customer/vendor activate script - Start");
			using (var sw = new StreamWriter($@"c:\temp\{sandbox}\{sandbox}_{_type}_6_activations.sql"))
			{
				sw.WriteLine((_type == "customer" ? ActivateCustomers : ActivateVendors) + string.Join(",", processed) + ");");
			}

			Console.WriteLine("Step 6/6: Writing customer/vendor activate script - Done");

		}
		public static void populateSubs()
		{
		// SELECT CONCAT("dict.Add(\"", netsuite_bu_internal_id,"\",", id,");") d FROM business_unit WHERE netsuite_bu_internal_id IS NOT NULL ORDER BY NetSuite_BU_Internal_Id;
			dict.Add("1", 100);
			dict.Add("10", 106);
			dict.Add("59", 1);//
			dict.Add("60", 50);//
			dict.Add("61", 51);//
			dict.Add("62", 49);//
			dict.Add("63", 38);//
			dict.Add("66", 112);//
			dict.Add("67", 22);//
			dict.Add("75", 7);//
			dict.Add("76", 25);//
			dict.Add("77", 28);//
			dict.Add("78", 23);//
			dict.Add("79", 4);//
			dict.Add("80", 120);//
			dict.Add("89", 118);//
			dict.Add("9", 103);//?
			dict.Add("90", 45);//
			dict.Add("93", 52);//
			dict.Add("94", 62);//
			dict.Add("96", 11);//

			dict.Add("105", 132);
			dict.Add("107", 133);
			dict.Add("108", 53);
			dict.Add("109", 58);//?
			dict.Add("110", 134);
			/*
19	New Electric Enterprises Inc.
20	NESI
59	NE Oakville Service
60	NE Oakville Panel Shop
61	NE Oakville System Integration
62	NE Oakville ER
63	NE Barrie Service
66	NE Belleville Service
67	NE Brampton Service
75	NE Cambridge Service
76	NE London Service
77	NE Mississauga Service
78	NE Stoney Creek Service
79	NE Vaughan Service
80	NE Winnipeg Service
88	New Electric Fresno LLC
89	NE Fremont Services
90	NE Fresno Services
93	NE Vaughan System Integration
94	NE Whitby Service
96	NESI - Oakville
105	NE Calgary Service
107	NE Edmonton Service
108	Dept 500
109	Dept 800
110	NE Brantford Service

			 */
		}
	}
}
