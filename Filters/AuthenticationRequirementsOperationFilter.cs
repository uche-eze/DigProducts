using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.Filters
{
	public class AuthenticationRequirementsOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation.Security == null)
				operation.Security = new List<OpenApiSecurityRequirement>();


			var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } };
			operation.Security.Add(new OpenApiSecurityRequirement
			{
				[scheme] = new List<string>()
			});
		}
	}
}
