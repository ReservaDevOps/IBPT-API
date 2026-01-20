using Ibr.Core.Dtos.Integration.Asaas.Payment;
using Ibr.Core.Dtos.Utils;
using Ibr.Core.Models.Asaas.Payment;
using Ibr.Core.ValueObjects.Asaas.Payment;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Payment;

/// <summary>
/// Concentra as requisições para o endpoint de pagamentos do monitor Asaas.
/// </summary>
public interface IMonitorAsaasPaymentEndpoint
{
    #region Get

    /// <summary>
    /// Busca o pagamento no monitor através do id do pagamento do Asaas.
    /// </summary>
    /// <param name="asaasPaymentId">Id do pagamento do Asaas.</param>
    /// <returns></returns>
    Task<AsaasPaymentModel?> GetByAsaasId(string asaasPaymentId, string asaasAccountKey);

    /// <summary>
    /// Busca pagamentos pendentes de uma assinatura no Asaas. Informe ou o id do asaas ou o id da assinatura no monitor.
    /// </summary>
    /// <param name="rp">Parâmetros padrão de requisição.</param>
    /// <param name="asaasSubscriptionId">Id do plano no asaas.</param>
    /// <param name="subscriptionId">Id do plano no monitor.</param>
    /// <returns></returns>
    Task<List<AsaasPaymentDto>> GetPendingBySubscription(RequestParamsDTO rp, string? asaasSubscriptionId, Guid? subscriptionId, string asaasAccountKey);
    
    #endregion

    #region Create

    /// <summary>
    /// Faz a criação de um pagamento no monitor.
    /// </summary>
    /// <param name="vo">Objeto de criação da entidade.</param>
    /// <param name="customerEmail">Email do cliente.</param>
    /// <returns></returns>
    Task<AsaasPaymentModel?> Create(CreateAsaasPaymentVO vo, string customerEmail);

    #endregion
}
