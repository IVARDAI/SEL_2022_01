-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Meglevo adatsorok INSERT-je
-------------------------------------------------------------------------------------------------------------------------------------------------------

/*
SELECT		'INSERT INTO		ParmTbl_Texts	(MODULE, FORM_Code, GROUP_Code, CTRL_Code, SUB_Code, IDX, IS_PATTERN, LANG, VALUE) VALUES (''' + MODULE + ''', ''' + FORM_Code + ''', ''' + GROUP_Code + ''', ''' + CTRL_Code + ''', ''' + SUB_Code + ''', ' + CONVERT(NVARCHAR(20), IDX) + ', ' + CONVERT(NVARCHAR(5), IS_PATTERN) + ',   ''' + LANG + ''', ''' + REPLACE(VALUE, '''', '''''') + '''' + ')'
FROM		ParmTbl_Texts
ORDER BY	MODULE,
			FORM_Code,
			GROUP_Code,
			CTRL_Code,
			SUB_Code,
			IDX,
			LANG
*/

-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Konkret object scriptje
-------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT		*
FROM		ParmTbl_Texts
WHERE		MODULE			= ''
			And FORM_Code	= ''
ORDER BY	MODULE,
			FORM_Code,
			GROUP_Code,
			CTRL_Code,
			SUB_Code,
			IDX,
			LANG

SELECT		'INSERT INTO		ParmTbl_Texts	(MODULE, FORM_Code, GROUP_Code, CTRL_Code, SUB_Code, IDX, IS_PATTERN, LANG, VALUE) VALUES (''' + MODULE + ''', ''' + FORM_Code + ''', ''' + GROUP_Code + ''', ''' + CTRL_Code + ''', ''' + SUB_Code + ''', ' + CONVERT(NVARCHAR(5), IS_PATTERN) + ', ' + CONVERT(NVARCHAR(20), IDX) + ',   ''' + LANG + ''', ''' + REPLACE(VALUE, '''', '''''') + '''' + ')'
FROM		ParmTbl_Texts
WHERE		MODULE			= <''>
			And FORM_Code	= <''>
ORDER BY	MODULE,
			FORM_Code,
			GROUP_Code,
			CTRL_Code,
			SUB_Code,
			IDX,
			LANG


-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Uj dialogbox letrehozasa
-------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT		*
FROM		ParmTbl_Texts
WHERE		GROUP_Code = 'DIALOG'
			And CTRL_Code = ''
			And SUB_Code = ''
ORDER BY	MODULE,
			FORM_Code,
			GROUP_Code,
			CTRL_Code,
			SUB_Code,
			IDX,
			LANG

-- SELECT * FROM SEQMSPED1 WHERE Art like 'DIALOG%' ORDER BY Art DESC, Sprache, Nummer

DECLARE @MsgLanguage		NVARCHAR(3)
DECLARE @MODULE				NVARCHAR(50)
DECLARE @FORM_Code			NVARCHAR(50)
DECLARE @DialogNum			INT

DECLARE @Text1				NVARCHAR(128)
DECLARE @Text2				NVARCHAR(128)
DECLARE @Text3				NVARCHAR(128)
DECLARE @Text4				NVARCHAR(128)
DECLARE @MsgText			NVARCHAR(MAX)

SELECT	@MsgLanguage	= 'H',
		@MODULE			= '',
		@FORM_Code		= '',
		@DialogNum		= 0,
		@Text1			= '',
		@Text2			= '',
		@Text3			= '',
		@Text4			= ''

DELETE ParmTbl_Texts	WHERE	LANG = @MsgLanguage
								And MODULE = @MODULE
								And FORM_Code = @FORM_Code
								And IDX = @DialogNum
								And GROUP_Code = 'DIALOG'
								And CTRL_Code = ''
								And SUB_Code = ''

SELECT @MsgText = ISNULL(@Text1, '') + '|' + ISNULL(@Text2, '') + '|' + ISNULL(@Text3, '') + '|' + ISNULL(@Text4, '')

INSERT INTO	ParmTbl_Texts	(MODULE,  FORM_Code,  GROUP_Code, CTRL_Code, SUB_Code, IDX,        IS_PATTERN, LANG,         VALUE)
					VALUES	(@MODULE, @FORM_Code, 'DIALOG',   '',        '',       @DialogNum, 0,          @MsgLanguage, @MsgText)

SELECT		*
FROM		ParmTbl_Texts
WHERE		MODULE = @MODULE
			And FORM_Code = @FORM_Code
			And IDX = @DialogNum
			And GROUP_Code = 'DIALOG'
			And CTRL_Code = ''
			And SUB_Code = ''
ORDER BY	MODULE,
			FORM_Code,
			GROUP_Code,
			CTRL_Code,
			SUB_Code,
			IDX,
			LANG
