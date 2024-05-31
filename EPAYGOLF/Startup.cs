using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace EPAYGOLF
{
	public class Startup
	{

		public Startup()
		{
		}
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllersWithViews(options =>
			{
				options.ModelBinderProviders.Insert(0, new CustomDateTimeModelBinderProvider());
			});

		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
			app.UseRequestLocalization(localizationOptions.Value);


		}

		public class CustomDateTimeModelBinderProvider : IModelBinderProvider
		{
			public IModelBinder GetBinder(ModelBinderProviderContext context)
			{
				if (context == null)
				{
					throw new ArgumentNullException(nameof(context));
				}

				if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
				{
					return new BinderTypeModelBinder(typeof(CustomDateTimeModelBinder));
				}

				return null;
			}
		}
	}
}
