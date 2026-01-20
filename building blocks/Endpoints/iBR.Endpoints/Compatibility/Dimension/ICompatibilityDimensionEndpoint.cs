using Ibr.Core.Dtos.Compatibility.Dimension;
using Ibr.Core.ValueObjects.Compatibility.Dimension;

namespace iBR.Endpoints.Compatibility.Dimension;

/// <summary>
/// Concentra requisições para a API de Compatibility.
/// </summary>
public interface ICompatibilityDimensionEndpoint
{
    /// <summary>
    /// Realiza uma requisição para o endpoint de criação de dimensão padrão.
    /// </summary>
    /// <param name="valueObject">Objeto de criação de dimensão padrão.</param>
    /// <returns></returns>
    Task<CompatibilityDimensionDto> Create(CreateDimensionVO valueObject);

    /// <summary>
    /// Realiza uma requisição para o endpoint de criação de dimensão padrão caso não exista.
    /// </summary>
    /// <param name="valueObject">Objeto de criação de dimensão padrão.</param>
    /// <returns></returns>
    Task<CompatibilityDimensionDto> CreateIfDoesntExist(CreateDimensionVO valueObject);
}