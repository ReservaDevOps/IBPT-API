using System.Threading.Tasks;

namespace Ibr.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}