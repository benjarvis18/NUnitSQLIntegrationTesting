CREATE PROCEDURE [dbo].[SetCustomerEmailAddress]
	@CustomerId int,
	@EmailAddress varchar(255)
AS
BEGIN
	UPDATE	dbo.Customer
	SET		EmailAddress = @EmailAddress
	WHERE	CustomerId = @CustomerId
END
