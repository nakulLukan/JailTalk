﻿using FluentValidation.Results;

namespace JailTalk.Shared.Models;
public class FormError : Dictionary<string, string[]>
{
    public FormError(IEnumerable<ValidationFailure> validationFailures)
    {
        Clear();
        var errors = validationFailures.GroupBy(x => x.PropertyName).ToArray();
        foreach (var error in errors)
        {
            Add(error.Key, error.Select(x => x.ErrorMessage).ToArray());
        }
    }
}