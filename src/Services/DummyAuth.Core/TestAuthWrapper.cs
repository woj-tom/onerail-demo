namespace DummyAuth.Core;

public static class TestAuthWrapper
{
    private const string issuer = "Dummy issuer";
    private const string audience = "Misconfigured";
    private const string key = "RCGAPSZ8LKHXOPJ9JNZ8A2ST7H3X0QUI";

    public static string GetToken()
    {
        var btIssuer = new BadTokenIssuer();
        return btIssuer.GetToken(key, issuer, audience);
    }
}