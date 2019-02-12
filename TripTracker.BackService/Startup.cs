﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using TripTracker.BackService.Data;
using TripTracker.BackService.Models;

namespace TripTracker.BackService
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//services.AddTransient<Models.Repository>();
			services.AddMvc();

      services.AddDbContext<TripContext>(options=> options.UseSqlite("Data Source=PlannedTrips.db"));

			services.AddSwaggerGen(options =>
					options.SwaggerDoc("v1", new Info { Title = "Trip Tracker", Version = "v1" })
			);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseSwagger();

			if (env.IsDevelopment() || env.IsStaging())
			{

				app.UseSwaggerUI(options =>
						options.SwaggerEndpoint("/swagger/v1/swagger.json", "Trip Tracker v1")
				);

			}
			
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseExceptionHandler(errorApp =>
			{
				errorApp.Run(async context =>
				{
					var errorFeatures = context.Features.Get<IExceptionHandlerFeature>();
					var exception = errorFeatures.Error;

					var details = exception.ToString();

					var problemDetails = new ProblemDetails
					{
						Title = "An unexpected error occured!",
						Status = 500,
						Detail = details,
						Instance = $"urn:myorganization:error:{Guid.NewGuid()}"
					};
					context.Response.StatusCode = problemDetails.Status.Value;
					context.Response.WriteJson(problemDetails, "application/problem+json");
				});
			});
			
			
			
			app.UseMvc();
			

			TripContext.SeedData(app.ApplicationServices);
		}
	}
}
