using System;
using System.Net.Http;
using System.Security.Claims;
using AzureFromTheTrenches.Commanding.Abstractions;
using FunctionMonkey.Abstractions;
using FunctionMonkey.Abstractions.Builders;
using FunctionMonkey.FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ToDo.Commands;
using ToDo.Services;

namespace ToDo
{
    public class FunctionAppConfiguration : IFunctionAppConfiguration
    {
        public void Build(IFunctionHostBuilder builder)
        {
            builder
                .Setup((serviceCollection, commandRegistry) =>
                {
                    ApplicationSettings applicationSettings = new ApplicationSettings
                    {
                         CosmosContainerName = "todoItems",
                         CosmosDatabaseName = "testdatabase",
                         CosmosConnectionString = 
                             Environment.GetEnvironmentVariable("cosmosConnectionString", EnvironmentVariableTarget.Process)
                    };
                    serviceCollection
                        .AddSingleton(applicationSettings)
                        .AddTransient<IToDoItemRepository, ToDoItemRepository>()
                        .AddValidatorsFromAssemblyContaining<FunctionAppConfiguration>()
                        .AddSingleton<ITelemetry, Telemetry>()
                        .Replace(
                            new ServiceDescriptor(
                                typeof(ICommandDispatcher), 
                                typeof(CustomDispatcher),
                                ServiceLifetime.Transient)
                            )
                        ;
                    commandRegistry.Discover<FunctionAppConfiguration>();
                })
                .AddFluentValidation()
                .Authorization(authorization => authorization
                    .TokenValidator<TokenValidator>()
                    .AuthorizationDefault(AuthorizationTypeEnum.TokenValidation)
                    .Claims(claims => claims
                        .MapClaimToPropertyName("userId", "UserId")
                    )
                )
                .DefaultHttpResponseHandler<HttpResponseHandler>()
                .OpenApiEndpoint(openApi => openApi
                    .Title("ToDo API")
                    .Version("1.0.0")
                    .UserInterface()
                )
                .Functions(functions => functions
                    .HttpRoute("api/v1/todoItem", route => route
                        .HttpFunction<AddToDoItemCommand>(HttpMethod.Post)
                        .HttpFunction<GetAllToDoItemsQuery>(HttpMethod.Get)
                        .HttpFunction<MarkItemCompleteCommand>("/{itemId}/complete", HttpMethod.Put)
                    )
                    .ServiceBus(serviceBus => serviceBus
                        .QueueFunction<AddToDoItemCommand>("newtodoitem")
                    )
                );
        }
    }
}