using Application.Common.Wrappers;
using Application.DTOs.Common;
using Application.Features.Utilities.Commands.CreateContactCommand;
using Application.Features.Utilities.Commands.SendRealTimeNotificationsCommand;
using Application.Features.Utilities.Queries.GetAllCities;
using Application.Features.Utilities.Queries.GetAllProvinces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    /// <summary>
    ///  Proporciona acceso a recursos utilitarios y configuraciones comunes para la aplicación.
    /// Incluye operaciones relacionadas con entidades como ciudades, países, y otros recursos que no pertenecen directamente a un dominio específico.
    /// </summary>
    [ApiVersion("1.0")]
    [AllowAnonymous]
    //[Authorize]
    public class UtilitiesController : BaseApiController
    {

        /// <summary>
        /// Obtiene una lista paginada de ciudades según los parámetros de filtro proporcionados.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Una lista de ciudades con su provincia</returns>
        [HttpGet("Cities")]
        [ProducesResponseType(typeof(PagedResponse<List<GetAllCitiesResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCitiesAsync([FromQuery] GetAllCitiesQuery filter)
        {
            return Ok(await Mediator.Send(filter));
        }

        /// <summary>
        /// Obtiene todos las provincias.
        /// </summary>
        /// <response code="200">Devuelve la lista correctamente.</response>
        /// <response code="500">Error interno del servidor.</response>
        [ProducesResponseType(typeof(Response<List<ProvinceDTO>>), StatusCodes.Status200OK)]
        [HttpGet("Provinces")]
        public async Task<IActionResult> GetAllProvincesAsync()
        {
            return Ok(await Mediator.Send(new GetAllProvincesQuery { }));
        }

        /// <summary>
        /// Crea un nuevo contacto.
        /// </summary>
        /// <response code="200">El request fue OK.</response>
        /// <response code="500">Error interno del servidor.</response>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [HttpPost("Contact")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateContactCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Envia una nueva notificacion para ser escuchada por aquellos suscriptos.
        /// </summary>
        /// <response code="200">El request fue OK.</response>
        /// <response code="500">Error interno del servidor.</response>
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [HttpPost("Notification")]
        [AllowAnonymous]
        public async Task<IActionResult> SendNotificationm([FromBody] SendRealTimeNotificationsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
