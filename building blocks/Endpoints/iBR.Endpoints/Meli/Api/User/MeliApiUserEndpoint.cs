using System.Text.Json;
using Ibr.Core.Helpers;

namespace iBR.Endpoints.Meli.Api.User;

public class MeliApiUserEndpoint : IMeliApiUserEndpoint
{
    private readonly HttpClient _http;
    private const string BaseUrl = "users";

    public MeliApiUserEndpoint(HttpClient httpClient)
    {
        _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<string?> GetAccessTokenByCompany(string userToken, int companyId)
    {
        _http.DefaultRequestHeaders.Add("secretKey", "739jp#osJB$%#WRxiqC&N5Mk^p@EuQ4cuoWdwJCaF5TtMckpwKwvxiqja4D7Xj8!Wk!jvT^3eXN!%smS2H^iCk#bFR!KYR9LJUvo57yKHG*8f!");
        _http.DefaultRequestHeaders.Add("companyId", companyId.ToString());
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {userToken}");

        # region Realizando request

        HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/accessToken");

        if (response == null || response.Content == null)
        {
            throw new Exception("Resposta da API foi nula.");
        }

        Stream contentStream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        string token = await response.Content.ReadAsStringAsync();

        // Verificação explícita de corpo vazio
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        await HttpHelper.HandleResponseStatus(contentStream, options, response);

        #endregion
        if (contentStream.Length == 0)
        {
            throw new Exception("Ocorreu um erro na busca do seu token do mercado livre.");
        }

        return token;
    }
}
