using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EPAYGOLF
{
	
		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
		public class ConditionalAuthorizeAttribute : Attribute, IAuthorizationFilter
		{
			private readonly bool _requireAuthorization;

			public ConditionalAuthorizeAttribute(bool requireAuthorization)
			{
				_requireAuthorization = requireAuthorization;
			}

			public void OnAuthorization(AuthorizationFilterContext context)
			{
				var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
				bool enableAuthorization = config.GetValue<bool>("AuthorizationSettings:EnableAuthorization");

				if (_requireAuthorization && enableAuthorization)
				{
					var authorizeFilter = new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter();
					authorizeFilter.OnAuthorizationAsync(context);
					var user = context.HttpContext.User;
					if (!user.Identity.IsAuthenticated)
					{
						context.Result = new RedirectToActionResult("Login", "Account", null);
					}
            }

			}
		}
	
}
