using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Exceptions.Domain;
using Onefocus.Common.Results;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Membership.Domain.ValueObjects;

public sealed record PasswordCommandObject(Guid Id, string Password);

