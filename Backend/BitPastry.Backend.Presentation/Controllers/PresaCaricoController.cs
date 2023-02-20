using BitPastry.Backend.Core.Services;
using BitPastry.Backend.Presentation.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitPastry.Backend.DTO.HTTP_Messages.Response;
using BitPastry.Backend.Data;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BitPastry.Backend.DTO.Models;
using BitPastry.Backend.DTO.Mappers;
using BitPastry.Backend.DTO.HTTP_Messages.Request.Operatore;
using BitPastry.Backend.DTO.HTTP_Messages.Request.Ordine;
using BitPastry.Backend.DTO.HTTP_Messages.Request;
using System.Security.Claims;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;

namespace BitPastry.Backend.Presentation.Controllers {

    [Route("api/presa-carico")]
    [ApiController]
    public class PresaCaricoController : ControllerBase {
        private readonly ILogger<AuthController> _logger;
        private readonly BaseService _uow;
        private readonly TokensProvider _tokensProvider;

        public PresaCaricoController(
            ILogger<AuthController> logger,
            BaseService uow,
            TokensProvider tokenProvider
        ) {
            this._logger = logger;
            this._uow = uow;
            this._tokensProvider = tokenProvider;
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Check() {
            
            _logger.LogInformation("Check Presa Carico");

            bool response;
            using (_uow)
            {
                var service = new PresaCaricoService(_uow);
                response = service.Check();

                _uow.Complete();
            }

            return Ok(response);
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpPut("{idOrdine}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Pick(int idOrdine) {
            _logger.LogInformation($"Pick presa in carico dell'Ordine : " + idOrdine);
            
            using (_uow)
            {
                var service = new PresaCaricoService(_uow);
                service.Pick(idOrdine);

                _uow.Complete();
            }

            return Ok();
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpDelete()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Release() {
            _logger.LogInformation("Release presa in carico");

            using (_uow)
            {
                var service = new PresaCaricoService(_uow);
                service.Release();

                _uow.Complete();
            }

            return Ok();
        }
    }
}
