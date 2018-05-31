using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    /// <summary>
    /// Class to apply Swagger Metadata described at the class level to allow
    /// for generic classes with re-usuable api functionality
    /// the still expose useful api documentation
    /// </summary>
    public class SwaggerClassMetaDataOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var apiDesc = context.ApiDescription;
            var attributes = GetActionAttributes(apiDesc);

            if (!attributes.Any())
                return;

            if (operation.Responses == null)
            {
                operation.Responses = new Dictionary<string, Response>();
            }

            foreach (var attribute in attributes)
            {
                ApplyAttribute(operation, context, attribute);
            }
        }

        private static void ApplyAttribute(Operation operation, OperationFilterContext context, SwaggerClassMetaDataAttribute attribute)
        {
            /*
            var key = attribute.StatusCode.ToString();
            Response response;
            if (!operation.Responses.TryGetValue(key, out response))
            {
                response = new Response();
            }

            if (attribute.Description != null)
                response.Description = attribute.Description;

            if (attribute.Type != null && attribute.Type != typeof(void))
                response.Schema = context.SchemaRegistry.GetOrRegister(attribute.Type);

            operation.Responses[key] = response;
            */
        }

        private static SwaggerClassMetaDataAttribute[] GetActionAttributes(ApiDescription apiDesc)
        {
            return apiDesc.ControllerAttributes().OfType<SwaggerClassMetaDataAttribute>().ToArray();
        }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SwaggerClassMetaDataAttribute : Attribute
    {
        public SwaggerClassMetaDataAttribute(Type metadataClass)
        {
            // todo: use a roslyn analyzer to ensure the type passed in is correct.
        }
    }

    [Route("/fakecrud")]
    [SwaggerClassMetaData(typeof(FakeCrudControllerSwaggerMetaData))]
    [ExcludeFromCodeCoverage]
    public sealed class FakeCrudController : CrudController<FakeCrudController, FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, long?, FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, int>
    {
        public FakeCrudController(
            IAuthorizationService authorizationService,
            ILogger<FakeCrudController> logger,
            IMediator mediator,
            IAuditableCommandFactory<FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, int> commandFactory,
            IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, long?> queryFactory)
            : base(
                authorizationService,
                logger,
                mediator,
                commandFactory,
                queryFactory)
        {
        }

        protected override async Task<EventId> GetListEventIdAsync()
        {
            return await Task.Run(() => new EventId(1));
        }

        protected override async Task<EventId> GetViewEventIdAsync()
        {
            return await Task.Run(() => new EventId(2));
        }

        protected override async Task<string> GetListPolicyAsync()
        {
            return await Task.FromResult("listPolicyName");
        }

        protected override async Task<string> GetViewPolicyAsync()
        {
            return await Task.FromResult("viewPolicyName");
        }

        protected override async Task<IActionResult> GetListActionResultAsync(IList<int> listResponse)
        {
            return await Task.FromResult(Ok(listResponse)).ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetViewActionResultAsync(long? viewResult)
        {
            return await Task.FromResult(Ok(viewResult)).ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetAddActionResultAsync(int addResult)
        {
            return await Task.FromResult(Ok(addResult)).ConfigureAwait(false);
        }

        protected override async Task<EventId> GetAddEventIdAsync()
        {
            return await Task.FromResult(new EventId(3)).ConfigureAwait(false);
        }

        protected override async Task<string> GetAddPolicyAsync()
        {
            return await Task.FromResult("addPolicyName").ConfigureAwait(false);
        }

        protected override async Task<EventId> GetDeleteEventIdAsync()
        {
            return await Task.FromResult(new EventId(4)).ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetDeleteActionResultAsync(long result)
        {
            return await Task.FromResult(Ok(result)).ConfigureAwait(false);
        }

        protected override async Task<string> GetDeletePolicyAsync()
        {
            return await Task.FromResult("deletePolicyName").ConfigureAwait(false);
        }

        protected override async Task<EventId> GetUpdateEventIdAsync()
        {
            return await Task.Run(() => new EventId(5));
        }

        protected override async Task<string> GetUpdatePolicyAsync()
        {
            return await Task.FromResult("updatePolicyName").ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetUpdateActionResultAsync(int result)
        {
            return await Task.FromResult(Ok(result)).ConfigureAwait(false);
        }
    }
}
