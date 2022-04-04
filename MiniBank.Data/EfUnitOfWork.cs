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

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}