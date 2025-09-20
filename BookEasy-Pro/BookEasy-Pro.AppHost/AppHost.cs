var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak:24.0")
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithEnvironment("KC_DB", "dev-mem")
    .WithEndpoint(name: "http", port: 8080, targetPort: 8080, scheme: "http")
    .WithArgs("start-dev")
    .WithHttpHealthCheck("/health/ready");

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
