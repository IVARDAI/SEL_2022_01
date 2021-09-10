--select * from SEQMSPED1 WHERE Art like 'VBSEQ_REPORT_INVOICE_LABELS'

--select * from SEQMSPED1 where [Text] = 'LabelPage_Before'

DECLARE @SEQ_Key		NVARCHAR(255)
DECLARE @Label_Name		NVARCHAR(255)
DECLARE @Text_H			NVARCHAR(255)
DECLARE @Text_D			NVARCHAR(255)
DECLARE @Text_GB		NVARCHAR(255)
DECLARE @CurrentDate	DATETIME

SELECT	@SEQ_Key	= <'VBSEQ_REPORT_DEMO_LABELS'>,
		@Label_Name	= <'Label_DEMO'>,
		@Text_H		= <'magyar'>,
		@Text_D		= <'német'>,
		@Text_GB	= <'angol'>

SELECT	@CurrentDate	= dbo.FN_DATE_ONLY_DATE(GETDATE())

DELETE SEQMSPED1 WHERE Art = @SEQ_Key And [Text] = @Label_Name

INSERT INTO SEQMSPED1	(Sprache,	Art,      Nummer, [Text],      LetzteAenderung, Bemerkungen, StatusText, TipText)
			VALUES		('H',		@SEQ_Key, 0,      @Label_Name, @CurrentDate,    '',          @Text_H,    '')
INSERT INTO SEQMSPED1	(Sprache,	Art,      Nummer, [Text],      LetzteAenderung, Bemerkungen, StatusText, TipText)
			VALUES		('D',		@SEQ_Key, 0,      @Label_Name, @CurrentDate,    '',          @Text_D,    '')
INSERT INTO SEQMSPED1	(Sprache,	Art,      Nummer, [Text],      LetzteAenderung, Bemerkungen, StatusText, TipText)
			VALUES		('GB',		@SEQ_Key, 0,      @Label_Name, @CurrentDate,    '',          @Text_GB,   '')

SELECT * FROM SEQMSPED1 WHERE Art = @SEQ_Key And [Text] = @Label_Name

