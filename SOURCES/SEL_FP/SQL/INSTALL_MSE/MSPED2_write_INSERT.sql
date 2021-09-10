-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Uj dialogbox letrehozasa
-------------------------------------------------------------------------------------------------------------------------------------------------------

-- SELECT * FROM msped2 WHERE Art like 'DIALOG%' ORDER BY Art DESC, Sprache, Nummer


-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Empty dialog numbers
-------------------------------------------------------------------------------------------------------------------------------------------------------

#END_BAT#

SET NOCOUNT ON

DECLARE @From_Dialog_Number	INT	= 900000
DECLARE @To_Dialog_Number	INT = 901000

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
											FROM		MSPED2
											WHERE		Art LIKE 'DIALOG%'
														AND ISNUMERIC(SUBSTRING(Art, 7, 50)) = 1
											GROUP BY	CONVERT(INT, SUBSTRING(Art, 7, 50))
										)	Existing_Dialogs	ON	#TMP_Holes.Empty_Dialog_Number = Existing_Dialogs.DialNum

SELECT		*
FROM		#TMP_Holes
ORDER BY	#TMP_Holes.Empty_Dialog_Number

#END_BAT#


-------------------------------------------------------------------------------------------------------------------------------------------------------
-- Add or update dialog to MSPED2
-------------------------------------------------------------------------------------------------------------------------------------------------------

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
declare @ID					int
declare @TransactID			int

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

DELETE msped2 WHERE Sprache = @MsgLanguage And Art = @Art

IF ltrim(rtrim(isnull(@Text1, ''))) > ''
   BEGIN
		exec @ID = NachsteNummerVergeben
		exec @TransactID = Tr_getNewTransactID 'SERVER', 'MMS', @ID
		INSERT INTO msped2		(ID,  TransactID,  Sprache,      Art,  Nummer, [Text], LetzteAenderung,  Bemerkungen,  StatusText,  TipText)
						VALUES	(@ID, @TransactID, @MsgLanguage, @Art, 1,      @Text1, @LetzteAenderung, @Bemerkungen, @StatusText, @TipText)
   END

IF ltrim(rtrim(isnull(@Text2, ''))) > ''
   BEGIN
		exec @ID = NachsteNummerVergeben
		exec @TransactID = Tr_getNewTransactID 'SERVER', 'MMS', @ID
		INSERT INTO msped2		(ID,  TransactID,  Sprache,      Art,  Nummer, [Text], LetzteAenderung,  Bemerkungen,  StatusText,  TipText)
						VALUES	(@ID, @TransactID, @MsgLanguage, @Art, 2,      @Text2, @LetzteAenderung, @Bemerkungen, @StatusText, @TipText)
   END


IF ltrim(rtrim(isnull(@Text3, ''))) > ''
   BEGIN
		exec @ID = NachsteNummerVergeben
		exec @TransactID = Tr_getNewTransactID 'SERVER', 'MMS', @ID
		INSERT INTO msped2		(ID,  TransactID,  Sprache,      Art,  Nummer, [Text], LetzteAenderung,  Bemerkungen,  StatusText,  TipText)
						VALUES	(@ID, @TransactID, @MsgLanguage, @Art, 3,      @Text3, @LetzteAenderung, @Bemerkungen, @StatusText, @TipText)
   END

IF ltrim(rtrim(isnull(@Text4, ''))) > ''
   BEGIN
		exec @ID = NachsteNummerVergeben
		exec @TransactID = Tr_getNewTransactID 'SERVER', 'MMS', @ID
		INSERT INTO msped2		(ID,  TransactID,  Sprache,      Art,  Nummer, [Text], LetzteAenderung,  Bemerkungen,  StatusText,  TipText)
						VALUES	(@ID, @TransactID, @MsgLanguage, @Art, 4,      @Text4, @LetzteAenderung, @Bemerkungen, @StatusText, @TipText)
   END

SELECT * FROM msped2 WHERE Art = @Art ORDER BY Sprache, Nummer
