﻿using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Sample.Notify.WebApi.Core.Extensions;

public static class SwaggerExtensions
{
	public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Title = "Sample Notify",
				Version = "v1"
			});
			c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
		});
	}

	public static void UseSwaggerDoc(this IApplicationBuilder app)
	{
		app.UseSwagger();
		app.UseSwaggerUI(c =>
		{
			c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample Notify");
			c.RoutePrefix = "swagger";
		});
	}
}