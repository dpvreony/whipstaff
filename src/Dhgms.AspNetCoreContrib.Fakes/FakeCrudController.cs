using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [SwaggerClassMetaData(typeof(FakeCrudControllerSwaggerMetaData))]
    public sealed class FakeCrudController : CrudController<FakeCrudController, int, int, int, int, int, int, int, int, int, int>
    {
        public FakeCrudController(
            IAuthorizationService authorizationService,
            ILogger<FakeCrudController> logger,
            IMediator mediator,
            IAuditableCommandFactory<int, int, int, int, int> commandFactory,
            IAuditableQueryFactory<int, int, int> queryFactory)
            : base(
                authorizationService,
                logger,
                mediator,
                commandFactory,
                queryFactory)
        {
        }

        protected override async Task<EventId> GetOnListEventIdAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task<EventId> GetOnViewEventIdAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task<AuthorizationPolicy> GetListPolicyAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task<AuthorizationPolicy> GetViewPolicyAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task<IActionResult> GetListActionResultAsync(int listResponse)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IActionResult> GetViewActionResultAsync(int listResponse)
        {
            throw new NotImplementedException();
        }

        protected override Task<IActionResult> GetAddActionResultAsync(int result)
        {
            throw new NotImplementedException();
        }

        protected override Task<EventId> GetAddEventIdAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<AuthorizationPolicy> GetAddPolicyAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<EventId> GetDeleteEventIdAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<IActionResult> GetDeleteActionResultAsync(int result)
        {
            throw new NotImplementedException();
        }

        protected override Task<AuthorizationPolicy> GetDeletePolicyAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<EventId> GetUpdateEventIdAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<AuthorizationPolicy> GetUpdatePolicyAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task<IActionResult> GetUpdateActionResultAsync(int result)
        {
            throw new NotImplementedException();
        }
    }
}
