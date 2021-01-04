using API.Authentication;
using API.Data;
using API.Identity;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            var apiOrigin = Configuration["PublicOrigin:PublicOrigin"];
            var coneection = Configuration["connectionString:DefaultConn"];

            services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseSqlite(coneection);
            });

            IdentityBuilder builder = services.AddIdentityCore<UserIdentity>(opts =>
            {
                opts.Password.RequireDigit = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequiredLength = 6;

                

            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services); //instancia das configurações
            builder.AddEntityFrameworkStores<DatabaseContext>();
            builder.AddRoleValidator<RoleValidator<Role>>(); //setando o validator
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<UserIdentity>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)        //Configurando JWT
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, //validando pela assinatura da chave do emissor, neste caso o emissor da chave é a prórpia API
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("appSettings:Token").Value)), //configuração da chave
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
            });

          
            services.AddControllers(opts => //toda vez que chamar uma determinada CONTROLLER estabelecerá uma politica
            {
                var policy = new AuthorizationPolicyBuilder() //toda vez que chamar uma controller terá que respeitar a configuração setada neste código
                .RequireAuthenticatedUser()
                .Build();
                opts.Filters.Add(new AuthorizeFilter(policy)); //Politica: toda vez que alguem chamar uam rota, usuário terá que estar authenticado
            });

            services.AddScoped<IRepository, Repository>();
            services.AddAutoMapper(typeof(Startup));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

             
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
      


            app.UseAuthorization();
            app.UseAuthentication(); //

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
