
using System.Net;
using System.Text;
using BitPastry.Backend.Core.Interfaces;
using BitPastry.Backend.Core.Services;
using BitPastry.Backend.Data;
using BitPastry.Backend.DTO.Configurations;
using BitPastry.Backend.DTO.Exceptions;
using BitPastry.Backend.Presentation.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;

namespace BitPastry.Backend.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        // Config
        var cb = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Logger
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(cb)
            .CreateLogger();
        try
        {
            Log.Information("Application startup");
            CreateHostBuilder(args).Build().Run();
        } catch(Exception ex)
        {
            Log.Error(ex, "Application startup error");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(builder => {
                builder.UseStartup<Startup>();
            });

    /* ----------------------------------------------------------------------------------------- */
    // Startup dell'Host Web
    /* ----------------------------------------------------------------------------------------- */
    public class Startup
    {
        private const string MY_CORS = "AllowALL";

        private IConfiguration _Configuration { get; }
        public Startup(IConfiguration configuration) => this._Configuration = configuration;

        /* ------------------------------------------------------------------------------------- */
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Injection Config
            Configuration conf = new Configuration();
            this._Configuration.GetSection("AppConfiguration").Bind(conf);

            services.AddSingleton(conf);

            // Injection DB + Services
            services.AddDbContext<BitPastryDB>(options => options.UseMySql(_Configuration.GetConnectionString("DefaultConnection"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.27-mariadb"), mysqlOptions => mysqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))) ;
            services.AddScoped<BaseService>(x => new BaseService(x.GetService<BitPastryDB>(), x.GetService<Configuration>(), x.GetService<IAuthenticationProvider>()));

            // injection JWT
            JWTConfiguration jwtConf = new JWTConfiguration();
            this._Configuration.GetSection("JWTConfiguration").Bind(jwtConf);

            services.AddSingleton(jwtConf);
            services.AddSingleton<TokensProvider>();

            // Auth con JWT
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConf.Secret)),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Cors - Allow ALL
            services.AddCors(options =>
                options.AddPolicy(MY_CORS, builder =>
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                )
            );

            // Injection Authentication Provider
            services.AddSingleton<IHttpContextAccessor, AuthenticationProvider>();
            services.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();

            services.AddControllers().AddNewtonsoftJson(); // Utilizzo di NewtosoftJson nelle Requeste/Response HTTP
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        /* ------------------------------------------------------------------------------------- */
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Logger
            app.UseSerilogRequestLogging();

            // Cors - Allow ALL
            app.UseCors(MY_CORS);

            // Auth
            app.UseAuthentication();

            // Expection Handler
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    //Header
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    // Body
                    var contextFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (contextFeature != null && contextFeature.Error != null)
                    {
                        // Creo il body della Response
                        var ex = contextFeature.Error;
                        var problem = new ProblemDetails
                        {
                            Type = ex.GetType().Name,
                            Instance = contextFeature.Path,
                        };

                        if (contextFeature.Error is ManagedException myEx)
                        { // Exception generata da Me
                            // Recupero lo status code dalla Mia Exception
                            context.Response.StatusCode = (int)myEx.StatusCode;

                            problem.Status = (int)myEx.StatusCode;
                            problem.Title = myEx.Title;
                            problem.Detail = myEx.Detail;
                            foreach (KeyValuePair<string, object?> entry in myEx.Extensions)
                                problem.Extensions.Add(entry.Key, entry.Value);
                        }
                        else
                        { // Exception generata da C#
                            problem.Status = (int)HttpStatusCode.InternalServerError;
                            problem.Title = "An error occurred.";
                            problem.Detail = env.IsDevelopment() ? ex.StackTrace : null;
                        }

                        // Response
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(problem));
                    }
                });
            });

            // Gestione del Routing
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

