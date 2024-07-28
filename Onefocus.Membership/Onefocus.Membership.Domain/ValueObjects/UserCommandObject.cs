using Microsoft.AspNetCore.Identity;
using Onefocus.Common.Exceptions.Domain;
using Onefocus.Common.Results;
using System.ComponentModel.DataAnnotations;

namespace Onefocus.Membership.Domain.ValueObjects;

public sealed record UserCommandObject(string Email, string FirstName, string LastName);

