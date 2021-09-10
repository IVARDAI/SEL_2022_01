-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Meglevo adatsor INSERT-je
-------------------------------------------------------------------------------------------------------------------------------------------------------

SELECT	Art, 'INSERT INTO SEQMSPED1 (Sprache, Art, Nummer, Text, LetzteAenderung, Bemerkungen, StatusText, TipText) VALUES (''' + isnull(Sprache,'') + ''', '''+Art+''', ' + CONVERT(nvarchar(20), Nummer) + ', '''+[Text]+''', '''+CONVERT(nvarchar(5),year(getdate()))+'-'+CONVERT(nvarchar(5),month(getdate()))+'-'+CONVERT(nvarchar(5),day(getdate()))+''', '''+ISNULL(Bemerkungen,'')+''', '''+isnull(StatusText,'')+''', '''+isnull(TipText,'')+''')'
FROM	MSPED2
WHERE	Art like '%'


-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Empty dialog numbers
-------------------------------------------------------------------------------------------------------------------------------------------------------

#END_BAT#

SET NOCOUNT ON

DECLARE @From_Dialog_Number	INT	= 1
DECLARE @To_Dialog_Number	INT = 1000

IF OBJECT_ID('tempdb..#TMP_Holes') IS NOT NULL
   BEGIN
		DROP TABLE #TMP_Holes
   END

CREATE TABLE #TMP_Holes	(
							Empty_Dialog_Number		INT
						)

WHILE @From_Dialog_Number < @To_Dialog_Number
  BEGIN
		INSERT INTO		#TMP_Holes	(Empty_Dialog_Number)
							VALUES	(@From_Dialog_Number)

		SELECT @From_Dialog_Number = @From_Dialog_Number + 1
  END

DELETE #TMP_Holes	FROM	#TMP_Holes
							INNER JOIN	(
											SELECT		CONVERT(INT, SUBSTRING(Art, 7, 50))		DialNum
											FROM		SEQMSPED1
											WHERE		Art LIKE 'DIALOG%'
														AND ISNUMERIC(SUBSTRING(Art, 7, 50)) = 1
											GROUP BY	CONVERT(INT, SUBSTRING(Art, 7, 50))
										)	Existing_Dialogs	ON	#TMP_Holes.Empty_Dialog_Number = Existing_Dialogs.DialNum

SELECT		*
FROM		#TMP_Holes
ORDER BY	#TMP_Holes.Empty_Dialog_Number

#END_BAT#

-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Add or update dialog to SEQMSPED1
-------------------------------------------------------------------------------------------------------------------------------------------------------

-- SELECT * FROM SEQMSPED1 WHERE Art like 'DIALOG%' ORDER BY Art DESC, Sprache, Nummer

declare @DialogNum			int
declare @MsgLanguage		nvarchar(3)
declare @Text1				nvarchar(128)
declare @Text2				nvarchar(128)
declare @Text3				nvarchar(128)
declare @Text4				nvarchar(128)
declare @LetzteAenderung	datetime
declare @Art				nvarchar(255)
declare @Bemerkungen		nvarchar(255)
declare @StatusText			nvarchar(255)
declare @TipText			nvarchar(255)

SELECT	@DialogNum		= 0,
		@MsgLanguage	= 'H',
		@Text1			= '',
		@Text2			= '',
		@Text3			= '',
		@Text4			= '',
		@Bemerkungen	= '',	-- Ide ird az eljaras nevet
		@StatusText		= '',
		@TipText		= ''

SELECT	@Art				= 'DIALOG' + right('000000' + convert(nvarchar(20), @DialogNum), 6),
		@LetzteAenderung	= convert(datetime, convert(nvarchar(20), getdate(), 111), 111)

DELETE SEQMSPED1 WHERE Sprache = @MsgLanguage And Art = @Art

IF ltrim(rtrim(isnull(@Text1, ''))) > ''
   BEGIN
		INSERT INTO SEQMSPED1	(Sprache,      Art,  Nummer, [Text], LetzteAenderung,  Bemerkungen,  StatusText,  TipText)
						VALUES	(@MsgLanguage, @Art, 1,      @Text1, @LetzteAenderung, @Bemerkungen, @StatusText, @TipText)
   END

IF ltrim(rtrim(isnull(@Text2, ''))) > ''
   BEGIN
		INSERT INTO SEQMSPED1	(Sprache,      Art,  Nummer, [Text], LetzteAenderung,  Bemerkungen,  StatusText,  TipText)
						VALUES	(@MsgLanguage, @Art, 2,      @Text2, @LetzteAenderung, @Bemerkungen, @StatusText, @TipText)
   END


IF ltrim(rtrim(isnull(@Text3, ''))) > ''
   BEGIN
		INSERT INTO SEQMSPED1	(Sprache,      Art,  Nummer, [Text], LetzteAenderung,  Bemerkungen,  StatusText,  TipText)
						VALUES	(@MsgLanguage, @Art, 3,      @Text3, @LetzteAenderung, @Bemerkungen, @StatusText, @TipText)
   END

IF ltrim(rtrim(isnull(@Text4, ''))) > ''
   BEGIN
		INSERT INTO SEQMSPED1	(Sprache,      Art,  Nummer, [Text], LetzteAenderung,  Bemerkungen,  StatusText,  TipText)
						VALUES	(@MsgLanguage, @Art, 4,      @Text4, @LetzteAenderung, @Bemerkungen, @StatusText, @TipText)
   END

SELECT * FROM SEQMSPED1 WHERE Art = @Art ORDER BY Sprache, Nummer

