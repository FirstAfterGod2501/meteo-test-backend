namespace MeteoService.API.API.Extensions.Vault;

/// <summary>
/// Provides configuration values from Vault.
/// </summary>
public class VaultConfigurationProvider : ConfigurationProvider
{
    private readonly VaultOptions _config;

    public VaultConfigurationProvider(VaultOptions config)
    {
        _config = config;
    }

    public override void Load()
    {
        LoadAsync().GetAwaiter().GetResult();
    }

    private async Task LoadAsync()
    {
        await GetDatabaseCredentials();
    }

    private async Task GetDatabaseCredentials()
    {
        Data.Add("POSTGRES_USER", "user");
        Data.Add("POSTGRES_PASSWORD", "pass");
        Data.Add("POSTGRES_DATABASE", "mydatabase");
    }
}

/// <summary>
/// Represents the source of configuration data coming from Vault.
/// </summary>
public class VaultConfigurationSource : IConfigurationSource
{
    private readonly VaultOptions _config;

    public VaultConfigurationSource(Action<VaultOptions> configure)
    {
        _config = new VaultOptions();
        configure(_config);
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new VaultConfigurationProvider(_config);
    }
}

/// <summary>
/// Holds the options necessary for configuring the Vault integration.
/// </summary>
public class VaultOptions
{
    public string? Address { get; set; }
    public string? Role { get; set; }
    public string? Secret { get; set; }
    public string? MountPath { get; set; }
    public string? SecretType { get; set; }
}

/// <summary>
/// Extension methods for adding Vault configuration capabilities to an IConfigurationBuilder.
/// </summary>
public static class VaultExtensions
{
    public static IConfigurationBuilder AddVault(this IConfigurationBuilder configurationBuilder,
        Action<VaultOptions> configureOptions)
    {
        var source = new VaultConfigurationSource(configureOptions);
        configurationBuilder.Add(source);
        return configurationBuilder;
    }
}
