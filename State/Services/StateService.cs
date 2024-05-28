using Microsoft.EntityFrameworkCore;

namespace State.Services
{
    public class StateService : IStateService
    {
        private readonly MainDbContext _context;
        public StateService(MainDbContext context)
        {
            _context = context;
        }
        public async Task<List<State.Models.State>> AddState(State.Models.State State)
        {
            _context.States.Add(State);
            _context.SaveChanges();
            return await _context.States.ToListAsync();
        }
        public async Task<List<State.Models.State>> GetAllState()
        {
            return await _context.States.ToListAsync();
        }
    }
}
