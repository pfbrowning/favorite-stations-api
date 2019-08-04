using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FavoriteStations.Models;
using FavoriteStations.Services;

namespace FavoriteStations.Controllers {
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StationsController : ControllerBase {
        private readonly IDataLayerService dataLayerService;
        private readonly User user;
        public StationsController(IDataLayerService dataLayerService, User user) {
            this.dataLayerService = dataLayerService;
            this.user = user;
        }

        [HttpGet]
        public async Task<IEnumerable<Station>> Get() {
            return await this.dataLayerService.GetAllStationsForUserAsync(this.user.Sub);
        }
    }
}
