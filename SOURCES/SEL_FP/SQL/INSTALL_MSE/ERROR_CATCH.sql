BEGIN TRY
	BEGIN TRAN
		.....................
		....................
	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK
	SELECT	ERROR_NUMBER()		ErrorNumber,
			ERROR_SEVERITY()	ErrorSeverity,
			ERROR_STATE()		ErrorState,
			ERROR_PROCEDURE()	ErrorProcedure,
			ERROR_LINE()		ErrorLine,
			ERROR_MESSAGE()		ErrorMessage
END CATCH
