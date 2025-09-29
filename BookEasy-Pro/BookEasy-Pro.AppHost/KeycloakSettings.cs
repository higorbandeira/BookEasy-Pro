namespace BookEasy_Pro.AppHost;

public class KeycloakSettings
{
    public string Admin { get; set; } = "admin";
    public string AdminPassword { get; set; } = "admin";
    public string Database { get; set; } = "dev-mem";
    public string Image { get; set; } = "quay.io/keycloak/keycloak:24.0";
    public int Port { get; set; } = 8080;
    public string Args { get; set; } = "start-dev";
    public string HealthCheckPath { get; set; } = "/health/ready";
}
