using dotnetapi.Context;

namespace dotnetapi.Services
{
    public class InsumoService
    {
        private readonly AppDbContext _context;


        public InsumoService(AppDbContext context) => _context = context;

    }
}
