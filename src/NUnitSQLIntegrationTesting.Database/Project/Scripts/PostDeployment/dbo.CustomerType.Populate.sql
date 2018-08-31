MERGE	dbo.CustomerType AS target
USING
	(
		SELECT	1 AS CustomerTypeId,
				'VIP' AS CustomerTypeName
		UNION ALL
		SELECT	2 AS CustomerTypeId,
				'Normal' AS CustomerTypeName
	) AS source ON source.CustomerTypeId = target.CustomerTypeId
WHEN MATCHED THEN
	UPDATE
	SET		CustomerTypeName = source.CustomerTypeName
WHEN NOT MATCHED THEN
	INSERT ( CustomerTypeId, CustomerTypeName )
	VALUES ( source.CustomerTypeId, source.CustomerTypeName )
;