CREATE FUNCTION public."NotifyOnDataChange"()
  RETURNS trigger
  LANGUAGE 'plpgsql'
AS $BODY$ 
DECLARE 
  data JSON;
  notification JSON;
BEGIN
  IF (TG_OP = 'DELETE') THEN
    data = row_to_json(OLD);
  ELSE
    data = row_to_json(NEW);
  END IF;

  notification = json_build_object(
            'table',TG_TABLE_NAME,
            'action', TG_OP,
            'data', data);  
            
    PERFORM pg_notify('datachange', notification::TEXT);
  RETURN NEW;
END
$BODY$;



CREATE FUNCTION public."CreateOnDataChangeForAllTables"()
  RETURNS void
  LANGUAGE 'plpgsql'
AS $BODY$
DECLARE  
  createTriggerStatement TEXT;
BEGIN
  FOR createTriggerStatement IN SELECT
    'CREATE TRIGGER OnDataChange AFTER INSERT OR DELETE OR UPDATE ON '
    || tab_name
    || ' FOR EACH ROW EXECUTE PROCEDURE public."NotifyOnDataChange"();' AS trigger_creation_query
  FROM (
    SELECT
      quote_ident(table_schema) || '.' || quote_ident(table_name) as tab_name
    FROM
      information_schema.tables
    WHERE
      table_schema NOT IN ('pg_catalog', 'information_schema')
      AND table_schema NOT LIKE 'pg_toast%'
  ) as TableNames
  LOOP
    EXECUTE  createTriggerStatement;
  END LOOP;
END
$BODY$;