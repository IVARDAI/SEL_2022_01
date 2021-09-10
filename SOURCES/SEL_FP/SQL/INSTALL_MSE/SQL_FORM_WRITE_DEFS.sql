------------------------------------------------------------------------------------------------------
-- Services_SQL_FORM_WRITE_DEFS - Vers 1.01
------------------------------------------------------------------------------------------------------

DECLARE @Result					int
DECLARE @ErrText				nvarchar(255)
DECLARE @w						int
DECLARE @ServerObject_Prefix	nvarchar(255)
DECLARE @SubPrefix				nvarchar(255)
DECLARE @PatternObject			nvarchar(255)
DECLARE @ControlledObject		nvarchar(255)
DECLARE @SaveToFile				nvarchar(255)
DECLARE @UniqueTable			nvarchar(255)

------------------------------------------------------------------------------------------------------
-- BEMENO PARAMETEREK
------------------------------------------------------------------------------------------------------

SELECT	@PatternObject			= '<Az a tabla, view vagy tarolt eljaras, aminek a mezoibol felepul a Form>',
		@ControlledObject		= '<Ebben a tablaban/view-ban kell elvegezni a beszurast, torlest vagy modositast.>',
		@ServerObject_Prefix	= '<>',
		@SubPrefix				= '<>',
		@UniqueTable			= '<>',
		@SaveToFile				= 'c:\Selester\work.sql'


exec @w = Services_SQL_FORM_WRITE_DEFS	@PatternObject			= @PatternObject,
										@ControlledObject		= @ControlledObject,
										@ServerObject_Prefix	= @ServerObject_Prefix,
										@SubPrefix				= @SubPrefix,				-- RS_Fields.SubPrefix
																							-- (Ha tobb elrendezesben akarod
																							-- ugyanazokat az adatokat megjeleniteni,
																							-- akkor van ertelme. Segitsegevel
																							-- ugyanahhoz a record kezelo
																							-- @ServerObject_Prefix-hez kulonbozo
																							-- mezoelrendezeseket tarsithatsz.
										@UniqueTable			= @UniqueTable,
										@SaveToFile				= @SaveToFile,
										@Result					= @Result					OUTPUT,
										@ErrText				= @ErrText					OUTPUT

SELECT @w EREDMENY, @Result Result, @ErrText ErrText
