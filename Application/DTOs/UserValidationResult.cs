using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs;

public sealed class UserValidationResult
{
    public UserValidationResult(User user)
    {
        User = user;
        ValidateUser();
        HasErrors = ValidationResults.Any();
    }

    public bool HasErrors { get; }

    public User User { get; }

    public List<ValidationResult> ValidationResults { get; private set; } = new();

    public bool HasError<T>(System.Linq.Expressions.Expression<Func<User, T>> propertySelector)
    {
        if (propertySelector.Body is not System.Linq.Expressions.MemberExpression memberExpr)
        {
            throw new ArgumentException("Invalid property selector expression", nameof(propertySelector));
        }

        var propertyName = memberExpr.Member.Name;
        return ValidationResults.Any(e => e.MemberNames.Contains(propertyName));
    }

    private void ValidateUser()
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(User);
        Validator.TryValidateObject(User, context, validationResults, true);
        ValidationResults = validationResults;
    }
}