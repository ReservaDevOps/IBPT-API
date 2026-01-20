using Ibr.Core.Dtos.Integration.Asaas.Subscription;
using Ibr.Core.Helpers;
using Ibr.Core.Models.Asaas.Subscription;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Subscription;

public class MonitorAsaasSubscriptionEndpoint : IMonitorAsaasSubscriptionEndpoint
{
    private readonly HttpClient _http;
    private readonly ILogger<MonitorAsaasSubscriptionEndpoint>? _logger;
    private const string BaseUrl = "api/v1/monitor/integration/asaas/subscription";

    private readonly string _secretCode;

    public MonitorAsaasSubscriptionEndpoint(
        HttpClient client, 
        IConfiguration configuration, 
        ILogger<MonitorAsaasSubscriptionEndpoint>? logger = null)
    {
        _http = client;
        _logger = logger;
        _secretCode = configuration["SecretCode:Monitor:Asaas"]
            ?? throw new Exception("SecretCode para comunicação com o Integration Monitor (Asaas) não foi encontrado.");
    }

    private static JsonSerializerOptions GetJsonOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Adiciona conversor personalizado para DateTime
        options.Converters.Add(new CustomDateTimeConverter());
        
        return options;
    }

    public async Task<AsaasSubscriptionModel?> GetByAsaasId(string asaasId, string asaasAccountKey)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);
        _http.DefaultRequestHeaders.Add("asaasAccountKey", asaasAccountKey);

        try
        {
            _logger?.LogInformation("Buscando subscription por Asaas ID: {AsaasId}", asaasId);

            #region Realizando request

            HttpResponseMessage response = await _http.GetAsync($"{BaseUrl}/asaas/{asaasId}");
            Stream contentStream = await response.Content.ReadAsStreamAsync();
            var options = GetJsonOptions();

            await HttpHelper.HandleResponseStatus(contentStream, options, response);

            #endregion

            if (contentStream.Length == 0)
            {
                _logger?.LogInformation("Subscription não encontrada para Asaas ID: {AsaasId}", asaasId);
                return null;
            }

            AsaasSubscriptionModel? entity = await JsonSerializer.DeserializeAsync<AsaasSubscriptionModel>(contentStream, options);
            
            _logger?.LogInformation("Subscription encontrada: {SubscriptionId}", entity?.Id);
            return entity;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao buscar subscription por Asaas ID: {AsaasId}", asaasId);
            throw;
        }
    }

    public async Task<AsaasSubscriptionModel> Create(AsaasMonitorSubscriptionEvent valueObject, long companyId)
    {
        _http.DefaultRequestHeaders.Clear();
        _http.DefaultRequestHeaders.Add("secretCode", _secretCode);

        try
        {
            _logger?.LogInformation("Criando subscription no monitor - ID: {SubscriptionId}, CompanyId: {CompanyId}", 
                valueObject?.Subscription?.Id, companyId);

            // Normalizar as datas antes de enviar
            NormalizeDates(valueObject);

            #region Realizando request

            var options = GetJsonOptions();
            
            // Serializar manualmente para ter controle sobre o formato das datas
            var jsonContent = JsonSerializer.Serialize(valueObject, options);
            _logger?.LogDebug("JSON enviado: {Json}", jsonContent);

            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _http.PostAsync($"{BaseUrl}", httpContent);

            // Log do status da resposta
            _logger?.LogInformation("Resposta da API - Status: {StatusCode}, Reason: {ReasonPhrase}", 
                response.StatusCode, response.ReasonPhrase);

            Stream contentStream = await response.Content.ReadAsStreamAsync();

            await HttpHelper.HandleResponseStatus(contentStream, options, response);

            #endregion

            if (contentStream.Length == 0)
            {
                _logger?.LogWarning("Resposta vazia da API ao criar subscription");
                return null;
            }

            AsaasSubscriptionModel? entity = await JsonSerializer.DeserializeAsync<AsaasSubscriptionModel>(contentStream, options);

            if (entity is null)
            {
                _logger?.LogError("Falha na deserialização da resposta ao criar subscription");
                throw new Exception("Ocorreu um erro na tentativa da conversão do retorno.");
            }

            _logger?.LogInformation("Subscription criada com sucesso: {SubscriptionId}", entity.Id);
            return entity;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Erro ao criar subscription no monitor - Event: {EventId}, CompanyId: {CompanyId}", 
                valueObject?.Id, companyId);
            throw;
        }
    }

    private void NormalizeDates(AsaasMonitorSubscriptionEvent valueObject)
    {
        if (valueObject?.Subscription == null) return;

        try
        {
            // Normalizar NextDueDate se estiver no formato brasileiro
            if (!string.IsNullOrEmpty(valueObject.Subscription.NextDueDate))
            {
                valueObject.Subscription.NextDueDate = NormalizeDateString(valueObject.Subscription.NextDueDate);
                _logger?.LogDebug("NextDueDate normalizada: {Date}", valueObject.Subscription.NextDueDate);
            }

            // Normalizar EndDate se estiver no formato brasileiro
            if (!string.IsNullOrEmpty(valueObject.Subscription.EndDate))
            {
                valueObject.Subscription.EndDate = NormalizeDateString(valueObject.Subscription.EndDate);
                _logger?.LogDebug("EndDate normalizada: {Date}", valueObject.Subscription.EndDate);
            }

            // Normalizar DateCreated se estiver no formato brasileiro
            if (!string.IsNullOrEmpty(valueObject.Subscription.DateCreated))
            {
                valueObject.Subscription.DateCreated = NormalizeDateString(valueObject.Subscription.DateCreated);
                _logger?.LogDebug("DateCreated normalizada: {Date}", valueObject.Subscription.DateCreated);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Erro ao normalizar datas do objeto subscription");
        }
    }

    private string NormalizeDateString(string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return dateString;

        try
        {
            // Se já está no formato ISO, retorna como está
            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out _))
            {
                return dateString;
            }

            // Tenta fazer parse no formato brasileiro (dd/MM/yyyy)
            if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            // Tenta outros formatos comuns
            string[] formats = {
                "dd/MM/yyyy HH:mm:ss",
                "dd/MM/yyyy HH:mm",
                "yyyy-MM-dd",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-ddTHH:mm:ssZ",
                "yyyy-MM-ddTHH:mm:ss.fffZ"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
            }

            _logger?.LogWarning("Não foi possível normalizar a data: {DateString}", dateString);
            return dateString;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Erro ao normalizar data: {DateString}", dateString);
            return dateString;
        }
    }
}

// Conversor personalizado para DateTime que suporta múltiplos formatos
public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private static readonly string[] DateFormats = {
        "yyyy-MM-dd",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss.fffZ",
        "dd/MM/yyyy",
        "dd/MM/yyyy HH:mm:ss",
        "dd/MM/yyyy HH:mm"
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateString = reader.GetString();
        
        if (string.IsNullOrEmpty(dateString))
            return default;

        foreach (var format in DateFormats)
        {
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }

        // Fallback para parse padrão
        if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime fallbackResult))
        {
            return fallbackResult;
        }

        throw new JsonException($"Unable to convert \"{dateString}\" to DateTime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}

public class CustomNullableDateTimeConverter : JsonConverter<DateTime?>
{
    private readonly CustomDateTimeConverter _baseConverter = new CustomDateTimeConverter();

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        return _baseConverter.Read(ref reader, typeof(DateTime), options);
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            _baseConverter.Write(writer, value.Value, options);
        else
            writer.WriteNullValue();
    }
}
