using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Ofeck.Bartify.Core.Sendbird;

namespace Ofeck.Bartify.Core.Integrations.Sendbird;

public class SendbirdService : ISendbirdRepository
{
    private readonly HttpClient httpClient;
    private readonly string appId;

    public SendbirdService(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.appId = configuration["Sendbird:AppId"]!;
        this.httpClient.DefaultRequestHeaders.Add("Api-Token", configuration["Sendbird:MasterToken"]!);
    }

    public async Task CreateUser(Guid id, string nombre)
    {
        var body = JsonSerializer.Serialize(new
        {
            user_id = id.ToString(),
            nickname = nombre,
            profile_url = ""
        });

        var content = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await this.httpClient.PostAsync(
            $"https://api-{appId}.sendbird.com/v3/users",
            content
        );

        response.EnsureSuccessStatusCode();
    }

    public async Task<string> CreateChannel(Guid Comprador, Guid Vendedor, Guid Articulo, string NombreArticulo)
    {
        var body = JsonSerializer.Serialize(new
        {
            channel_url = $"bartify_{Guid.NewGuid()}",
            name = $"Chat sobre artículo {NombreArticulo}",
            user_ids = new[] { Comprador.ToString(), Vendedor.ToString() },
            is_distinct = false
        });

        var content = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await this.httpClient.PostAsync(
            $"https://api-{appId}.sendbird.com/v3/group_channels",
            content
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(result);
        return json.RootElement.GetProperty("channel_url").GetString()!;
    }
}