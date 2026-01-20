using System;
using System.Collections.Generic;

namespace Ibr.Core.Data;

public interface IGenericRepository<T> : IDisposable where T : class
{
    IUnitOfWork UnitOfWork { get; }
    IEnumerable<T> GetAll(Func<T, bool> func);
    T Get(Guid id);

    T Find(Func<T, bool> func);
    T FindLast<TKey>(Func<T, bool> func, Func<T, TKey> keySelector);
    bool Any(Func<T, bool> func);

    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);

    void UpdateRangeEntities(IEnumerable<T> entities);
}