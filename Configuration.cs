namespace Blog;

public static class Configuration
{
    // TOKEN - JWT
    public static string JwtKey = "BFNYudRaIHZNsXbrjAFgQu0sjs5U5VVcVgozp4ORHyk";
    
    // API Key
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "curso_api_IlTevUm/Zosdw3NwC/uqw==";

    //Smtp
    public static SmtpConfiguration Smtp = new();
    //Envio de email
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
