START TRANSACTION;
DROP TABLE "Setting";

CREATE TABLE "Settings" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Description" character varying(255),
    "IsActive" boolean NOT NULL,
    "CreatedOn" timestamp with time zone,
    "UpdatedOn" timestamp with time zone,
    "CreatedBy" uuid,
    "UpdatedBy" uuid,
    "Preferences" jsonb NOT NULL,
    CONSTRAINT "PK_Settings" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Settings_User_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Settings_UserId" ON "Settings" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251004024505_RenameSettingToSettings', '9.0.9');

COMMIT;

