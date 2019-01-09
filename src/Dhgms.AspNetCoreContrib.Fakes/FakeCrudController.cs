namespace Dhgms.AspNetCoreContrib.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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

    public class SwaggerClassMetaDataDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
        }
    }

    /// <summary>
    /// Class to apply Swagger Metadata described at the class level to allow
    /// for generic classes with re-usuable api functionality
    /// the still expose useful api documentation
    /// </summary>
    public class SwaggerClassMetaDataOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // var successResponseType = GetSuccessResponseType(operation, context);
            // if (successResponseType != null)
            // {
                // operation.Responses["200"].Schema = context.SchemaRegistry.GetOrRegister(successResponseType);
            // }

            operation.Responses["404"] = new Response { Description = "Not Found" };

            // var apiDesc = context.ApiDescription;

            //
            // if (customAttributes.All(ca => ca.AttributeType != typeof(SwaggerRequestExampleAttribute)))
            // {
                // customAttributes.
            // }

            // var attributes = GetActionAttributes(apiDesc);

            // if (!attributes.Any())
                // return;

            // if (operation.Responses == null)
            // {
                // operation.Responses = new Dictionary<string, Response>();
            // }

            // foreach (var attribute in attributes)
            // {
                // ApplyAttribute(operation, context, attribute);
            // }
        }

        private Type GetSuccessResponseType(Operation operation, OperationFilterContext context)
        {
            context.ApiDescription.TryGetMethodInfo(out var methodInfo);
            var declaringType = methodInfo.DeclaringType;
            var baseType = declaringType.BaseType;
            var comparison = typeof(QueryOnlyController<,,,,,>);
            if (IsSubclassOfRawGeneric(comparison, baseType))
            {
                var genericArgs = baseType.GenericTypeArguments;
                return genericArgs[3];
            }

            return null;
        }

        /// <summary>
        /// taken from https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
            // while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) {
                    return true;
                }
                // toCheck = toCheck.BaseType;
            // }
            return false;
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
    public sealed class FakeCrudController : CrudController<FakeCrudController, FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse, FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse>
    {
        public FakeCrudController(
            IAuthorizationService authorizationService,
            ILogger<FakeCrudController> logger,
            IMediator mediator,
            IAuditableCommandFactory<FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse> commandFactory,
            IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse> queryFactory)
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
            return await Task.Run(() => new EventId(1)).ConfigureAwait(false);
        }

        protected override async Task<EventId> GetViewEventIdAsync()
        {
            return await Task.Run(() => new EventId(2)).ConfigureAwait(false);
        }

        protected override async Task<string> GetListPolicyAsync()
        {
            return await Task.FromResult("listPolicyName").ConfigureAwait(false);
        }

        protected override async Task<string> GetViewPolicyAsync()
        {
            return await Task.FromResult("viewPolicyName").ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetListActionResultAsync(IList<int> listResponse)
        {
            return await Task.FromResult(this.Ok(listResponse)).ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetViewActionResultAsync(FakeCrudViewResponse viewResult)
        {
            return await Task.FromResult(this.Ok(viewResult)).ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetAddActionResultAsync(int addResult)
        {
            return await Task.FromResult(this.Ok(addResult)).ConfigureAwait(false);
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
            return await Task.FromResult(this.Ok(result)).ConfigureAwait(false);
        }

        protected override async Task<string> GetDeletePolicyAsync()
        {
            return await Task.FromResult("deletePolicyName").ConfigureAwait(false);
        }

        protected override async Task<EventId> GetUpdateEventIdAsync()
        {
            return await Task.Run(() => new EventId(5)).ConfigureAwait(false);
        }

        protected override async Task<string> GetUpdatePolicyAsync()
        {
            return await Task.FromResult("updatePolicyName").ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetUpdateActionResultAsync(FakeCrudUpdateResponse result)
        {
            return await Task.FromResult(this.Ok(result)).ConfigureAwait(false);
        }
    }
}
