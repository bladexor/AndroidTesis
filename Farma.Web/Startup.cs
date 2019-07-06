using Farma.Web.Data;
using Farma.Web.Data.Entities;
using Farma.Web.Data.Repositories;
using Farma.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Farma.Web
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
            bool confirmEmail = bool.Parse(Configuration["SignIn:RequireConfirmedEmail"]);
            //Configuracion de Usuarios de IdentityFramework
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                //Para Requerir Confirmación de Email Al Registrarse un nuevo usuario---------------------
                    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                    cfg.SignIn.RequireConfirmedEmail = confirmEmail; 
                //----------------------------------------------------------------------------------------
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 6;
            })
              .AddDefaultTokenProviders() //Para Tokens de Confirmación de Email al Registrar Usuario
              .AddEntityFrameworkStores<DataContext>();

            //Para la Generación y Autenticacion con Tokens de seguridad para el API
            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = this.Configuration["Tokens:Issuer"],
                        ValidAudience = this.Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Tokens:Key"]))
                    };
                });

            //Servicio de Conexion a SQL Server
            services.AddDbContext<DataContext>(cfg =>
                {
                    //cfg.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
                    cfg.UseSqlite(this.Configuration.GetConnectionString("SqliteDefault"));
                });

                //SERVICIO DEL Seeder de BD
                services.AddTransient<SeedDb>();

            //INYECCION DEL REPOSITORIO INTERMEDIO PARA CONEXION A BD
            //IMPORTANTE PARA CREAR PRUEBAS UNITARIAS CON DATOS FALSOS SIN BD
                services.AddScoped<IStateRepository, StateRepository>();
                services.AddScoped<ICityRepository, CityRepository>();

                services.AddScoped<IUserHelper, UserHelper>();
                services.AddScoped<IMailHelper, MailHelper>();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Para controlar Redireccion de vistas NoAuthorized por seguridad
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}"); //Para Manejar Errores 404 Not Found
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication(); ///Para Usar Usuarios identityFramework
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
