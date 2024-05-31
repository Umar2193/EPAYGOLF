using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class CustomDateTimeModelBinder : IModelBinder
{
	private readonly string _customFormat = "dd/MM/yyyy";

	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		if (bindingContext == null)
		{
			throw new ArgumentNullException(nameof(bindingContext));
		}

		var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
		if (valueProviderResult == ValueProviderResult.None)
		{
			return Task.CompletedTask;
		}

		bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

		var value = valueProviderResult.FirstValue;
		if (string.IsNullOrEmpty(value))
		{
			return Task.CompletedTask;
		}

		DateTime date;
		if (DateTime.TryParseExact(value, _customFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
		{
			bindingContext.Result = ModelBindingResult.Success(date);
		}
		else
		{
			bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid date format. Please use dd/MM/yyyy.");
		}

		return Task.CompletedTask;
	}
}
