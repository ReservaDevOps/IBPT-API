using Ibr.Core.Enums.Asaas;
using Ibr.Core.Models.Asaas.Payment;
using Ibr.Core.ValueObjects.Asaas.Lifecycle;

namespace iBR.Endpoints.IntegrationMonitor.Asaas.Payment.Lifecycle;

/// <summary>
/// Centraliza as requests para a API de monitoração Asaas para os ciclos de vida de pagamentos.
/// </summary>
public interface IMonitorAsaasPaymentLifecycleEndpoint
{
    /// <summary>
    /// Busca o histórico de eventos de um pagamento através do asaasPaymentId.
    /// </summary>
    /// <param name="asaasPaymentId">Id do pagamento gerado pelo Asaas.</param>
    /// <returns></returns>
    Task<List<AsaasPaymentLifecycleModel>?> GetByAsaas(string asaasPaymentId);

    /// <summary>
    /// Busca os ciclos de eventos a partir do tipo de evento.
    /// </summary>
    /// <param name="status">Status a ser buscado.</param>
    /// <returns></returns>
    Task<List<AsaasPaymentLifecycleModel>?> GetByStatus(EAsaasEventWebhook status);

    /// <summary>
    /// Busca os ciclos de eventos que representam o último estado do ciclo de vida de um pagamento, a partir do tipo de evento.
    /// Supondo que você deseje buscar todos os ciclos de vida de pagamentos que estão com erros.
    /// Existe um pagamento que possui o evento de erro, mas o pagamento foi reprocessado e está atualmente concluído com sucesso.
    /// Esse método irá ignorar esse pagamento e só irá buscar aqueles que tenham o evento de erro como último status. 
    /// </summary>
    /// <param name="status">Status a ser buscado.</param>
    /// <returns></returns>
    Task<List<AsaasPaymentLifecycleModel>?> GetByLastStatus(EAsaasEventWebhook status);

    /// <summary>
    /// Cria um ciclo de vida de um pagamento.
    /// </summary>
    /// <param name="asaasPaymentId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    Task<AsaasPaymentLifecycleModel?> Create(CreateAsaasPaymentLifecycleVO vo);
}