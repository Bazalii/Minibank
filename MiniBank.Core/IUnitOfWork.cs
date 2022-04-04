using System.Threading.Tasks;

namespace MiniBank.Core
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
    }
}