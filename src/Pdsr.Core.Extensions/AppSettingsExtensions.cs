using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Pdsr.Core.Extensions
{
    public static class AppSettingsExtensions
    {
        private const string _appSettingsFileName = "appsettings";
        private const string _appSettingsFileExtension = "json";

        /// <summary>
        /// Adds an application settings config section
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddAppSettingsConfigs(this IConfigurationBuilder builder, IHostEnvironment environment)
        {
            builder.AddJsonFile($"{_appSettingsFileName}.{_appSettingsFileExtension}", optional: false, reloadOnChange: true);
            builder.AddJsonFile($"{_appSettingsFileName}.{environment.EnvironmentName}.{_appSettingsFileExtension}", optional: true, reloadOnChange: true);
            return builder;
        }

        /// <summary>
        /// Adds a Config section file
        /// </summary>
        /// <typeparam name="TConfigSection"></typeparam>
        /// <param name="builder"></param>
        /// <param name="section"></param>
        /// <param name="environment"></param>
        /// <param name="reloadOnChange"></param>
        /// <param name="optional"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If section is null</exception>
        public static IConfigurationBuilder AddConfigSection<TConfigSection>(this IConfigurationBuilder builder, TConfigSection section, IHostEnvironment environment, bool reloadOnChange = true, bool optional = true)
            where TConfigSection : notnull, Enum
        {
            if (section is null)
            {
                throw new NullReferenceException(nameof(section));
            }

            string? sectionName = Enum.GetName(typeof(TConfigSection), section);
            if (sectionName is not null)
            {
                builder.AddConfigSection(sectionName, environment, reloadOnChange, optional);
            }
            return builder;
        }

        public static IConfigurationBuilder AddConfigSection(this IConfigurationBuilder builder, string section, IHostEnvironment environment, bool reloadOnChange = true, bool optional = true)
        {
            // without environment name (only section name)
            // ex: appsetings.Serilog.json
            builder.AddJsonFile($"{_appSettingsFileName}.{section}.{_appSettingsFileExtension}", optional: optional, reloadOnChange: reloadOnChange);

            // with environment name
            // ex: appsetings.Serilog.Development.json
            builder.AddJsonFile($"{_appSettingsFileName}.{section}.{environment.EnvironmentName}.{_appSettingsFileExtension}", optional: optional, reloadOnChange: reloadOnChange);

            return builder;
        }

        /// <summary>
        /// Adds all config sections available in <see cref="ConfigSectionDefault"/>
        /// </summary>
        /// <param name="builder">reference to <see cref="IConfigurationBuilder"/></param>
        /// <param name="environment">reference to <see cref="IHostEnvironment"/></param>
        /// <returns></returns>
        public static IConfigurationBuilder AddAllConfigSections<TConfigSection>(this IConfigurationBuilder builder, IHostEnvironment environment)
            where TConfigSection : notnull, Enum
        {
            var itemsInSections = typeof(TConfigSection).GetEnumValues();
            foreach (var section in itemsInSections)
            {
                if (section is null)
                {
                    throw new NullReferenceException(nameof(section));
                }

                builder.AddConfigSection((TConfigSection)section, environment);
            }
            return builder;
        }

    }

    /// <summary>
    /// Config sections fot appsettings
    /// </summary>
    public enum ConfigSectionDefault
    {
        Certificate,
        IdentityClient,
        IdentityApiResource,
        IdentityIdResource,
        ClientRateLimiting,
        ClientRateLimitPolicies,
        IpRateLimiting,
        IpRateLimitPolicies,
        Serilog,
        Redis
    }
}
