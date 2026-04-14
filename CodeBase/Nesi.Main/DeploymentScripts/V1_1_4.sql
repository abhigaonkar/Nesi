-- Task 1974 New Stored Procedure
DELIMITER $$

USE `neintranet`$$

DROP PROCEDURE IF EXISTS `ds_creditcard_purchase`$$

CREATE DEFINER=`root`@`%` PROCEDURE `ds_creditcard_purchase`(_member_id INT, _type CHAR(3))
BEGIN
		SET @member_id = _member_id;
		SET @reports_to = REPORTS_TO(_member_id); 
		SET @admin = AUTHENTICATED_FOR_PRIVILEGE(_member_id, 175);
		SET @other_branch = AUTHENTICATED_FOR_PRIVILEGE(_member_id, 187);
		SET @purchaser = AUTHENTICATED_FOR_PRIVILEGE(_member_id, 71);
		SET @business_unit_id = (SELECT business_unit_id FROM member WHERE member_id = _member_id);
		SET @acting_purchaser_business_units = (SELECT GROUP_CONCAT(id) FROM business_unit WHERE acting_purchaser = @member_id AND active = 'T');

		SET @sqlquery = CONCAT("
		SELECT ",
			CASE _type 
				WHEN "CC" THEN "b.id, CONCAT(c.ddl_name, ' - ', a.member_fullname,' - ', b.type,' (',RIGHT(b.number,4), ')') name" 
				WHEN "BU" THEN "c.id, c.ddl_name name"
				WHEN "EMP" THEN "a.member_id id, CONCAT(c.ddl_name, ' - ', a.member_fullname) name"
			END 
			,"
		FROM
			member a 
		INNER JOIN
			credit_cards b ON a.member_id = b.member_id AND a.business_unit_id = b.business_unit_id AND b.status = 'Active'
		INNER JOIN
			business_unit c ON a.business_unit_id = c.id 
		WHERE 
			a.member_status = 'Active' AND
			(
			(a.member_id = ?) OR -- singular
			FIND_IN_SET(a.member_id, @reports_to) OR -- reports to
			IF(@admin = TRUE, TRUE, FALSE) OR -- admin 
			IF(@purchaser = TRUE AND @other_branch = FALSE, a.business_unit_id = @business_unit_id, FALSE) OR -- purchaser
			IF(@purchaser = TRUE AND @other_branch = TRUE, FIND_IN_SET(a.business_unit_id, @acting_purchaser_business_units), FALSE) -- other branch
			)  
			",
			CASE _type 
				WHEN "CC" THEN " GROUP BY b.id ORDER BY c.ddl_name,a.member_fullname"
				WHEN "BU" THEN " GROUP BY c.id ORDER BY c.ddl_name"
				WHEN "EMP" THEN " GROUP BY a.member_id ORDER BY c.ddl_name,a.member_fullname"
			END);
		PREPARE stmt FROM @sqlquery;
		EXECUTE stmt USING @member_id;
		DEALLOCATE PREPARE stmt;
	END$$
DELIMITER ;