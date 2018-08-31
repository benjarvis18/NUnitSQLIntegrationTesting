CREATE TABLE [dbo].[Customer]
(
	[CustomerId] INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Customer PRIMARY KEY,
	[Name] VARCHAR(255) NOT NULL,
	[EmailAddress] VARCHAR(255) NULL,
	[CustomerTypeId] INT NOT NULL CONSTRAINT FK_Customer_CustomerType FOREIGN KEY REFERENCES dbo.CustomerType(CustomerTypeId)
)
