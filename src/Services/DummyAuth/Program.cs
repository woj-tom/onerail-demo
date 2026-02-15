using DummyAuth.Core;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();
 
// It doesn't make any sense
// This should be managed by IdP with asymmetric key
app.MapGet("/token", (IConfiguration config) =>
{
    var issuer = new BadTokenIssuer();
    var at = issuer.GetToken(
        config["DummyJwt:Key"] ?? throw new NullReferenceException(),
        config["DummyJwt:Issuer"] ?? throw new NullReferenceException(),
        config["DummyJwt:Audience"] ?? throw new NullReferenceException());
    return Results.Ok(new AccessTokenDto { AccessToken = at });
});

app.Run();
