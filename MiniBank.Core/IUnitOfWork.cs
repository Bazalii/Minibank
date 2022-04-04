namespace MiniBank.Core
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }
}