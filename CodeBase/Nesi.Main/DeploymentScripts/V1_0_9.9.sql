-- Updated Stored Proc report_jobcost for task 1722
DELIMITER $$

USE `neintranet`$$

DROP PROCEDURE IF EXISTS `report_jobcost`$$

CREATE DEFINER=`root`@`%` PROCEDURE `report_jobcost`( _vb VARCHAR(10000))
BEGIN 
DROP TEMPORARY TABLE IF EXISTS _wo_jc;	
DROP TEMPORARY TABLE IF EXISTS _wo_pb;
DROP TEMPORARY TABLE IF EXISTS _wo_opo;
DROP TEMPORARY TABLE IF EXISTS _wo_qt_lb;
DROP TEMPORARY TABLE IF EXISTS _wo_act_lb;
DROP TEMPORARY TABLE IF EXISTS _wo_associated;
DROP TEMPORARY TABLE IF EXISTS _wo_qt_mat;
CREATE TEMPORARY TABLE _wo_jc
	(
	woprog_id INT(8),
	sell DOUBLE (13,5) DEFAULT 0,
	cost DOUBLE (13,5) DEFAULT 0
	) ENGINE = MEMORY;
ALTER TABLE _wo_jc ADD INDEX (woprog_id);
CREATE TEMPORARY TABLE _wo_pb
	(
	woprog_id INT(8),
	billed DOUBLE (13,5) DEFAULT 0,
	cost DOUBLE (13,5) DEFAULT 0
	) ENGINE = MEMORY;
ALTER TABLE _wo_pb ADD INDEX (woprog_id);
CREATE TEMPORARY TABLE _wo_opo
	(
	woprog_id INT(8),
	cost DOUBLE(13,5) DEFAULT 0
	);
ALTER TABLE _wo_opo ADD INDEX (woprog_id);
CREATE TEMPORARY TABLE _wo_qt_lb
	(
	quoteid INT(8),
	qty DOUBLE(13,5) DEFAULT 0,
	cost DOUBLE(13,5) DEFAULT 0 
	) ENGINE = MEMORY;
ALTER TABLE _wo_qt_lb ADD INDEX (quoteid);
CREATE TEMPORARY TABLE _wo_qt_mat
	(
	quoteid INT(8),
	cost DOUBLE(13,5) DEFAULT 0 
	) ENGINE = MEMORY;
ALTER TABLE _wo_qt_mat ADD INDEX (quoteid);
CREATE TEMPORARY TABLE _wo_act_lb
	(
	woprog_id INT(8),
	qty DOUBLE(13,5) DEFAULT 0,
	cost DOUBLE(13,5) DEFAULT 0
	) ENGINE = MEMORY;
ALTER TABLE _wo_act_lb ADD INDEX (woprog_id);
CREATE TEMPORARY TABLE _wo_associated
	(
	woprog_id INT(8),
	linked_wos VARCHAR(2000)
	) ENGINE = MEMORY;
ALTER TABLE _wo_associated ADD INDEX (woprog_id);

INSERT INTO _wo_jc (woprog_id, sell, cost)
SELECT woprog_id, SUM(sell), SUM(cost) FROM (
SELECT 
	a.woprog_id,
	SUM(b.wo_detail_current_qty_committed*b.wo_detail_current_price_sell) sell,
	SUM(b.wo_detail_current_qty_committed*b.wo_detail_current_price_cost) cost
FROM 
	woprog a 
LEFT JOIN 
	wo_detail_current b ON b.wo_detail_current_woprog_id = a.woprog_id AND wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (3,11)
WHERE 
	FIND_IN_SET(a.business_unit_id, _vb) AND 
	a.WOProg_Associate_WOProg_ID = 0 AND 
	a.woprog_status NOT IN ('Invoiced', 'Waiting to be Invoiced')
GROUP BY 
	a.woprog_id
UNION 
SELECT 
	a.woprog_id,
	SUM(b.wo_detail_history_qty_committed*b.wo_detail_history_price_sell) sell,
	SUM(b.wo_detail_history_qty_committed*b.wo_detail_history_price_cost) cost
FROM 
	woprog a 
LEFT JOIN 
	wo_detail_history b ON b.wo_detail_history_woprog_id = a.woprog_id AND wo_detail_history_master_id = 2139 AND wo_detail_history_type = 'Q' AND wo_detail_history_billtypeid IN (3,11)
WHERE 
	FIND_IN_SET(a.business_unit_id, _vb) AND 
	a.WOProg_Associate_WOProg_ID = 0 AND 
	a.woprog_status IN ('Invoiced', 'Waiting to be Invoiced')
GROUP BY 
	a.woprog_id) asdf GROUP BY woprog_id;
	

INSERT INTO _wo_pb (woprog_id, billed, cost)
SELECT woprog_id, SUM(billed), SUM(cost) FROM (
SELECT 
	a.woprog_id,
	SUM(b.wo_detail_current_qty_committed*b.wo_detail_current_price_sell) billed,
	SUM(b.wo_detail_current_qty_committed*b.wo_detail_current_price_cost) cost
FROM 
	woprog a 
LEFT JOIN 
	wo_detail_current b ON b.wo_detail_current_woprog_id = a.woprog_id AND wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (9,12)
WHERE 
	FIND_IN_SET(a.business_unit_id, _vb) AND 
	a.WOProg_Associate_WOProg_ID = 0 AND 
	a.woprog_status NOT IN ('Invoiced', 'Waiting to be Invoiced')
GROUP BY 
	a.woprog_id
UNION 
SELECT 
	a.woprog_id,
	SUM(b.wo_detail_history_qty_committed*b.wo_detail_history_price_sell) billed,
	SUM(b.wo_detail_history_qty_committed*b.wo_detail_history_price_cost) cost
FROM 
	woprog a 
LEFT JOIN 
	wo_detail_history b ON b.wo_detail_history_woprog_id = a.woprog_id AND wo_detail_history_master_id = 2139 AND wo_detail_history_type = 'Q' AND wo_detail_history_billtypeid IN (9,12)
WHERE 
	FIND_IN_SET(a.business_unit_id, _vb) AND 
	a.WOProg_Associate_WOProg_ID = 0 AND 
	a.woprog_status IN ('Invoiced', 'Waiting to be Invoiced')
GROUP BY 
	a.woprog_id) asdf GROUP BY woprog_id;
	
INSERT INTO _wo_opo (woprog_id,  cost)
SELECT woprog_id, SUM(cost) FROM (
SELECT 
	a.woprog_id,
	
	SUM((b.wo_detail_current_qty_ordered - b.wo_detail_current_qty_committed) *b.wo_detail_current_price_cost) cost
FROM 
	woprog a 
LEFT JOIN 
	wo_detail_current b ON b.wo_detail_current_woprog_id = a.woprog_id AND wo_detail_current_master_id = 2139 AND wo_detail_current_type = 'Q' AND wo_detail_current_billtypeid IN (3,11)
WHERE 
	FIND_IN_SET(a.business_unit_id, _vb) AND 
	a.WOProg_Associate_WOProg_ID = 0 AND 
	a.woprog_status NOT IN ('Invoiced', 'Waiting to be Invoiced')
GROUP BY 
	a.woprog_id
UNION 
SELECT 
	a.woprog_id,
	
	SUM((b.wo_detail_history_qty_ordered - b.wo_detail_history_qty_committed)*b.wo_detail_history_price_cost) cost
FROM 
	woprog a 
LEFT JOIN 
	wo_detail_history b ON b.wo_detail_history_woprog_id = a.woprog_id AND wo_detail_history_master_id = 2139 AND wo_detail_history_type = 'Q' AND wo_detail_history_billtypeid IN (3,11)
WHERE 
	FIND_IN_SET(a.business_unit_id, _vb) AND 
	a.WOProg_Associate_WOProg_ID = 0 AND 
	a.woprog_status IN ('Invoiced', 'Waiting to be Invoiced')
GROUP BY 
	a.woprog_id) asdf GROUP BY woprog_id;
	
	





	
	
	

INSERT INTO _wo_qt_lb (quoteid, qty, cost)
SELECT 
	CONCAT(a.quote_id,a.revision) q, 
	SUM(qty),
	SUM(qty*cost)
FROM 
	quote_worksheet a 
LEFT JOIN 
	quote_master b ON a.quote_id = b.quote_id AND a.revision = b.revision 
LEFT JOIN quote_section c ON a.section_id=c.id
WHERE 
	b.active_revision = TRUE AND 
	b.status_id = 8 AND 
	a.is_checked = 1 AND
	c.is_checked = 1 AND
	part_no REGEXP '[0-9]+' AND 
	part_no/1 BETWEEN 990000 AND 2000000  
GROUP BY 
	a.quote_id;

INSERT INTO _wo_qt_mat (quoteid, cost)
SELECT 
	CONCAT(a.quote_id,a.revision) q, 
	SUM(qty*cost)
FROM 
	quote_worksheet a 
LEFT JOIN 
	quote_master b ON a.quote_id = b.quote_id AND a.revision = b.revision 
WHERE 
	b.active_revision = TRUE AND 
	b.status_id = 8 AND 
	IF(part_no = '', 0, part_no) REGEXP '[0-9]+' AND 
	part_no/1 < 990000  
GROUP BY 
	a.quote_id;
	
INSERT INTO _wo_associated (woprog_id, linked_wos)
SELECT a.woprog_id, GROUP_CONCAT(b.woprog_id) FROM woprog a LEFT JOIN woprog b ON b.woprog_associate_woprog_id = a.woprog_id WHERE FIND_IN_SET(a.business_unit_id, _vb) AND a.woprog_associate_woprog_id = 0 AND b.woprog_id IS NOT NULL  GROUP BY a.woprog_id;
INSERT INTO _wo_act_lb (woprog_id, qty, cost)
SELECT woprog_id, SUM(qty), SUM(qty*cost) FROM 
	(
	SELECT 
		wo_detail_current_woprog_id woprog_id, 
		wo_detail_current_qty_committed qty,
		wo_detail_current_price_cost cost
	FROM 
		wo_detail_current a 
	WHERE 
		wo_detail_current_type = 'L' AND 
		FIND_IN_SET(a.business_unit_id, _vb) AND
		wo_detail_current_woprog_id IN (SELECT woprog_id FROM woprog WHERE FIND_IN_SET(business_unit_id, _vb)AND woprog_status = 'Open' )
	UNION
	SELECT 
		wo_detail_current_woprog_id woprog_id, 
		wo_detail_current_qty_committed qty,
		wo_detail_current_price_cost cost
	FROM 
		wo_detail_current b 
	WHERE 
		wo_detail_current_type = 'L' AND 
		FIND_IN_SET(b.business_unit_id, _vb) AND
		wo_detail_current_woprog_id IN (SELECT woprog_id FROM woprog WHERE FIND_IN_SET(business_unit_id, _vb) AND woprog_associate_woprog_id > 0)
	UNION
	SELECT 
		wo_detail_history_woprog_id woprog_id, 
		wo_detail_history_qty_committed qty,
		wo_detail_history_price_cost cost
	FROM 
		wo_detail_history a 
	WHERE 
		wo_detail_history_type = 'L' AND 
		FIND_IN_SET(a.business_unit_id, _vb) AND
		wo_detail_history_woprog_id IN (SELECT woprog_id FROM woprog WHERE FIND_IN_SET(business_unit_id, _vb) AND woprog_status IN ('Waiting to be Invoiced', 'Invoiced'))
	UNION
	SELECT 
		wo_detail_history_woprog_id woprog_id, 
		wo_detail_history_qty_committed qty,
		wo_detail_history_price_cost cost
	FROM 
		wo_detail_history c 
	WHERE 
		wo_detail_history_type = 'L' AND 
		FIND_IN_SET(c.business_unit_id, _vb) AND
		wo_detail_history_woprog_id IN (SELECT woprog_id FROM woprog WHERE FIND_IN_SET(business_unit_id, _vb) AND woprog_status IN ('Waiting to be Invoiced', 'Invoiced') AND woprog_associate_woprog_id > 0)
	) asdf
GROUP BY woprog_id;


SELECT 
	a.woprog_id,
	pm.member_fullname pm,
	TRIM(a.woprog_customername) customer_name,
	a.woprog_quoteid quoteid,
	b.name company_name, 
	a.woprog_bvwo wo, 
	a.woprog_status `status`,
	a.woprog_description descript,
	a.WOProg_QuotedAmount quoted_amount,
	a.benchmark_material_sell matl_used,
	(a.benchmark_labor_sell + a.benchmark_material_sell) - IFNULL(e.cost,0)  bench_sell,
	a.WOProg_StillToBeBilled to_be_billed,
	ROUND((IFNULL(ABS(d.billed),0) / c.sell), 2)  prog_billed,
	IFNULL(woprog_laborcost,0) + IFNULL(woprog_materialcost,0) + ABS(IFNULL(c.cost, 0))  total_cost,
	ROUND(((a.benchmark_labor_sell + a.benchmark_material_sell) - IFNULL(e.cost,0)) / a.WOProg_QuotedAmount, 2)  quoted_amount_used,
	ROUND(((a.WOProg_QuotedAmount - (IFNULL(woprog_laborcost,0) + IFNULL(woprog_materialcost,0) + ABS(IFNULL(c.cost, 0)) )) / a.WOProg_QuotedAmount), 2) overall_margin,
	IFNULL(f.qty,0) quoted_labor_hours,
	IFNULL(g.qty,0) actual_labor_hours,
	IFNULL(g.cost,0) labor_cost_tot,
	IFNULL(ROUND(IFNULL(g.cost,0) / IFNULL(g.qty,1),2), 0) labor_cost_per,
	IFNULL(ROUND(IFNULL(g.qty,0) / IFNULL(f.qty,0), 2), 0) labor_used,
	IFNULL(h.cost,0) material_quote_price,
	IFNULL(a.WOProg_MaterialCost, 0) material_actual_price,
	IFNULL(a.WOProg_MaterialCost /IFNULL(h.cost,0), 0)  material_used,
	(IFNULL(f.qty,0) - IFNULL(g.qty,0)) labor_hours_remaining,
	i.address_addr1 service_addr,
	a.woprog_cutdatetime cut_date,
a.WOProg_InvoiceDate invoiced_date,
	IFNULL(woprog_materialcost,0) + ABS(IFNULL(c.cost, 0)) + IFNULL(e.cost,0) projected_material_cost,
(IFNULL(woprog_laborcost,0) * IF(a.WOProg_TimeSheet_Percentage=0,1,1/(a.WOProg_TimeSheet_Percentage/100))) projected_labor_cost,
IFNULL(woprog_materialcost,0) + ABS(IFNULL(c.cost, 0)) + IFNULL(e.cost,0) + (IFNULL(woprog_laborcost,0) * IF(a.WOProg_TimeSheet_Percentage=0,1,1/(a.WOProg_TimeSheet_Percentage/100))) projected_total_cost,

(
	(a.WOProg_QuotedAmount- (IFNULL(woprog_materialcost,0) + ABS(IFNULL(c.cost, 0)) + IFNULL(e.cost,0) + (IFNULL(woprog_laborcost,0) * IF(a.WOProg_TimeSheet_Percentage=0,1,1/(a.WOProg_TimeSheet_Percentage/100))))) /a.WOProg_QuotedAmount) projected_margin,

ROUND((a.WOProg_QuotedAmount-(f.cost) - (h.cost)),2) quote_margin_dollars,
ROUND((a.WOProg_QuotedAmount-(f.cost) - (h.cost))/a.WOProg_QuotedAmount,2) quote_margin_p

FROM 
	woprog a 
LEFT JOIN 
	member pm ON a.woprog_pm_memberid = pm.member_id
LEFT JOIN 
	business_unit b ON a.business_unit_id = b.id 
LEFT JOIN
	_wo_jc c ON a.woprog_id = c.woprog_id
LEFT JOIN
	_wo_pb d ON a.woprog_id = d.woprog_id
LEFT JOIN
	_wo_opo e ON a.woprog_id = e.woprog_id
LEFT JOIN
	_wo_qt_lb f ON a.woprog_quoteid = f.quoteid
LEFT JOIN
	_wo_act_lb g ON a.woprog_id = g.woprog_id
LEFT JOIN
	_wo_qt_mat h ON a.woprog_quoteid = h.quoteid
LEFT JOIN 
	address i ON a.woprog_address_id = i.address_id
WHERE 
	FIND_IN_SET(a.business_unit_id, _vb) AND 
	a.woprog_id > 10000 AND
	WOProg_Associate_WOProg_ID = 0 AND 
	woprog_status != "Deleted" 
ORDER BY 
	woprog_bvwo DESC;
DROP TEMPORARY TABLE IF EXISTS _wo_jc;	
DROP TEMPORARY TABLE IF EXISTS _wo_pb;
DROP TEMPORARY TABLE IF EXISTS _wo_opo;
DROP TEMPORARY TABLE IF EXISTS _wo_qt_lb;
DROP TEMPORARY TABLE IF EXISTS _wo_act_lb;
DROP TEMPORARY TABLE IF EXISTS _wo_associated;
DROP TEMPORARY TABLE IF EXISTS _wo_qt_mat;	
END$$

DELIMITER ;