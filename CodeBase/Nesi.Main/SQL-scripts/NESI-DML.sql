/*Master Contact*/
UPDATE page
SET menu_router = '/home/51/reports/120/master_contacts', 
menu_type = 2
WHERE page_id = 120;

/*Cust Asset*/
UPDATE page
SET menu_router = '/home/51/reports/126/customer_assets', 
menu_type = 2
WHERE page_id = 126;

/*Master Vendor*/
UPDATE page
SET menu_router = '/home/51/reports/211/master_vendors', 
menu_type = 2
WHERE page_id = 211;

/*Master WorkOrder*/
UPDATE page
SET menu_router = '/home/51/reports/77/master_workorder', 
menu_type = 2
WHERE page_id = 77;