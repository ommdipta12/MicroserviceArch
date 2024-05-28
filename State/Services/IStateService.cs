namespace State.Services
{
    public interface IStateService
    {
        Task<List<State.Models.State>> AddState(State.Models.State State);
        Task<List<State.Models.State>> GetAllState();
    }
}
