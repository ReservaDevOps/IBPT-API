using Ibr.Core.Data;

namespace Ibr.IBPT.Models;

public class IbptTax : IAggregateRoot
{
    public IbptTax(string uF, string code, string ex, string type, string description, decimal federalNational, decimal importedFederal, decimal state, decimal municipal, DateTime startValidity, DateTime endValidity, string key, string version, string source)
    {
        UF = uF;
        Code = code;
        Ex = ex;
        Type = type;
        Description = description;
        FederalNational = federalNational;
        ImportedFederal = importedFederal;
        State = state;
        Municipal = municipal;
        StartValidity = startValidity;
        EndValidity = endValidity;
        Key = key;
        Version = version;
        Source = source;

        CreatedAt = DateTime.UtcNow;
    }

    public string UF { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string Ex { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal FederalNational { get; private set; }
    public decimal ImportedFederal { get; private set; }
    public decimal State { get; private set; }
    public decimal Municipal { get; private set; }
    public DateTime StartValidity { get; private set; }
    public DateTime EndValidity { get; private set; }
    public string Key { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public string Source { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void SetIbptTax(string type, string description, decimal federalNational, decimal importedFederal, decimal state, decimal municipal, DateTime startValidity, DateTime endValidity, string key, string version, string source)
    {
        Type = type;
        Description = description;
        FederalNational = federalNational;
        ImportedFederal = importedFederal;
        State = state;
        Municipal = municipal;
        StartValidity = startValidity;
        EndValidity = endValidity;
        Key = key;
        Version = version;
        Source = source;

        UpdatedAt = DateTime.UtcNow;
    }
}