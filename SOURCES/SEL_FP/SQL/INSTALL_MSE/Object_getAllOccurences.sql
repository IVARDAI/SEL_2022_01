declare @ObjectName			nvarchar(255)

SELECT	@ObjectName	= 'VERS 1.03 2014-05-01'

SELECT	'TABLE'		ObjectType,
		name		ObjectName
FROM	sysobjects
WHERE   xtype='U'
		And charindex(@ObjectName, name) > 0

SELECT	'Column'			ObjectType,
		syscolumns.name		ColumnName,
		Tables.name			TableName
FROM	syscolumns
		INNER JOIN	(
						SELECT  id,
								name
						FROM	sysobjects
						WHERE   xtype='U'
					) Tables    ON syscolumns.id=Tables.id
WHERE	charindex(@ObjectName, syscolumns.name) > 0

SELECT	'Primary Key'			ObjectType,
		PKs.name				PK_Name,
		sysobjects.name			Table_Name
FROM	sysobjects
		INNER JOIN  (
						SELECT		parent_obj,
									name
						FROM		sysobjects
						WHERE		xtype='PK'
					) PKs	ON sysobjects.id=PKs.parent_obj
WHERE	charindex(@ObjectName, PKs.name) > 0


SELECT		'View Name'				ObjectType,
			sysobjects.name			View_Name
FROM		sysobjects
WHERE		charindex(@ObjectName, sysobjects.name) > 0
			And xtype='V'
GROUP BY	sysobjects.name


SELECT		'View'				ObjectType,
			Views.name			View_Name
FROM		syscomments
			INNER JOIN		(
								SELECT	id,
										name
								FROM	sysobjects
								WHERE	xtype='V'
							) Views		ON syscomments.id=Views.id
WHERE		charindex(@ObjectName, syscomments.text) > 0
GROUP BY	Views.name


SELECT		'SP Name'			ObjectType,
			sysobjects.name		SP_Name
FROM		sysobjects
WHERE		isnull(charindex(@ObjectName, sysobjects.name),0)>0
			And xtype='P'
GROUP BY	sysobjects.name


SELECT		'SP'				ObjectType,
			SPs.name			SP_Name
FROM		syscomments
			INNER JOIN  (
							SELECT	id,
									name
							FROM	sysobjects
							WHERE	xtype='P'
						) SPs   ON syscomments.id=SPs.id
WHERE		charindex(@ObjectName, syscomments.text) > 0
GROUP BY SPs.name


SELECT		'Trigger Name'		ObjectType,
			sysobjects.name		Trigger_Name
FROM		sysobjects
WHERE		charindex(@ObjectName, sysobjects.name) > 0
			And xtype='TR'
GROUP BY	sysobjects.name


SELECT		'Trigger'			ObjectType,
			SPs.name			Trigger_Name
FROM		syscomments
			INNER JOIN	(
							SELECT	id,
									name
							FROM	sysobjects
							WHERE	xtype='TR'
						) SPs   ON syscomments.id=SPs.id
WHERE		charindex(@ObjectName, syscomments.text) > 0
GROUP BY	SPs.name


SELECT		'In FixText0'	ObjectType,
			Code			Code
FROM		FixText0
WHERE		charindex(@ObjectName, Text) > 0
ORDER BY	Code


SELECT		'In Fixtext'		ObjectType,
			Code				Code
FROM		FixText
WHERE		charindex(@ObjectName, Text) > 0
ORDER BY	Code


SELECT		'In DispoDefKopf0'		ObjectType,
			Identifier				Identifier
FROM		DispoDefKopf0
WHERE		charindex(@ObjectName, Identifier) > 0
			Or charindex(@ObjectName, FeldSELECT) > 0
			Or charindex(@ObjectName, FeldFROM) > 0
			Or charindex(@ObjectName, FeldORDERBY) > 0
			Or charindex(@ObjectName, FeldWHERE) > 0
			Or charindex(@ObjectName, WHEREQueryName) > 0
			Or charindex(@ObjectName, IDFeld) > 0
			Or charindex(@ObjectName, StringValueFeld) > 0
			Or charindex(@ObjectName, LongValueFeld) > 0
			Or charindex(@ObjectName, SQLMehrFachWahl) > 0
			Or charindex(@ObjectName, SQLMehrFachWahlResyncCommand) > 0
			Or charindex(@ObjectName, DispoUpdate) > 0
			Or charindex(@ObjectName, DoFilter_Identifier) > 0
			Or charindex(@ObjectName, DispoUpdateAfterWahl) > 0
			Or charindex(@ObjectName, StoredProcNameAfterWahl) > 0
			Or charindex(@ObjectName, ReportName) > 0


SELECT		'In DispoDefKopf'		ObjectType,
			Identifier				Identifier
FROM		DispoDefKopf
WHERE		charindex(@ObjectName, Identifier) > 0
			Or charindex(@ObjectName, FeldSELECT) > 0
			Or charindex(@ObjectName, FeldFROM) > 0
			Or charindex(@ObjectName, FeldORDERBY) > 0
			Or charindex(@ObjectName, FeldWHERE) > 0
			Or charindex(@ObjectName, WHEREQueryName) > 0
			Or charindex(@ObjectName, IDFeld) > 0
			Or charindex(@ObjectName, StringValueFeld) > 0
			Or charindex(@ObjectName, LongValueFeld) > 0
			Or charindex(@ObjectName, SQLMehrFachWahl) > 0
			Or charindex(@ObjectName, SQLMehrFachWahlResyncCommand) > 0
			Or charindex(@ObjectName, DispoUpdate) > 0
			Or charindex(@ObjectName, DoFilter_Identifier) > 0
			Or charindex(@ObjectName, DispoUpdateAfterWahl) > 0
			Or charindex(@ObjectName, StoredProcNameAfterWahl) > 0
			Or charindex(@ObjectName, ReportName) > 0


SELECT		'In SEQMSPED1'			ObjectType,
			Sprache					Sprache,
			Art						Art,
			Nummer					Nummer
FROM		SEQMSPED1
WHERE		charindex(@ObjectName, Art) > 0
			Or charindex(@ObjectName, Text) > 0
			Or charindex(@ObjectName, Bemerkungen) > 0
			Or charindex(@ObjectName, StatusText) > 0
			Or charindex(@ObjectName, TipText) > 0


SELECT		'In MSPED2'			ObjectType,
			Sprache				Sprache,
			Art					Art,
			Nummer				Nummer
FROM		MSPED2
WHERE		charindex(@ObjectName, Art) > 0
			Or charindex(@ObjectName, Text) > 0
			Or charindex(@ObjectName, Bemerkungen) > 0
			Or charindex(@ObjectName, StatusText) > 0
			Or charindex(@ObjectName, TipText) > 0



SELECT		'In Parambeschreibung'		ObjectType,
			ID							ID,
			Sprache						Sprache,
			KulcsElotag					KulcsElotag,
			Kulcs						Kulcs
FROM		ParamBeschreibung
WHERE		charindex(@ObjectName, KulcsElotag) > 0
			Or charindex(@ObjectName, Kulcs) > 0
			Or charindex(@ObjectName, Beschreibung) > 0


SELECT		'In ZMENUE0'				ObjectType,
			ID							ID,
			UserID						UserID,
			UserGruppe					UserGruppe,
			Sprache						Sprache,
			MenuNr						MenuNr,
			Spalte						Spalte,
			Reihe						Reihe,
			Titel						Titel
FROM		ZMENUE0
WHERE		charindex(@ObjectName, Titel) > 0
			Or charindex(@ObjectName, Startformular) > 0
			Or charindex(@ObjectName, Param1) > 0


SELECT		'In ZMENUE'					ObjectType,
			ID							ID,
			UserID						UserID,
			UserGruppe					UserGruppe,
			Sprache						Sprache,
			MenuNr						MenuNr,
			Spalte						Spalte,
			Reihe						Reihe,
			Titel						Titel
FROM		ZMENUE
WHERE		charindex(@ObjectName, Titel) > 0
			Or charindex(@ObjectName, Startformular) > 0
			Or charindex(@ObjectName, Param1) > 0

SELECT		'Table-valued functions-Name'			ObjectType,
			sysobjects.name							Function_Name
FROM		sysobjects
WHERE		isnull(charindex(@ObjectName, sysobjects.name),0)>0
			And xtype='TF'
GROUP BY	sysobjects.name

SELECT		'Table-valued function'				ObjectType,
			SPs.name							Function_Name
FROM		syscomments
			INNER JOIN  (
							SELECT	id,
									name
							FROM	sysobjects
							WHERE	xtype='TF'
						) SPs   ON syscomments.id=SPs.id
WHERE		charindex(@ObjectName, syscomments.text) > 0
GROUP BY SPs.name

SELECT		'Scalar-valued functions-Name'			ObjectType,
			sysobjects.name							Function_Name
FROM		sysobjects
WHERE		isnull(charindex(@ObjectName, sysobjects.name),0)>0
			And xtype='FN'
GROUP BY	sysobjects.name

SELECT		'Scalar-valued function'				ObjectType,
			SPs.name								Function_Name
FROM		syscomments
			INNER JOIN  (
							SELECT	id,
									name
							FROM	sysobjects
							WHERE	xtype='FN'
						) SPs   ON syscomments.id=SPs.id
WHERE		charindex(@ObjectName, syscomments.text) > 0
GROUP BY SPs.name

SELECT		'In RS_Fields0'				ObjectType,
			RS_Fields0.*
FROM		RS_Fields0
WHERE		charindex(@ObjectName, ServerObject_Prefix) > 0
			Or charindex(@ObjectName, SubPrefix) > 0
			Or charindex(@ObjectName, FieldName) > 0
			Or charindex(@ObjectName, LabelName) > 0
			Or charindex(@ObjectName, Parent) > 0
			Or charindex(@ObjectName, DT_FixText_Key) > 0
			Or charindex(@ObjectName, DT_WHERE2) > 0
			Or charindex(@ObjectName, DT_ID_Field) > 0
			Or charindex(@ObjectName, BG_Image) > 0
			Or charindex(@ObjectName, Label_Text) > 0
			Or charindex(@ObjectName, Forced_NextField) > 0
			Or charindex(@ObjectName, Tag) > 0

SELECT		'In RS_Fields'				ObjectType,
			RS_Fields.*
FROM		RS_Fields
WHERE		charindex(@ObjectName, ServerObject_Prefix) > 0
			Or charindex(@ObjectName, SubPrefix) > 0
			Or charindex(@ObjectName, FieldName) > 0
			Or charindex(@ObjectName, LabelName) > 0
			Or charindex(@ObjectName, Parent) > 0
			Or charindex(@ObjectName, DT_FixText_Key) > 0
			Or charindex(@ObjectName, DT_WHERE2) > 0
			Or charindex(@ObjectName, DT_ID_Field) > 0
			Or charindex(@ObjectName, BG_Image) > 0
			Or charindex(@ObjectName, Label_Text) > 0
			Or charindex(@ObjectName, Forced_NextField) > 0
			Or charindex(@ObjectName, Tag) > 0

SELECT		'In RS_Fields_DEBUG'		ObjectType,
			RS_Fields_DEBUG.*
FROM		RS_Fields_DEBUG
WHERE		charindex(@ObjectName, ServerObject_Prefix) > 0
			Or charindex(@ObjectName, SubPrefix) > 0
			Or charindex(@ObjectName, FieldName) > 0
			Or charindex(@ObjectName, LabelName) > 0
			Or charindex(@ObjectName, Parent) > 0
			Or charindex(@ObjectName, DT_FixText_Key) > 0
			Or charindex(@ObjectName, DT_WHERE2) > 0
			Or charindex(@ObjectName, DT_ID_Field) > 0
			Or charindex(@ObjectName, BG_Image) > 0
			Or charindex(@ObjectName, Label_Text) > 0
			Or charindex(@ObjectName, Forced_NextField) > 0
			Or charindex(@ObjectName, Tag) > 0


SELECT		'In RS_ARRANGE0'			ObjectType,
			RS_ARRANGE0.*
FROM		RS_ARRANGE0
WHERE		charindex(@ObjectName, ServerObject_Prefix) > 0
			Or charindex(@ObjectName, SubPrefix) > 0
			Or charindex(@ObjectName, ControlName) > 0
			Or charindex(@ObjectName, P1_Control) > 0
			Or charindex(@ObjectName, P2_Control) > 0

SELECT		'In RS_ARRANGE'				ObjectType,
			RS_ARRANGE.*
FROM		RS_ARRANGE
WHERE		charindex(@ObjectName, ServerObject_Prefix) > 0
			Or charindex(@ObjectName, SubPrefix) > 0
			Or charindex(@ObjectName, ControlName) > 0
			Or charindex(@ObjectName, P1_Control) > 0
			Or charindex(@ObjectName, P2_Control) > 0

SELECT		'In RS_ARRANGE_DEBUG'		ObjectType,
			RS_ARRANGE_DEBUG.*
FROM		RS_ARRANGE_DEBUG
WHERE		charindex(@ObjectName, ServerObject_Prefix) > 0
			Or charindex(@ObjectName, SubPrefix) > 0
			Or charindex(@ObjectName, ControlName) > 0
			Or charindex(@ObjectName, P1_Control) > 0
			Or charindex(@ObjectName, P2_Control) > 0


