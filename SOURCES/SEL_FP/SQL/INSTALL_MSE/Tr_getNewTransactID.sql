DECLARE @ID				INT
DECLARE @TransactID		INT

EXEC @ID = NachsteNummerVergeben
EXEC @TransactID = Tr_getNewTransactID <'TerminalName'>, <'ProcName'>	-- , <@RecordID>, <ParentTransactID>, <NoTriggers>, <'SavePoint'>
