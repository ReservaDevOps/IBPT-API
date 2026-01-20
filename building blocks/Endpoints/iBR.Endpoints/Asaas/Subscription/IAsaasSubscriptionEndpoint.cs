using Ibr.Core.Dtos.Integration.Asaas.Subscription;

namespace iBR.Endpoints.Asaas.Subscription;

public interface IAsaasSubscriptionEndpoint
{
    Task<AsaasMonitorSubscriptionDto?> GetById(string subscriptionId, string baseUrl, string accessToken);

    Task<IReadOnlyList<AsaasMonitorSubscriptionDto>> GetAllSubscriptions(
      string baseUrl,
      string accessToken);
}
