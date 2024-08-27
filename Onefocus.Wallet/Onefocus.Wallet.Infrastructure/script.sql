CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Bank" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(255),
    "ActiveFlag" boolean NOT NULL,
    "CreatedOn" timestamp with time zone,
    "UpdatedOn" timestamp with time zone,
    "CreatedBy" uuid,
    "UpdatedBy" uuid,
    CONSTRAINT "PK_Bank" PRIMARY KEY ("Id")
);

CREATE TABLE "Currency" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "ShortName" character varying(4) NOT NULL,
    "DefaultFlag" boolean NOT NULL,
    "Description" character varying(255),
    "ActiveFlag" boolean NOT NULL,
    "CreatedOn" timestamp with time zone,
    "UpdatedOn" timestamp with time zone,
    "CreatedBy" uuid,
    "UpdatedBy" uuid,
    CONSTRAINT "PK_Currency" PRIMARY KEY ("Id")
);

CREATE TABLE "User" (
    "Id" uuid NOT NULL,
    "FirstName" character varying(50) NOT NULL,
    "LastName" character varying(50) NOT NULL,
    "Email" character varying(256) NOT NULL,
    "Description" character varying(255),
    "ActiveFlag" boolean NOT NULL,
    "CreatedOn" timestamp with time zone,
    "UpdatedOn" timestamp with time zone,
    "CreatedBy" uuid,
    "UpdatedBy" uuid,
    CONSTRAINT "PK_User" PRIMARY KEY ("Id")
);

CREATE TABLE "Transaction" (
    "Id" uuid NOT NULL,
    "Amount" numeric NOT NULL,
    "TransactedOn" timestamp with time zone NOT NULL,
    "UserId" uuid NOT NULL,
    "CurrencyId" uuid NOT NULL,
    "Description" character varying(255),
    "ActiveFlag" boolean NOT NULL,
    "CreatedOn" timestamp with time zone,
    "UpdatedOn" timestamp with time zone,
    "CreatedBy" uuid,
    "UpdatedBy" uuid,
    CONSTRAINT "PK_Transaction" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Transaction_Currency_Id" FOREIGN KEY ("Id") REFERENCES "Currency" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Transaction_User_Id" FOREIGN KEY ("Id") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE TABLE "BankingTransaction" (
    "Id" uuid NOT NULL,
    "BankId" uuid NOT NULL,
    "BankAccount_AccountNumber" character varying(50) NOT NULL,
    "BankAccount_CloseFlag" boolean NOT NULL,
    "BankAccount_ClosedOn" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_BankingTransaction" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BankingTransaction_Bank_BankId" FOREIGN KEY ("BankId") REFERENCES "Bank" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BankingTransaction_Transaction_Id" FOREIGN KEY ("Id") REFERENCES "Transaction" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ExchangeTransaction" (
    "Id" uuid NOT NULL,
    "ExchangedCurrencyId" uuid NOT NULL,
    "ExchangeRate" numeric NOT NULL,
    CONSTRAINT "PK_ExchangeTransaction" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ExchangeTransaction_Currency_ExchangedCurrencyId" FOREIGN KEY ("ExchangedCurrencyId") REFERENCES "Currency" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ExchangeTransaction_Transaction_Id" FOREIGN KEY ("Id") REFERENCES "Transaction" ("Id") ON DELETE CASCADE
);

CREATE TABLE "IncomeTransaction" (
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_IncomeTransaction" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_IncomeTransaction_Transaction_Id" FOREIGN KEY ("Id") REFERENCES "Transaction" ("Id") ON DELETE CASCADE
);

CREATE TABLE "OutcomeTransaction" (
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_OutcomeTransaction" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OutcomeTransaction_Transaction_Id" FOREIGN KEY ("Id") REFERENCES "Transaction" ("Id") ON DELETE CASCADE
);

CREATE TABLE "TransactionDetail" (
    "Id" uuid NOT NULL,
    "Amount" numeric NOT NULL,
    "TransactedOn" timestamp with time zone NOT NULL,
    "Action" integer NOT NULL,
    "TransactionId" uuid NOT NULL,
    "Description" character varying(255),
    "ActiveFlag" boolean NOT NULL,
    "CreatedOn" timestamp with time zone,
    "UpdatedOn" timestamp with time zone,
    "CreatedBy" uuid,
    "UpdatedBy" uuid,
    CONSTRAINT "PK_TransactionDetail" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TransactionDetail_Transaction_Id" FOREIGN KEY ("Id") REFERENCES "Transaction" ("Id") ON DELETE CASCADE
);

CREATE TABLE "TransferTransaction" (
    "Id" uuid NOT NULL,
    "TransferredUserId" uuid NOT NULL,
    CONSTRAINT "PK_TransferTransaction" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_TransferTransaction_Transaction_Id" FOREIGN KEY ("Id") REFERENCES "Transaction" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_TransferTransaction_User_TransferredUserId" FOREIGN KEY ("TransferredUserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_BankingTransaction_BankId" ON "BankingTransaction" ("BankId");

CREATE INDEX "IX_ExchangeTransaction_ExchangedCurrencyId" ON "ExchangeTransaction" ("ExchangedCurrencyId");

CREATE INDEX "IX_TransferTransaction_TransferredUserId" ON "TransferTransaction" ("TransferredUserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240824075204_InitiateWalletDbSchema', '8.0.7');

COMMIT;

