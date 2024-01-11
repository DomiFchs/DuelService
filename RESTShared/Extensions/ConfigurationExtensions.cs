using DtoLibrary.Enums;
using Microsoft.Extensions.Configuration;

namespace DtoLibrary.Extensions; 

public static class ConfigurationExtensions {
    public static Uri BuildUri(this IConfiguration configuration, EService service) {
        var serviceName = service.ToEnumString();
        return new Uri($"https://{configuration[$"{serviceName}Host"]}:{configuration[$"{serviceName}Port"]}/{configuration[$"{serviceName}APIName"]}");
    }
}