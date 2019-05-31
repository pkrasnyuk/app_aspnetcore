using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.BLL.Attributes;
using WebAppCore.BLL.Helpers;
using WebAppCore.BLL.Interfaces;

namespace WebAppCore.Api.Controllers
{
    [Authorize]
    public class EntitiesController<TModel, TViewModel> : BaseController
    {
        private readonly IEntitiesManager<TModel, TViewModel> _manager;

        public EntitiesController(IEntitiesManager<TModel, TViewModel> manager)
        {
            _manager = manager;
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpGet]
        public async Task<IActionResult> GetEntitiesAsync()
        {
            return Ok(await _manager.GetEntitiesAsync());
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpPost("withPagination")]
        public async Task<IActionResult> GetEntitiesWithPaginationAsync([FromBody] PageParameters parameters)
        {
            return Ok(await _manager.GetPagedEntitiesAsync(parameters));
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEntityAsync([FromRoute] string id)
        {
            return Ok(await _manager.GetEntityAsync(id));
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpPost]
        public async Task<IActionResult> CreateEntityAsync([FromBody] TModel model)
        {
            return Ok(await _manager.CreateEntityAsync(model));
        }

        [AuthorizePolicy(Policy.RegisteredUser)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntityAsync([FromRoute] string id, [FromBody] TModel model)
        {
            return Ok(await _manager.UpdateEntityAsync(id, model));
        }

        [AuthorizePolicy(Policy.AdminOnly)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveEntityAsync([FromRoute] string id)
        {
            return Ok(await _manager.RemoveEntityAsync(id));
        }
    }
}
