using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Management.Data;
using Management.Services;
using Microsoft.EntityFrameworkCore;

namespace Management
{
	public class Startup
	{

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// Services.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<OBDContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddSingleton<ChartService>();
			services.AddScoped<LogEntryService>();
			services.AddScoped<CarService>();
			services.AddScoped<CarBrandService>();
			services.AddScoped<CarModelService>();
			services.AddScoped<PersonService>();
			services.AddScoped<SessionService>();
			services.AddScoped<ConfigurationService>();
		}

		// HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});

		}

	}
}
