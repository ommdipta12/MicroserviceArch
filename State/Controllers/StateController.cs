using State.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using State.Helper;

namespace State.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _StateService;
        private readonly IRabbitMQPublisherService _rabbitMQPublisherService;
        public StateController(IStateService StateService, IRabbitMQPublisherService rabbitMQPublisherService)
        {
            _StateService = StateService;
            _rabbitMQPublisherService = rabbitMQPublisherService;
        }

        [HttpPost]
        public async Task<List<State.Models.State>> AddState(State.Models.State State)
        {
            return await _StateService.AddState(State);
        }

        [HttpGet]
        public async Task<List<State.Models.State>> GetAllState()
        {
            Log.Logger.Error("ok");
            _rabbitMQPublisherService.Publish("Hello world", "test");
            return await _StateService.GetAllState();
        }
    }
}
