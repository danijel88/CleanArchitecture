using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Core.Repository
{
    public interface IDataContextAsync : IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<int> SaveChangesAsync();
    }
}