using OliWorkshop.AccountingSys.Data;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupServicesExtensions
    {
        /// <summary>
        /// Add Locations and I18n Services
        /// </summary>
        /// <param name="services"></param>
        public static void AddI18nSupport(this IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
        }

        /// <summary>
        /// Add identity system with necesary services
        /// </summary>
        /// <param name="services"></param>
        public static void AddIdentitySystem(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.SignIn.RequireConfirmedEmail = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>();
        }

        /// <summary>
        /// Add database service to set the storage engine
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddDatabaseService(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddDbContext<ApplicationDbContext>(options => {

                DbContextOptionsBuilder optionsBuilder = null;
                string typeProvider = configuration.GetValue<string>("Database:type");

                if (typeProvider == "sqlserver")
                {
                    optionsBuilder = options.UseSqlServer(
                    configuration.GetConnectionString("SqlDbString"));
                }
                else if (typeProvider == "mysql")
                {
                    optionsBuilder = options.UseMySql(
                    configuration.GetConnectionString("MysqlDbString"));
                }
                else
                {
                    throw new NotSupportedException($"The provider {typeProvider} is not supported");
                }

                optionsBuilder.EnableDetailedErrors();
            });
        }
    }
}
