using System.Threading;
using System.Threading.Tasks;
using MiniBank.Core;

namespace MiniBank.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly MiniBankContext _context;

        public EfUnitOfWork(MiniBankContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}