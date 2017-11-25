using System.Threading.Tasks;

namespace vega.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext _context;

        public UnitOfWork (VegaDbContext context)
        {
            this._context = context;

        }

        public async Task Complete ()
        {
            await _context.SaveChangesAsync();
        }
    }
}