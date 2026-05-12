using Application.Ports.Driven;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class AppSettings : IAppSettings {
    public string FrontendUrl { get; }

    public AppSettings(IConfiguration configuration) {
        FrontendUrl = configuration["AppSettings:FrontendUrl"]
            ?? throw new InvalidOperationException("AppSettings:FrontendUrl no configurado en appsettings.");
    }
}
