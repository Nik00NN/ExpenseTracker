CREATE TABLE [Transactions] (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
    Description NVARCHAR(255) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Date DATE NOT NULL,
    IsRecurrent BIT NOT NULL DEFAULT 0,
    TransactionType INT DEFAULT 0,
    CategoryId UNIQUEIDENTIFIER NOT NULL,
    UserId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);


CREATE TABLE [Categories] (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL,
    ParentCategoryId UNIQUEIDENTIFIER
)


CREATE TABLE [Users] (
    Id INT IDENTITY NOT NULL PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    UserPassword NVARCHAR(100) NOT NULL,
    CreateDate DATETIMEOFFSET(7) NOT NULL,
    UpdateDate DATETIMEOFFSET(7),
    LastName NVARCHAR(20) NOT NULL,
    FirstName NVARCHAR(20) NOT NULL,
    LockedOut BIT DEFAULT 0 NOT NULL,
    LoginTries SMALLINT DEFAULT 0 NOT NULL,
)

