-----------------------------------------------------------------------------------------------
-- SCRIPT DROP ALL FUNCTIONS
-----------------------------------------------------------------------------------------------
SELECT		'DROP FUNCTION ' + sysobjects.name
FROM		sysobjects
WHERE		xtype IN ('FN', 'TF')
			AND LEFT(sysobjects.name, 2) <> 'U_'
ORDER BY	sysobjects.name



-----------------------------------------------------------------------------------------------
-- SCRIPT DROP ALL VIEWS
-----------------------------------------------------------------------------------------------
SELECT		'DROP VIEW ' + sysobjects.name
FROM		sysobjects
WHERE		xtype = 'V'
			AND LEFT(sysobjects.name, 2) <> 'U_'
ORDER BY	sysobjects.name



-----------------------------------------------------------------------------------------------
-- SCRIPT DROP ALL STORED PROCEDURE
-----------------------------------------------------------------------------------------------
SELECT		'DROP PROCEDURE ' + sysobjects.name
FROM		sysobjects
WHERE		xtype = 'P'
			AND LEFT(sysobjects.name, 2) <> 'U_'
ORDER BY	sysobjects.name



-----------------------------------------------------------------------------------------------
-- SCRIPT DROP ALL PRIMARY KEY
-----------------------------------------------------------------------------------------------
SELECT		'ALTER TABLE ' + Parent_TABLE.name + ' DROP CONSTRAINT ' + PK_KEY.name
FROM		sysobjects		PK_KEY
			INNER JOIN		sysobjects		Parent_TABLE	ON	PK_KEY.parent_obj = Parent_TABLE.id
WHERE		PK_KEY.xtype = 'PK'
			AND LEFT(Parent_TABLE.name, 2) <> 'U_'
			AND LEFT(PK_KEY.name, 2) <> 'U_'
ORDER BY	Parent_TABLE.name


-----------------------------------------------------------------------------------------------
-- SCRIPT DROP ALL INDEXES
-----------------------------------------------------------------------------------------------

#END_BAT#

SELECT ' DROP INDEX ' +
       I.name + ' ON ' +
       SCHEMA_NAME(T.schema_id) + '.' + T.name +  CHAR(13) + CHAR(10) + ' #END_BAT#' [CreateIndexScript]
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
	   AND LEFT(I.name, 2) <> 'U_'
	   AND LEFT(T.name, 2) <> 'U_'
           --AND I.Object_id = object_id('Person.Address') --Comment for all tables
           --AND I.name = 'IX_Address_PostalCode' --comment for all indexes
ORDER BY	I.name


-----------------------------------------------------------------------------------------------
-- SCRIPT DROP ALL TRIGGERS
-----------------------------------------------------------------------------------------------

SELECT		'DROP TRIGGER ' + OBJ_TRIGGERS.name
FROM		sysobjects						OBJ_TRIGGERS
			INNER JOIN		sysobjects		OBJ_TABLES		ON	OBJ_TRIGGERS.parent_obj = OBJ_TABLES.id
WHERE		OBJ_TRIGGERS.xtype = 'TR'
			AND LEFT(OBJ_TABLES.name, 2) <> 'U_'
			AND LEFT(OBJ_TRIGGERS.name, 2) <> 'U_'
ORDER BY	OBJ_TABLES.name

