using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SS.GiftShop.Api.Filters;
using SS.GiftShop.Api.Identity;
using SS.GiftShop.Application.Examples;
using SS.GiftShop.Application.Infrastructure;
using SS.GiftShop.Application.Services;
using SS.GiftShop.Core;
using SS.GiftShop.Core.Persistence;
using SS.GiftShop.Infrastructure;
using SS.GiftShop.Persistence;

namespace SS.GiftShop.Api
{
    public class Startup
    {
        private const string AssetsDirectory = "assets";

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            var applicationAssembly = typeof(ExamplesMapping).Assembly;

            AddIdentity(services);
            AddAuthentication(services, Configuration);
            AddCors(services);

            AddMvcCore(services, applicationAssembly);
            AddAppServices(services, applicationAssembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IMapper mapper, ILoggerFactory loggerFactory)
        {
            app.UseHealthChecks("/healthcheck");

            if (Environment.IsDevelopment())
            {
                mapper.ConfigurationProvider.AssertConfigurationIsValid();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseCors(AuthConstants.DefaultCorsPolicy);

            if (Environment.IsDevelopment())
            {
                // Serve local file system files
                //app.UseStaticFiles();
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Environment.ContentRootPath, AssetsDirectory)),
                    RequestPath = new PathString($"/{AssetsDirectory}")
                });
            }

            //app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMvc();
        }

        private void AddAppServices(IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddScoped<IdentityInitializer>();
            var useRowNumberForPaging = Configuration.GetValue<bool>("UseRowNumberForPaging");

            void SqlServerOptionsAction(SqlServerDbContextOptionsBuilder b)
            {
                if (useRowNumberForPaging)
                {
                    b.UseRowNumberForPaging();
                }
            }

            // DbContext's
            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AuthConnection"), SqlServerOptionsAction);
            });
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppConnection"), SqlServerOptionsAction);
            });
            services.AddScoped<AppDbContextInitializer>();

            // Persistence
            services.AddTransient<IRepository, EfRepository<AppDbContext>>();
            services.AddTransient<IReadOnlyRepository, ReadOnlyEfRepository<AppDbContext>>();
            services.AddSingleton<IPaginator, DefaultPaginator>();

            // Infrastructure
            services.AddSingleton<IDateTime>(new SystemDateTime(Configuration["TimeZone"]));
            services.AddAutoMapper(AutoMapperConfig.Configure, applicationAssembly);

            if (Environment.IsDevelopment())
            {
                var root = Path.Join(Environment.ContentRootPath, AssetsDirectory);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                services.AddSingleton<IFileSystem>(new LocalFileSystem(root));
                services.AddSingleton<IUrlService>(new DomainUrlService(Configuration.GetValue<Uri>("AssetsDomain"), AssetsDirectory,
                    Configuration.GetValue<Uri>("ClientDomain")));
                // If SMTP configuration exists for development, use it
                if (!RegisterSmtpSender(services, false))
                {
                    services.AddSingleton<IEmailSender>(new DummyEmailSender(Path.Join(Environment.ContentRootPath, "Logs")));
                }
            }
            else
            {
                RegisterSmtpSender(services, required: true);
                services.AddSingleton<IUrlService>(new DomainUrlService(Configuration.GetValue<Uri>("AssetsDomain"), null,
                    Configuration.GetValue<Uri>("ClientDomain")));
            }

            // Services
            services.AddScoped<IExampleService, ExampleService>();
        }

        private bool RegisterSmtpSender(IServiceCollection services, bool required)
        {
            var smtpSection = Configuration.GetSection("SMTP");
            var children = smtpSection.GetChildren();
            if (!children.Any())
            {
                if (required)
                {
                    throw new ApplicationException("No SMTP configuration found.");
                }

                return false;
            }

            services.ConfigureAndValidate<SmtpSettings>(smtpSection);
            services.AddTransient<IEmailSender, SmtpEmailSender>();
            return true;
        }

        private static void AddMvcCore(IServiceCollection services, Assembly applicationAssembly)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressMapClientErrors = true;
                //options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = true;
            });
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddMvcCore(options =>
            {
                options.Filters.Add(new ExceptionHandlerFilterAttribute());
                options.Filters.Add(new AuthorizeFilter(AuthConstants.DefaultPolicy));
                })
                .AddAuthorization(options =>
                {
                    options.AddPolicy(AuthConstants.DefaultPolicy, builder => builder.RequireAuthenticatedUser());
                    options.AddPolicy(AuthConstants.BearerPolicy, builder => builder.AddAuthenticationSchemes(AuthConstants.BearerScheme)
                        .RequireAuthenticatedUser());
                    options.DefaultPolicy = options.GetPolicy(AuthConstants.DefaultPolicy);
                })
                .AddJsonFormatters(options =>
                {
                    options.NullValueHandling = NullValueHandling.Ignore;
                    options.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    //options.Converters.Add(new EmptyToNullConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(options =>
                {
                    options.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    options.RegisterValidatorsFromAssemblies(new[] { applicationAssembly, typeof(Startup).Assembly });
                });
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddCookie(IdentityConstants.ApplicationScheme, options =>
                {
                    options.Cookie.Name = AuthConstants.CookieName;
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };

                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.CompletedTask;
                    };
                    options.ForwardDefaultSelector = ctx =>
                    {
                        var authHeader = ctx.Request.Headers[HeaderNames.Authorization];
                        if (authHeader.Count > 0)
                        {
                            return AuthConstants.BearerScheme;
                        }
                        return IdentityConstants.ApplicationScheme;
                    };
                })
                .AddScheme<SecretBearerAuthenticationOptions, SecretBearerAuthenticationHandler>(
                    AuthConstants.BearerScheme, "Bearer",
                    options =>
                    {
                        options.Secret = configuration["ApiSecret"];
                        options.ClaimsIssuer = AuthConstants.BearerScheme;
                    })
                .AddCookie(IdentityConstants.ExternalScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.ExternalScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                })
                .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                    o.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
                    };
                })
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });
        }

        private static void AddIdentity(IServiceCollection services)
        {
            services
                .AddIdentityCore<User>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = AuthConstants.MinPasswordLength;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireDigit = true;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddRoles<Role>()
                .AddSignInManager()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
        }

        private void AddCors(IServiceCollection services)
        {
            //var corsConfig = Configuration.GetSection("Cors");
            services.AddCors(options =>
            {
                options.DefaultPolicyName = AuthConstants.DefaultCorsPolicy;
                options.AddPolicy(AuthConstants.DefaultCorsPolicy, builder =>
                    builder.AllowAnyHeader()
                        .WithExposedHeaders(HeaderNames.ContentDisposition)
                        .AllowAnyMethod()
                        .AllowCredentials()
                        //.WithOrigins(corsConfig["Origins"].Split(','))
                        .WithOrigins(Configuration["ClientDomain"])
                );
            });
        }

        private static bool IsClosedType(Type type, Type openGenericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }
    }
}
