-- Csereld ki az <Old_DB> hivatkozasokat az adatbazisodra (es erre az adatbazisra legyel racsatlakozva is!)
-- Csereld ki a  <New_DB> hivatkozasokat annak az adatbazisnak a nevere, amivel ossze akarod hasonlitani

DECLARE @MyUserID								INT
DECLARE @CRLF									NVARCHAR(2)
DECLARE @C_IsChanged							BIT
DECLARE @C_Sprache_Changed						BIT
DECLARE @C_User_Changed							BIT
DECLARE @C_UserGruppe_Changed					BIT

DECLARE @C_Code									NVARCHAR(255)
DECLARE @C_User									INT
DECLARE @C_UserGruppe							INT
DECLARE @C_GotoID_Changed						BIT
DECLARE @C_Identifier							NVARCHAR(255)
DECLARE @C_Beschreibung_Changed					BIT
DECLARE @C_FeldSELECT_Changed					BIT
DECLARE @C_FeldFROM_Changed						BIT
DECLARE @C_FeldORDERBY_Changed					BIT
DECLARE @C_FeldWHERE_Changed					BIT
DECLARE @C_WhereQueryName_Changed				BIT
DECLARE @C_WahlArt_Changed						BIT
DECLARE @C_IDFeld_Changed						BIT
DECLARE @C_StringValueFeld_Changed				BIT
DECLARE @C_LongValueFeld_Changed				BIT
DECLARE @C_SQLMehrFachWahlResyncCommand_Changed	BIT
DECLARE @C_SQLMehrFachWahl_Changed				BIT
DECLARE @C_DispoUpdate_Changed					BIT
DECLARE @C_DoFilter_Identifier_Changed			BIT
DECLARE @C_DispoUpdateAfterWahl_Changed			BIT
DECLARE @C_StoredProcNameAfterWahl_Changed		BIT
DECLARE @C_StoredProcNameAtTheEnd_Changed		BIT
DECLARE @C_ReportName_Changed					BIT
DECLARE @C_ReportOpenTyp_Changed				BIT
DECLARE @C_SQLExcel_Changed						BIT
DECLARE @C_SQLTxt_Changed						BIT
DECLARE @C_ReportRichTextExport_Changed			BIT
DECLARE @C_Titel_Changed						BIT
DECLARE @C_Bemerkungen_Changed					BIT
DECLARE @C_TipText_Changed						BIT
DECLARE @C_StatusText_Changed					BIT
DECLARE @C_Text_Changed							BIT
DECLARE @C_Kulcs								NVARCHAR(255)
DECLARE @C_KulcsElotag							NVARCHAR(255)
DECLARE @C_NumErtek_Changed						BIT
DECLARE @C_AlfaNumErtek_Changed					BIT

DECLARE @C_Name									NVARCHAR(255)
DECLARE @C_ObjectName							NVARCHAR(255)
DECLARE @C_TableName							NVARCHAR(255)
DECLARE @C_PK_Name								NVARCHAR(255)
DECLARE @C_OldObjectID							INT
DECLARE @C_OldTableID							INT
DECLARE @C_OldTableName							SYSNAME
DECLARE @C_NewObjectid							INT
DECLARE @C_NewTableID							INT
DECLARE @C_NewTableName							SYSNAME

DECLARE @C_ReTyp								INT
DECLARE @C_Bord_Or_BordMegb_Changed				BIT
DECLARE @C_Mask_Changed							BIT
DECLARE @C_Bemerkung_Changed					BIT

DECLARE @C_Sprache								NVARCHAR(255)
DECLARE @C_Art									NVARCHAR(255)
DECLARE @C_Nummer								NVARCHAR(255)

DECLARE @C2_New_Name							SYSNAME
DECLARE @C2_New_Position						SMALLINT
DECLARE @C2_New_DataType						SYSNAME
DECLARE @C2_New_Length							SMALLINT
DECLARE @C2_New_DefaultValue					NVARCHAR(MAX)
DECLARE @C2_New_AllowNulls						INT
DECLARE @C2_New_IsIdentity						SMALLINT
DECLARE @C2_Name								SYSNAME
DECLARE @C2_OldColumnName						SYSNAME
DECLARE @C2_OldColumnDataType					SYSNAME
DECLARE @C2_NewColumnDataType					SYSNAME
DECLARE @C2_OldColumnLength						INT
DECLARE @C2_NewColumnLength						INT
DECLARE @C2_OldColumnDefaultValue				NVARCHAR(MAX)
DECLARE @C2_NewColumnDefaultValue				NVARCHAR(MAX)
DECLARE @C2_OldColumnAllowNulls					BIT
DECLARE @C2_NewColumnAllowNulls					BIT
DECLARE @C2_OldColumnIdentity					BIT
DECLARE @C2_NewColumnIdentity					BIT

DECLARE @Title_Written							BIT
DECLARE	@PrevColumnName							NVARCHAR(128)
DECLARE	@NextColumnName							NVARCHAR(128)

DECLARE @CodeText								NVARCHAR(MAX)
DECLARE	@wText									NVARCHAR(MAX)
DECLARE @wAnd									NVARCHAR(50)

IF OBJECT_ID('tempdb..#TMP_OUT') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_OUT
   END

CREATE TABLE #TMP_OUT	(
							SeqNum		INT				IDENTITY(1, 1),
							Line_Text	NVARCHAR(MAX)
						)

SELECT	@CRLF	= CHAR(13) + CHAR(10)

SELECT @MyUserID = uid FROM sysusers WHERE name = 'dbo'

IF OBJECT_ID('tempdb..#TMP_NewObjects') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_NewObjects
   END

CREATE TABLE #TMP_NewObjects			(
											ID			int,
											Name		SYSNAME,
											ObjectType	NVARCHAR(5)
										)

IF OBJECT_ID('tempdb..#TMP_Tables') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_Tables
   END

CREATE TABLE #TMP_Tables				(
											OldTableID		INT,
											OldTableName	SYSNAME,
											NewTableID		INT,
											NewTableName	SYSNAME
										)

IF OBJECT_ID('tempdb..#TMP_OldTablesPK') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_OldTablesPK
   END

CREATE TABLE #TMP_OldTablesPK			(
											OldTableID		INT,
											OldTableName	SYSNAME,
											OldTablePK		SYSNAME
										)

IF OBJECT_ID('tempdb..#TMP_NewTablesPK') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_NewTablesPK
   END

CREATE TABLE #TMP_NewTablesPK			(
											NewTableID		INT,
											NewTableName	SYSNAME,
											NewTablePK		SYSNAME
										)

IF OBJECT_ID('tempdb..#TMP_OldColumns') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_OldColumns
   END

CREATE TABLE #TMP_OldColumns			(
											ID				INT,
											Name			SYSNAME,
											Position		SMALLINT,
											DataType		SYSNAME,
											[Length]		SMALLINT,
											DefaultValue	NVARCHAR(MAX),
											AllowNulls		INT,
											IsIdentity		SMALLINT
										)

IF OBJECT_ID('tempdb..#TMP_NewColumns') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_NewColumns
   END

CREATE TABLE #TMP_NewColumns			(
											ID				INT,
											Name			SYSNAME,
											Position		SMALLINT,
											DataType		SYSNAME,
											[Length]		SMALLINT,
											DefaultValue	NVARCHAR(MAX),
											AllowNulls		INT,
											IsIdentity		SMALLINT
										)

IF OBJECT_ID('tempdb..#TMP_Columns') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_Columns
   END

CREATE TABLE #TMP_Columns				(
											OldColumnName			SYSNAME,
											OldColumnDataType		SYSNAME,
											NewColumnDataType		SYSNAME,
											OldColumnLength			SMALLINT,
											NewColumnLength			SMALLINT,
											OldColumnDefaultValue	NVARCHAR(MAX),
											NewColumnDefaultValue	NVARCHAR(MAX),
											OldColumnAllowNulls		INT,
											NewColumnAllowNulls		INT,
											OldColumnIdentity		SMALLINT,
											NewColumnIdentity		SMALLINT
										)

IF OBJECT_ID('tempdb..#TMP_ObjectsCode') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_ObjectsCode
   END

CREATE TABLE #TMP_ObjectsCode			(
											OldObjectID			INT,
											OldObjectName		SYSNAME,
											OldObjectType		NVARCHAR(5),
											OldObjectCode		NVARCHAR(MAX),
											OldObjectLength		INT,
											NewObjectID			INT,
											NewObjectName		SYSNAME,
											NewObjectType		CHAR(2),
											NewObjectCode		NVARCHAR(MAX),
											NewObjectLength		INT
										)

IF OBJECT_ID('tempdb..#TMP_ObjectsCode_Compare') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_ObjectsCode_Compare
   END

CREATE TABLE #TMP_ObjectsCode_Compare	(
											ObjectName		SYSNAME,
											PartNo			INT,
											OldObjectCode	NVARCHAR(MAX),
											NewObjectCode	NVARCHAR(MAX),
											Differ			INT
										)

IF OBJECT_ID('tempdb..#TMP_OldObjects') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_OldObjects
   END

CREATE TABLE #TMP_OldObjects			(
											ID			int,
											Name		SYSNAME,
											ObjectType	NVARCHAR(5)
										)

INSERT INTO #TMP_OldObjects (ID, Name, ObjectType)
			SELECT		 ID, Name, xtype
			FROM		 sysobjects
			WHERE		(xtype='U' OR xtype='TR' OR xtype='V' OR xtype='P' OR xtype='FN' OR xtype='TF')
						AND SUBSTRING(Name, 1, 2) <> 'dt'
						AND SUBSTRING (Name, 1, 1) <> '_'
						AND uid = @MyUserID
			ORDER BY	Name

INSERT INTO #TMP_OldObjects	(ID, Name, ObjectType)
SELECT		so.object_id,
			so.name + '.' + si.name,
			'index'
FROM		sys.indexes si
			INNER JOIN sys.objects so ON si.[object_id] = so.[object_id]
WHERE		so.type = 'U'    --Only get indexes for User Created Tables
			AND si.is_primary_key = 0
			AND si.name IS NOT NULL
ORDER BY    so.name,
			si.type 

INSERT INTO #TMP_NewObjects (ID, Name, ObjectType)
			SELECT		 ID, Name, xtype
			FROM		<New_DB>..sysobjects
			WHERE		(xtype='U' OR xtype='TR' OR xtype='V' OR xtype='P' OR xtype='FN' OR xtype='TF')
						AND SUBSTRING(Name, 1, 2) <> 'dt'
						AND SUBSTRING(Name, 1, 1) <> '_'
						AND uid = @MyUserID
			ORDER BY	Name

USE <New_DB>
INSERT INTO #TMP_NewObjects	(ID, Name, ObjectType)
SELECT		so.object_id,
			so.name + '.' + si.name,
			'index'
FROM		sys.indexes si
			INNER JOIN sys.objects so ON si.[object_id] = so.[object_id]
WHERE		so.type = 'U'    --Only get indexes for User Created Tables
			AND si.is_primary_key = 0
			AND si.name IS NOT NULL
ORDER BY    so.name,
			si.type 
USE <Old_DB>



SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		#TMP_NewObjects.Name
									FROM		#TMP_NewObjects
												LEFT JOIN	#TMP_OldObjects		ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
									WHERE		#TMP_NewObjects.ObjectType='U'
												AND ISNULL(#TMP_OldObjects.Name, '') = ''
									ORDER BY	#TMP_NewObjects.Name

OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD TABLES')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C
SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT ISNULL(#TMP_OldObjects.Name, '')
									FROM		#TMP_NewObjects RIGHT OUTER JOIN #TMP_OldObjects ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
									WHERE		ISNULL(#TMP_NewObjects.Name, '') = ''
												AND #TMP_OldObjects.ObjectType='U'
									ORDER BY	#TMP_OldObjects.Name

OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DROP TABLES')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C

SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_NewObjects.Name, '')
									FROM		#TMP_NewObjects LEFT OUTER JOIN #TMP_OldObjects ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
									WHERE		(ISNULL(#TMP_OldObjects.Name, '') = '') AND #TMP_NewObjects.ObjectType='TR'
									ORDER BY	#TMP_NewObjects.Name

OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD TRIGGERS')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C

SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_OldObjects.Name, '')
									FROM		#TMP_OldObjects
												LEFT JOIN #TMP_NewObjects ON #TMP_OldObjects.Name = #TMP_NewObjects.Name
									WHERE		(ISNULL(#TMP_NewObjects.Name, '') = '')
												AND #TMP_OldObjects.ObjectType = 'TR'
									ORDER BY	#TMP_OldObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DROP TRIGGERS')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C

SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_NewObjects.Name, '')
									FROM		#TMP_NewObjects LEFT JOIN	#TMP_OldObjects		ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
									WHERE		ISNULL(#TMP_OldObjects.Name, '') = ''
												AND #TMP_NewObjects.ObjectType='V'
									ORDER BY	#TMP_NewObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD VIEWS')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_OldObjects.Name, '')
									FROM		#TMP_OldObjects
												LEFT JOIN #TMP_NewObjects	ON #TMP_OldObjects.Name = #TMP_NewObjects.Name
									WHERE		ISNULL(#TMP_NewObjects.Name, '') = ''
												AND #TMP_OldObjects.ObjectType = 'V'
									ORDER BY	#TMP_OldObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DROP VIEWS')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_NewObjects.Name, '')
									FROM		#TMP_NewObjects LEFT JOIN #TMP_OldObjects ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
									WHERE		ISNULL(#TMP_OldObjects.Name, '') = ''
												AND #TMP_NewObjects.ObjectType='P'
									ORDER BY	#TMP_NewObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD STORED PROCEDURES')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_OldObjects.Name, '')
									FROM		#TMP_OldObjects
												LEFT JOIN	#TMP_NewObjects		ON #TMP_OldObjects.Name = #TMP_NewObjects.Name
									WHERE		ISNULL(#TMP_NewObjects.Name, '') = ''
												AND #TMP_OldObjects.ObjectType='P'
									ORDER BY	#TMP_OldObjects.Name
  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DROP STORED PROCEDURES')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_NewObjects.Name, '')
									FROM		#TMP_NewObjects
												LEFT JOIN #TMP_OldObjects	ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
									WHERE		ISNULL(#TMP_OldObjects.Name, '') = ''
												AND #TMP_NewObjects.ObjectType = 'FN'
									ORDER BY	#TMP_NewObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD SCALAR FUNCTION')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_OldObjects.Name, '')
									FROM		#TMP_OldObjects
												LEFT JOIN #TMP_NewObjects	ON #TMP_OldObjects.Name = #TMP_NewObjects.Name
									WHERE		ISNULL(#TMP_NewObjects.Name, '') = ''
												AND #TMP_OldObjects.ObjectType='FN'
									ORDER BY	#TMP_OldObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DROP SCALAR FUNCTION')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_NewObjects.Name, '')
									FROM		#TMP_NewObjects
												LEFT JOIN	#TMP_OldObjects		ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
									WHERE		ISNULL(#TMP_OldObjects.Name, '') = ''
												AND #TMP_NewObjects.ObjectType = 'TF'
									ORDER BY	#TMP_NewObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD TABLE FUNCTION')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_OldObjects.Name, '')
									FROM		#TMP_OldObjects
												LEFT JOIN #TMP_NewObjects	ON #TMP_OldObjects.Name = #TMP_NewObjects.Name
									WHERE		ISNULL(#TMP_NewObjects.Name, '') = ''
												AND #TMP_OldObjects.ObjectType = 'TF'
									ORDER BY	#TMP_OldObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DROP TABLE FUNCTION')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_NewObjects.Name, '') AS NewObjectName
									FROM		#TMP_NewObjects
												LEFT JOIN	#TMP_OldObjects		ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
									WHERE		ISNULL(#TMP_OldObjects.Name, '') = ''
												AND #TMP_NewObjects.ObjectType = 'index'
									ORDER BY	#TMP_NewObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD INDEX')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(#TMP_OldObjects.Name, '')
									FROM		#TMP_OldObjects
												LEFT JOIN #TMP_NewObjects ON #TMP_OldObjects.Name = #TMP_NewObjects.Name
									WHERE		ISNULL(#TMP_NewObjects.Name, '') = ''
												AND #TMP_OldObjects.ObjectType = 'index'
									ORDER BY	#TMP_OldObjects.Name

  OPEN C
		FETCH NEXT FROM C INTO @C_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DROP INDEX')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_Name)
				FETCH NEXT FROM C INTO @C_Name
		   END
CLOSE C
DEALLOCATE C

INSERT INTO #TMP_Tables (OldTableID, OldTableName, NewTableID, NewTableName)
SELECT		#TMP_OldObjects.ID,		-- OldTableID
			#TMP_OldObjects.Name,	-- OldTableName
			#TMP_NewObjects.ID,		-- NewTableID
			#TMP_NewObjects.Name	-- NewTableName
FROM		#TMP_NewObjects
			INNER JOIN	#TMP_OldObjects		ON #TMP_NewObjects.Name = #TMP_OldObjects.Name
WHERE		#TMP_OldObjects.ObjectType='U'

INSERT INTO #TMP_OldTablesPK (OldTableID, OldTableName, OldTablePK)
SELECT	#TMP_Tables.OldTableID,		-- OldTableID
		#TMP_Tables.OldTableName,	-- OldTableName
		syscolumns.name			-- OldTablePK
FROM	#TMP_Tables
		INNER JOIN	sysobjects		ON	#TMP_Tables.OldTableID = sysobjects.parent_obj
		INNER JOIN	sysindexes		ON	sysobjects.Name = sysindexes.Name
		LEFT JOIN	sysindexkeys	ON	sysindexes.id = sysindexkeys.id
										AND sysindexes.indid = sysindexkeys.indid
		INNER JOIN	syscolumns		ON	sysindexkeys.id = syscolumns.id
										AND sysindexkeys.colid = syscolumns.colid
WHERE		sysobjects.xtype = 'PK'
ORDER BY	#TMP_Tables.OldTableName

INSERT INTO #TMP_NewTablesPK (NewTableID, NewTableName, NewTablePK)
SELECT	#TMP_Tables.NewTableID,		-- NewTableID
		#TMP_Tables.NewTableName,	-- NewTableName
		syscolumns.name				-- NewTablePK
FROM	#TMP_Tables
		INNER JOIN <New_DB>..sysobjects	sysobjects		ON		#TMP_Tables.NewTableID	= sysobjects.parent_obj
		INNER JOIN <New_DB>..sysindexes	sysindexes		ON		sysobjects.Name			= sysindexes.Name
		LEFT JOIN  <New_DB>..sysindexkeys	sysindexkeys	ON		sysindexes.id			= sysindexkeys.id AND
																	sysindexes.indid		= sysindexkeys.indid
		INNER JOIN <New_DB>..syscolumns	syscolumns		ON		sysindexkeys.id			= syscolumns.id AND
																	sysindexkeys.colid		= syscolumns.colid
WHERE sysobjects.xtype = 'PK'
ORDER BY #TMP_Tables.NewTableName


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(NewTableName, ''),
												NewTablePK
									FROM		#TMP_NewTablesPK
												LEFT JOIN #TMP_OldTablesPK		ON	#TMP_NewTablesPK.NewTableName = #TMP_OldTablesPK.OldTableName
																				AND #TMP_NewTablesPK.NewTablePK = #TMP_OldTablesPK.OldTablePK
									WHERE		ISNULL(#TMP_OldTablesPK.OldTablePK, '') = ''

  OPEN C
		FETCH NEXT FROM C INTO @C_TableName, @C_PK_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD PRIMARY KEYS')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (Left(@C_TableName + SPACE(50), 50) + ' - PK name: ' + @C_PK_Name)
				FETCH NEXT FROM C INTO @C_TableName, @C_PK_Name
		   END
CLOSE C
DEALLOCATE C

SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		ISNULL(OldTableName, ''),
												OldTablePK
									FROM		#TMP_OldTablesPK
												LEFT JOIN	#TMP_NewTablesPK	ON	#TMP_OldTablesPK.OldTableName = #TMP_NewTablesPK.NewTableName
																				AND #TMP_OldTablesPK.OldTablePK = #TMP_NewTablesPK.NewTablePK
									WHERE		ISNULL(#TMP_NewTablesPK.NewTablePK, '') = ''

  OPEN C
		FETCH NEXT FROM C INTO @C_TableName, @C_PK_Name
		WHILE @@FETCH_STATUS = 0
		   BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DROP PRIMARY KEYS')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (Left(@C_TableName + SPACE(50), 50) + ' - PK name: ' + @C_PK_Name)
				FETCH NEXT FROM C INTO @C_TableName, @C_PK_Name
		   END
CLOSE C
DEALLOCATE C


DECLARE C CURSOR LOCAL STATIC FOR	SELECT		OldTableID		INT,
												OldTableName	SYSNAME,
												NewTableID		INT,
												NewTableName	SYSNAME
									FROM		#TMP_Tables

OPEN C
FETCH NEXT FROM C INTO @C_OldTableID, @C_OldTableName, @C_NewTableID, @C_NewTableName
WHILE @@FETCH_STATUS = 0
  BEGIN
		DELETE #TMP_OldColumns
		DELETE #TMP_NewColumns
		DELETE #TMP_Columns

		INSERT INTO #TMP_OldColumns (ID, Name, Position, DataType, [Length], DefaultValue, AllowNulls, IsIdentity)
		SELECT		syscolumns.ID,												-- ID
					syscolumns.Name,											-- Name
					syscolumns.colorder,										-- Position
					systypes.name,												-- DataType
					syscolumns.[length],										-- [Length]

					RTRIM(LTRIM(
						REPLACE(REPLACE(syscomments.[text],'(',''), ')','')
						)),														-- DefaultValue

					syscolumns.isnullable,										-- AllowNulls
					syscolumns.colstat											-- IsIdentity
		FROM		syscolumns
					LEFT JOIN systypes		ON syscolumns.xusertype=systypes.xusertype
					LEFT JOIN syscomments	ON syscolumns.cdefault=syscomments.id
		WHERE		syscolumns.ID = @C_OldTableID
					AND systypes.name <> 'SYSNAME'
		ORDER BY	syscolumns.colorder

		UPDATE #TMP_OldColumns	SET		[Length] = [Length] / 2
								WHERE	(DataType = 'NVARCHAR' OR DataType='NCHAR')
     
		INSERT INTO #TMP_NewColumns (ID, Name, Position, DataType, [Length], DefaultValue, AllowNulls, IsIdentity)
		SELECT		syscolumns.ID,												-- ID
					syscolumns.Name,											-- Name
					syscolumns.colorder,										-- Position
					systypes.name,												-- DataType
					syscolumns.[length],										-- [Length]
					RTRIM(LTRIM(
						REPLACE(REPLACE(syscomments.[text],'(',''), ')','')
						)),														-- DefaultValue
					syscolumns.isnullable,										-- AllowNulls
					syscolumns.colstat											-- IsIdentity
		FROM		<New_DB>..syscolumns				syscolumns
					LEFT JOIN <New_DB>..systypes		systypes	ON	syscolumns.xusertype = systypes.xusertype
					LEFT JOIN <New_DB>..syscomments	syscomments ON	syscolumns.cdefault = syscomments.id
		WHERE		syscolumns.ID = @C_NewTableID
					AND systypes.name <> 'SYSNAME'
		ORDER BY	syscolumns.colorder

		UPDATE #TMP_NewColumns	SET		[Length]	= [Length] / 2
								WHERE	(DataType = 'NVARCHAR' OR DataType='NCHAR')


		SELECT @Title_Written = 0

		DECLARE C2 CURSOR LOCAL STATIC FOR	SELECT		#TMP_NewColumns.Name,
														#TMP_NewColumns.Position,
														#TMP_NewColumns.DataType,
														#TMP_NewColumns.[Length],
														#TMP_NewColumns.DefaultValue,
														#TMP_NewColumns.AllowNulls,
														#TMP_NewColumns.IsIdentity
											FROM		#TMP_NewColumns LEFT JOIN #TMP_OldColumns ON #TMP_NewColumns.Name = #TMP_OldColumns.Name
											WHERE		ISNULL(#TMP_OldColumns.Name, '') = ''

		OPEN C2
		FETCH NEXT FROM C2 INTO @C2_New_Name, @C2_New_Position, @C2_New_DataType, @C2_New_Length, @C2_New_DefaultValue, @C2_New_AllowNulls, @C2_New_IsIdentity
		WHILE @@FETCH_STATUS = 0
		  BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('Table: ' + @C_NewTableName)
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				SELECT	@PrevColumnName = '',
						@NextColumnName = ''

				SELECT	@PrevColumnName = Name
				FROM	#TMP_NewColumns
				WHERE	Position = @C2_New_Position - 1

				SELECT	@NextColumnName = Name
				FROM	#TMP_NewColumns
				WHERE	Position = @C2_New_Position + 1

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('AddColumn     - ' + @C2_New_Name + ', Position: ' + CONVERT(NVARCHAR(20), @C2_New_Position) + ', PrevColumn: ' + @PrevColumnName + ', NextColumn: ' + @NextColumnName + ', DataType: ' + @C2_New_DataType + ', Length: ' + CONVERT(NVARCHAR(20), @C2_New_Length) + ', DefaultValue: ' + ISNULL(@C2_New_DefaultValue, 'Nincsen') + ', AllowNulls: ' + CONVERT(NVARCHAR(20), @C2_New_AllowNulls) + ', Identity: ' + CONVERT(NVARCHAR(20), @C2_New_IsIdentity))

				FETCH NEXT FROM C2 INTO @C2_New_Name, @C2_New_Position, @C2_New_DataType, @C2_New_Length, @C2_New_DefaultValue, @C2_New_AllowNulls, @C2_New_IsIdentity
		  END
		CLOSE C2
		DEALLOCATE C2

		DECLARE C2 CURSOR LOCAL STATIC FOR	SELECT	#TMP_OldColumns.Name
											FROM	#TMP_OldColumns
													LEFT JOIN	#TMP_NewColumns		ON #TMP_OldColumns.Name = #TMP_NewColumns.Name
											WHERE	ISNULL(#TMP_NewColumns.Name, '') = ''

		OPEN C2
		FETCH NEXT FROM C2 INTO @C2_Name
		WHILE @@FETCH_STATUS = 0
		  BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('Table: ' + @C_NewTableName)
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

			INSERT INTO #TMP_OUT (Line_Text) VALUES ('DropColumn    - ' + @C2_Name)

			FETCH NEXT FROM C2 INTO @C2_Name
		  END
		CLOSE C2
		DEALLOCATE C2

		INSERT INTO #TMP_Columns (OldColumnName, OldColumnDataType, OldColumnLength, OldColumnDefaultValue, OldColumnAllowNulls, OldColumnIdentity, NewColumnDataType, NewColumnLength, NewColumnDefaultValue, NewColumnAllowNulls, NewColumnIdentity)
		SELECT	#TMP_OldColumns.Name,			-- OldColumnName
				#TMP_OldColumns.DataType,		-- OldColumnDataType
				#TMP_OldColumns.[Length],		-- OldColumnLength
				#TMP_OldColumns.DefaultValue,	-- OldColumnDefaultValue
				#TMP_OldColumns.AllowNulls,		-- OldColumnAllowNulls
				#TMP_OldColumns.IsIdentity,		-- OldColumnIdentity
				#TMP_NewColumns.DataType,		-- NewColumnDataType
				#TMP_NewColumns.[Length],		-- NewColumnLength
				#TMP_NewColumns.DefaultValue,	-- NewColumnDefaultValue
				#TMP_NewColumns.AllowNulls,		-- NewColumnAllowNulls
				#TMP_NewColumns.IsIdentity		-- NewColumnIdentity
		FROM	#TMP_NewColumns
				INNER JOIN		#TMP_OldColumns		ON #TMP_NewColumns.Name = #TMP_OldColumns.Name
   

		DECLARE C2 CURSOR LOCAL STATIC FOR	SELECT		OldColumnName, OldColumnDataType, NewColumnDataType, OldColumnLength, NewColumnLength, OldColumnDefaultValue, NewColumnDefaultValue, OldColumnAllowNulls, NewColumnAllowNulls, OldColumnIdentity, NewColumnIdentity
											FROM		#TMP_Columns
											WHERE		(
															OldColumnDataType <> NewColumnDataType
															OR OldColumnLength <> NewColumnLength
															OR OldColumnDefaultValue<>NewColumnDefaultValue
															OR OldColumnAllowNulls<>NewColumnAllowNulls
															OR OldColumnIdentity<>NewColumnIdentity
														)


		OPEN C2
		FETCH NEXT FROM C2 INTO @C2_OldColumnName, @C2_OldColumnDataType, @C2_NewColumnDataType, @C2_OldColumnLength, @C2_NewColumnLength, @C2_OldColumnDefaultValue, @C2_NewColumnDefaultValue, @C2_OldColumnAllowNulls, @C2_NewColumnAllowNulls, @C2_OldColumnIdentity, @C2_NewColumnIdentity
		WHILE @@FETCH_STATUS = 0
		  BEGIN
				IF @Title_Written = 0
				   BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('Table: ' + @C_NewTableName)
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('AlterColumn   - ' + @C2_OldColumnName + SPACE(50 - LEN(@C2_OldColumnName))
											+	CASE
													WHEN @C2_OldColumnDataType <> @C2_NewColumnDataType			THEN ', NewDataType: ' + @C2_NewColumnDataType
													ELSE ''
												END
											+	CASE
													WHEN @C2_OldColumnLength <> @C2_NewColumnLength				THEN ', NewLength: ' + CONVERT(NVARCHAR(20), @C2_NewColumnLength)
													ELSE ''
												END
											+	CASE
													WHEN @C2_OldColumnDefaultValue Is Null And @C2_NewColumnDefaultValue Is Not Null THEN ', NewDefaultValue: ' + CONVERT(NVARCHAR(4000), @C2_NewColumnDefaultValue)
													WHEN @C2_OldColumnDefaultValue Is Not Null And @C2_NewColumnDefaultValue Is Null THEN ', NewDefaultValue: Null'
													WHEN @C2_OldColumnDefaultValue <> @C2_NewColumnDefaultValue	THEN ', NewDefaultValue: ' + CONVERT(NVARCHAR(4000), @C2_NewColumnDefaultValue)
													ELSE ''
												END
											+	CASE
													WHEN @C2_OldColumnAllowNulls <> @C2_NewColumnAllowNulls		THEN ', NewAllowNulls: ' + CONVERT(NVARCHAR(20), @C2_NewColumnAllowNulls)
													ELSE ''
												END
											+	CASE
													WHEN @C2_OldColumnIdentity <> @C2_NewColumnIdentity			THEN ', NewIdentity: ' + CONVERT(NVARCHAR(20), @C2_NewColumnIdentity)
													ELSE ''
												END
									)

				FETCH NEXT FROM C2 INTO @C2_OldColumnName, @C2_OldColumnDataType, @C2_NewColumnDataType, @C2_OldColumnLength, @C2_NewColumnLength, @C2_OldColumnDefaultValue, @C2_NewColumnDefaultValue, @C2_OldColumnAllowNulls, @C2_NewColumnAllowNulls, @C2_OldColumnIdentity, @C2_NewColumnIdentity
		  END
		CLOSE C2
		DEALLOCATE C2

		FETCH NEXT FROM C INTO @C_OldTableID, @C_OldTableName, @C_NewTableID, @C_NewTableName
  END

CLOSE C
DEALLOCATE C


INSERT INTO #TMP_ObjectsCode (OldObjectID, OldObjectName, OldObjectType, NewObjectID, NewObjectName, NewObjectType)
SELECT		#TMP_OldObjects.ID,				-- OldObjectID
			#TMP_OldObjects.Name,			-- OldObjectName
			#TMP_OldObjects.ObjectType,		-- OldObjectType
			#TMP_NewObjects.ID,				-- NewObjectID
			#TMP_NewObjects.Name,			-- NewObjectName
			#TMP_NewObjects.ObjectType		-- NewObjectType
FROM		#TMP_OldObjects
			INNER JOIN #TMP_NewObjects  ON  #TMP_OldObjects.Name = #TMP_NewObjects.Name
										AND #TMP_OldObjects.ObjectType = #TMP_NewObjects.ObjectType
WHERE		#TMP_NewObjects.ObjectType <> 'U'
			AND #TMP_NewObjects.ObjectType <> 'index'
ORDER BY    #TMP_OldObjects.ObjectType,
			#TMP_OldObjects.Name

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	OldObjectID
									FROM	#TMP_ObjectsCode
OPEN C
	FETCH NEXT FROM C INTO @C_OldObjectID
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		 SELECT @CodeText = ''

		--SELECT @CodeText = dbo.Services_getSTR_for_Compare([Text]) FROM syscomments WHERE id=@C_OldObjectID ORDER BY colid

		SELECT		@CodeText = defs.definition
		FROM		sys.objects							obj
					INNER JOIN		sys.sql_modules		defs		ON	obj.object_id = defs.object_id
		WHERE		obj.object_id = @C_OldObjectID

		UPDATE #TMP_ObjectsCode	SET		OldObjectCode	= @CodeText,
										OldObjectLength	= DATALENGTH(@CodeText)
								WHERE	OldObjectID = @C_oldobjectid

		FETCH NEXT FROM C INTO @C_OldObjectID
	  END
CLOSE C
DEALLOCATE C

USE <New_DB>

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	NewObjectID
									FROM	#TMP_ObjectsCode
OPEN C
	FETCH NEXT FROM C INTO @C_NewObjectID
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		 SELECT @CodeText = ''

		--SELECT @CodeText = dbo.Services_getSTR_for_Compare([Text]) FROM <New_DB>..syscomments WHERE id=@C_NewObjectID ORDER BY colid
		SELECT		@CodeText = defs.definition
		FROM		sys.objects							obj
					INNER JOIN		sys.sql_modules		defs		ON	obj.object_id = defs.object_id
		WHERE		obj.object_id = @C_NewObjectID

		UPDATE #TMP_ObjectsCode	SET		NewObjectCode	= @CodeText,
										NewObjectLength	= DATALENGTH(@CodeText)
								WHERE	NewObjectID = @C_NewObjectid

		FETCH NEXT FROM C INTO @C_NewObjectID
	  END
CLOSE C
DEALLOCATE C

USE <Old_DB>

UPDATE #TMP_ObjectsCode SET		OldObjectCode	= dbo.Services_getSTR_for_Compare(OldObjectCode),
								NewObjectCode	= dbo.Services_getSTR_for_Compare(NewObjectCode)

INSERT INTO #TMP_ObjectsCode_Compare (ObjectName, PartNo, OldObjectCode, NewObjectCode, Differ)
SELECT	OldObjectName,			-- ObjectName
		1,						-- PartNo
		OldObjectCode,			-- OldObjectCode
		NewObjectCode,			-- NewObjectCode
		0						-- Differ
FROM	#TMP_ObjectsCode


UPDATE #TMP_ObjectsCode_Compare		SET		Differ = 1
									WHERE	OldObjectCode <> NewObjectCode

SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		#TMP_ObjectsCode_Compare.ObjectName
									 FROM		#TMP_ObjectsCode
												INNER JOIN		#TMP_ObjectsCode_Compare	ON #TMP_ObjectsCode.OldObjectName = #TMP_ObjectsCode_Compare.ObjectName
									 WHERE		#TMP_ObjectsCode.OldObjectType = 'TR'
												AND #TMP_ObjectsCode_Compare.Differ = 1
									 GROUP BY	#TMP_ObjectsCode_Compare.ObjectName
									 ORDER BY	#TMP_ObjectsCode_Compare.ObjectName

OPEN C
	FETCH NEXT FROM C INTO @C_ObjectName
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @Title_Written = 0
				BEGIN
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN TRIGGERS')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					SELECT @Title_Written = 1
				END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_ObjectName)

			FETCH NEXT FROM C INTO @C_ObjectName
	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		#TMP_ObjectsCode_Compare.ObjectName
									FROM		#TMP_ObjectsCode
												INNER JOIN #TMP_ObjectsCode_Compare		ON #TMP_ObjectsCode.OldObjectName = #TMP_ObjectsCode_Compare.ObjectName
									WHERE		#TMP_ObjectsCode.OldObjectType = 'V'
												AND #TMP_ObjectsCode_Compare.Differ = 1
									GROUP BY	#TMP_ObjectsCode_Compare.ObjectName
									ORDER BY	#TMP_ObjectsCode_Compare.ObjectName

OPEN C
	FETCH NEXT FROM C INTO @C_ObjectName
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @Title_Written = 0
				BEGIN
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN VIEWS')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					SELECT @Title_Written = 1
				END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_ObjectName)

			FETCH NEXT FROM C INTO @C_ObjectName
	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0
  
DECLARE C CURSOR LOCAL STATIC FOR	SELECT		#TMP_ObjectsCode_Compare.ObjectName
									FROM		#TMP_ObjectsCode
												INNER JOIN	#TMP_ObjectsCode_Compare	ON #TMP_ObjectsCode.OldObjectName = #TMP_ObjectsCode_Compare.ObjectName
									WHERE		#TMP_ObjectsCode.OldObjectType = 'P'
												AND #TMP_ObjectsCode_Compare.Differ = 1
									GROUP BY	#TMP_ObjectsCode_Compare.ObjectName
									ORDER BY	#TMP_ObjectsCode_Compare.ObjectName

OPEN C
	FETCH NEXT FROM C INTO @C_ObjectName
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @Title_Written = 0
				BEGIN
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN STORED PROCEDURES')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					SELECT @Title_Written = 1
				END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_ObjectName)

			FETCH NEXT FROM C INTO @C_ObjectName
	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		#TMP_ObjectsCode_Compare.ObjectName
									FROM		#TMP_ObjectsCode
												INNER JOIN	#TMP_ObjectsCode_Compare	ON #TMP_ObjectsCode.OldObjectName = #TMP_ObjectsCode_Compare.ObjectName
									WHERE		#TMP_ObjectsCode.OldObjectType = 'FN'
												AND #TMP_ObjectsCode_Compare.Differ = 1
									GROUP BY	#TMP_ObjectsCode_Compare.ObjectName
									ORDER BY	#TMP_ObjectsCode_Compare.ObjectName


OPEN C
	FETCH NEXT FROM C INTO @C_ObjectName
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @Title_Written = 0
				BEGIN
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN FUNCTIONS')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					SELECT @Title_Written = 1
				END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_ObjectName)

			FETCH NEXT FROM C INTO @C_ObjectName
	  END
CLOSE C
DEALLOCATE C

SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		#TMP_ObjectsCode_Compare.ObjectName
									FROM		#TMP_ObjectsCode 
												INNER JOIN #TMP_ObjectsCode_Compare		ON #TMP_ObjectsCode.OldObjectName = #TMP_ObjectsCode_Compare.ObjectName
									WHERE		#TMP_ObjectsCode.OldObjectType = 'TF'
												AND #TMP_ObjectsCode_Compare.Differ = 1
									GROUP BY	#TMP_ObjectsCode_Compare.ObjectName
									ORDER BY	#TMP_ObjectsCode_Compare.ObjectName


OPEN C
	FETCH NEXT FROM C INTO @C_ObjectName
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @Title_Written = 0
				BEGIN
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN TABLE FUNCTIONS')
					INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
					SELECT @Title_Written = 1
				END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@C_ObjectName)

			FETCH NEXT FROM C INTO @C_ObjectName
	  END
CLOSE C
DEALLOCATE C

-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT	@Title_Written	= 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		D.Identifier,
												D.Sprache											Sprache,
												D.[User]											[User],
												D.UserGruppe										UserGruppe
									FROM		DispoDefKopf0		D
												LEFT JOIN <New_DB>..DispoDefKopf0		NewDispo	ON	D.Identifier COLLATE DATABASE_DEFAULT = NewDispo.Identifier COLLATE DATABASE_DEFAULT
																										And D.Sprache COLLATE DATABASE_DEFAULT = NewDispo.Sprache COLLATE DATABASE_DEFAULT
																										And ISNULL(D.[User], 0) = ISNULL(NewDispo.[User], 0)
																										And ISNULL(D.UserGruppe, 0) = ISNULL(NewDispo.UserGruppe, 0)
									WHERE		NewDispo.Identifier Is Null

  OPEN C
	FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @C_IsChanged = 1
			   BEGIN
					IF @Title_Written = 0
						BEGIN
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('DELETE FROM STANDARD TABLE: DISPODEFKOPF0')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							SELECT @Title_Written = 1
						END

					SELECT @wText = 'Identifier: ' + @C_Identifier + ' Sprache: ' + @C_Sprache + ' User: ' + CONVERT(NVARCHAR(20), @C_User) + ' UserGruppe: ' + CONVERT(NVARCHAR(20), @C_UserGruppe)

					INSERT INTO #TMP_OUT (Line_Text) VALUES (@wText)
			   END

			FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe
	  END
CLOSE C
DEALLOCATE C


SELECT	@Title_Written	= 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		NewDispo.Identifier,
												NewDispo.Sprache							Sprache,
												NewDispo.[User]								[User],
												NewDispo.UserGruppe							UserGruppe
									FROM		<New_DB>..DispoDefKopf0		NewDispo
												LEFT JOIN 	DispoDefKopf0		D	ON	NewDispo.Identifier COLLATE DATABASE_DEFAULT = D.Identifier COLLATE DATABASE_DEFAULT
																						And NewDispo.Sprache COLLATE DATABASE_DEFAULT = D.Sprache COLLATE DATABASE_DEFAULT
																						And ISNULL(NewDispo.[User], 0) = ISNULL(D.[User], 0)
																						And ISNULL(NewDispo.UserGruppe, 0) = ISNULL(D.UserGruppe, 0)
									WHERE		NewDispo.Identifier Is Null

  OPEN C
	FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @C_IsChanged = 1
			   BEGIN
					IF @Title_Written = 0
						BEGIN
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('INSERT INTO STANDARD TABLE: DISPODEFKOPF0')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							SELECT @Title_Written = 1
						END

					SELECT @wText = 'Identifier: ' + @C_Identifier + ' Sprache: ' + @C_Sprache + ' User: ' + CONVERT(NVARCHAR(20), @C_User) + ' UserGruppe: ' + CONVERT(NVARCHAR(20), @C_UserGruppe)

					INSERT INTO #TMP_OUT (Line_Text) VALUES (@wText)
			   END

			FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe, @C_FeldSELECT_Changed, @C_FeldFROM_Changed, @C_FeldORDERBY_Changed, @C_FeldWHERE_Changed, @C_WHEREQueryName_Changed, @C_WahlArt_Changed, @C_IDFeld_Changed, @C_StringValueFeld_Changed, @C_LongValueFeld_Changed, @C_SQLMehrFachWahlResyncCommand_Changed, @C_SQLMehrFachWahl_Changed, @C_DispoUpdate_Changed, @C_DoFilter_Identifier_Changed, @C_DispoUpdateAfterWahl_Changed, @C_StoredProcNameAfterWahl_Changed, @C_StoredProcNameAtTheEnd_Changed, @C_ReportName_Changed, @C_ReportOpenTyp_Changed, @C_SQLExcel_Changed, @C_SQLTxt_Changed, @C_ReportRichTextExport_Changed, @C_IsChanged
	  END
CLOSE C
DEALLOCATE C








SELECT	@Title_Written	= 0
SELECT	@wText			= '',
        @wAnd			= ''

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		D.Identifier,
												D.Sprache											Sprache,
												D.[User]											[User],
												D.UserGruppe										UserGruppe,

												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldSELECT) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldSELECT) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													FeldSELECT,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldFROM) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldFROM) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													FeldFROM,
												CASE
													WHEN D.FeldORDERBY COLLATE DATABASE_DEFAULT <> NewDispo.FeldORDERBY COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													FeldORDERBY,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldWHERE) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldWHERE) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													FeldWHERE,
												CASE
													WHEN D.WHEREQueryName COLLATE DATABASE_DEFAULT <> NewDispo.WHEREQueryName COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													WHEREQueryName,
												CASE
													WHEN D.WahlArt <> NewDispo.WahlArt THEN 1
													ELSE 0
												END													WahlArt,
												CASE
													WHEN D.IDFeld COLLATE DATABASE_DEFAULT <> NewDispo.IDFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													IDFeld,
												CASE
													WHEN D.StringValueFeld COLLATE DATABASE_DEFAULT <> NewDispo.StringValueFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													StringValueFeld,
												CASE
													WHEN D.LongValueFeld COLLATE DATABASE_DEFAULT <> NewDispo.LongValueFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													LongValueFeld,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLMehrFachWahlResyncCommand) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLMehrFachWahlResyncCommand) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													SQLMehrFachWahlResyncCommand,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLMehrFachWahl) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLMehrFachWahl) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													SQLMehrFachWahl,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.DispoUpdate) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.DispoUpdate) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													DispoUpdate,
												CASE
													WHEN D.DoFilter_Identifier COLLATE DATABASE_DEFAULT <> NewDispo.DoFilter_Identifier COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													DoFilter_Identifier,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.DispoUpdateAfterWahl) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.DispoUpdateAfterWahl) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													DispoUpdateAfterWahl,
												CASE
													WHEN D.StoredProcNameAfterWahl COLLATE DATABASE_DEFAULT <> NewDispo.StoredProcNameAfterWahl COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													StoredProcNameAfterWahl,
												CASE
													WHEN D.StoredProcNameAtTheEnd COLLATE DATABASE_DEFAULT <> NewDispo.StoredProcNameAtTheEnd COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													StoredProcNameAtTheEnd,
												CASE
													WHEN D.ReportName COLLATE DATABASE_DEFAULT <> NewDispo.ReportName COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													ReportName,
												CASE
													WHEN D.ReportOpenTyp <> NewDispo.ReportOpenTyp	THEN 1
													ELSE 0
												END													ReportOpenTyp,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLExcel) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLExcel) COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													SQLExcel,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLTxt) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLTxt) COLLATE DATABASE_DEFAULT		THEN 1
													ELSE	0
												END													SQLTxt,
												CASE
													WHEN D.ReportRichTextExport <> NewDispo.ReportRichTextExport							THEN 1
													ELSE 0
												END													ReportRichTextExport,

												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldSELECT) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldSELECT) COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldFROM) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldFROM) COLLATE DATABASE_DEFAULT			THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.FeldORDERBY COLLATE DATABASE_DEFAULT <> NewDispo.FeldORDERBY COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldWHERE) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldWHERE) COLLATE DATABASE_DEFAULT			THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.WHEREQueryName COLLATE DATABASE_DEFAULT <> NewDispo.WHEREQueryName COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.WahlArt <> NewDispo.WahlArt THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.IDFeld COLLATE DATABASE_DEFAULT <> NewDispo.IDFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.StringValueFeld COLLATE DATABASE_DEFAULT <> NewDispo.StringValueFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.LongValueFeld COLLATE DATABASE_DEFAULT <> NewDispo.LongValueFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLMehrFachWahlResyncCommand) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLMehrFachWahlResyncCommand) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLMehrFachWahl) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLMehrFachWahl) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.DispoUpdate) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.DispoUpdate) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.DoFilter_Identifier COLLATE DATABASE_DEFAULT <> NewDispo.DoFilter_Identifier COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.DispoUpdateAfterWahl) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.DispoUpdateAfterWahl) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.StoredProcNameAfterWahl COLLATE DATABASE_DEFAULT <> NewDispo.StoredProcNameAfterWahl COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.StoredProcNameAtTheEnd COLLATE DATABASE_DEFAULT <> NewDispo.StoredProcNameAtTheEnd COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.ReportName COLLATE DATABASE_DEFAULT <> NewDispo.ReportName COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.ReportOpenTyp <> NewDispo.ReportOpenTyp THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLExcel) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLExcel) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLTxt) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLTxt) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.ReportRichTextExport <> NewDispo.ReportRichTextExport THEN 1
													ELSE 0 END																IsChanged
									FROM		DispoDefKopf0		D
												INNER JOIN <New_DB>..DispoDefKopf0		NewDispo	ON	D.Identifier COLLATE DATABASE_DEFAULT = NewDispo.Identifier COLLATE DATABASE_DEFAULT
																											And D.Sprache COLLATE DATABASE_DEFAULT = NewDispo.Sprache COLLATE DATABASE_DEFAULT
																											And ISNULL(D.[User], 0) = ISNULL(NewDispo.[User], 0)
																											And ISNULL(D.UserGruppe, 0) = ISNULL(NewDispo.UserGruppe, 0)


  OPEN C
	FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe, @C_FeldSELECT_Changed, @C_FeldFROM_Changed, @C_FeldORDERBY_Changed, @C_FeldWHERE_Changed, @C_WHEREQueryName_Changed, @C_WahlArt_Changed, @C_IDFeld_Changed, @C_StringValueFeld_Changed, @C_LongValueFeld_Changed, @C_SQLMehrFachWahlResyncCommand_Changed, @C_SQLMehrFachWahl_Changed, @C_DispoUpdate_Changed, @C_DoFilter_Identifier_Changed, @C_DispoUpdateAfterWahl_Changed, @C_StoredProcNameAfterWahl_Changed, @C_StoredProcNameAtTheEnd_Changed, @C_ReportName_Changed, @C_ReportOpenTyp_Changed, @C_SQLExcel_Changed, @C_SQLTxt_Changed, @C_ReportRichTextExport_Changed, @C_IsChanged
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @C_IsChanged = 1
			   BEGIN
					IF @Title_Written = 0
						BEGIN
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN STANDARD TABLE: DISPODEFKOPF0')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							SELECT @Title_Written = 1
						END

					SELECT @wText = 'Identifier: ' + @C_Identifier + ' Sprache: ' + ISNULL(@C_Sprache, '') + ' User: ' + CONVERT(NVARCHAR(20), ISNULL(@C_User, 0)) + ' UserGruppe: ' + CONVERT(NVARCHAR(20), ISNULL(@C_UserGruppe, 0)) + ' Different fields: '

					IF @C_FeldSELECT_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'FeldSELECT'
							SELECT @wAnd = ', '
					   END

					IF @C_FeldFROM_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'FeldFROM'
							SELECT @wAnd = ', '
					   END

					IF @C_FeldORDERBY_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'FeldORDERBY'
							SELECT @wAnd = ', '
					   END

					IF @C_FeldWHERE_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'FeldWHERE'
							SELECT @wAnd = ', '
					   END

					IF @C_WhereQueryName_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'WhereQueryName'
							SELECT @wAnd = ', '
					   END

					IF @C_WahlArt_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'WahlArt'
							SELECT @wAnd = ', '
					   END

					IF @C_IDFeld_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'IDFeld'
							SELECT @wAnd = ', '
					   END

					IF @C_StringValueFeld_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'StringValueFeld'
							SELECT @wAnd = ', '
					   END

					IF @C_LongValueFeld_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'LongValueFeld'
							SELECT @wAnd = ', '
					   END

					IF @C_SQLMehrFachWahlResyncCommand_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'SQLMehrFachWahlResyncCommand'
							SELECT @wAnd = ', '
					   END

					IF @C_SQLMehrFachWahl_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'SQLMehrFachWahl'
							SELECT @wAnd = ', '
					   END

					IF @C_DispoUpdate_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'DispoUpdate'
							SELECT @wAnd = ', '
					   END

					IF @C_DoFilter_Identifier_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'DoFilter_Identifier'
							SELECT @wAnd = ', '
					   END

					IF @C_DispoUpdateAfterWahl_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'DispoUpdateAfterWahl'
							SELECT @wAnd = ', '
					   END

					IF @C_StoredProcNameAfterWahl_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'StoredProcNameAfterWahl'
							SELECT @wAnd = ', '
					   END

					IF @C_StoredProcNameAtTheEnd_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'StoredProcNameAtTheEnd'
							SELECT @wAnd = ', '
					   END

					IF @C_ReportName_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'ReportName'
							SELECT @wAnd = ', '
					   END

					IF @C_ReportOpenTyp_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'ReportOpenTyp'
							SELECT @wAnd = ', '
					   END

					IF @C_SQLExcel_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'SQLExcel'
							SELECT @wAnd = ', '
					   END

					IF @C_SQLTxt_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'SQLTxt'
							SELECT @wAnd = ', '
					   END

					IF @C_ReportRichTextExport_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'ReportRichTextExport'
							SELECT @wAnd = ', '
					   END

					INSERT INTO #TMP_OUT (Line_Text) VALUES (@wText)
			   END

			FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe, @C_FeldSELECT_Changed, @C_FeldFROM_Changed, @C_FeldORDERBY_Changed, @C_FeldWHERE_Changed, @C_WHEREQueryName_Changed, @C_WahlArt_Changed, @C_IDFeld_Changed, @C_StringValueFeld_Changed, @C_LongValueFeld_Changed, @C_SQLMehrFachWahlResyncCommand_Changed, @C_SQLMehrFachWahl_Changed, @C_DispoUpdate_Changed, @C_DoFilter_Identifier_Changed, @C_DispoUpdateAfterWahl_Changed, @C_StoredProcNameAfterWahl_Changed, @C_StoredProcNameAtTheEnd_Changed, @C_ReportName_Changed, @C_ReportOpenTyp_Changed, @C_SQLExcel_Changed, @C_SQLTxt_Changed, @C_ReportRichTextExport_Changed, @C_IsChanged
	  END
CLOSE C
DEALLOCATE C


-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT	@Title_Written	= 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		D.Identifier,
												D.Sprache											Sprache,
												D.[User]											[User],
												D.UserGruppe										UserGruppe
									FROM		DispoDefKopf		D
												LEFT JOIN <New_DB>..DispoDefKopf		NewDispo	ON	D.Identifier COLLATE DATABASE_DEFAULT = NewDispo.Identifier COLLATE DATABASE_DEFAULT
																										And D.Sprache COLLATE DATABASE_DEFAULT = NewDispo.Sprache COLLATE DATABASE_DEFAULT
																										And ISNULL(D.[User], 0) = ISNULL(NewDispo.[User], 0)
																										And ISNULL(D.UserGruppe, 0) = ISNULL(NewDispo.UserGruppe, 0)
									WHERE		NewDispo.Identifier Is Null

  OPEN C
	FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @C_IsChanged = 1
			   BEGIN
					IF @Title_Written = 0
						BEGIN
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('DELETE FROM TABLE: DISPODEFKOPF')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							SELECT @Title_Written = 1
						END

					SELECT @wText = 'Identifier: ' + @C_Identifier + ' Sprache: ' + @C_Sprache + ' User: ' + CONVERT(NVARCHAR(20), @C_User) + ' UserGruppe: ' + CONVERT(NVARCHAR(20), @C_UserGruppe)

					INSERT INTO #TMP_OUT (Line_Text) VALUES (@wText)
			   END

			FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe
	  END
CLOSE C
DEALLOCATE C


SELECT	@Title_Written	= 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		NewDispo.Identifier,
												NewDispo.Sprache							Sprache,
												NewDispo.[User]								[User],
												NewDispo.UserGruppe							UserGruppe
									FROM		<New_DB>..DispoDefKopf		NewDispo
												LEFT JOIN 	DispoDefKopf		D	ON	NewDispo.Identifier	COLLATE DATABASE_DEFAULT = D.Identifier COLLATE DATABASE_DEFAULT
																						And NewDispo.Sprache COLLATE DATABASE_DEFAULT = D.Sprache COLLATE DATABASE_DEFAULT
																						And ISNULL(NewDispo.[User], 0) = ISNULL(D.[User], 0)
																						And ISNULL(NewDispo.UserGruppe, 0) = ISNULL(D.UserGruppe, 0)
									WHERE		NewDispo.Identifier Is Null

  OPEN C
	FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @C_IsChanged = 1
			   BEGIN
					IF @Title_Written = 0
						BEGIN
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('INSERT INTO TABLE: DISPODEFKOPF')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							SELECT @Title_Written = 1
						END

					SELECT @wText = 'Identifier: ' + @C_Identifier + ' Sprache: ' + @C_Sprache + ' User: ' + CONVERT(NVARCHAR(20), @C_User) + ' UserGruppe: ' + CONVERT(NVARCHAR(20), @C_UserGruppe)

					INSERT INTO #TMP_OUT (Line_Text) VALUES (@wText)
			   END

			FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe, @C_FeldSELECT_Changed, @C_FeldFROM_Changed, @C_FeldORDERBY_Changed, @C_FeldWHERE_Changed, @C_WHEREQueryName_Changed, @C_WahlArt_Changed, @C_IDFeld_Changed, @C_StringValueFeld_Changed, @C_LongValueFeld_Changed, @C_SQLMehrFachWahlResyncCommand_Changed, @C_SQLMehrFachWahl_Changed, @C_DispoUpdate_Changed, @C_DoFilter_Identifier_Changed, @C_DispoUpdateAfterWahl_Changed, @C_StoredProcNameAfterWahl_Changed, @C_StoredProcNameAtTheEnd_Changed, @C_ReportName_Changed, @C_ReportOpenTyp_Changed, @C_SQLExcel_Changed, @C_SQLTxt_Changed, @C_ReportRichTextExport_Changed, @C_IsChanged
	  END
CLOSE C
DEALLOCATE C


SELECT	@Title_Written	= 0
SELECT	@wText			= '',
        @wAnd			= ''

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		D.Identifier,
												D.Sprache											Sprache,
												D.[User]											[User],
												D.UserGruppe										UserGruppe,

												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldSELECT) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldSELECT) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													FeldSELECT,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldFROM) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldFROM) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													FeldFROM,
												CASE
													WHEN D.FeldORDERBY COLLATE DATABASE_DEFAULT <> NewDispo.FeldORDERBY COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													FeldORDERBY,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldWHERE) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldWHERE) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													FeldWHERE,
												CASE
													WHEN D.WHEREQueryName COLLATE DATABASE_DEFAULT <> NewDispo.WHEREQueryName COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													WHEREQueryName,
												CASE
													WHEN D.WahlArt <> NewDispo.WahlArt THEN 1
													ELSE 0
												END													WahlArt,
												CASE
													WHEN D.IDFeld COLLATE DATABASE_DEFAULT <> NewDispo.IDFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													IDFeld,
												CASE
													WHEN D.StringValueFeld COLLATE DATABASE_DEFAULT <> NewDispo.StringValueFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													StringValueFeld,
												CASE
													WHEN D.LongValueFeld COLLATE DATABASE_DEFAULT <> NewDispo.LongValueFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													LongValueFeld,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLMehrFachWahlResyncCommand) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLMehrFachWahlResyncCommand) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													SQLMehrFachWahlResyncCommand,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLMehrFachWahl) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLMehrFachWahl) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													SQLMehrFachWahl,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.DispoUpdate) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.DispoUpdate) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													DispoUpdate,
												CASE
													WHEN D.DoFilter_Identifier COLLATE DATABASE_DEFAULT <> NewDispo.DoFilter_Identifier COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													DoFilter_Identifier,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.DispoUpdateAfterWahl) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.DispoUpdateAfterWahl) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END													DispoUpdateAfterWahl,
												CASE
													WHEN D.StoredProcNameAfterWahl COLLATE DATABASE_DEFAULT <> NewDispo.StoredProcNameAfterWahl COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													StoredProcNameAfterWahl,
												CASE
													WHEN D.StoredProcNameAtTheEnd COLLATE DATABASE_DEFAULT <> NewDispo.StoredProcNameAtTheEnd COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													StoredProcNameAtTheEnd,
												CASE
													WHEN D.ReportName COLLATE DATABASE_DEFAULT <> NewDispo.ReportName COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													ReportName,
												CASE
													WHEN D.ReportOpenTyp <> NewDispo.ReportOpenTyp	THEN 1
													ELSE 0
												END													ReportOpenTyp,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLExcel) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLExcel) COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													SQLExcel,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLTxt) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLTxt) COLLATE DATABASE_DEFAULT		THEN 1
													ELSE	0
												END													SQLTxt,
												CASE
													WHEN D.ReportRichTextExport <> NewDispo.ReportRichTextExport							THEN 1
													ELSE 0
												END													ReportRichTextExport,

												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldSELECT) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldSELECT) COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldFROM) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldFROM) COLLATE DATABASE_DEFAULT			THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.FeldORDERBY COLLATE DATABASE_DEFAULT <> NewDispo.FeldORDERBY COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.FeldWHERE) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.FeldWHERE) COLLATE DATABASE_DEFAULT			THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.WHEREQueryName COLLATE DATABASE_DEFAULT <> NewDispo.WHEREQueryName COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.WahlArt <> NewDispo.WahlArt THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.IDFeld COLLATE DATABASE_DEFAULT <> NewDispo.IDFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.StringValueFeld COLLATE DATABASE_DEFAULT <> NewDispo.StringValueFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.LongValueFeld COLLATE DATABASE_DEFAULT <> NewDispo.LongValueFeld COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLMehrFachWahlResyncCommand) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLMehrFachWahlResyncCommand) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLMehrFachWahl) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLMehrFachWahl) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.DispoUpdate) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.DispoUpdate) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.DoFilter_Identifier COLLATE DATABASE_DEFAULT <> NewDispo.DoFilter_Identifier COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.DispoUpdateAfterWahl) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.DispoUpdateAfterWahl) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.StoredProcNameAfterWahl COLLATE DATABASE_DEFAULT <> NewDispo.StoredProcNameAfterWahl COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.StoredProcNameAtTheEnd COLLATE DATABASE_DEFAULT <> NewDispo.StoredProcNameAtTheEnd COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.ReportName COLLATE DATABASE_DEFAULT <> NewDispo.ReportName COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.ReportOpenTyp <> NewDispo.ReportOpenTyp THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLExcel) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLExcel) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), D.SQLTxt) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewDispo.SQLTxt) COLLATE DATABASE_DEFAULT THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN D.ReportRichTextExport <> NewDispo.ReportRichTextExport THEN 1
													ELSE 0 END																IsChanged
									FROM		DispoDefKopf		D
												INNER JOIN <New_DB>..DispoDefKopf		NewDispo	ON	D.Identifier COLLATE DATABASE_DEFAULT = NewDispo.Identifier COLLATE DATABASE_DEFAULT
																											And D.Sprache COLLATE DATABASE_DEFAULT = NewDispo.Sprache COLLATE DATABASE_DEFAULT
																											And ISNULL(D.[User], 0) = ISNULL(NewDispo.[User], 0)
																											And ISNULL(D.UserGruppe, 0) = ISNULL(NewDispo.UserGruppe, 0)


  OPEN C
	FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe, @C_FeldSELECT_Changed, @C_FeldFROM_Changed, @C_FeldORDERBY_Changed, @C_FeldWHERE_Changed, @C_WHEREQueryName_Changed, @C_WahlArt_Changed, @C_IDFeld_Changed, @C_StringValueFeld_Changed, @C_LongValueFeld_Changed, @C_SQLMehrFachWahlResyncCommand_Changed, @C_SQLMehrFachWahl_Changed, @C_DispoUpdate_Changed, @C_DoFilter_Identifier_Changed, @C_DispoUpdateAfterWahl_Changed, @C_StoredProcNameAfterWahl_Changed, @C_StoredProcNameAtTheEnd_Changed, @C_ReportName_Changed, @C_ReportOpenTyp_Changed, @C_SQLExcel_Changed, @C_SQLTxt_Changed, @C_ReportRichTextExport_Changed, @C_IsChanged
	WHILE @@FETCH_STATUS = 0
	  BEGIN
			IF @C_IsChanged = 1
			   BEGIN
					IF @Title_Written = 0
						BEGIN
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN TABLE: DISPODEFKOPF')
							INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
							SELECT @Title_Written = 1
						END

					SELECT @wText = 'Identifier: ' + @C_Identifier + ' Sprache: ' + ISNULL(@C_Sprache, '') + ' User: ' + CONVERT(NVARCHAR(20), ISNULL(@C_User, 0)) + ' UserGruppe: ' + CONVERT(NVARCHAR(20), ISNULL(@C_UserGruppe, 0)) + ' Different fields: '

					IF @C_FeldSELECT_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'FeldSELECT'
							SELECT @wAnd = ', '
					   END

					IF @C_FeldFROM_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'FeldFROM'
							SELECT @wAnd = ', '
					   END

					IF @C_FeldORDERBY_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'FeldORDERBY'
							SELECT @wAnd = ', '
					   END

					IF @C_FeldWHERE_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'FeldWHERE'
							SELECT @wAnd = ', '
					   END

					IF @C_WhereQueryName_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'WhereQueryName'
							SELECT @wAnd = ', '
					   END

					IF @C_WahlArt_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'WahlArt'
							SELECT @wAnd = ', '
					   END

					IF @C_IDFeld_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'IDFeld'
							SELECT @wAnd = ', '
					   END

					IF @C_StringValueFeld_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'StringValueFeld'
							SELECT @wAnd = ', '
					   END

					IF @C_LongValueFeld_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'LongValueFeld'
							SELECT @wAnd = ', '
					   END

					IF @C_SQLMehrFachWahlResyncCommand_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'SQLMehrFachWahlResyncCommand'
							SELECT @wAnd = ', '
					   END

					IF @C_SQLMehrFachWahl_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'SQLMehrFachWahl'
							SELECT @wAnd = ', '
					   END

					IF @C_DispoUpdate_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'DispoUpdate'
							SELECT @wAnd = ', '
					   END

					IF @C_DoFilter_Identifier_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'DoFilter_Identifier'
							SELECT @wAnd = ', '
					   END

					IF @C_DispoUpdateAfterWahl_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'DispoUpdateAfterWahl'
							SELECT @wAnd = ', '
					   END

					IF @C_StoredProcNameAfterWahl_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'StoredProcNameAfterWahl'
							SELECT @wAnd = ', '
					   END

					IF @C_StoredProcNameAtTheEnd_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'StoredProcNameAtTheEnd'
							SELECT @wAnd = ', '
					   END

					IF @C_ReportName_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'ReportName'
							SELECT @wAnd = ', '
					   END

					IF @C_ReportOpenTyp_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'ReportOpenTyp'
							SELECT @wAnd = ', '
					   END

					IF @C_SQLExcel_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'SQLExcel'
							SELECT @wAnd = ', '
					   END

					IF @C_SQLTxt_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'SQLTxt'
							SELECT @wAnd = ', '
					   END

					IF @C_ReportRichTextExport_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'ReportRichTextExport'
							SELECT @wAnd = ', '
					   END

					INSERT INTO #TMP_OUT (Line_Text) VALUES (@wText)
			   END

			FETCH NEXT FROM C INTO @C_Identifier, @C_Sprache, @C_User, @C_UserGruppe, @C_FeldSELECT_Changed, @C_FeldFROM_Changed, @C_FeldORDERBY_Changed, @C_FeldWHERE_Changed, @C_WHEREQueryName_Changed, @C_WahlArt_Changed, @C_IDFeld_Changed, @C_StringValueFeld_Changed, @C_LongValueFeld_Changed, @C_SQLMehrFachWahlResyncCommand_Changed, @C_SQLMehrFachWahl_Changed, @C_DispoUpdate_Changed, @C_DoFilter_Identifier_Changed, @C_DispoUpdateAfterWahl_Changed, @C_StoredProcNameAfterWahl_Changed, @C_StoredProcNameAtTheEnd_Changed, @C_ReportName_Changed, @C_ReportOpenTyp_Changed, @C_SQLExcel_Changed, @C_SQLTxt_Changed, @C_ReportRichTextExport_Changed, @C_IsChanged
	  END
CLOSE C
DEALLOCATE C

SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		NewFixText0.Code
									FROM		<New_DB>..FixText0	NewFixText0
												LEFT JOIN 	FixText0	F			 ON	NewFixText0.Code = NewFixText0.Code
									WHERE		F.Code IS NULL


OPEN C
	FETCH NEXT FROM C INTO @C_Code
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD CODE IN STANDARD TABLE FIXTEXT0')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Code: ' + @C_Code)
        END

		FETCH NEXT FROM C INTO @C_Code

	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		F.Code
									FROM		FixText0	F
												LEFT JOIN <New_DB>..FixText0		NewFixText0 ON	F.Code COLLATE DATABASE_DEFAULT = NewFixText0.Code COLLATE DATABASE_DEFAULT
									WHERE		NewFixText0.Code IS NULL


OPEN C
	FETCH NEXT FROM C INTO @C_Code
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DELETE CODE IN STANDARD TABLE FIXTEXT0')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Code: ' + @C_Code)
        END

		FETCH NEXT FROM C INTO @C_Code

	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		F.Code,
												CASE
													WHEN F.Titel COLLATE DATABASE_DEFAULT <> NewFixText.Titel COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END													Titel_Changed,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), F.Text) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewFixText.Text) COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													Text_Changed,

												CASE
													WHEN F.Titel COLLATE DATABASE_DEFAULT <> NewFixText.Titel COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), F.[Text]) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewFixText.[Text]) COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													 IsChanged
									FROM		FixText0	F
												INNER JOIN <New_DB>..FixText0		NewFixText ON	F.Code COLLATE DATABASE_DEFAULT = NewFixText.Code COLLATE DATABASE_DEFAULT


OPEN C
	FETCH NEXT FROM C INTO @C_Code, @C_Titel_Changed, @C_Text_Changed, @C_IsChanged
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN STANDARD TABLE: FIXTEXT0')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				IF @C_Titel_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'Titel'
						SELECT @wAnd = ', '
				   END

				IF @C_Text_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'Text'
						SELECT @wAnd = ', '
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Code: ' + @C_Code + ' (Fields: ' + @wText + ')')
        END

		FETCH NEXT FROM C INTO @C_Code, @C_Titel_Changed, @C_Text_Changed, @C_IsChanged

	  END
CLOSE C
DEALLOCATE C




SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		NewFixText.Code
									FROM		<New_DB>..FixText	NewFixText
												LEFT JOIN 	FixText	F			 ON	NewFixText.Code = NewFixText.Code
									WHERE		F.Code IS NULL


OPEN C
	FETCH NEXT FROM C INTO @C_Code
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD CODE IN TABLE FIXTEXT')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Code: ' + @C_Code)
        END

		FETCH NEXT FROM C INTO @C_Code

	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		F.Code
									FROM		FixText	F
												LEFT JOIN <New_DB>..FixText		NewFixText ON	F.Code COLLATE DATABASE_DEFAULT = NewFixText.Code COLLATE DATABASE_DEFAULT
									WHERE		NewFixText.Code IS NULL


OPEN C
	FETCH NEXT FROM C INTO @C_Code
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DELETE CODE IN FIXTEXT')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Code: ' + @C_Code)
        END

		FETCH NEXT FROM C INTO @C_Code

	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT		F.Code,
												CASE
													WHEN F.Titel COLLATE DATABASE_DEFAULT <> NewFixText.Titel COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END													Titel_Changed,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), F.Text) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewFixText.Text) COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													Text_Changed,

												CASE
													WHEN F.Titel COLLATE DATABASE_DEFAULT <> NewFixText.Titel COLLATE DATABASE_DEFAULT		THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), F.[Text]) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewFixText.[Text]) COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END													 IsChanged
									FROM		FixText	F
												INNER JOIN <New_DB>..FixText		NewFixText ON	F.Code COLLATE DATABASE_DEFAULT = NewFixText.Code COLLATE DATABASE_DEFAULT


OPEN C
	FETCH NEXT FROM C INTO @C_Code, @C_Titel_Changed, @C_Text_Changed, @C_IsChanged
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN TABLE: FIXTEXT')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				IF @C_Titel_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'Titel'
						SELECT @wAnd = ', '
				   END

				IF @C_Text_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'Text'
						SELECT @wAnd = ', '
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Code: ' + @C_Code + ' (Fields: ' + @wText + ')')
        END

		FETCH NEXT FROM C INTO @C_Code, @C_Titel_Changed, @C_Text_Changed, @C_IsChanged

	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0
  
DECLARE C CURSOR LOCAL STATIC FOR	SELECT		P.Sprache,
												P.KulcsElotag,
												P.Kulcs,
												CASE
													WHEN P.GotoID <> NewParam.GotoID	THEN 1
													ELSE 0
												END												GotoID,
												CASE
													WHEN CONVERT(NVARCHAR(MAX), P.Beschreibung) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewParam.Beschreibung) COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END												Beschreibung,

												CASE
													WHEN P.GotoID <> NewParam.GotoID	THEN 1
													ELSE 0
												END
												+
												CASE
													WHEN CONVERT(NVARCHAR(MAX), P.Beschreibung) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), NewParam.Beschreibung) COLLATE DATABASE_DEFAULT	THEN 1
													ELSE 0
												END												IsChanged
									FROM		ParamBeschreibung	P
												INNER JOIN <New_DB>..ParamBeschreibung		NewParam	ON	P.Sprache COLLATE DATABASE_DEFAULT = NewParam.Sprache COLLATE DATABASE_DEFAULT
																											AND P.KulcsElotag COLLATE DATABASE_DEFAULT = NewParam.KulcsElotag COLLATE DATABASE_DEFAULT
																													AND P.Kulcs COLLATE DATABASE_DEFAULT = NewParam.Kulcs COLLATE DATABASE_DEFAULT

OPEN C
	FETCH NEXT FROM C INTO @C_Sprache, @C_KulcsElotag, @C_Kulcs, @C_GotoID_Changed, @C_Beschreibung_Changed, @C_IsChanged
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN STANDARD TABLE: PARAMBESCHREIBUNG')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= 'Sprache: ' + @C_Sprache + ' KulcsElotag: ' + ISNULL(@C_KulcsElotag, '') + ' Kulcs: ' + ISNULL(@C_Kulcs, '') + ' Fields: ',
							@wAnd	= ''

					IF @C_GotoID_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'GotoID'
							SELECT @wAnd = ', '
					   END

					IF @C_Beschreibung_Changed <> 0
					   BEGIN
							SELECT @wText = @wText + @wAnd + 'Beschreibung'
							SELECT @wAnd = ', '
					   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES (@wText)
		   END

		FETCH NEXT FROM C INTO @C_Sprache, @C_KulcsElotag, @C_Kulcs, @C_GotoID_Changed, @C_Beschreibung_Changed, @C_IsChanged
	  END
 -- END

CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	P.KulcsElotag																KulcsElotag,
											P.Kulcs																		Kulcs
									FROM	Parameterek0	P
											LEFT JOIN <New_DB>..Parameterek0		NewParam	ON	P.KulcsElotag COLLATE DATABASE_DEFAULT = NewParam.KulcsElotag COLLATE DATABASE_DEFAULT
																									AND P.Kulcs COLLATE DATABASE_DEFAULT = NewParam.Kulcs COLLATE DATABASE_DEFAULT
									WHERE	NewParam.KulcsElotag Is Null

OPEN C
	FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DELETE FROM STANDARD TABLE: PARAMETEREK0')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Kulcs: ' + @C_KulcsElotag + '/' + @C_Kulcs)
           END
  
		FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs
	  END
CLOSE C
DEALLOCATE C



SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	NewParam.KulcsElotag								KulcsElotag,
											NewParam.Kulcs										Kulcs
									FROM	<New_DB>..Parameterek0	NewParam
											LEFT JOIN 	Parameterek0	P		ON	NewParam.KulcsElotag COLLATE DATABASE_DEFAULT = P.KulcsElotag COLLATE DATABASE_DEFAULT
																					AND NewParam.Kulcs COLLATE DATABASE_DEFAULT = P.Kulcs COLLATE DATABASE_DEFAULT
									WHERE	NewParam.KulcsElotag Is Null

OPEN C
	FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD TO STANDARD TABLE: PARAMETEREK0')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Kulcs: ' + @C_KulcsElotag + '/' + @C_Kulcs)
           END
  
		FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs
	  END
CLOSE C
DEALLOCATE C



SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	P.KulcsElotag																KulcsElotag,
											P.Kulcs																		Kulcs,
											CASE
												WHEN P.NumErtek <> NewParam.NumErtek			THEN 1
												ELSE 0
											END																			NumErtek,

											CASE
												WHEN P.AlfaNumErtek COLLATE DATABASE_DEFAULT <> NewParam.AlfaNumErtek COLLATE DATABASE_DEFAULT	THEN 1
												ELSE 0
											END																			AlfaNumErtek,
											CASE
												WHEN P.NumErtek <> NewParam.NumErtek			THEN 1
												ELSE 0
											END
											+
											CASE
												WHEN P.AlfaNumErtek COLLATE DATABASE_DEFAULT <> NewParam.AlfaNumErtek COLLATE DATABASE_DEFAULT	THEN 1
												ELSE 0
											END																			IsChanged
									FROM	Parameterek0	P
											INNER JOIN <New_DB>..Parameterek0		NewParam	ON	P.KulcsElotag COLLATE DATABASE_DEFAULT = NewParam.KulcsElotag COLLATE DATABASE_DEFAULT
																										AND P.Kulcs COLLATE DATABASE_DEFAULT = NewParam.Kulcs COLLATE DATABASE_DEFAULT

OPEN C
	FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs, @C_NumErtek_Changed, @C_AlfaNumErtek_Changed, @C_IsChanged
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN STANDARD TABLE: PARAMETEREK0')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				IF @C_NumErtek_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'NumErtek'
						SELECT @wAnd = ', '
				   END

				IF @C_AlfaNumertek_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'AlfaNumErtek'
						SELECT @wAnd = ', '
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Kulcs: ' + @C_KulcsElotag + '/' + @C_Kulcs + ' (Fields: ' + @wText + ')')
           END
  
		FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs, @C_NumErtek_Changed, @C_AlfaNumErtek_Changed, @C_IsChanged
	  END
CLOSE C
DEALLOCATE C




SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	P.KulcsElotag																KulcsElotag,
											P.Kulcs																		Kulcs
									FROM	Parameterek	P
											LEFT JOIN <New_DB>..Parameterek		NewParam	ON	P.KulcsElotag COLLATE DATABASE_DEFAULT = NewParam.KulcsElotag COLLATE DATABASE_DEFAULT
																									AND P.Kulcs COLLATE DATABASE_DEFAULT = NewParam.Kulcs COLLATE DATABASE_DEFAULT
									WHERE	NewParam.KulcsElotag Is Null

OPEN C
	FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('DELETE FROM TABLE: PARAMETEREK')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Kulcs: ' + @C_KulcsElotag + '/' + @C_Kulcs)
           END
  
		FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs
	  END
CLOSE C
DEALLOCATE C



SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	NewParam.KulcsElotag								KulcsElotag,
											NewParam.Kulcs										Kulcs
									FROM	<New_DB>..Parameterek	NewParam
											LEFT JOIN 	Parameterek	P		ON	NewParam.KulcsElotag COLLATE DATABASE_DEFAULT = P.KulcsElotag COLLATE DATABASE_DEFAULT
																					AND NewParam.Kulcs COLLATE DATABASE_DEFAULT = P.Kulcs COLLATE DATABASE_DEFAULT
									WHERE	NewParam.KulcsElotag Is Null

OPEN C
	FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('ADD TO TABLE: PARAMETEREK')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Kulcs: ' + @C_KulcsElotag + '/' + @C_Kulcs)
           END
  
		FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs
	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	P.KulcsElotag																KulcsElotag,
											P.Kulcs																		Kulcs,
											CASE
												WHEN P.NumErtek <> NewParam.NumErtek			THEN 1
												ELSE 0
											END																			NumErtek,

											CASE
												WHEN P.AlfaNumErtek COLLATE DATABASE_DEFAULT <> NewParam.AlfaNumErtek COLLATE DATABASE_DEFAULT	THEN 1
												ELSE 0
											END																			AlfaNumErtek,
											CASE
												WHEN P.NumErtek <> NewParam.NumErtek			THEN 1
												ELSE 0
											END
											+
											CASE
												WHEN P.AlfaNumErtek COLLATE DATABASE_DEFAULT <> NewParam.AlfaNumErtek COLLATE DATABASE_DEFAULT	THEN 1
												ELSE 0
											END																			IsChanged
									FROM	Parameterek	P
											INNER JOIN <New_DB>..Parameterek		NewParam	ON	P.KulcsElotag COLLATE DATABASE_DEFAULT = NewParam.KulcsElotag COLLATE DATABASE_DEFAULT
																										AND P.Kulcs COLLATE DATABASE_DEFAULT = NewParam.Kulcs COLLATE DATABASE_DEFAULT

OPEN C
	FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs, @C_NumErtek_Changed, @C_AlfaNumErtek_Changed, @C_IsChanged
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN TABLE: PARAMETEREK')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				IF @C_NumErtek_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'NumErtek'
						SELECT @wAnd = ', '
				   END

				IF @C_AlfaNumertek_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'AlfaNumErtek'
						SELECT @wAnd = ', '
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('Kulcs: ' + @C_KulcsElotag + '/' + @C_Kulcs + ' (Fields: ' + @wText + ')')
           END
  
		FETCH NEXT FROM C INTO @C_KulcsElotag, @C_Kulcs, @C_NumErtek_Changed, @C_AlfaNumErtek_Changed, @C_IsChanged
	  END
CLOSE C
DEALLOCATE C


SELECT @Title_Written = 0

DECLARE C CURSOR LOCAL STATIC FOR	SELECT	R.ReTyp																		ReTyp,
											CASE
												WHEN R.Bord_Or_BordMegb <> NewReMask.Bord_Or_BordMegb	THEN 1
												ELSE 0
											END																			Bord_Or_BordMegb,
											CASE
												WHEN R.Mask <> NewReMask.Mask	THEN 1
												ELSE 0
											END																			Mask,
											CASE
												WHEN R.Bemerkung COLLATE DATABASE_DEFAULT <> NewReMask.Bemerkung COLLATE DATABASE_DEFAULT	THEN 1
												ELSE 0
											END																			Bemerkung,
											CASE
												WHEN R.Bord_Or_BordMegb <> NewReMask.Bord_Or_BordMegb	THEN 1
												ELSE 0
											END
											+
											CASE
												WHEN R.Mask <> NewReMask.Mask	THEN 1
												ELSE 0
											END
											+
											CASE
												WHEN R.Bemerkung COLLATE DATABASE_DEFAULT <> NewReMask.Bemerkung	COLLATE DATABASE_DEFAULT THEN 1
												ELSE 0
											END																			IsChanged
									FROM	ReMask		R
											INNER JOIN <New_DB>..ReMask	NewReMask	ON R.ReTyp = NewReMask.ReTyp

OPEN C
	FETCH NEXT FROM C INTO @C_ReTyp, @C_Bord_Or_BordMegb_Changed, @C_Mask_Changed, @C_Bemerkung_Changed, @C_IsChanged
	WHILE @@FETCH_STATUS = 0
	  BEGIN
		IF @C_IsChanged = 1
		   BEGIN
				IF @Title_Written = 0
					BEGIN
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('FIND DIFFERENCIES IN STANDARD TABLE: REMASK')
						INSERT INTO #TMP_OUT (Line_Text) VALUES ('----------------------------------------------------------------------------------------------------------------------------------------------------------------')
						SELECT @Title_Written = 1
					END

					SELECT	@wText	= '',
							@wAnd	= ''

				IF @C_Bord_Or_BordMegb_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'Bord_Or_BordMegb'
						SELECT @wAnd = ', '
				   END

				IF @C_Mask_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'Mask'
						SELECT @wAnd = ', '
				   END

				IF @C_Bemerkung_Changed <> 0
				   BEGIN
						SELECT @wText = @wText + @wAnd + 'Bemerkung'
						SELECT @wAnd = ', '
				   END

				INSERT INTO #TMP_OUT (Line_Text) VALUES ('ReTyp: ' + CONVERT(NVARCHAR(20), @C_ReTyp) + ' (Fields: ' + @wText + ')')
        END

		FETCH NEXT FROM C INTO @C_ReTyp, @C_Bord_Or_BordMegb_Changed, @C_Mask_Changed, @C_Bemerkung_Changed, @C_IsChanged
	  END
CLOSE C
DEALLOCATE C



-----------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Befejezes
-----------------------------------------------------------------------------------------------------------------------------------------------------------------

DROP TABLE #TMP_OldObjects
DROP TABLE #TMP_NewObjects
DROP TABLE #TMP_Tables
DROP TABLE #TMP_OldTablesPK
DROP TABLE #TMP_NewTablesPK
DROP TABLE #TMP_OldColumns
DROP TABLE #TMP_NewColumns
DROP TABLE #TMP_Columns
DROP TABLE #TMP_ObjectsCode
DROP TABLE #TMP_ObjectsCode_Compare

SELECT * FROM #TMP_OUT ORDER BY SeqNum
