﻿CREATE TRIGGER produtos_data_change
AFTER INSERT OR UPDATE OR DELETE ON produtos
FOR EACH ROW EXECUTE FUNCTION NotifyOnDataChange();