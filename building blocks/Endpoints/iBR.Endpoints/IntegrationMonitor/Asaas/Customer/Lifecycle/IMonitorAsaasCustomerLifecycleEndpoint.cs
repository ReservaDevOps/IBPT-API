using Ibr.Core.Models.Asaas.Customer;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Customer.Lifecycle;

/// <summary>
/// Centraliza as requests para a API de monitoração Asaas para o de ciclo de vida de pagamentos.
/// </summary>
public interface IMonitorAsaasCustomerLifecycleEndpoint
{
    /// <summary>
    /// Faz o registro de um status do ciclo de vida de um pagamento.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<AsaasCustomerLifecycleModel?> Create(AsaasCustomerLifecycleModel model);
}