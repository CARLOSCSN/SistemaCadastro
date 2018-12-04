using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaCadastro.Models.DB;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using FluentValidation.AspNetCore;
using HtmlTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaCadastro.Infrastructure.Tags;
using SistemaCadastro.Infrastructure;
using SistemaCadastro.Interfaces;

// Funções de ajuda
using static SistemaCadastro.Pages.PageModelExtensions;

namespace SistemaCadastro
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
            // [Asma Khalid]: Authorization settings.
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Index");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
            });

            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddMediatR(typeof(Startup));

            services.AddHtmlTags(new TagConventions());

            services.AddScoped<AspNetAcessContextHTTP>(); // Acessar e trabalhar com objeto http
            services.AddScoped<AspNetDataTempMessageInSession>(); // Serviço para criar mensagens de alert usando a session
            services.AddScoped<AddItemInSessionJson>(); // Serviço para gravar na session qualquer obj generico
            services.AddScoped<GetItemInSessionJson>(); // Serviço para pegar na session qualquer obj generico pelo nome
            services.AddScoped<IUser, AspNetUser>(); // Pegar usuario da session

            services.AddMvc(opt =>
            {
                //opt.Filters.Add(typeof(DbContextTransactionPageFilter));
                opt.Filters.Add(typeof(ValidatorPageFilter));
                //opt.ModelBinderProviders.Insert(0, new EntityModelBinderProvider());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            // [Asma Khalid]: Authorization settings.
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/");
                options.Conventions.AllowAnonymousToPage("/Index");
            });

            // [Asma Khalid]: Register SQL database configuration context as services.  
            services.AddDbContext<db_coreloginContext>(options => options.UseSqlServer(Configuration.GetConnectionString("db_corelogin")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            // [Asma Khalid]: Register simple authorization.
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
        }
    }
}
