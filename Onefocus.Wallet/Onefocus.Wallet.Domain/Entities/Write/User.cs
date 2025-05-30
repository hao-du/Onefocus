﻿using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;
using System;
using System.Collections.Generic;
using static Onefocus.Wallet.Domain.Errors.Transaction;

namespace Onefocus.Wallet.Domain.Entities.Write;

public sealed class User : WriteEntityBase, IAggregateRoot
{
    private readonly List<Transaction> _transactions = [];
    private readonly List<PeerTransfer> _peerTransfers = [];

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<PeerTransfer> PeerTransfers => _peerTransfers.AsReadOnly();

    private User()
    {
        FirstName = default!; 
        LastName = default!; 
        Email = default!;
    }

    private User(Guid? id, string email, string firstName, string lastName, string? description, Guid actionedBy)
    {
        Init(id ?? Guid.NewGuid(), description, actionedBy);

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Result<User> Create(Guid? id, string email, string firstName, string lastName, string? description, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<User>(validationResult.Error);
        }

        return new User(id, email, firstName, lastName, description, actionedBy);
    }

    public Result<User> Update(string email, string firstName, string lastName, string? description, bool isActive, Guid actionedBy)
    {
        var validationResult = Validate(email, firstName, lastName);
        if (validationResult.IsFailure)
        {
            return Result.Failure<User>(validationResult.Error);
        }

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Description = description;

        if (isActive) MarkActive(actionedBy);
        else MarkInactive(actionedBy);

        return this;
    }
    private static Result Validate(string email, string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<User>(Errors.User.EmailRequired);
        }
        if (string.IsNullOrEmpty(firstName))
        {
            return Result.Failure<User>(Errors.User.FirstNameRequired);
        }
        if (string.IsNullOrEmpty(lastName))
        {
            return Result.Failure<User>(Errors.User.LastNameRequired);
        }

        return Result.Success();
    }
}