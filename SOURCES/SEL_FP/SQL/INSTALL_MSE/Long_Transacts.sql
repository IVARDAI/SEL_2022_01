-------------------------------------------------------------------------------------------------------------------------------------
-- Visszaadja a hosszu ideig tarto tranzakciokat
-------------------------------------------------------------------------------------------------------------------------------------

SELECT		Terminal,
			ObjName,
			CommandText,
			Date_START,
			DATEDIFF(SECOND, Date_START, Date_END)		Time_elapsed
			
FROM		SEL_SYS_Error_LongTransacts

WHERE		ABS(DATEDIFF(SECOND, Date_START, Date_END)) > 1
			AND Terminal <> 'MLACI'

ORDER BY	DATEDIFF(SECOND, Date_START, Date_END) DESC

