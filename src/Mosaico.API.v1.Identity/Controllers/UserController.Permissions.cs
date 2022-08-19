using System;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Identity.Queries.GetUserPermissions;
using Mosaico.Application.Identity.Queries.HasPermission;
using Mosaico.Application.Identity.Queries.GetUsersWithPermission;

namespace Mosaico.API.v1.Identity.Controllers
{
    public partial class UserController
    {
        [HttpGet("{id}/permission/{key}")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HasPermission([FromRoute] string id, [FromRoute] string key, [FromQuery] Guid? entityId = null)
        {
            if (string.IsNullOrWhiteSpace(id)) 
                throw new InvalidParameterException(nameof(id), id);
            if (string.IsNullOrWhiteSpace(key)) 
                throw new InvalidParameterException(nameof(key), key);

            var response = await _mediator.Send(new HasPermissionQuery
            {
                Key = key,
                UserId = id,
                EntityId = entityId
            });

            return new SuccessResult(response);
        }
        
        [HttpGet("{id}/permissions")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUserPermissionsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPermissions([FromRoute] string id, [FromQuery] string entityId = null)
        {
            if (string.IsNullOrWhiteSpace(id)) 
                throw new InvalidParameterException(nameof(id), id);

            Guid? entityGuid = null;
            
            if (!string.IsNullOrWhiteSpace(entityId) && Guid.TryParse(entityId, out var parsedValue))
            {
                entityGuid = parsedValue;
            }
            
            var response = await _mediator.Send(new GetUserPermissionsQuery
            {
                Id = id,
                EntityId = entityGuid
            });

            return new SuccessResult(response);
        }


        [HttpGet("/permissions/{key}")]
        [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
        [ProducesResponseType(typeof(SuccessResult<GetUsersWithPermissionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsersWithPermission([FromRoute] string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidParameterException(nameof(key), key);

            var response = await _mediator.Send(new GetUsersWithPermissionQuery
            {
                Key = key
            });

            return new SuccessResult(response);
        }
    }
}