using CMS.Application.Features.MasterCompanyCascade.Queries.GetCities;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetCityById;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetCountries;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetCountryById;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetLocationById;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetLocations;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetStateById;
using CMS.Application.Features.MasterCompanyCascade.Queries.GetStates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin , Super_Admin , Management_User")]
    public class MasterCompanyCascadeController : ControllerBase
    {
        readonly IMediator _mediator;

        public MasterCompanyCascadeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _mediator.Send(new GetCountriesQuery());
            return Ok(countries);
        }

        [HttpGet("GetStates")]
        public async Task<IActionResult> GetStates(int countryId)
        {
            var states = await _mediator.Send(new GetStatesQuery(countryId));
            return Ok(states);
        }

        [HttpGet("GetCities")]
        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await _mediator.Send(new GetCitiesQuery(stateId));
            return Ok(cities);
        }
        [HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocations(int cityId)
        {
            var locations = await _mediator.Send(new GetLocationsQuery(cityId));
            return Ok(locations);
        }
        [HttpGet("GetCountryById")]
        public async Task<IActionResult> GetCountryById(int id)
        {
            var country = await _mediator.Send(new GetCountryByIdQuery(id));
            return Ok(country);
        }
        [HttpGet("GetStateById")]
        public async Task<IActionResult> GetStateById(int id)
        {
            var state = await _mediator.Send(new GetStateByIdQuery(id));
            return Ok(state);
        }
        [HttpGet("GetCityById")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var city = await _mediator.Send(new GetCityByIdQuery(id));
            return Ok(city);
        }
        [HttpGet("GetLocationById")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _mediator.Send(new GetLocationByIdQuery(id));
            return Ok(location);
        }
    }
}
