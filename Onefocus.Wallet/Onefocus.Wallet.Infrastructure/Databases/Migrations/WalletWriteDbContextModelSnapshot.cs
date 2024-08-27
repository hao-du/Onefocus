﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

#nullable disable

namespace Onefocus.Wallet.Infrastructure.Databases.Migrations
{
    [DbContext(typeof(WalletWriteDbContext))]
    partial class WalletWriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Bank", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("ActiveFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Bank");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("ActiveFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("DefaultFlag")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<bool>("ActiveFlag")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTimeOffset>("TransactedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Transaction");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.TransactionDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<int>("Action")
                        .HasColumnType("integer");

                    b.Property<bool>("ActiveFlag")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTimeOffset>("TransactedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("TransactionDetail");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("ActiveFlag")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.BankingTransaction", b =>
                {
                    b.HasBaseType("Onefocus.Wallet.Domain.Entities.Write.Transaction");

                    b.Property<Guid>("BankId")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("BankAccount", "Onefocus.Wallet.Domain.Entities.Write.Transactions.BankingTransaction.BankAccount#BankAccount", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("AccountNumber")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)");

                            b1.Property<bool>("CloseFlag")
                                .HasColumnType("boolean");

                            b1.Property<DateTimeOffset>("ClosedOn")
                                .HasColumnType("timestamp with time zone");
                        });

                    b.HasIndex("BankId");

                    b.ToTable("BankingTransaction");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.ExchangeTransaction", b =>
                {
                    b.HasBaseType("Onefocus.Wallet.Domain.Entities.Write.Transaction");

                    b.Property<decimal>("ExchangeRate")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ExchangedCurrencyId")
                        .HasColumnType("uuid");

                    b.HasIndex("ExchangedCurrencyId");

                    b.ToTable("ExchangeTransaction");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.IncomeTransaction", b =>
                {
                    b.HasBaseType("Onefocus.Wallet.Domain.Entities.Write.Transaction");

                    b.ToTable("IncomeTransaction");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.OutcomeTransaction", b =>
                {
                    b.HasBaseType("Onefocus.Wallet.Domain.Entities.Write.Transaction");

                    b.ToTable("OutcomeTransaction");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.TransferTransaction", b =>
                {
                    b.HasBaseType("Onefocus.Wallet.Domain.Entities.Write.Transaction");

                    b.Property<Guid>("TransferredUserId")
                        .HasColumnType("uuid");

                    b.HasIndex("TransferredUserId");

                    b.ToTable("TransferTransaction");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transaction", b =>
                {
                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Currency", "Currency")
                        .WithMany("Transactions")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.TransactionDetail", b =>
                {
                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Transaction", "Transaction")
                        .WithMany("TransactionDetails")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.BankingTransaction", b =>
                {
                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Bank", "Bank")
                        .WithMany("BankingTransactions")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Transaction", null)
                        .WithOne()
                        .HasForeignKey("Onefocus.Wallet.Domain.Entities.Write.Transactions.BankingTransaction", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.ExchangeTransaction", b =>
                {
                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Currency", "ExchangedCurrency")
                        .WithMany("ExchangeTransactions")
                        .HasForeignKey("ExchangedCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Transaction", null)
                        .WithOne()
                        .HasForeignKey("Onefocus.Wallet.Domain.Entities.Write.Transactions.ExchangeTransaction", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExchangedCurrency");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.IncomeTransaction", b =>
                {
                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Transaction", null)
                        .WithOne()
                        .HasForeignKey("Onefocus.Wallet.Domain.Entities.Write.Transactions.IncomeTransaction", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.OutcomeTransaction", b =>
                {
                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Transaction", null)
                        .WithOne()
                        .HasForeignKey("Onefocus.Wallet.Domain.Entities.Write.Transactions.OutcomeTransaction", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transactions.TransferTransaction", b =>
                {
                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.Transaction", null)
                        .WithOne()
                        .HasForeignKey("Onefocus.Wallet.Domain.Entities.Write.Transactions.TransferTransaction", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Onefocus.Wallet.Domain.Entities.Write.User", "TransferredUser")
                        .WithMany("TransferTransactions")
                        .HasForeignKey("TransferredUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TransferredUser");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Bank", b =>
                {
                    b.Navigation("BankingTransactions");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Currency", b =>
                {
                    b.Navigation("ExchangeTransactions");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.Transaction", b =>
                {
                    b.Navigation("TransactionDetails");
                });

            modelBuilder.Entity("Onefocus.Wallet.Domain.Entities.Write.User", b =>
                {
                    b.Navigation("Transactions");

                    b.Navigation("TransferTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
