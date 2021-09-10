CREATE PROCEDURE <ProcName>
@Terminal			NVARCHAR(10),
@ProcessID			INT,
@RS_ID				INT,

@Result				INT					OUTPUT,
@ErrText			NVARCHAR(255)		OUTPUT,
@ErrParams			NVARCHAR(255)		OUTPUT

AS

SELECT	@Terminal		= ISNULL(@Terminal, ''),
		@ProcessID		= ISNULL(@ProcessID, 0),
		@RS_ID			= ISNULL(@RS_ID, 0),

		@Result			= 0,
		@ErrText		= '',
		@ErrParams		= ''

RETURN -1
