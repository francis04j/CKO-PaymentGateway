using System;
using System.Threading.Tasks;

namespace WebApi.Database
{
    public interface IPaymentDbContext<T> : IDisposable where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task SaveAsync(T item);
    }
}
