CREATE PROCEDURE [dbo].[GetCustomer]
	@CustomerId int
AS
BEGIN
	SELECT	C.CustomerId,
			C.Name,
			C.EmailAddress,
			CT.CustomerTypeId,
			CT.CustomerTypeName
	FROM	dbo.Customer C
			INNER JOIN dbo.CustomerType CT ON CT.CustomerTypeId = C.CustomerTypeId
	WHERE	C.CustomerId = @CustomerId
END
