using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FavoriteStations.Models.Dto;
using FavoriteStations.Services;
using FavoriteStations.Filters;

namespace FavoriteStations.Controllers {
    /// <summary>
    /// RESTful resource which abstracts all of the favorite stations associated with the current authenticated user
    /// </summary>
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class UserStationsController : ControllerBase {
        private readonly IBusinessLayer businessLayer;
        private readonly IBusinessResponseMapper businessResponseMapper;

        public UserStationsController(IBusinessLayer businessLayer, IBusinessResponseMapper businessResponseMapper) {
            this.businessLayer = businessLayer;
            this.businessResponseMapper = businessResponseMapper;
        }

        /// <summary>
        /// Fetch all stations associated with the authenticated user
        /// </summary>
        /// <response code="200">The favorite stations associated with the current user</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IEnumerable<StationDto>> Get() {
            return await this.businessLayer.GetAllStations();
        }

        /// <summary>
        /// Fetch a specific station associated with the authenticated user
        /// </summary>
        /// <param name="id">Id of the station to retrieve</param>
        /// <response code="200">The requested station was found</response>
        /// <response code="403">The requested station does not belong to the authenticated user</response>
        /// <response code="404">The requested station id was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id) {
            var result = await this.businessLayer.GetStationAsync(id);
            return result.Match<IActionResult>(
                success => Ok(result.RightValue),
                failure => this.businessResponseMapper.ToActionResult(failure)
            );
        }

        /// <summary>
        /// Create a new station for the authenticated user
        /// </summary>
        /// <param name="create">The new station to create</param>
        /// <response code="201">The station was created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] StationCreateUpdateDto create) {
            var created = await this.businessLayer.CreateStationAsync(create);
            return Created($"UserStations/{created.StationId}", created);
        }

        /// <summary>
        /// Update the specified existing station
        /// </summary>
        /// <param name="id">The id of the station to update</param>
        /// <param name="update">The new representation of the specified station</param>
        /// <response code="200">The station was updated</response>
        /// <response code="403">The specified station does not belong to the authenticated user</response>
        /// <response code="404">The specified station id was not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] StationCreateUpdateDto update) {
            var result = await this.businessLayer.UpdateStationAsync(update, id);
            return result.Match<IActionResult>(
                success => Ok(result.RightValue),
                failure => this.businessResponseMapper.ToActionResult(failure)
            );
        }

        /// <summary>
        /// Delete the specified station
        /// </summary>
        /// <param name="id">The id of the station to delete</param>
        /// <response code="204">The station was deleted</response>
        /// <response code="403">The specified station does not belong to the authenticated user</response>
        /// <response code="404">The specified station id was not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id) {
            var result = await this.businessLayer.DeleteStationAsync(id);
            return result.Match<IActionResult>(
                success => NoContent(),
                failure => this.businessResponseMapper.ToActionResult(failure)
            );
        }
    }
}
