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
        private readonly IDataLayer dataLayer;
        private readonly User user;
        public StationsController(IDataLayer dataLayer, User user) {
            this.dataLayer = dataLayer;
            this.user = user;
        }

        [HttpGet]
        public async Task<IEnumerable<Station>> Get() {
            return await this.dataLayer.GetAllStationsForUserAsync(this.user.Sub);
        }
    }
}
