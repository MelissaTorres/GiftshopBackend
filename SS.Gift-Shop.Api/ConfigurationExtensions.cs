using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SS.GiftShop.Core;

namespace SS.GiftShop.Api
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigureAndValidate<TOptions>(this IServiceCollection services, IConfiguration config)
            where TOptions : class, ISettings
        => services.ConfigureAndValidate<TOptions>(Options.DefaultName, config);

        public static IServiceCollection ConfigureAndValidate<TOptions>(this IServiceCollection services, string name, IConfiguration config)
            where TOptions : class, ISettings
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.AddOptions();
            services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, config));
            return services.AddSingleton<IConfigureOptions<TOptions>>(new ValidateNamedConfigureFromConfigurationOptions<TOptions>(name, config));
        }

        private class ValidateNamedConfigureFromConfigurationOptions<TOptions> : ConfigureNamedOptions<TOptions>
            where TOptions : class, ISettings
        {
            public ValidateNamedConfigureFromConfigurationOptions(string name, IConfiguration config)
                : base(name, options => BindAndValidate(config, options))
            {
            }

            private static void BindAndValidate(IConfiguration configuration, TOptions options)
            {
                configuration.Bind(options);
                options.Validate();
            }
        }
    }
}
