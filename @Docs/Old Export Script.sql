-- EXPORT MYSQL TO MONGODB
USE IWIP;

-- EXPORT RELATIONSHIP TABLES INTO JSON MONGODB COLLECTION AND ARRAY
SELECT json_pretty(json_object(
"_id", 'BinData(3, ''' + CONVERT(emp.PO_HEADER_ID, CHAR) + ''')',
'MANUFACTURER', emp.MANUFACTURER, 
'PO_HEADER_ID', emp.PO_HEADER_ID, 
'PURCHASE_ORDER_NUMBER', emp.PURCHASE_ORDER_NUMBER, 
'REVISED_DATE',
json_object("$date", DATE_FORMAT(emp.REVISED_DATE,'%Y-%m-%dT%TZ')),
'PO_LINES', JSON_ARRAY(json_object('PO_LINE_ID', dept.PO_LINE_ID, 'PO_HEADER_ID', dept.PO_HEADER_ID))
--  , 'Salary', s.salary
)) AS 'json' 
INTO OUTFILE 'c:/wamp64/tmp/POS.json' ## IMPORTANT you may want to adjust outfile path here
FROM s_po_header emp
INNER JOIN s_po_lines dept on dept.PO_HEADER_ID = emp.PO_HEADER_ID
LIMIT 1000;

-- SHOW PRIVILEDGET PATH TO EXPORT
SHOW variables like "secure_file_priv";
