using Microsoft.Extensions.Logging;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;
using Onefocus.Search.Infrastructure.Helpers;
using OpenSearch.Client;
using Results = Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure.Services
{
    public class SearchSchemaService(IOpenSearchClient client, ILogger<SearchIndexService> logger) : ISearchSchemaService
    {
        public async Task<Results.Result> UpsertIndexMappings(SearchSchemaDto searchSchemaDto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(searchSchemaDto.IndexName))
                return Results.Result.Failure(Errors.IndexIsRequired);

            var existsResponse = await client.Indices.ExistsAsync(searchSchemaDto.IndexName, ct: cancellationToken);
            if (existsResponse.Exists)
                return await UpdateIndexMappings(searchSchemaDto, cancellationToken);

            return await AddIndexMappings(searchSchemaDto, cancellationToken);
        }

        private async Task<Results.Result> AddIndexMappings(MappingSchema schema, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating index: {IndexName}", schema.IndexName);

            try
            {
                var response = await client.Indices.CreateAsync(schema.IndexName, c => c
                    .Settings(settings => AddAnalysis(schema, settings))
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
                        if (schema.DynamicTemplates != null && schema.DynamicTemplates.Count != 0)
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
                    logger.LogError("Error creating index {IndexName}: {Error}", schema.IndexName, response.ServerError?.Error?.Reason);
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
                    logger.LogError("Error updating mappings for {IndexName}: {Error}", schema.IndexName, response.ServerError?.Error?.Reason);
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

        private static IndexSettingsDescriptor AddAnalysis(MappingSchema schema, IndexSettingsDescriptor settings)
        {
            var settingsDescriptor = settings
                    .NumberOfShards(schema.Settings.NumberOfShards)
                    .NumberOfReplicas(schema.Settings.NumberOfReplicas)
                    .RefreshInterval(new Time(schema.Settings.RefreshInterval));

            if (settings.Analysis != null)
            {
                settingsDescriptor = settingsDescriptor.Analysis(a =>
                {
                    var analysisDescriptor = a;

                    // Add analyzers
                    if (schema.Settings.Analysis?.Analyzer != null)
                    {
                        foreach (var analyzer in schema.Settings.Analysis.Analyzer)
                        {
                            analysisDescriptor = analysisDescriptor.Analyzers(aa => aa
                                .Custom(analyzer.Key, ca =>
                                {
                                    var customAnalyzer = ca.Tokenizer(analyzer.Value.Tokenizer);

                                    if (analyzer.Value.Filter != null && analyzer.Value.Filter.Any())
                                    {
                                        customAnalyzer = customAnalyzer.Filters(analyzer.Value.Filter);
                                    }

                                    if (analyzer.Value.CharFilter != null && analyzer.Value.CharFilter.Any())
                                    {
                                        customAnalyzer = customAnalyzer.CharFilters(analyzer.Value.CharFilter);
                                    }

                                    return customAnalyzer;
                                })
                            );
                        }
                    }

                    // Add token filters
                    if (schema.Settings.Analysis?.Filter != null)
                    {
                        foreach (var filter in schema.Settings.Analysis.Filter)
                        {
                            analysisDescriptor = analysisDescriptor.TokenFilters(tf =>
                            {
                                var tokenFilterDescriptor = tf;

                                switch (filter.Value.Type?.ToLower())
                                {
                                    case "asciifolding":
                                        tokenFilterDescriptor = tokenFilterDescriptor.AsciiFolding(filter.Key, af =>
                                        {
                                            var asciiFolding = af;
                                            if (filter.Value.PreserveOriginal.HasValue)
                                            {
                                                asciiFolding = asciiFolding.PreserveOriginal(filter.Value.PreserveOriginal.Value);
                                            }
                                            return asciiFolding;
                                        });
                                        break;

                                    case "edge_ngram":
                                        tokenFilterDescriptor = tokenFilterDescriptor.EdgeNGram(filter.Key, en =>
                                        {
                                            var edgeNGram = en;
                                            if (filter.Value.MinGram.HasValue)
                                            {
                                                edgeNGram = edgeNGram.MinGram(filter.Value.MinGram.Value);
                                            }
                                            if (filter.Value.MaxGram.HasValue)
                                            {
                                                edgeNGram = edgeNGram.MaxGram(filter.Value.MaxGram.Value);
                                            }
                                            return edgeNGram;
                                        });
                                        break;

                                    case "stop":
                                        tokenFilterDescriptor = tokenFilterDescriptor.Stop(filter.Key, st =>
                                        {
                                            var stop = st;
                                            if (filter.Value.Stopwords != null && filter.Value.Stopwords.Count != 0)
                                            {
                                                stop = stop.StopWords(filter.Value.Stopwords.AsEnumerable());
                                            }
                                            return stop;
                                        });
                                        break;

                                    case "lowercase":
                                        tokenFilterDescriptor = tokenFilterDescriptor.Lowercase(filter.Key);
                                        break;

                                    case "uppercase":
                                        tokenFilterDescriptor = tokenFilterDescriptor.Uppercase(filter.Key);
                                        break;
                                }

                                return tokenFilterDescriptor;
                            });
                        }
                    }

                    // Add tokenizers if present
                    if (schema.Settings.Analysis?.Tokenizer != null)
                    {
                        foreach (var tokenizer in schema.Settings.Analysis.Tokenizer)
                        {
                            analysisDescriptor = analysisDescriptor.Tokenizers(t =>
                            {
                                var tokenizerDescriptor = t;

                                switch (tokenizer.Value.Type?.ToLower())
                                {
                                    case "ngram":
                                        tokenizerDescriptor = tokenizerDescriptor.NGram(tokenizer.Key, ng =>
                                        {
                                            var nGram = ng;
                                            if (tokenizer.Value.MinGram.HasValue)
                                            {
                                                nGram = nGram.MinGram(tokenizer.Value.MinGram.Value);
                                            }
                                            if (tokenizer.Value.MaxGram.HasValue)
                                            {
                                                nGram = nGram.MaxGram(tokenizer.Value.MaxGram.Value);
                                            }
                                            if (tokenizer.Value.TokenChars != null && tokenizer.Value.TokenChars.Any())
                                            {
                                                nGram = nGram.TokenChars(tokenizer.Value.TokenChars.Select(tc =>
                                                    Enum.Parse<TokenChar>(tc, true)));
                                            }
                                            return nGram;
                                        });
                                        break;

                                    case "edge_ngram":
                                        tokenizerDescriptor = tokenizerDescriptor.EdgeNGram(tokenizer.Key, en =>
                                        {
                                            var edgeNGram = en;
                                            if (tokenizer.Value.MinGram.HasValue)
                                            {
                                                edgeNGram = edgeNGram.MinGram(tokenizer.Value.MinGram.Value);
                                            }
                                            if (tokenizer.Value.MaxGram.HasValue)
                                            {
                                                edgeNGram = edgeNGram.MaxGram(tokenizer.Value.MaxGram.Value);
                                            }
                                            if (tokenizer.Value.TokenChars != null && tokenizer.Value.TokenChars.Any())
                                            {
                                                edgeNGram = edgeNGram.TokenChars(tokenizer.Value.TokenChars.Select(tc =>
                                                    Enum.Parse<TokenChar>(tc, true)));
                                            }
                                            return edgeNGram;
                                        });
                                        break;
                                }

                                return tokenizerDescriptor;
                            });
                        }
                    }

                    return analysisDescriptor;
                });
            }
            return settingsDescriptor;
        }
    }
}
