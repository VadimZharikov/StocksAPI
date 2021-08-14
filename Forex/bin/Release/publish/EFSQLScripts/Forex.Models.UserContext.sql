IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE TABLE [Stocks] (
        [StockId] nvarchar(450) NOT NULL,
        [StockCode] nvarchar(max) NULL,
        CONSTRAINT [PK_Stocks] PRIMARY KEY ([StockId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE TABLE [Users] (
        [UserId] int NOT NULL IDENTITY,
        [UserName] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE TABLE [Items] (
        [ItemId] int NOT NULL IDENTITY,
        [Quantity] int NOT NULL,
        [StockId] nvarchar(450) NULL,
        [UserId] int NOT NULL,
        CONSTRAINT [PK_Items] PRIMARY KEY ([ItemId]),
        CONSTRAINT [FK_Items_Stocks_StockId] FOREIGN KEY ([StockId]) REFERENCES [Stocks] ([StockId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Items_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE TABLE [Offers] (
        [OfferId] int NOT NULL IDENTITY,
        [Quantity] int NOT NULL,
        [StocksLeft] int NOT NULL,
        [Price] Decimal(18,2) NOT NULL,
        [OfferType] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [UserId] int NOT NULL,
        [StockId] nvarchar(450) NULL,
        CONSTRAINT [PK_Offers] PRIMARY KEY ([OfferId]),
        CONSTRAINT [FK_Offers_Stocks_StockId] FOREIGN KEY ([StockId]) REFERENCES [Stocks] ([StockId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Offers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE TABLE [Wallets] (
        [UserId] int NOT NULL,
        [Funds] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Wallets] PRIMARY KEY ([UserId]),
        CONSTRAINT [FK_Wallets_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE TABLE [Trades] (
        [TradeId] int NOT NULL IDENTITY,
        [Seller] nvarchar(max) NULL,
        [Buyer] nvarchar(max) NULL,
        [Quantity] int NOT NULL,
        [TotalPrice] Decimal(18,2) NOT NULL,
        [SellerOfferId] int NULL,
        [BuyerOfferId] int NULL,
        CONSTRAINT [PK_Trades] PRIMARY KEY ([TradeId]),
        CONSTRAINT [FK_Trades_Offers_BuyerOfferId] FOREIGN KEY ([BuyerOfferId]) REFERENCES [Offers] ([OfferId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Trades_Offers_SellerOfferId] FOREIGN KEY ([SellerOfferId]) REFERENCES [Offers] ([OfferId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE INDEX [IX_Items_StockId] ON [Items] ([StockId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE INDEX [IX_Items_UserId] ON [Items] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE INDEX [IX_Offers_StockId] ON [Offers] ([StockId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    CREATE INDEX [IX_Offers_UserId] ON [Offers] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Trades_BuyerOfferId] ON [Trades] ([BuyerOfferId]) WHERE [BuyerOfferId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Trades_SellerOfferId] ON [Trades] ([SellerOfferId]) WHERE [SellerOfferId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609133339_Init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210609133339_Init', N'5.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609184719_Init1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210609184719_Init1', N'5.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609185312_Init2')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210609185312_Init2', N'5.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609185501_Init3')
BEGIN
    ALTER TABLE [Wallets] DROP CONSTRAINT [PK_Wallets];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609185501_Init3')
BEGIN
    ALTER TABLE [Wallets] ADD [WalletId] int NOT NULL IDENTITY;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609185501_Init3')
BEGIN
    ALTER TABLE [Wallets] ADD CONSTRAINT [PK_Wallets] PRIMARY KEY ([WalletId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609185501_Init3')
BEGIN
    CREATE UNIQUE INDEX [IX_Wallets_UserId] ON [Wallets] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210609185501_Init3')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210609185501_Init3', N'5.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210610054023_Init4')
BEGIN
    EXEC sp_rename N'[Stocks].[StockCode]', N'StockName', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210610054023_Init4')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210610054023_Init4', N'5.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210610093516_Init5')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210610093516_Init5', N'5.0.7');
END;
GO

COMMIT;
GO

