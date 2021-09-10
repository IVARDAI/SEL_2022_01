-- Date: 2013-04-01
-- Ha mindent szeretnel esz nelkul script-elni, akkor
-- keress ra erre:
-- SCRIPT ALL USER SPECIFICS
--
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-- FixText
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------------------
-- Osszehasonlitas FixText - INSERT NEW
-------------------------------------------------------------------------------------------------------------------------------------------

/* Az INSERT ele tedd be:
DECLARE @TransactID		INT
DECLARE @ID				INT

EXEC @TransactID = Tr_getNewTransactID 'SERVER', 'MSE'

*/

DECLARE @VBCRLF			NVARCHAR(2)
SELECT @VBCRLF = char(13) + char(10)

SELECT		'EXEC @ID = NachsteNummerVergeben' + @VBCRLF + 'INSERT INTO FixText (ID, TransactID, Code, Titel, [Text]) VALUES (@ID, @TransactID, ''' + Etalon.Code + ''', ''' + REPLACE(ISNULL(Etalon.Titel, ''), '''', '''''') + ''',' + @VBCRLF + 'N''' + REPLACE(CONVERT(VARCHAR(8000), Etalon.[Text]), '''', '''''') + '''' + @VBCRLF + ')' + @VBCRLF
FROM		<Etalon>..Fixtext	Etalon
			LEFT JOIN			FixText		ON	Etalon.Code COLLATE DATABASE_DEFAULT = FixText.Code COLLATE DATABASE_DEFAULT
WHERE		FixText.ID Is Null


-------------------------------------------------------------------------------------------------------------------------------------------
-- Osszehasonlitas FixText - UPDATE
-------------------------------------------------------------------------------------------------------------------------------------------

/* Az UPDATE-k ele tedd be:
DECLARE @TransactID		INT

EXEC @TransactID = Tr_getNewTransactID 'SERVER', 'MSE'

*/

DECLARE @VBCRLF			NVARCHAR(2)
SELECT @VBCRLF = char(13) + char(10)


SELECT		'UPDATE FixText SET TransactID = @TransactID,' + @VBCRLF + '                   Titel      = ''' + ISNULL(Etalon.Titel, '') + ''',' + @VBCRLF + '                   [Text]     = ' + 'N''' + REPLACE(CONVERT(VARCHAR(8000), Etalon.[Text]), '''', '''''') + '''' + @VBCRLF + 'WHERE Code = ''' + Etalon.Code + '''' + @VBCRLF + @VBCRLF
FROM		<Etalon>..Fixtext	Etalon
			INNER JOIN			FixText		ON	Etalon.Code COLLATE DATABASE_DEFAULT = FixText.Code COLLATE DATABASE_DEFAULT
WHERE		CONVERT(NVARCHAR(MAX), FixText.[Text]) COLLATE DATABASE_DEFAULT <> CONVERT(NVARCHAR(MAX), Etalon.[Text]) COLLATE DATABASE_DEFAULT
			And Not (Etalon.Code like 'PFD_%')


-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-- MSPED2
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------------------
-- Osszehasonlitas MSPED2 - INSERT NEW
-------------------------------------------------------------------------------------------------------------------------------------------

/* Az INSERT ele tedd be:
DECLARE @TransactID		INT
DECLARE @ID				INT

EXEC @TransactID = Tr_getNewTransactID 'SERVER', 'MSE'
*/
SELECT		'EXEC @ID = NachsteNummerVergeben  INSERT INTO MSPED2 (ID, TransactID, Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (@ID, @TransactID, ''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..MSPED2	Etalon
			LEFT JOIN			MSPED2		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT = MSPED2.Sprache COLLATE DATABASE_DEFAULT
												And Etalon.Art COLLATE DATABASE_DEFAULT = MSPED2.Art COLLATE DATABASE_DEFAULT
												And Etalon.Nummer = MSPED2.Nummer
WHERE		MSPED2.ID Is Null
			And Not (Etalon.Art like 'HELP_%')
			And Not (Etalon.Art like 'VBSEQ_%')
			And Etalon.Art <> 'TARIFWERT'

UNION ALL

SELECT		'EXEC @ID = NachsteNummerVergeben  INSERT INTO MSPED2 (ID, TransactID, Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (@ID, @TransactID, ''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..MSPED2	Etalon
			LEFT JOIN			MSPED2		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT = MSPED2.Sprache COLLATE DATABASE_DEFAULT
												And Etalon.Art COLLATE DATABASE_DEFAULT = MSPED2.Art COLLATE DATABASE_DEFAULT
												And Etalon.Nummer = MSPED2.Nummer
												And Etalon.[Text] COLLATE DATABASE_DEFAULT = MSPED2.[Text] COLLATE DATABASE_DEFAULT
WHERE		MSPED2.ID Is Null
			And Etalon.Art like 'HELP_%'

UNION ALL

SELECT		'EXEC @ID = NachsteNummerVergeben  INSERT INTO MSPED2 (ID, TransactID, Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (@ID, @TransactID, ''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..MSPED2	Etalon
			LEFT JOIN			MSPED2		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT = MSPED2.Sprache COLLATE DATABASE_DEFAULT
												And Etalon.Art COLLATE DATABASE_DEFAULT = MSPED2.Art COLLATE DATABASE_DEFAULT
												And Etalon.Nummer = MSPED2.Nummer
												And Etalon.[Text] COLLATE DATABASE_DEFAULT = MSPED2.[Text] COLLATE DATABASE_DEFAULT
												And Etalon.Bemerkungen COLLATE DATABASE_DEFAULT = MSPED2.Bemerkungen COLLATE DATABASE_DEFAULT
WHERE		MSPED2.ID Is Null
			And Etalon.Art like 'VBSEQ_%'

UNION ALL

SELECT		'EXEC @ID = NachsteNummerVergeben  INSERT INTO MSPED2 (ID, TransactID, Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (@ID, @TransactID, ''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..MSPED2	Etalon
			LEFT JOIN			MSPED2		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT = MSPED2.Sprache COLLATE DATABASE_DEFAULT
												And Etalon.Art COLLATE DATABASE_DEFAULT = MSPED2.Art COLLATE DATABASE_DEFAULT
												And Etalon.Nummer = MSPED2.Nummer
												And Etalon.[Text] COLLATE DATABASE_DEFAULT = MSPED2.[Text] COLLATE DATABASE_DEFAULT
WHERE		MSPED2.ID Is Null
			And Etalon.Art = 'TARIFWERT'



-------------------------------------------------------------------------------------------------------------------------------------------
-- Osszehasonlitas MSPED2 - UPDATE
-------------------------------------------------------------------------------------------------------------------------------------------

/* Az UPDATE ele tedd be
DECLARE @TransactID		INT

EXEC @TransactID = Tr_getNewTransactID 'SERVER', 'MSE'

*/

SELECT		'UPDATE MSPED2 SET TransactID = @TransactID, [Text] = ''' + replace(Etalon.[Text], '''', '''''') + ''', Bemerkungen = ''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', StatusText = ''' + replace(Etalon.StatusText, '''', '''''') + ''', TipText = ''' + replace(Etalon.TipText, '''', '''''') + ''' WHERE MSPED2.Sprache = ''' + Etalon.Sprache + ''' And MSPED2.Art = ''' + Etalon.Art + ''' And MSPED2.Nummer = ' + CONVERT(NVARCHAR(20), Etalon.Nummer)
FROM		<Etalon>..MSPED2	Etalon
			INNER JOIN			MSPED2		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= MSPED2.Sprache COLLATE DATABASE_DEFAULT
												And Etalon.Art COLLATE DATABASE_DEFAULT		= MSPED2.Art COLLATE DATABASE_DEFAULT
												And Etalon.Nummer	= MSPED2.Nummer
WHERE		Not (Etalon.Art like 'HELP_%')
			And Not (Etalon.Art like 'VBSEQ_%')
			And Etalon.Art <> 'TARIFWERT'
			And	(
					MSPED2.Bemerkungen COLLATE DATABASE_DEFAULT	<> Etalon.Bemerkungen COLLATE DATABASE_DEFAULT
					Or MSPED2.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT
					Or MSPED2.Tiptext COLLATE DATABASE_DEFAULT		<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)

UNION ALL

SELECT		'UPDATE MSPED2 SET TransactID = @TransactID, Bemerkungen = ''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', StatusText = ''' + replace(Etalon.StatusText, '''', '''''') + ''', TipText = ''' + replace(Etalon.TipText, '''', '''''') + ''' WHERE MSPED2.Sprache = ''' + Etalon.Sprache + ''' And MSPED2.Art = ''' + Etalon.Art + ''' And MSPED2.Nummer = ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ' And [Text] = ''' + Etalon.[Text] + ''''
FROM		<Etalon>..MSPED2	Etalon
			INNER JOIN			MSPED2		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= MSPED2.Sprache COLLATE DATABASE_DEFAULT
												And Etalon.Art COLLATE DATABASE_DEFAULT		= MSPED2.Art COLLATE DATABASE_DEFAULT
												And Etalon.Nummer	= MSPED2.Nummer
												And Etalon.[Text] COLLATE DATABASE_DEFAULT	= MSPED2.[Text] COLLATE DATABASE_DEFAULT
WHERE		Etalon.Art like 'HELP_%'
			And	(
					MSPED2.Bemerkungen COLLATE DATABASE_DEFAULT	<> Etalon.Bemerkungen COLLATE DATABASE_DEFAULT
					Or MSPED2.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT
					Or MSPED2.Tiptext COLLATE DATABASE_DEFAULT		<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)

UNION ALL

SELECT		'UPDATE MSPED2 SET TransactID = @TransactID, Bemerkungen = ''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', StatusText = ''' + replace(Etalon.StatusText, '''', '''''') + ''', TipText = ''' + replace(Etalon.TipText, '''', '''''') + ''' WHERE MSPED2.Sprache = ''' + Etalon.Sprache + ''' And MSPED2.Art = ''' + Etalon.Art + ''' And MSPED2.Nummer = ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ' And [Text] = ''' + Etalon.[Text] + ''''
FROM		<Etalon>..MSPED2	Etalon
			INNER JOIN			MSPED2		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT			= MSPED2.Sprache COLLATE DATABASE_DEFAULT
												And Etalon.Art COLLATE DATABASE_DEFAULT			= MSPED2.Art COLLATE DATABASE_DEFAULT
												And Etalon.Nummer		= MSPED2.Nummer
												And Etalon.Bemerkungen COLLATE DATABASE_DEFAULT	= MSPED2.Bemerkungen COLLATE DATABASE_DEFAULT
												And Etalon.[Text] COLLATE DATABASE_DEFAULT		= MSPED2.[Text] COLLATE DATABASE_DEFAULT
WHERE		Etalon.Art like 'VBSEQ_%'
			And	(
					MSPED2.Bemerkungen COLLATE DATABASE_DEFAULT	<> Etalon.Bemerkungen COLLATE DATABASE_DEFAULT
					Or MSPED2.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT
					Or MSPED2.Tiptext COLLATE DATABASE_DEFAULT		<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)

UNION ALL

SELECT		'UPDATE MSPED2 SET TransactID = @TransactID, Bemerkungen = ''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', StatusText = ''' + replace(Etalon.StatusText, '''', '''''') + ''', TipText = ''' + replace(Etalon.TipText, '''', '''''') + ''' WHERE MSPED2.Sprache = ''' + Etalon.Sprache + ''' And MSPED2.Art = ''' + Etalon.Art + ''' And MSPED2.Nummer = ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ' And [Text] = ''' + Etalon.[Text] + ''''
FROM		<Etalon>..MSPED2	Etalon
			INNER JOIN			MSPED2		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= MSPED2.Sprache COLLATE DATABASE_DEFAULT
												And Etalon.Art COLLATE DATABASE_DEFAULT		= MSPED2.Art COLLATE DATABASE_DEFAULT
												And Etalon.Nummer	= MSPED2.Nummer
												And Etalon.[Text] COLLATE DATABASE_DEFAULT	= MSPED2.[Text] COLLATE DATABASE_DEFAULT
WHERE		Etalon.Art = 'TARIFWERT'
			And	(
					MSPED2.Bemerkungen COLLATE DATABASE_DEFAULT	<> Etalon.Bemerkungen COLLATE DATABASE_DEFAULT
					Or MSPED2.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT
					Or MSPED2.Tiptext COLLATE DATABASE_DEFAULT		<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)


-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-- SEQMSPED1
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------------------
-- Osszehasonlitas SEQMSPED1 - INSERT NEW
-------------------------------------------------------------------------------------------------------------------------------------------

SELECT		'INSERT INTO SEQMSPED1 (Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..SEQMSPED1	Etalon
			LEFT JOIN			SEQMSPED1		ON	Etalon.Sprache  COLLATE DATABASE_DEFAULT = SEQMSPED1.Sprache  COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT = SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer = SEQMSPED1.Nummer
WHERE		SEQMSPED1.ID Is Null
			And Not (Etalon.Art like 'HELP_%')
			And Not (Etalon.Art like 'VBSEQ_%')
			And Etalon.Art <> 'TARIFWERT'

UNION ALL

SELECT		'INSERT INTO SEQMSPED1 (Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..SEQMSPED1	Etalon
			LEFT JOIN			SEQMSPED1		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT = SEQMSPED1.Sprache COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT = SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer = SEQMSPED1.Nummer
													And Etalon.[Text] COLLATE DATABASE_DEFAULT = SEQMSPED1.[Text] COLLATE DATABASE_DEFAULT
WHERE		SEQMSPED1.ID Is Null
			And Etalon.Art like 'HELP_%'

UNION ALL

SELECT		'INSERT INTO SEQMSPED1 (Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..SEQMSPED1	Etalon
			LEFT JOIN			SEQMSPED1		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= SEQMSPED1.Sprache COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT		= SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer	= SEQMSPED1.Nummer
													And Etalon.[Text] COLLATE DATABASE_DEFAULT	= SEQMSPED1.[Text] COLLATE DATABASE_DEFAULT
													And Etalon.Bemerkungen COLLATE DATABASE_DEFAULT	= SEQMSPED1.Bemerkungen COLLATE DATABASE_DEFAULT
													And Etalon.Statustext COLLATE DATABASE_DEFAULT	= SEQMSPED1.StatusText COLLATE DATABASE_DEFAULT
													And Etalon.Tiptext COLLATE DATABASE_DEFAULT		= SEQMSPED1.Tiptext COLLATE DATABASE_DEFAULT
WHERE		Etalon.Art like 'VBSEQ_%'
			AND SEQMSPED1.Art IS NULL

UNION ALL

SELECT		'INSERT INTO SEQMSPED1 (Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..SEQMSPED1	Etalon
			INNER JOIN			SEQMSPED1		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= SEQMSPED1.Sprache COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT		= SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer	= SEQMSPED1.Nummer
													And Etalon.[Text] COLLATE DATABASE_DEFAULT	= SEQMSPED1.[Text] COLLATE DATABASE_DEFAULT
													And Etalon.Bemerkungen COLLATE DATABASE_DEFAULT	= SEQMSPED1.Bemerkungen COLLATE DATABASE_DEFAULT
WHERE		Etalon.Art like 'VBSEQ_%'
			And	(
					SEQMSPED1.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT	Or
					SEQMSPED1.Tiptext COLLATE DATABASE_DEFAULT		<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)

UNION ALL

SELECT		'INSERT INTO SEQMSPED1 (Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (''' + Etalon.Sprache + ''', ''' + Etalon.Art + ''', ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ', N''' + replace(Etalon.[Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), Etalon.LetzteAenderung, 121) + ''', 121), N''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', N''' + replace(Etalon.StatusText, '''', '''''') + ''', N''' + replace(Etalon.TipText, '''', '''''') + ''')'
FROM		<Etalon>..SEQMSPED1	Etalon
			LEFT JOIN			SEQMSPED1		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT = SEQMSPED1.Sprache COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT = SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer = SEQMSPED1.Nummer
													And Etalon.[Text] COLLATE DATABASE_DEFAULT = SEQMSPED1.[Text] COLLATE DATABASE_DEFAULT
WHERE		SEQMSPED1.ID Is Null
			And Etalon.Art = 'TARIFWERT'


-------------------------------------------------------------------------------------------------------------------------------------------
-- Osszehasonlitas SEQMSPED1 - UPDATE
-------------------------------------------------------------------------------------------------------------------------------------------

SELECT		'UPDATE SEQMSPED1 SET [Text] = ''' + replace(Etalon.[Text], '''', '''''') + ''', Bemerkungen = ''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', StatusText = ''' + replace(Etalon.StatusText, '''', '''''') + ''', TipText = ''' + replace(Etalon.TipText, '''', '''''') + ''' WHERE SEQMSPED1.Sprache = ''' + Etalon.Sprache + ''' And SEQMSPED1.Art = ''' + Etalon.Art + ''' And SEQMSPED1.Nummer = ' + CONVERT(NVARCHAR(20), Etalon.Nummer)
FROM		<Etalon>..SEQMSPED1	Etalon
			INNER JOIN			SEQMSPED1		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= SEQMSPED1.Sprache COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT		= SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer	= SEQMSPED1.Nummer
WHERE		Not (Etalon.Art like 'HELP_%')
			And Not (Etalon.Art like 'VBSEQ_%')
			And Etalon.Art <> 'TARIFWERT'
			And	(
					SEQMSPED1.Bemerkungen COLLATE DATABASE_DEFAULT	<> Etalon.Bemerkungen COLLATE DATABASE_DEFAULT
					Or SEQMSPED1.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT
					Or SEQMSPED1.Tiptext COLLATE DATABASE_DEFAULT		<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)

UNION ALL

SELECT		'UPDATE SEQMSPED1 SET Bemerkungen = ''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', StatusText = ''' + replace(Etalon.StatusText, '''', '''''') + ''', TipText = ''' + replace(Etalon.TipText, '''', '''''') + ''' WHERE SEQMSPED1.Sprache = ''' + Etalon.Sprache + ''' And SEQMSPED1.Art = ''' + Etalon.Art + ''' And SEQMSPED1.Nummer = ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ' And [Text] = ''' + Etalon.[Text] + ''''
FROM		<Etalon>..SEQMSPED1	Etalon
			INNER JOIN			SEQMSPED1		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= SEQMSPED1.Sprache COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT		= SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer	= SEQMSPED1.Nummer
													And Etalon.[Text] COLLATE DATABASE_DEFAULT	= SEQMSPED1.[Text] COLLATE DATABASE_DEFAULT
WHERE		Etalon.Art like 'HELP_%'
			And	(
					SEQMSPED1.Bemerkungen COLLATE DATABASE_DEFAULT	<> Etalon.Bemerkungen COLLATE DATABASE_DEFAULT
					Or SEQMSPED1.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT
					Or SEQMSPED1.Tiptext COLLATE DATABASE_DEFAULT		<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)

UNION ALL

SELECT		'UPDATE SEQMSPED1 SET Bemerkungen = ''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', StatusText = ''' + replace(Etalon.StatusText, '''', '''''') + ''', TipText = ''' + replace(Etalon.TipText, '''', '''''') + ''' WHERE SEQMSPED1.Sprache = ''' + Etalon.Sprache + ''' And SEQMSPED1.Art = ''' + Etalon.Art + ''' And SEQMSPED1.Nummer = ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ' And [Text] = ''' + Etalon.[Text] + ''''
FROM		<Etalon>..SEQMSPED1	Etalon
			INNER JOIN			SEQMSPED1		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= SEQMSPED1.Sprache COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT		= SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer	= SEQMSPED1.Nummer
													And Etalon.[Text] COLLATE DATABASE_DEFAULT	= SEQMSPED1.[Text] COLLATE DATABASE_DEFAULT
													And Etalon.Bemerkungen COLLATE DATABASE_DEFAULT	= SEQMSPED1.Bemerkungen COLLATE DATABASE_DEFAULT
WHERE		Etalon.Art like 'VBSEQ_%'
			And	(
					SEQMSPED1.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT Or
					SEQMSPED1.Tiptext COLLATE DATABASE_DEFAULT		<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)

UNION ALL

SELECT		'UPDATE SEQMSPED1 SET Bemerkungen = ''' + replace(Etalon.Bemerkungen, '''', '''''') + ''', StatusText = ''' + replace(Etalon.StatusText, '''', '''''') + ''', TipText = ''' + replace(Etalon.TipText, '''', '''''') + ''' WHERE SEQMSPED1.Sprache = ''' + Etalon.Sprache + ''' And SEQMSPED1.Art = ''' + Etalon.Art + ''' And SEQMSPED1.Nummer = ' + CONVERT(NVARCHAR(20), Etalon.Nummer) + ' And [Text] = ''' + Etalon.[Text] + ''''
FROM		<Etalon>..SEQMSPED1	Etalon
			INNER JOIN			SEQMSPED1		ON	Etalon.Sprache COLLATE DATABASE_DEFAULT		= SEQMSPED1.Sprache COLLATE DATABASE_DEFAULT
													And Etalon.Art COLLATE DATABASE_DEFAULT		= SEQMSPED1.Art COLLATE DATABASE_DEFAULT
													And Etalon.Nummer	= SEQMSPED1.Nummer
													And Etalon.[Text] COLLATE DATABASE_DEFAULT	= SEQMSPED1.[Text] COLLATE DATABASE_DEFAULT
WHERE		Etalon.Art = 'TARIFWERT'
			And	(
					SEQMSPED1.Bemerkungen COLLATE DATABASE_DEFAULT	<> Etalon.Bemerkungen COLLATE DATABASE_DEFAULT
					Or SEQMSPED1.Statustext COLLATE DATABASE_DEFAULT	<> Etalon.StatusText COLLATE DATABASE_DEFAULT
					Or SEQMSPED1.Tiptext COLLATE DATABASE_DEFAULT	<> Etalon.Tiptext COLLATE DATABASE_DEFAULT
				)


-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-- RS_Fields_DEBUG, RS_ARRANGE_DEBUG
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

-----------------------------------------------------------------------------------------------------------------
-- RS_ARRANGE_DEBUG teteles osszehasonlitas...
-----------------------------------------------------------------------------------------------------------------

SELECT	'NOT EXISTS',
		Etalon.ServerObject_Prefix,
		Etalon.SubPrefix,
		Etalon.SeqNum,
		Etalon.ControlName,
		Etalon.ArrangeType
FROM	<Etalon>..RS_Arrange_DEBUG		Etalon
		LEFT JOIN		RS_Arrange_DEBUG		ON	Etalon.ServerObject_Prefix COLLATE DATABASE_DEFAULT = RS_Arrange_DEBUG.ServerObject_Prefix COLLATE DATABASE_DEFAULT
													And Etalon.SubPrefix COLLATE DATABASE_DEFAULT = RS_Arrange_DEBUG.SubPrefix COLLATE DATABASE_DEFAULT
													And Etalon.SeqNum = RS_Arrange_DEBUG.SeqNum
WHERE	RS_Arrange_DEBUG.ServerObject_Prefix Is Null


SELECT	'DIFFERENT',
		Etalon.ServerObject_Prefix,
		Etalon.SubPrefix,
		Etalon.SeqNum,
		Etalon.ControlName,
		Etalon.ArrangeType
FROM	<Etalon>..RS_Arrange_DEBUG		Etalon
		INNER JOIN		RS_Arrange_DEBUG		ON	Etalon.ServerObject_Prefix COLLATE DATABASE_DEFAULT = RS_Arrange_DEBUG.ServerObject_Prefix COLLATE DATABASE_DEFAULT
													And Etalon.SubPrefix COLLATE DATABASE_DEFAULT = RS_Arrange_DEBUG.SubPrefix COLLATE DATABASE_DEFAULT
													And Etalon.SeqNum = RS_Arrange_DEBUG.SeqNum
WHERE	(
			Etalon.ControlName COLLATE DATABASE_DEFAULT <> RS_Arrange_DEBUG.ControlName COLLATE DATABASE_DEFAULT
			Or Etalon.ArrangeType COLLATE DATABASE_DEFAULT <> RS_Arrange_DEBUG.ArrangeType COLLATE DATABASE_DEFAULT
			Or Etalon.P1_Control COLLATE DATABASE_DEFAULT <> RS_Arrange_DEBUG.P1_Control COLLATE DATABASE_DEFAULT
			Or Etalon.P1_Koo <> RS_Arrange_DEBUG.P1_Koo
			Or Etalon.P1_SpaceInPixel <> RS_Arrange_DEBUG.P1_SpaceInPixel
			Or Etalon.P2_Control COLLATE DATABASE_DEFAULT <> RS_Arrange_DEBUG.P2_Control COLLATE DATABASE_DEFAULT
			Or Etalon.P2_Koo <> RS_Arrange_DEBUG.P2_Koo
			Or Etalon.P2_SpaceInPixel <> RS_Arrange_DEBUG.P2_SpaceInPixel
			Or Etalon.Percentage <> RS_Arrange_DEBUG.Percentage
		)


-----------------------------------------------------------------------------------------------------------------
-- RS_FIELDS_DEBUG teteles osszehasonlitas...
-----------------------------------------------------------------------------------------------------------------

SELECT	'NOT EXISTS',
		Etalon.ServerObject_Prefix,
		Etalon.SubPrefix,
		Etalon.SeqNum,
		Etalon.FieldName
FROM	<Etalon>..RS_Fields_DEBUG		Etalon
		LEFT JOIN		RS_Fields_DEBUG		ON	Etalon.ServerObject_Prefix COLLATE DATABASE_DEFAULT = RS_Fields_DEBUG.ServerObject_Prefix COLLATE DATABASE_DEFAULT
											And Etalon.SubPrefix COLLATE DATABASE_DEFAULT = RS_Fields_DEBUG.SubPrefix COLLATE DATABASE_DEFAULT
											And Etalon.SeqNum = RS_Fields_DEBUG.SeqNum
WHERE	RS_Fields_DEBUG.ServerObject_Prefix Is Null


SELECT	'DIFFERENT',
		Etalon.ServerObject_Prefix,
		Etalon.SubPrefix,
		Etalon.SeqNum,
		Etalon.FieldName
FROM	<Etalon>..RS_Fields_DEBUG		Etalon
		INNER JOIN		RS_Fields_DEBUG		ON	Etalon.ServerObject_Prefix COLLATE DATABASE_DEFAULT = RS_Fields_DEBUG.ServerObject_Prefix COLLATE DATABASE_DEFAULT
											And Etalon.SubPrefix  COLLATE DATABASE_DEFAULT= RS_Fields_DEBUG.SubPrefix COLLATE DATABASE_DEFAULT
											And Etalon.SeqNum = RS_Fields_DEBUG.SeqNum
WHERE	(
			Etalon.FieldName COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.FieldName COLLATE DATABASE_DEFAULT
			Or Etalon.LabelName COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.LabelName COLLATE DATABASE_DEFAULT
			Or Etalon.Parent COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.Parent COLLATE DATABASE_DEFAULT
			Or Etalon.CreateAtRuntime <> RS_Fields_DEBUG.CreateAtRuntime
			Or Etalon.CreateAtRuntime_FieldType COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.CreateAtRuntime_FieldType COLLATE DATABASE_DEFAULT
			Or Etalon.Mandatory <> RS_Fields_DEBUG.Mandatory
			Or Etalon.Locked <> RS_Fields_DEBUG.Locked
			Or Etalon.Visible <> RS_Fields_DEBUG.Visible
			Or Etalon.xType_VB COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.xType_VB COLLATE DATABASE_DEFAULT
			Or Etalon.F_Format COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.F_Format COLLATE DATABASE_DEFAULT
			Or Etalon.DT_FixText_Key COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.DT_FixText_Key COLLATE DATABASE_DEFAULT
			Or Etalon.DT_WHERE2 COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.DT_WHERE2 COLLATE DATABASE_DEFAULT
			Or Etalon.DT_ID_Field COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.DT_ID_Field COLLATE DATABASE_DEFAULT
			Or Etalon.COLORS COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.COLORS COLLATE DATABASE_DEFAULT
			Or Etalon.BG_Image COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.BG_Image COLLATE DATABASE_DEFAULT
			Or Etalon.BG_Toggle <> RS_Fields_DEBUG.BG_Toggle
			Or Etalon.Label_Text COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.Label_Text COLLATE DATABASE_DEFAULT
			Or Etalon.ShowInGRID <> RS_Fields_DEBUG.ShowInGRID
			Or Etalon.SavePoint <> RS_Fields_DEBUG.SavePoint
			Or Etalon.Forced_NextField COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.Forced_NextField COLLATE DATABASE_DEFAULT
			Or Etalon.Tag COLLATE DATABASE_DEFAULT <> RS_Fields_DEBUG.Tag COLLATE DATABASE_DEFAULT
			Or Etalon.TabIndex <> RS_Fields_DEBUG.TabIndex
			Or Etalon.TabStop <> RS_Fields_DEBUG.TabStop
		)


-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-- 1 objektum script-je az RS tablakbol
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------


DECLARE @ServerObject_Prefix	NVARCHAR(50)
DECLARE @SubPrefix				NVARCHAR(50)

SELECT	@ServerObject_Prefix	= <''>,
		@SubPrefix				= <''>

SELECT 'DELETE  RS_Fields_DEBUG WHERE ServerObject_Prefix = ''' + @ServerObject_Prefix + ''' And SubPrefix = ''' + @SubPrefix + ''''
UNION ALL
SELECT 'DELETE  RS_ARRANGE_DEBUG WHERE ServerObject_Prefix = ''' + @ServerObject_Prefix + ''' And SubPrefix = ''' + @SubPrefix + ''''

UNION ALL -- RS_Fields

SELECT 'INSERT INTO RS_FIELDS_DEBUG	(ServerObject_Prefix, SubPrefix, SeqNum, FieldName, LabelName, Parent, CreateAtRuntime, CreateAtRuntime_FieldType, Mandatory, Locked, Visible, xType_VB, F_Format, DT_FixText_Key, DT_WHERE2, DT_ID_Field, COLORS, BG_Image, BG_Toggle, Label_Text, ShowInGRID, SavePoint, Forced_NextField, Tag, TabIndex, TabStop) '
+ 'VALUES (''' + ServerObject_Prefix + ''', ''' + SubPrefix + ''', ' + convert(nvarchar(20), SeqNum)+ ', ''' + FieldName + ''', ''' + LabelName + ''', ''' + Parent + ''', ' + convert(nvarchar(20), CreateAtRuntime)+ ', ''' + CreateAtRuntime_FieldType + ''', ' + convert(nvarchar(20), Mandatory)+ ', ' + convert(nvarchar(20), Locked)+ ', ' + convert(nvarchar(20), Visible)+ ', ''' + xType_VB + ''', ''' + replace(F_Format, '''', '''''') + ''', ''' + DT_FixText_Key + ''', ''' + replace(DT_WHERE2, '''', '''''') + ''',''' + DT_ID_Field + ''', ''' + COLORS + ''', ''' + BG_Image + ''', ' + convert(nvarchar(20), BG_Toggle)+ ', ''' + replace(Label_Text, '''', '''''') + ''', ' + convert(nvarchar(20), ShowInGRID)+ ', ' + convert(nvarchar(20), SavePoint)+ ', ''' + Forced_NextField+ ''', ''' + replace(Tag, '''', '''''') + ''', ' + convert(nvarchar(20), TabIndex)+ ', ' + convert(nvarchar(20), TabStop)+ ')'	COMMAND
FROM		<Etalon>..RS_FIELDS_DEBUG
WHERE	ServerObject_Prefix	= @ServerObject_Prefix
		And SubPrefix		= @SubPrefix

UNION ALL -- RS_ARRANGE

SELECT 'INSERT INTO RS_ARRANGE_DEBUG	(ServerObject_Prefix, SubPrefix, SeqNum, ControlName, ArrangeType, P1_Control, P1_Koo, P1_SpaceInPixel, P2_Control, P2_Koo, P2_SpaceInPixel, Percentage)'
+ 'VALUES (''' + ServerObject_Prefix + ''', ''' + SubPrefix + ''', ' + convert(nvarchar(20), SeqNum)+ ', ''' + ControlName + ''', ''' + ArrangeType + ''', ''' + P1_Control + ''', ' + convert(nvarchar(20), P1_Koo)+ ', ' + convert(nvarchar(20), P1_SpaceInPixel)+ ', ''' + P2_Control + ''', ' + convert(nvarchar(20), P2_Koo)+ ', ' + convert(nvarchar(20), P2_SpaceInPixel)+ ', ' + convert(nvarchar(20), Percentage) + ')'	COMMAND
FROM		<Etalon>..RS_ARRANGE_DEBUG
WHERE	ServerObject_Prefix	= @ServerObject_Prefix
		And SubPrefix		= @SubPrefix


-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-- DOFILTER komplett scriptje
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

EXEC Services_Script_DOFILTER	<'Identifier'>


-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-- SCRIPT ALL USER SPECIFICS
--
-- F I G Y E L E M ! ! !
--
-- Ahhoz, hogy a FixText script-je jol mukodjon,
-- a Management studio-ban allitsd be a kovetkezoket:
-- 1. Results to text (jobb egérgomb a query ablakra majd Results to --> Results to text)
-- 2. A visszatero text mezok max hosszat 8192-re! (Tools --> Options --> Query Results --> Results to Text --> Maximum number of characters displayed in each column
-- 3. A biztonsag kedveert futtasd le a kovetkezo view-t:
SELECT 'TUL HOSSZU SOR !!!', Code, LEN(CONVERT(NVARCHAR(MAX), [Text])) Length_of_Text FROM FixText WHERE LEN(CONVERT(NVARCHAR(MAX), [Text])) > 8000 ORDER BY Code

-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------

--exec dbo.DEBUG_MODE_INIT

-- RS_FIELDS_DEBUG, RS_ARRANGE_DEBUG ----------------------------------------------------------------------------

SELECT 'DELETE  RS_Fields_DEBUG'
UNION ALL
SELECT 'DELETE  RS_ARRANGE_DEBUG'

UNION ALL -- RS_Fields_DEBUG

SELECT 'INSERT INTO RS_FIELDS_DEBUG	(ServerObject_Prefix, SubPrefix, SeqNum, FieldName, LabelName, Parent, CreateAtRuntime, CreateAtRuntime_FieldType, Mandatory, Locked, Visible, xType_VB, F_Format, DT_FixText_Key, DT_WHERE2, DT_ID_Field, COLORS, BG_Image, BG_Toggle, Label_Text, ShowInGRID, SavePoint, Forced_NextField, Tag, TabIndex, TabStop) '
+ 'VALUES (''' + ServerObject_Prefix + ''', ''' + SubPrefix + ''', ' + convert(nvarchar(20), SeqNum)+ ', ''' + FieldName + ''', ''' + LabelName + ''', ''' + Parent + ''', ' + convert(nvarchar(20), CreateAtRuntime)+ ', ''' + CreateAtRuntime_FieldType + ''', ' + convert(nvarchar(20), Mandatory)+ ', ' + convert(nvarchar(20), Locked)+ ', ' + convert(nvarchar(20), Visible)+ ', ''' + xType_VB + ''', ''' + replace(F_Format, '''', '''''') + ''', ''' + DT_FixText_Key + ''', ''' + replace(DT_WHERE2, '''', '''''') + ''',''' + DT_ID_Field + ''', ''' + COLORS + ''', ''' + BG_Image + ''', ' + convert(nvarchar(20), BG_Toggle)+ ', ''' + replace(Label_Text, '''', '''''') + ''', ' + convert(nvarchar(20), ShowInGRID)+ ', ' + convert(nvarchar(20), SavePoint)+ ', ''' + Forced_NextField+ ''', ''' + replace(Tag, '''', '''''') + ''', ' + convert(nvarchar(20), TabIndex)+ ', ' + convert(nvarchar(20), TabStop)+ ')'	COMMAND
FROM		RS_FIELDS_DEBUG

UNION ALL -- RS_ARRANGE_DEBUG

SELECT 'INSERT INTO RS_ARRANGE_DEBUG	(ServerObject_Prefix, SubPrefix, SeqNum, ControlName, ArrangeType, P1_Control, P1_Koo, P1_SpaceInPixel, P2_Control, P2_Koo, P2_SpaceInPixel, Percentage)'
+ 'VALUES (''' + ServerObject_Prefix + ''', ''' + SubPrefix + ''', ' + convert(nvarchar(20), SeqNum)+ ', ''' + ControlName + ''', ''' + ArrangeType + ''', ''' + P1_Control + ''', ' + convert(nvarchar(20), P1_Koo)+ ', ' + convert(nvarchar(20), P1_SpaceInPixel)+ ', ''' + P2_Control + ''', ' + convert(nvarchar(20), P2_Koo)+ ', ' + convert(nvarchar(20), P2_SpaceInPixel)+ ', ' + convert(nvarchar(20), Percentage) + ')'	COMMAND
FROM		RS_ARRANGE_DEBUG


-- RS_FIELDS, RS_ARRANGE ----------------------------------------------------------------------------------------

SELECT 'DELETE  RS_Fields'
UNION ALL
SELECT 'DELETE  RS_ARRANGE'

UNION ALL -- RS_Fields

SELECT 'INSERT INTO RS_FIELDS	(ServerObject_Prefix, SubPrefix, SeqNum, FieldName, LabelName, Parent, CreateAtRuntime, CreateAtRuntime_FieldType, Mandatory, Locked, Visible, xType_VB, F_Format, DT_FixText_Key, DT_WHERE2, DT_ID_Field, COLORS, BG_Image, BG_Toggle, Label_Text, ShowInGRID, SavePoint, Forced_NextField, Tag, TabIndex, TabStop) '
+ 'VALUES (''' + ServerObject_Prefix + ''', ''' + SubPrefix + ''', ' + convert(nvarchar(20), SeqNum)+ ', ''' + FieldName + ''', ''' + LabelName + ''', ''' + Parent + ''', ' + convert(nvarchar(20), CreateAtRuntime)+ ', ''' + CreateAtRuntime_FieldType + ''', ' + convert(nvarchar(20), Mandatory)+ ', ' + convert(nvarchar(20), Locked)+ ', ' + convert(nvarchar(20), Visible)+ ', ''' + xType_VB + ''', ''' + replace(F_Format, '''', '''''') + ''', ''' + DT_FixText_Key + ''', ''' + replace(DT_WHERE2, '''', '''''') + ''',''' + DT_ID_Field + ''', ''' + COLORS + ''', ''' + BG_Image + ''', ' + convert(nvarchar(20), BG_Toggle)+ ', ''' + replace(Label_Text, '''', '''''') + ''', ' + convert(nvarchar(20), ShowInGRID)+ ', ' + convert(nvarchar(20), SavePoint)+ ', ''' + Forced_NextField+ ''', ''' + replace(Tag, '''', '''''') + ''', ' + convert(nvarchar(20), TabIndex)+ ', ' + convert(nvarchar(20), TabStop)+ ')'	COMMAND
FROM		RS_FIELDS

UNION ALL -- RS_ARRANGE

SELECT 'INSERT INTO RS_ARRANGE	(ServerObject_Prefix, SubPrefix, SeqNum, ControlName, ArrangeType, P1_Control, P1_Koo, P1_SpaceInPixel, P2_Control, P2_Koo, P2_SpaceInPixel, Percentage)'
+ 'VALUES (''' + ServerObject_Prefix + ''', ''' + SubPrefix + ''', ' + convert(nvarchar(20), SeqNum)+ ', ''' + ControlName + ''', ''' + ArrangeType + ''', ''' + P1_Control + ''', ' + convert(nvarchar(20), P1_Koo)+ ', ' + convert(nvarchar(20), P1_SpaceInPixel)+ ', ''' + P2_Control + ''', ' + convert(nvarchar(20), P2_Koo)+ ', ' + convert(nvarchar(20), P2_SpaceInPixel)+ ', ' + convert(nvarchar(20), Percentage) + ')'	COMMAND
FROM		RS_ARRANGE

-- FixText ------------------------------------------------------------------------------------------------------

SELECT		N'#END_BAT#
DECLARE @ID				INT
DECLARE @TransactID		INT

EXEC @TransactID = Tr_getNewTransactID ''SERVER'', ''MSE''
'										COMMAND

UNION ALL

SELECT		N'EXEC @ID = NachsteNummerVergeben
INSERT INTO FixText (ID, TransactID, Code, [Text], Titel) VALUES (@ID, @TransactID, N''' + replace(Code, '''', '''''') + ''', N''' + replace(convert(nvarchar(max), [Text]), '''', '''''') + ''', N''' + replace(isnull(Titel, ''), '''', '''''') + ''')'
FROM		Fixtext
WHERE		LEN(CONVERT(NVARCHAR(MAX), [Text])) <= 8000

UNION ALL

SELECT		N'#END_BAT#
'

-- MSPED2 -------------------------------------------------------------------------------------------------------

SELECT		N'#END_BAT#
DECLARE @ID				INT
DECLARE @TransactID		INT

EXEC @TransactID = Tr_getNewTransactID ''SERVER'', ''MSE''
'										COMMAND

UNION ALL


SELECT N'EXEC @ID = NachsteNummerVergeben
INSERT INTO MSPED2 (ID, TransactID, Sprache, Art, Nummer, [Text], LetzteAenderung, Bemerkungen, StatusText, Tiptext) VALUES (@ID, @TransactID, ''' + Sprache + ''', ''' + Art + ''', ' + CONVERT(NVARCHAR(20), Nummer) + ', N''' + replace([Text], '''', '''''') + ''', CONVERT(DATETIME, ''' + CONVERT(NVARCHAR(30), LetzteAenderung, 121) + ''', 121), N''' + replace(isnull(Bemerkungen, ''), '''', '''''') + ''', N''' + replace(isnull(StatusText, ''), '''', '''''') + ''', N''' + replace(isnull(TipText, ''), '''', '''''') + ''')'
FROM		MSPED2

UNION ALL

SELECT		N'#END_BAT#
'

-- DispoDEF -------------------------------------------------------------------------------------------------------

SELECT	'INSERT INTO DispoDEF (Identifier, StoredProc_1, Fix_WHERE, DoFilter, DoFilter_WhereQuery, Simple_Select, StoredProc_2, Report_Name, Report_SQL, Report_OpenType, Excel_export, Txt_export, FilePath, StoredProc_3, Next_Identifier) '
		+ 'VALUES (''' + Identifier + ''', ''' + ISNULL(StoredProc_1, '') + ''', ''' + ISNULL(Fix_WHERE, '') + ''', ''' + ISNULL(DoFilter, '') + ''', ''' + ISNULL(DoFilter_WhereQuery, '') + ''', ''' + ISNULL(Simple_Select, '') + ''', ''' + ISNULL(StoredProc_2, '') + ''', ''' + ISNULL(Report_Name, '') + ''', ''' + ISNULL(Report_SQL, '') + ''', ''' + CONVERT(NVARCHAR(20), ISNULL(Report_OpenType, 0)) + ''', ''' + ISNULL(Excel_export, '') + ''', ''' + ISNULL(Txt_export, '') + ''', ''' + ISNULL(FilePath, '') + ''', ''' + ISNULL(StoredProc_3, '') + ''', ''' + ISNULL(Next_Identifier, '') + ''')'
FROM	DispoDEF

-----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-- MENU teljes script-je
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------


SELECT	'DECLARE @ID				INT'
UNION ALL
SELECT	'DECLARE @TransactID		INT'
UNION ALL
SELECT	'EXEC @TransactID = Tr_getNewTransactID ''SERVER'', ''MSE'''
UNION ALL
SELECT 'DELETE  RS_Fields_DEBUG WHERE ServerObject_Prefix = ''FP_MENU'''
UNION ALL
SELECT 'DELETE  RS_ARRANGE_DEBUG WHERE ServerObject_Prefix = ''FP_MENU'''
UNION ALL
SELECT 'DELETE MSPED2 WHERE Art = ''VBSEQ_FP_MENU'''

UNION ALL -- RS_Fields_DEBUG

SELECT 'INSERT INTO RS_FIELDS_DEBUG	(ServerObject_Prefix, SubPrefix, SeqNum, FieldName, LabelName, Parent, CreateAtRuntime, CreateAtRuntime_FieldType, Mandatory, Locked, Visible, xType_VB, F_Format, DT_FixText_Key, DT_WHERE2, DT_ID_Field, COLORS, BG_Image, BG_Toggle, Label_Text, ShowInGRID, SavePoint, Forced_NextField, Tag, TabIndex, TabStop) '
+ 'VALUES (''' + ServerObject_Prefix + ''', ''' + SubPrefix + ''', ' + convert(nvarchar(20), SeqNum)+ ', ''' + FieldName + ''', ''' + LabelName + ''', ''' + Parent + ''', ' + convert(nvarchar(20), CreateAtRuntime)+ ', ''' + CreateAtRuntime_FieldType + ''', ' + convert(nvarchar(20), Mandatory)+ ', ' + convert(nvarchar(20), Locked)+ ', ' + convert(nvarchar(20), Visible)+ ', ''' + xType_VB + ''', ''' + replace(F_Format, '''', '''''') + ''', ''' + DT_FixText_Key + ''', ''' + replace(DT_WHERE2, '''', '''''') + ''',''' + DT_ID_Field + ''', ''' + COLORS + ''', ''' + BG_Image + ''', ' + convert(nvarchar(20), BG_Toggle)+ ', ''' + replace(Label_Text, '''', '''''') + ''', ' + convert(nvarchar(20), ShowInGRID)+ ', ' + convert(nvarchar(20), SavePoint)+ ', ''' + Forced_NextField+ ''', ''' + replace(Tag, '''', '''''') + ''', ' + convert(nvarchar(20), TabIndex)+ ', ' + convert(nvarchar(20), TabStop)+ ')'	COMMAND
FROM		RS_FIELDS_DEBUG
WHERE		ServerObject_Prefix = 'FP_MENU'

UNION ALL -- RS_ARRANGE_DEBUG

SELECT 'INSERT INTO RS_ARRANGE_DEBUG	(ServerObject_Prefix, SubPrefix, SeqNum, ControlName, ArrangeType, P1_Control, P1_Koo, P1_SpaceInPixel, P2_Control, P2_Koo, P2_SpaceInPixel, Percentage)'
+ 'VALUES (''' + ServerObject_Prefix + ''', ''' + SubPrefix + ''', ' + convert(nvarchar(20), SeqNum)+ ', ''' + ControlName + ''', ''' + ArrangeType + ''', ''' + P1_Control + ''', ' + convert(nvarchar(20), P1_Koo)+ ', ' + convert(nvarchar(20), P1_SpaceInPixel)+ ', ''' + P2_Control + ''', ' + convert(nvarchar(20), P2_Koo)+ ', ' + convert(nvarchar(20), P2_SpaceInPixel)+ ', ' + convert(nvarchar(20), Percentage) + ')'	COMMAND
FROM		RS_ARRANGE_DEBUG
WHERE		ServerObject_Prefix = 'FP_MENU'

UNION ALL -- MSPED2

SELECT	'EXEC @ID = NachsteNummerVergeben		INSERT INTO MSPED2 (ID, TransactID, Sprache, Art, Nummer, Text, LetzteAenderung, Bemerkungen, StatusText, TipText) VALUES (@ID, @TransactID, ''' + isnull(Sprache,'') + ''', '''+Art+''', ' + CONVERT(nvarchar(20), Nummer) + ', '''+[Text]+''', '''+CONVERT(nvarchar(5),year(getdate()))+'-'+CONVERT(nvarchar(5),month(getdate()))+'-'+CONVERT(nvarchar(5),day(getdate()))+''', '''+ISNULL(Bemerkungen,'')+''', '''+isnull(StatusText,'')+''', '''+isnull(TipText,'')+''')'
FROM	MSPED2
WHERE	Art = 'VBSEQ_FP_MENU'


-----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-- DEBUG tablak kozvetlen szerkesztese SQL-bol
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------


INSERT INTO RS_Fields_DEBUG (TransactID, ServerObject_Prefix,         SubPrefix,     SeqNum,        FieldName,     LabelName, Parent, CreateAtRuntime, CreateAtRuntime_FieldType, Mandatory, Locked, Visible, xType_VB, F_Format, DT_FixText_Key, DT_WHERE2, DT_ID_Field, COLORS, BG_Image,                  BG_Toggle, Label_Text, ShowInGRID, SavePoint, Forced_NextField, Tag, TabIndex, TabStop)
				VALUES		(0,          <'ServerObject_Prefix'>,     <'Subprefix'>, <SeqNum>,      <'FieldName'>, '',        '',     0,               '',                        0,         0,      1,       '',       '',       '',             '',        '',          '',     '',                        0,         '',         0,          0,         '',               '',  0,        0)


INSERT INTO RS_ARRANGE_DEBUG (TransactID, ServerObject_Prefix,         SubPrefix,     SeqNum,        ControlName,     ArrangeType,            P1_Control,    P1_Koo, P1_SpaceInPixel, P2_Control, P2_Koo, P2_SpaceInPixel, Percentage)
			VALUES			 (0,          <'ServerObject_Prefix'>,     <'Subprefix'>, <SeqNum>,      <'ControlName'>, <'ArrangeType'>,        '',            0,      0,               '',         0,      0,               0)

begin tran
UPDATE RS_ARRANGE_DEBUG SET SeqNum = SeqNum + 1 WHERE ServerObject_Prefix = <'ServerObject_Prefix'> AND Subprefix = <'Subprefix'> AND SeqNum >= <SeqNum>
commit tran


-----------------------------------------------------------------------------------------------------------------------------------------------------------
-- Mezok nevenek megvaltoztatasa
-----------------------------------------------------------------------------------------------------------------------------------------------------------

DECLARE @ServerObject_Prefix	NVARCHAR(128)	= <'ServerObject_Prefix'>
DECLARE @SubPrefix				NVARCHAR(128)	= <'SubPrefix'>
DECLARE @FieldName_Old			NVARCHAR(128)	= <'FieldName_Old'>
DECLARE @FieldName_New			NVARCHAR(128)	= <'FieldName_New'>

BEGIN TRAN

UPDATE RS_ARRANGE_Debug		SET		ControlName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND ControlName = @FieldName_Old
UPDATE RS_ARRANGE_Debug		SET		P1_Control	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND P1_Control = @FieldName_Old
UPDATE RS_ARRANGE_Debug		SET		P2_Control	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND P2_Control = @FieldName_Old
UPDATE RS_Fields_DEBUG		SET		FieldName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND FieldName = @FieldName_Old
UPDATE RS_Fields_DEBUG		SET		LabelName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND FieldName = @FieldName_Old

UPDATE RS_ARRANGE			SET		ControlName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND ControlName = @FieldName_Old
UPDATE RS_ARRANGE			SET		P1_Control	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND P1_Control = @FieldName_Old
UPDATE RS_ARRANGE			SET		P2_Control	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND P2_Control = @FieldName_Old
UPDATE RS_Fields			SET		FieldName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND FieldName = @FieldName_Old
UPDATE RS_Fields			SET		LabelName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND FieldName = @FieldName_Old

UPDATE RS_ARRANGE0			SET		ControlName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND ControlName = @FieldName_Old
UPDATE RS_ARRANGE0			SET		P1_Control	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND P1_Control = @FieldName_Old
UPDATE RS_ARRANGE0			SET		P2_Control	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND P2_Control = @FieldName_Old
UPDATE RS_Fields0			SET		FieldName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND FieldName = @FieldName_Old
UPDATE RS_Fields0			SET		LabelName	= @FieldName_New	WHERE	ServerObject_Prefix = @ServerObject_Prefix AND SubPrefix = @SubPrefix AND FieldName = @FieldName_Old

COMMIT TRAN

