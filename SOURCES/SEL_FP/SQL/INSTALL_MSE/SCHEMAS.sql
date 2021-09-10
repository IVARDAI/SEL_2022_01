--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
-- STEP 1 SELECT EXISTING SCHEMAS FOR SELESTER
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------

SELECT name FROM sys.schemas WHERE LEFT(name, 4) = 'SEL_'

--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
-- STEP 2 CREATE SCRIPT DROP SCHEMAS And USERS if neccessary
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------

GO

DECLARE @w			INT
DECLARE @Result		INT
DECLARE @ErrText	NVARCHAR(255)

EXEC @w = Services_SCHEMAS_SCRIPT_DROP_USER_AND_SCHEMA	@NameOfSchema	= SEL_<'NameOfSchema'>,		-- Az eljaras kiegesziti a nevet a 'SEL_' elotaggal
														@SaveToFile		= 'c:\Selester\work.sql',
														@Result			= @Result					OUTPUT,
														@ErrText		= @ErrText					OUTPUT

SELECT @w EREDMENY, @Result RESULT, @ErrText ERRTEXT

GO

--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
-- STEP 3: CREATE SCRIPT for Functions depending on existings schemas and CREATE SCRIPT for CREATE USER
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------

GO

DECLARE @w			INT
DECLARE @Result		INT
DECLARE @ErrText	NVARCHAR(255)

EXEC @w = dbo.Services_SCHEMAS_SCRIPT_ADD_NEW_USER_AND_SCHEMA	@NameOfNewSchema		= 'SEL_'<NameOfNewSchema>,	-- Az eljaras kiegesziti a nevet a 'SEL_' elotaggal
																@SaveToFile				= 'c:\Selester\work.sql',
																@Result					= @Result		OUTPUT,
																@ErrText				= @ErrText		OUTPUT

SELECT @w EREDMENY, @Result RESULT, @ErrText ERRTEXT

GO
