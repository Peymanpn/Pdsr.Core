using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pdsr.Core.Extensions;

public static class StartupConfigExtensions
{
    /// <summary>
    /// Gets a section of <see cref="IConfiguration"/> and binds it to the <typeparamref name="TConfig"/>
    /// and adds it to the DI container as Singleton.
    /// </summary>
    /// <typeparam name="TConfig">The type of configuration section to instantiate</typeparam>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to</param>
    /// <param name="configuration">The <see cref="IConfigurationSection"/> to instatiate <typeparamref name="TConfig"/> from.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration)
      where TConfig : class, new()
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        // create instance of config
        TConfig config = new TConfig();

        // bind it to the appropriate section of configuration
        configuration.Bind(config);

        // and register it as a service
        services.AddSingleton(config);

        return config;
    }

    /// <summary>
    /// Gets a section of <see cref="IConfiguration"/> and binds it to the <typeparamref name="ITConfig"/>
    /// and adds it to the DI container as Singleton.
    /// It is same as <see cref="ConfigureStartupConfig{TConfig}(IServiceCollection, IConfiguration)"/> but instead of <typeparamref name="TConfig"/>,
    /// it registers it in DI container by the interface <typeparamref name="ITConfig"/>.
    /// </summary>
    /// <typeparam name="ITConfig">The interface of <typeparamref name="TConfig"/> to regsiter in DI container</typeparam>
    /// <typeparam name="TConfig">The concerete class of <typeparamref name="ITConfig"/> interface.</typeparam>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to</param>
    /// <param name="configuration">The <see cref="IConfigurationSection"/> to instatiate <typeparamref name="TConfig"/> from.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    public static ITConfig ConfigureStartupConfig<ITConfig, TConfig>(this IServiceCollection services, IConfiguration configuration)
        where ITConfig : class
        where TConfig : ITConfig, new()
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        ITConfig config = new TConfig();

        configuration.Bind(config);

        services.AddSingleton(config);

        return config;
    }
}
