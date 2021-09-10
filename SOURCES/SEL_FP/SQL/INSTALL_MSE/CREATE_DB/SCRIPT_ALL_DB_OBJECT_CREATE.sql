-----------------------------------------------------------------------------------------------
-- SCRIPT CREATE ALL PRIMARY KEY
-----------------------------------------------------------------------------------------------

#END_BAT#

DECLARE @object_id int;
DECLARE @parent_object_id int;
DECLARE @TSQL NVARCHAR(4000);
DECLARE @COLUMN_NAME SYSNAME;
DECLARE @is_descending_key bit;
DECLARE @col1 BIT;
DECLARE @action CHAR(6);
 
--SET @action = 'DROP';
SET @action = 'CREATE';
 
DECLARE PKcursor CURSOR FOR
    select kc.object_id, kc.parent_object_id
    from sys.key_constraints kc
    inner join sys.objects o
    on kc.parent_object_id = o.object_id
    where kc.type = 'PK' and o.type = 'U'
    and o.name not in ('dtproperties','sysdiagrams')  -- not true user tables
    order by QUOTENAME(OBJECT_SCHEMA_NAME(kc.parent_object_id))
            ,QUOTENAME(OBJECT_NAME(kc.parent_object_id));
 
OPEN PKcursor;
FETCH NEXT FROM PKcursor INTO @object_id, @parent_object_id;
  
WHILE @@FETCH_STATUS = 0
BEGIN
    IF @action = 'DROP'
        SET @TSQL = 'ALTER TABLE '
                  + QUOTENAME(OBJECT_SCHEMA_NAME(@parent_object_id))
                  + '.' + QUOTENAME(OBJECT_NAME(@parent_object_id))
                  + ' DROP CONSTRAINT ' + QUOTENAME(OBJECT_NAME(@object_id))
    ELSE
        BEGIN
        SET @TSQL = 'ALTER TABLE '
                  + QUOTENAME(OBJECT_SCHEMA_NAME(@parent_object_id))
                  + '.' + QUOTENAME(OBJECT_NAME(@parent_object_id))
                  + ' ADD CONSTRAINT ' + QUOTENAME(OBJECT_NAME(@object_id))
                  + ' PRIMARY KEY'
                  + CASE INDEXPROPERTY(@parent_object_id
                                      ,OBJECT_NAME(@object_id),'IsClustered')
                        WHEN 1 THEN ' CLUSTERED'
                        ELSE ' NONCLUSTERED'
                    END
                  + ' (';
 
        DECLARE ColumnCursor CURSOR FOR
            select COL_NAME(@parent_object_id,ic.column_id), ic.is_descending_key
            from sys.indexes i
            inner join sys.index_columns ic
            on i.object_id = ic.object_id and i.index_id = ic.index_id
            where i.object_id = @parent_object_id
            and i.name = OBJECT_NAME(@object_id)
            order by ic.key_ordinal;
 
        OPEN ColumnCursor;
 
        SET @col1 = 1;
 
        FETCH NEXT FROM ColumnCursor INTO @COLUMN_NAME, @is_descending_key;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            IF (@col1 = 1)
                SET @col1 = 0
            ELSE
                SET @TSQL = @TSQL + ',';
 
            SET @TSQL = @TSQL + QUOTENAME(@COLUMN_NAME)
                      + ' '
                      + CASE @is_descending_key
                            WHEN 0 THEN 'ASC'
                            ELSE 'DESC'
                        END;
 
            FETCH NEXT FROM ColumnCursor INTO @COLUMN_NAME, @is_descending_key;
        END;
 
        CLOSE ColumnCursor;
        DEALLOCATE ColumnCursor;
 
        SET @TSQL = @TSQL + ');';
 
        END;
 
    PRINT @TSQL;
 
    FETCH NEXT FROM PKcursor INTO @object_id, @parent_object_id;
END;
 
CLOSE PKcursor;
DEALLOCATE PKcursor;

#END_BAT#


-----------------------------------------------------------------------------------------------
-- SCRIPT CREATE ALL INDEXES
-----------------------------------------------------------------------------------------------

#END_BAT#

SELECT ' CREATE ' +
       CASE 
            WHEN I.is_unique = 1 THEN ' UNIQUE '
            ELSE ''
       END +
       I.type_desc COLLATE DATABASE_DEFAULT + ' INDEX ' +
       I.name + ' ON ' +
       SCHEMA_NAME(T.schema_id) + '.' + T.name + ' ( ' +
       KeyColumns + ' )  ' +
       ISNULL(' INCLUDE (' + IncludedColumns + ' ) ', '') +
       ISNULL(' WHERE  ' + I.filter_definition, '') + ' WITH ( ' +
       CASE 
            WHEN I.is_padded = 1 THEN ' PAD_INDEX = ON '
            ELSE ' PAD_INDEX = OFF '
       END + ',' +
       'FILLFACTOR = ' + CONVERT(
           CHAR(5),
           CASE 
                WHEN I.fill_factor = 0 THEN 100
                ELSE I.fill_factor
           END
       ) + ',' +
       -- default value 
       'SORT_IN_TEMPDB = OFF ' + ',' +
       CASE 
            WHEN I.ignore_dup_key = 1 THEN ' IGNORE_DUP_KEY = ON '
            ELSE ' IGNORE_DUP_KEY = OFF '
       END + ',' +
       CASE 
            WHEN ST.no_recompute = 0 THEN ' STATISTICS_NORECOMPUTE = OFF '
            ELSE ' STATISTICS_NORECOMPUTE = ON '
       END + ',' +
       ' ONLINE = OFF ' + ',' +
       CASE 
            WHEN I.allow_row_locks = 1 THEN ' ALLOW_ROW_LOCKS = ON '
            ELSE ' ALLOW_ROW_LOCKS = OFF '
       END + ',' +
       CASE 
            WHEN I.allow_page_locks = 1 THEN ' ALLOW_PAGE_LOCKS = ON '
            ELSE ' ALLOW_PAGE_LOCKS = OFF '
       END + ' ) ON [' +
       DS.name + ' ] ' +  CHAR(13) + CHAR(10) + ' #END_BAT#' [CreateIndexScript]
FROM   sys.indexes I
       JOIN sys.tables T
            ON  T.object_id = I.object_id
       JOIN sys.sysindexes SI
            ON  I.object_id = SI.id
            AND I.index_id = SI.indid
       JOIN (
                SELECT *
                FROM   (
                           SELECT IC2.object_id,
                                  IC2.index_id,
                                  STUFF(
                                      (
                                          SELECT ' , ' + C.name + CASE 
                                                                       WHEN MAX(CONVERT(INT, IC1.is_descending_key)) 
                                                                            = 1 THEN 
                                                                            ' DESC '
                                                                       ELSE 
                                                                            ' ASC '
                                                                  END
                                          FROM   sys.index_columns IC1
                                                 JOIN sys.columns C
                                                      ON  C.object_id = IC1.object_id
                                                      AND C.column_id = IC1.column_id
                                                      AND IC1.is_included_column = 
                                                          0
                                          WHERE  IC1.object_id = IC2.object_id
                                                 AND IC1.index_id = IC2.index_id
                                          GROUP BY
                                                 IC1.object_id,
                                                 C.name,
                                                 index_id
                                          ORDER BY
                                                 MAX(IC1.key_ordinal) 
                                                 FOR XML PATH('')
                                      ),
                                      1,
                                      2,
                                      ''
                                  ) KeyColumns
                           FROM   sys.index_columns IC2 
                                  --WHERE IC2.Object_id = object_id('Person.Address') --Comment for all tables
                           GROUP BY
                                  IC2.object_id,
                                  IC2.index_id
                       ) tmp3
            )tmp4
            ON  I.object_id = tmp4.object_id
            AND I.Index_id = tmp4.index_id
       JOIN sys.stats ST
            ON  ST.object_id = I.object_id
            AND ST.stats_id = I.index_id
       JOIN sys.data_spaces DS
            ON  I.data_space_id = DS.data_space_id
       JOIN sys.filegroups FG
            ON  I.data_space_id = FG.data_space_id
       LEFT JOIN (
                SELECT *
                FROM   (
                           SELECT IC2.object_id,
                                  IC2.index_id,
                                  STUFF(
                                      (
                                          SELECT ' , ' + C.name
                                          FROM   sys.index_columns IC1
                                                 JOIN sys.columns C
                                                      ON  C.object_id = IC1.object_id
                                                      AND C.column_id = IC1.column_id
                                                      AND IC1.is_included_column = 
                                                          1
                                          WHERE  IC1.object_id = IC2.object_id
                                                 AND IC1.index_id = IC2.index_id
                                          GROUP BY
                                                 IC1.object_id,
                                                 C.name,
                                                 index_id 
                                                 FOR XML PATH('')
                                      ),
                                      1,
                                      2,
                                      ''
                                  ) IncludedColumns
                           FROM   sys.index_columns IC2 
                                  --WHERE IC2.Object_id = object_id('Person.Address') --Comment for all tables
                           GROUP BY
                                  IC2.object_id,
                                  IC2.index_id
                       ) tmp1
                WHERE  IncludedColumns IS NOT NULL
            ) tmp2
            ON  tmp2.object_id = I.object_id
            AND tmp2.index_id = I.index_id
WHERE  I.is_primary_key = 0
       AND I.is_unique_constraint = 0
           --AND I.Object_id = object_id('Person.Address') --Comment for all tables
           --AND I.name = 'IX_Address_PostalCode' --comment for all indexes
ORDER BY	I.name

#END_BAT#


-----------------------------------------------------------------------------------------------
-- SCRIPT CREATE ALL TRIGGERS
-----------------------------------------------------------------------------------------------

select [definition] + CHAR(13) + CHAR(10) + '#END_BAT#' from sys.sql_modules m
inner join sys.objects obj on obj.object_id=m.object_id 
 where obj.type ='TR'

-----------------------------------------------------------------------------------------------
-- SCRIPT SET TRIGGER ORDERS
-----------------------------------------------------------------------------------------------

SELECT		'EXEC sp_settriggerorder @triggername=N''[dbo].[' + sys.triggers.name + ']'', @order=N'''
			+	CASE
					WHEN is_first = 1	THEN 'First'
					WHEN is_last = 1	THEN 'Last'
				END
			+ ''', @stmttype=N'''
			
			+ CASE sys.trigger_events.type_desc
				WHEN 'DELETE'		THEN N'DELETE'
				WHEN 'INSERT'		THEN N'INSERT'
				WHEN 'UPDATE'		THEN N'UPDATE'
				ELSE '<!!! UNKNOWN>'
			  END
		
			+ '''' + CHAR(13) + CHAR(10) + '#END_BAT#' + CHAR(13) + CHAR(10)
/*
			sys.tables.name,
			sys.triggers.name,
			sys.trigger_events.type,
			sys.trigger_events.type_desc,
			is_first,
			is_last,
			sys.triggers.create_date,
			sys.triggers.modify_date
*/
FROM		sys.triggers
			INNER JOIN	sys.trigger_events	ON sys.trigger_events.object_id = sys.triggers.object_id
			INNER JOIN	sys.tables			ON sys.tables.object_id = sys.triggers.parent_id

WHERE		(is_first = 1 OR is_last = 1)

ORDER BY	sys.triggers.name


select sys.tables.name,sys.triggers.name,sys.trigger_events.type
,sys.trigger_events.type_desc, is_first,is_last
,sys.triggers.create_date,sys.triggers.modify_date
from sys.triggers inner join sys.trigger_events
on sys.trigger_events.object_id = sys.triggers.object_id
inner join sys.tables on sys.tables.object_id = sys.triggers.parent_id
order by modify_date
