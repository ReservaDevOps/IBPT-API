using Ibr.Core.Models.Asaas.Subscription.Lifecycle;
using Ibr.Core.ValueObjects.Asaas.Subscription;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Subscription.Lifecycle;

public interface IMonitorAsaasSubscriptionLifecycleEndpoint
{
    #region Get

    /// <summary>
    /// Busca um ciclo de vida de plano pelo id do asaas.
    /// </summary>
    /// <param name="asaasId"></param>
    /// <returns></returns>
    Task<AsaasSubscriptionLifecycleModel?> GetByAsaasId(string asaasId);

    /// <summary>
    /// Busca ciclos de vida de um plano através do id do plano.
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <returns></returns>
    Task<List<AsaasSubscriptionLifecycleModel>> GetByAsaasSubscriptionId(string subscriptionId);

    /// <summary>
    /// Busca o cilo de vida de planos de um cliente a partir do id do cliente no desmonte.
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<List<AsaasSubscriptionLifecycleModel>> GetByCompanyId(int companyId);

    #endregion

    #region Add

    /// <summary>
    /// Faz uma requisição para o endpoint que adiciona um ciclo de vida do plano.
    /// </summary>
    /// <returns></returns>
    Task<AsaasSubscriptionLifecycleModel> Create(CreateAsaasSubscriptionLifecycleVo valueObject);

    #endregion
}