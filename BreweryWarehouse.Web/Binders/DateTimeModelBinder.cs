using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace BreweryWarehouse.Web.Binders;

public class DateTimeModelBinder : IModelBinder
{
    private static readonly string[] Formats = ["yyyy-MM-dd", "d.M.yyyy", "dd.MM.yyyy", "M/d/yyyy", "MM/dd/yyyy"];

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

        var value = valueResult.FirstValue;
        if (string.IsNullOrWhiteSpace(value))
        {
            if (bindingContext.ModelType == typeof(DateTime?))
                bindingContext.Result = ModelBindingResult.Success((DateTime?)null);
            return Task.CompletedTask;
        }

        foreach (var fmt in Formats)
        {
            if (DateTime.TryParseExact(value, fmt, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                var result = bindingContext.ModelType == typeof(DateTime?)
                    ? (object)(DateTime?)parsed
                    : (object)parsed;
                bindingContext.Result = ModelBindingResult.Success(result);
                return Task.CompletedTask;
            }
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fallback))
        {
            var result = bindingContext.ModelType == typeof(DateTime?)
                ? (object)(DateTime?)fallback
                : (object)fallback;
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }

        bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"Invalid date: '{value}'.");
        return Task.CompletedTask;
    }
}

public class DateTimeModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
            return new DateTimeModelBinder();
        return null;
    }
}
