using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Configurations;
using Onefocus.Common.Search;
using Onefocus.Common.Search.Schema;
using Onefocus.Search.Application.Interfaces.Services;
using OpenSearch.Client;
using System.Threading;
using DynamicTemplate = Onefocus.Common.Search.Schema.DynamicTemplate;
using FieldMapping = Onefocus.Common.Search.Schema.FieldMapping;
using Results = Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure.Services
{
    public class SearchSchemaService(IOpenSearchClient client, ILogger<SearchIndexService> logger) : ISearchSchemaService
    {
        public async Task<Results.Result> UpsertIndexMappings(MappingSchema schema, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(schema.IndexName))
                return Results.Result.Failure(Errors.IndexIsRequired);

            var existsResponse = await client.Indices.ExistsAsync(schema.IndexName, ct: cancellationToken);
            if (existsResponse.Exists)
                return await UpdateIndexMappings(schema, cancellationToken);

            return await AddIndexMappings(schema, cancellationToken);
        }

        private async Task<Results.Result> AddIndexMappings(MappingSchema schema, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating index: {IndexName}", schema.IndexName);

            try
            {
                var response = await client.Indices.CreateAsync(schema.IndexName, c => c
                    .Settings(s => s
                        .NumberOfShards(schema.Settings.NumberOfShards)
                        .NumberOfReplicas(schema.Settings.NumberOfReplicas)
                        .RefreshInterval(new Time(schema.Settings.RefreshInterval))
                    )
                    .Map<dynamic>(m =>
                    {
                        var descriptor = m.Dynamic(true);

                        // Add explicit field mappings
                        descriptor = descriptor.Properties<dynamic>(p =>
                        {
                            var propDescriptor = p;

                            foreach (var field in schema.Fields)
                            {
                                propDescriptor = SearchSchemaHelper.ApplyFieldMapping(propDescriptor, field.Key, field.Value, logger);
                            }

                            return propDescriptor;
                        });

                        // Add dynamic templates
                        if (schema.DynamicTemplates?.Any() == true)
                        {
                            descriptor = descriptor.DynamicTemplates(dt =>
                            {
                                var templateDescriptor = dt;

                                foreach (var template in schema.DynamicTemplates)
                                {
                                    templateDescriptor = SearchSchemaHelper.ApplyDynamicTemplate(templateDescriptor, template);
                                }

                                return templateDescriptor;
                            });
                        }

                        return descriptor;
                    })
                , ct: cancellationToken);

                if (!response.IsValid)
                {
                    logger.LogError( "Error creating index {IndexName}: {Error}", schema.IndexName, response.ServerError?.Error?.Reason);
                    return Results.Result.Failure(Errors.InvalidIndexCreation);
                }

                logger.LogInformation("Successfully created index: {IndexName}", schema.IndexName);
                return Results.Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception creating index {IndexName}", schema.IndexName);
                return Results.Result.Failure("IndexException", ex.Message);
            }
        }

        private async Task<Results.Result> UpdateIndexMappings(MappingSchema schema, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating mappings for index: {IndexName}", schema.IndexName);

            try
            {
                var response = await client.Indices.PutMappingAsync<dynamic>(m => m
                    .Index(schema.IndexName)
                    .Dynamic(true)
                    .Properties(p =>
                    {
                        var propDescriptor = p;

                        foreach (var field in schema.Fields)
                        {
                            propDescriptor = SearchSchemaHelper.ApplyFieldMapping(propDescriptor, field.Key, field.Value, logger);
                        }

                        return propDescriptor;
                    })
                , ct: cancellationToken);

                if (!response.IsValid)
                {
                    logger.LogError( "Error updating mappings for {IndexName}: {Error}", schema.IndexName, response.ServerError?.Error?.Reason);
                    return Results.Result.Failure(Errors.InvalidIndexCreation);
                }

                logger.LogInformation("Successfully updated mappings for: {IndexName}", schema.IndexName);
                return Results.Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception updating mappings for {IndexName}", schema.IndexName);
                return Results.Result.Failure("IndexException", ex.Message);
            }
        }
    }
}
