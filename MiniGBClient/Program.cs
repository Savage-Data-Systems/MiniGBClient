using MiniGBClient.Data;
using MiniGBClient.Extensions;
using MiniGBClient.Services;
using System.Web;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("config.json");
builder.Services.AddDbContext<MiniGBClientDbContext>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<GbClientSubscriptionService>();
// Add services to the container.
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//start a connection to the sandbox
app.MapGet("/connect", (HttpResponse httpReponse, IConfiguration config) =>
{
    var scope = config["GBConnection:Scope"];
    var redirectURI = config["GBConnection:RedirectURI"];
    var clientid = config["client_id"];
    var authorizeURI = config["GBConnection:AuthorizationURI"];

    if (string.IsNullOrWhiteSpace(scope) ||
        string.IsNullOrWhiteSpace(redirectURI) ||
        string.IsNullOrWhiteSpace(clientid) ||
        string.IsNullOrWhiteSpace(authorizeURI))
    {
        return Results.BadRequest("Missing configuration values");
    }

    var uriBuilder = new UriBuilder(authorizeURI);
    var queryParams = HttpUtility.ParseQueryString(string.Empty);
    queryParams["client_id"] = clientid;
    queryParams["redirect_uri"] = "https://localhost:7283/callback"; // TODO: Update this. For now needs to be manually set based on your launch information
    queryParams["response_type"] = "code";
    queryParams["scope"] = scope;
    queryParams["state"] = "optional";
    uriBuilder.Query = queryParams.ToString();

    return Results.Redirect(uriBuilder.Uri.ToString());
});

app.MapPost("/notify", (string notification) =>
{
    Console.WriteLine(notification);
    return Results.Ok();
});

//CMD flow call back uri. We need to handle this to get our access token
app.MapGet("/callback", async (HttpRequest request, TokenService tokenService, GbClientSubscriptionService gbSubscriptionService, IConfiguration config) =>
{
    var code = request.Query["code"];
    var state = request.Query["state"];
    if (string.IsNullOrWhiteSpace(code))
    {
        var error = request.Query["error"];
        return Results.BadRequest($"Code not provided from the Data Custodian.\r\n {error}");
    }

    var tokenResponse = await tokenService.ExchangeCodeForTokenAsync(code!);
    
    if (tokenResponse is null)
        return Results.BadRequest("Unable to retrieve access token. Subscription information will not be stored.");

    var sub = tokenResponse.ToSubscription();
    await gbSubscriptionService.AddSubscriptionAsync(sub);

    var custData = await gbSubscriptionService.DownloadCustomerResourcebySubscription(sub);
    var euData = await gbSubscriptionService.DownloadEUResourceBySubscription(sub);

    var savePath = config["Data Location"] ?? "";

    var fn = $"Subscription{sub.Id}_EU_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xml";
    File.WriteAllText(Path.Combine(savePath,fn),euData);

    fn = $"Subscription{sub.Id}_Customer_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xml";
    File.WriteAllText(Path.Combine(savePath, fn), custData);

    return Results.Ok("GB Authorization Successful");
});

app.Run();
