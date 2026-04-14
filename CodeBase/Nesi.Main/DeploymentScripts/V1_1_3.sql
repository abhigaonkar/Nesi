DELIMITER $$

USE `neintranet`$$

DROP PROCEDURE IF EXISTS `ds_quoteparts`$$

CREATE DEFINER=`root`@`%` PROCEDURE `ds_quoteparts`(q_id INT, rev INT)
BEGIN
	DECLARE _placeholder INT;
	DECLARE _id INT;
	DECLARE _master_id VARCHAR(10);
	DECLARE _description VARCHAR(1024);
	DECLARE _qty DOUBLE;
	DECLARE _repair_id INT;
	DECLARE quote_items CURSOR FOR
		SELECT 
			id, 
			part_no master_id, 
			 description, 
			qty, 
			consignment_id RepairID 
		FROM 
			quote_worksheet 
		WHERE 
			quote_id = q_id AND
			revision = rev  AND
			is_checked = 1 AND
			section_id IN (SELECT id FROM quote_section WHERE quote_id = q_id AND revision = rev AND is_checked = 1);
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET @no_more_rows = 1;
	SET @no_more_rows = 0;
	DROP TABLE IF EXISTS __lineitems;
	CREATE TABLE __lineitems
		(
		groupselectid INT,
		master_id VARCHAR(10),
		description VARCHAR(1024),
		Qty DOUBLE,
		RepairID INT,
    repair_id INT
		) ENGINE = MEMORY;
		
	OPEN quote_items;
			the_loop: LOOP
				FETCH quote_items INTO _id, _master_id, _description, _qty, _repair_id;
				IF @no_more_rows = 1 THEN LEAVE the_loop; END IF;
				IF _master_id = "" OR _master_id = 777 THEN
					INSERT INTO __lineitems (groupselectid, master_id, description, qty, RepairID,repair_id) VALUES (_id, 777, _description, _qty, _repair_id, _repair_id);
				ELSE
					IF CONCAT('', _master_id * 1) = _master_id THEN
					
							IF _master_id < 99000 THEN
							
							INSERT INTO __lineitems (groupselectid, master_id, description, qty, RepairID, repair_id) VALUES (_id, _master_id, _description, _qty, _repair_id, _repair_id);
							ELSEIF _master_id >= 2000000 THEN
							
							INSERT INTO __lineitems (groupselectid, master_id, description, qty, RepairID, repair_id) 
								SELECT
									a.inventory_kit_dtl_id,
									a.inventory_kit_dtl_master_id,
									IF(a.inventory_kit_dtl_master_id >= 990000, FULL_PART_DESCRIPTION_WITH_LABOUR(a.inventory_kit_dtl_master_id, 1, ''),b.description),
									_qty * a.inventory_kit_dtl_qty,
									0,
                  0
								FROM
									inventory_kit_dtl a
								LEFT JOIN
									inventory_description b ON a.inventory_kit_dtl_master_id = b.master_id
								WHERE
									a.inventory_kit_dtl_hdr_id = _master_id;
							END IF;
					END IF;
				END IF;
			END LOOP the_loop;
	CLOSE quote_items;
	
	
	SELECT groupselectid, master_id,description, SUM(qty) qty,RepairID FROM __lineitems WHERE master_id != "777" AND is_exclude(master_id) = FALSE GROUP BY master_id
  UNION
	SELECT groupselectid, master_id,description, qty,RepairID FROM __lineitems WHERE master_id != "777" AND is_exclude(master_id) = TRUE 
	UNION 
	SELECT groupselectid, master_id,description, qty,RepairID FROM __lineitems WHERE master_id = "777"
  ORDER BY CAST(master_id AS UNSIGNED);
	DROP TABLE IF EXISTS __lineitems;
	END$$

DELIMITER ;