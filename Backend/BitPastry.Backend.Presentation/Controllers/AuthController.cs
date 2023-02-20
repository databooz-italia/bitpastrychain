using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitPastry.Backend.Core.Services;
using BitPastry.Backend.DTO.Exceptions;
using BitPastry.Backend.DTO.Exceptions.ManagedExceptions;
using BitPastry.Backend.DTO.HTTPMessages.Request;
using BitPastry.Backend.DTO.HTTPMessages.Response;
using BitPastry.Backend.DTO.Models;
using BitPastry.Backend.Presentation.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BitPastry.Backend.DTO.Exceptions.ManagedException;

namespace BitPastry.Backend.Presentation.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly BaseService _uow;
        private readonly TokensProvider _tokensProvider;

        public AuthController(
            ILogger<AuthController> logger,
            BaseService uow,
            TokensProvider tokenProvider
        ) {
            this._logger = logger;
            this._uow = uow;
            this._tokensProvider = tokenProvider;
        }

        /* ------------------------------------------------------------------------------------- */
        // Login
        [HttpPost("login")]
        public AuthenticationResponse Authenticate([FromBody] AuthenticationRequest request)
        {
            _logger.LogInformation("Authenticating User: {0}", request.Username);
            return DoAuthenticate(request);
        }
        
        [HttpPost("login-operatore")]
        public AuthenticationResponse AuthenticateOperatore([FromBody] AuthenticationRequest request) 
        {
            _logger.LogInformation("Authenticating Operatore {0}", request.Username);
            return DoAuthenticate(request, true);
        }

        /* ------------------------------------------------------------------------------------- */
        // Do Login
        private AuthenticationResponse DoAuthenticate(
            AuthenticationRequest request,
            bool isOperatore = false
        ) {
            AccountProxy? user;

            using (_uow)
            {
                user = new AccountsService(_uow)
                    .Authenticate(request.Username, request.Password, isOperatore);
                _uow.Complete();
            }

            return new AuthenticationResponse()
            {
                ID = user.ID,
                Username = user.Username,
                Token = _tokensProvider.ProvideToken(
                    user.ID,
                    user.Username,
                    request.IsRememberMe
                ),
                FullName = user.FullName,
            };
        } 
    }
}

