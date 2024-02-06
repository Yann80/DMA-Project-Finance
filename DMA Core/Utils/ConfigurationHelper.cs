using Microsoft.Extensions.Configuration;

namespace DMA_ProjectManagement.Core.Utils
{
    public static class ConfigurationHelper
    {
        public static string? GetConnectionString(IConfiguration configuration, string configKey)
        {
            return Environment.GetEnvironmentVariable(configKey) ?? configuration.GetConnectionString(configKey);
        }
    }
}