using Cache.Services;
using Microsoft.AspNetCore.Mvc;

namespace HazelcastAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class LoginAttemptsController : ControllerBase
    {
        private readonly IHazelcastService<string, int> _hazelcastService;

        public LoginAttemptsController(IHazelcastService<string, int> hazelcastService)
        {
            _hazelcastService = hazelcastService;
        }

        [HttpGet]
        [Route("/login/get-attempt/{user}")]
        public async Task<JsonResult> GetAttempt(string user)
        {
            var rec = await _hazelcastService.GetRecordAsync(user).ConfigureAwait(false);
            return new JsonResult(rec);
        }

        [HttpGet]
        [Route("/login/add-attempt/{user}")]
        public async Task<JsonResult> AddAttempt(string user)
        {
            var rec = await _hazelcastService.GetRecordAsync(user).ConfigureAwait(false);
            rec++;
            await _hazelcastService.PutRecordAsync(user, rec).ConfigureAwait(false);
            var newCount = await _hazelcastService.GetRecordAsync(user).ConfigureAwait(false);
            return new JsonResult($"new count: {newCount}");
        }

        [HttpGet]
        [Route("/login/delete-attempt/{user}")]
        public async Task<JsonResult> DeleteAttempt(string user)
        {
            await _hazelcastService.DeleteRecordAsync(user).ConfigureAwait(false);
            return new JsonResult($"record deleted for {user}");
        }
    }
}
