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
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using BitPastry.Backend.DTO.Exceptions;

using BitPastry.Backend.DTO.Models.Ordini;
using BitPastry.Backend.DTO.HTTP_Messages.Response.Ordine;
using BitPastry.Backend.DTO.HTTP_Messages.Response.Ordine.Lavorazione;
using BitPastry.Backend.DTO.HTTP_Messages.Request.Ordine.Lavorazione;
using Microsoft.Extensions.Logging;

namespace BitPastry.Backend.Presentation.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class OrdiniController : ControllerBase {
        private readonly ILogger<AuthController> _logger;
        private readonly BaseService _uow;
        private readonly TokensProvider _tokensProvider;

        public OrdiniController(
            ILogger<AuthController> logger,
            BaseService uow,
            TokensProvider tokenProvider
        ) {
            this._logger = logger;
            this._uow = uow;
            this._tokensProvider = tokenProvider;
        }

        #region Ordini
        /* ------------------------------------------------------------------------------------- */
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<GetOrdineResponse> Get(
            [FromQuery(Name = "onlyMy")] bool isOnlyMy,
            [FromQuery(Name = "onlyFree")] bool isOnlyFree
        ) {
            _logger.LogInformation($"Requesting ordini: <Only My := {isOnlyMy} - Only Free := {isOnlyFree}>");
            IEnumerable<GetOrdineResponse> response = Enumerable.Empty<GetOrdineResponse>();

            using (_uow)
            {
                var service = new OrdiniService(_uow);

                response = service
                    .Get(isOnlyMy, isOnlyFree)
                    .Select(OrdineMapper.Map)
                    .ToList();

                _uow.Complete();
            }

            return response;
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create([FromBody] CreateOrdineRequest request) 
        {
            _logger.LogInformation($"Create ordine");
            int id;

            using (_uow)
            {
                var service = new OrdiniService(_uow);
                id = service.Create(
                    request.RicettaID,
                    request.Quantità,
                    request.TsOrdine,
                    request.RifCliente,
                    request.ContattoCliente
                );

                _uow.Complete();
            }

            return Ok(id);
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int id) 
        {
            _logger.LogInformation($"Delete ordine: ID {id}");

            using (_uow)
            {
                var service = new OrdiniService(_uow);
                service.Delete(id);

                _uow.Complete();
            }

            return Ok();
        }

        /* ------------------------------------------------------------------------------------- */
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Update(int id, [FromBody] CreateOrdineRequest request) 
        {
            _logger.LogInformation($"Update ordine: ID {id}");
            
            using (_uow)
            {
                var service = new OrdiniService(_uow);
                service.Update(
                    id,
                    request.RicettaID,
                    request.Quantità,
                    request.TsOrdine,
                    request.RifCliente,
                    request.ContattoCliente
                );

                _uow.Complete();
            }

            return Ok();
        }
        #endregion

        #region Lavorazioni
        /* ------------------------------------------------------------------------------------- */
        [HttpPut("complete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Complete() 
        {
            _logger.LogInformation("Complete Ordine");

            using (_uow)
            {
                var service = new OrdiniService(_uow);
                service.Completee();

                _uow.Complete();
            }

            return Ok();
        }

        [HttpPut("complete/{idBop}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Complete(int idBop) 
        {
            _logger.LogInformation($"Complete Bop: <ID := {idBop}>");

            using (_uow)
            {
                var service = new OrdiniService(_uow);
                service.Completee(idBop);

                _uow.Complete();
            }

            return Ok();
        }

        /* ------------------------------------------------------------------------------------- */

        [HttpPost("complete/{idBop}/{idBom}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public GetCollectedMaterialResponse CollectMaterial([FromBody] CollectMaterialRequest request) 
        {
            // Recupero i Parametri di Route
            var idBop = (string?)Request.RouteValues["idBop"];
            var idBom = (string?)Request.RouteValues["idBom"];

            GetCollectedMaterialResponse response;
            _logger.LogInformation($"Complete Bom: <ID Bop := {idBop} - ID Bom {idBom}>");

            using (_uow)
            {
                var service = new OrdiniService(_uow);
                response = OrdineMapper.Map(
                    service.Completee(idBop, idBom, request.QuantitàPrelevata)
                );

                _uow.Complete();
            }

            return response;
        }

        #endregion
    }
}
