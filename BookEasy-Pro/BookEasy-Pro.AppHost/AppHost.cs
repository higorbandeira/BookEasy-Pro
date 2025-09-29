using BookEasy_Pro.AppHost;
using Microsoft.Extensions.Configuration; // Necessário para Get<T>()

var builder = DistributedApplication.CreateBuilder(args);

// Carrega configurações do Keycloak
var keycloakSettings = builder.Configuration.GetSection("Keycloak").Get<KeycloakSettings>() ?? new KeycloakSettings();

var cache = builder.AddRedis("cache");

var keycloak = builder.AddContainer("keycloak", keycloakSettings.Image)
    .WithEnvironment("KEYCLOAK_ADMIN", keycloakSettings.Admin)
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", keycloakSettings.AdminPassword)
    .WithEnvironment("KC_DB", "dev-file") // Use dev-file para persistência local simples
    .WithVolume("keycloak-data", "/opt/keycloak/data") // Volume persistente para dados
    .WithVolume("keycloak-import", "/opt/keycloak/data/import") // Volume para importação/exportação
    .WithEndpoint(name: "http", port: keycloakSettings.Port, targetPort: keycloakSettings.Port, scheme: "http")
    .WithArgs("start-dev")
    .WithHttpHealthCheck(keycloakSettings.HealthCheckPath);

//var keycloak = builder.AddKeycloakContainer("keycloak", tag: "25.0.1")
//    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
//    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
//    .WithEnvironment("KC_DB", "dev-file")
//    .WithEnvironment("KC_HEALTH_ENABLED", "true")
//    .WithDataVolume("bookeasy-keycloak-data")
//    .WithImportVolume("bookeasy-keycloak-import")
//    .WithArgs("start-dev", "--import-realm");

var apiService = builder.AddProject<Projects.BookEasy_Pro_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.BookEasy_Pro_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
