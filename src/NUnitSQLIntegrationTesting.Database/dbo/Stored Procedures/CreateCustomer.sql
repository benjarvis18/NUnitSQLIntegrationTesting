CREATE PROCEDURE [dbo].[CreateCustomer]
	@Name varchar(255),
	@EmailAddress varchar(255),
	@CustomerTypeId int
AS
BEGIN
	INSERT INTO dbo.Customer
	(
		Name,
		EmailAddress,
		CustomerTypeId
	)
	VALUES 
	(
		@Name,
		@EmailAddress,
		@CustomerTypeId
	)

	SELECT SCOPE_IDENTITY() AS CustomerId
END