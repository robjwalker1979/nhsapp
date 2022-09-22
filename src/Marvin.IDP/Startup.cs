// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Marvin.IDP
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            //services.AddTransient<IProfileService, ProfileService>();

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(TestUsers.Users);

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
            builder.AddProfileService<ProfileService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;

        public ProfileService()
        {
            var x = "";
        }

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            //var user = await _userManager.GetUserAsync(context.Subject);

            var claims = new List<Claim>
            {
                new Claim("webid", "https://id.inrupt.com/robwalker1979")
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            //var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = true; //(user != null) && user.IsActive;
        }
    }
}
