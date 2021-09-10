select [definition],'#END_BAT#' from sys.sql_modules m
inner join sys.objects obj on obj.object_id=m.object_id 
 where obj.type ='TR'
