using Ibr.Core.Dtos.Integration.Asaas.Subscription;
using Ibr.Core.Helpers;
using System.Text.Json;

namespace iBR.Endpoints.Asaas.Subscription;

public class AsaasSubscriptionEndpoint : IAsaasSubscriptionEndpoint
{
    private readonly HttpClient _http;

    public AsaasSubscriptionEndpoint(HttpClient http)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
    }

    public async Task<AsaasMonitorSubscriptionDto?> GetById(string subscriptionId, string baseUrl, string accessToken)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.BaseAddress = new Uri(baseUrl);
        _http.DefaultRequestHeaders.Add("access_token", accessToken);
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("Desmonte/1.0");

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"subscriptions/{subscriptionId}");

        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion

        try
        {
            AsaasMonitorSubscriptionDto? entities =
                await JsonSerializer.DeserializeAsync<AsaasMonitorSubscriptionDto?>(contentStream, options);

            return entities;
        }
        catch (Exception e)
        {

            throw new Exception(e.Message);
        }

    }

    public async Task<IReadOnlyList<AsaasMonitorSubscriptionDto>> GetAllSubscriptions(
      string baseUrl,
      string accessToken)
    {
      _http.DefaultRequestHeaders.Clear();
      _http.BaseAddress = new Uri(baseUrl);
      _http.DefaultRequestHeaders.Add("access_token", accessToken);
      _http.DefaultRequestHeaders.UserAgent.ParseAdd("Desmonte/1.0");

      const int limit = 100;
      int offset = 0;

      var allSubscriptions = new List<AsaasMonitorSubscriptionDto>();

      var options = new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      };

      while (true)
      {
        var url = $"subscriptions?limit={limit}&offset={offset}";

        using HttpResponseMessage response = await _http.GetAsync(url);
        await using Stream contentStream = await response.Content.ReadAsStreamAsync();
        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        var page = await JsonSerializer.DeserializeAsync<AsaasSubscriptionPageDto>(contentStream, options);

        if (page == null || page.Data == null || page.Data.Count == 0)
          break;

        allSubscriptions.AddRange(page.Data);

        if (!page.HasMore)
          break;

        offset += limit;
      }

      return allSubscriptions;
    }
}
