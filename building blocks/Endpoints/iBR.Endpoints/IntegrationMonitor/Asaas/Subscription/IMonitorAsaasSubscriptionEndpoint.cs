using Ibr.Core.Dtos.Integration.Asaas.Subscription;
using Ibr.Core.Models.Asaas.Subscription;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Subscription;

/// <summary>
/// Concentra requisições para o endpoint de subscription do monitor de integrações com o asaas.
/// </summary>
public interface IMonitorAsaasSubscriptionEndpoint
{
    /// <summary>
    /// Busca pelo id do plano no asaas.
    /// </summary>
    /// <param name="asaasId">Id do plano (subscription)</param>
    /// <returns></returns>
    Task<AsaasSubscriptionModel> GetByAsaasId(string asaasId, string asaasAccountKey);

    /// <summary>
    /// Cria o registro de uma subscription.
    /// </summary>
    /// <returns></returns>
    Task<AsaasSubscriptionModel> Create(AsaasMonitorSubscriptionEvent valueObject, long companyId);
}
