using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;

public class ViewRenderingService
{
	private readonly IRazorViewEngine _viewEngine;
	private readonly ITempDataProvider _tempDataProvider;
	private readonly IServiceProvider _serviceProvider;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ViewRenderingService(
		IRazorViewEngine viewEngine,
		ITempDataProvider tempDataProvider,
		IServiceProvider serviceProvider,
		IHttpContextAccessor httpContextAccessor)
	{
		_viewEngine = viewEngine;
		_tempDataProvider = tempDataProvider;
		_serviceProvider = serviceProvider;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<string> RenderToStringAsync(string viewName, object model)
	{
		var httpContext = _httpContextAccessor.HttpContext;

		if (httpContext == null)
		{
			throw new ArgumentNullException(nameof(httpContext), "HTTP context cannot be null.");
		}

		var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), new ControllerActionDescriptor());

		using (var sw = new StringWriter())
		{
			var viewResult = _viewEngine.FindView(actionContext, viewName, false);

			if (!viewResult.Success)
			{
				throw new InvalidOperationException($"View '{viewName}' not found.");
			}

			var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
			{
				Model = model
			};

			var tempData = new TempDataDictionary(httpContext, _tempDataProvider);

			var viewContext = new ViewContext(
				actionContext,
				viewResult.View,
				viewDictionary,
				tempData,
				sw,
				new HtmlHelperOptions()
			);

			await viewResult.View.RenderAsync(viewContext);
			return sw.ToString();
		}
	}
}
