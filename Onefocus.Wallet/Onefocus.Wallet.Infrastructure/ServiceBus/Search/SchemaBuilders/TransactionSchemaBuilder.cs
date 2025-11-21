using Onefocus.Common.Search.Schema;

namespace Onefocus.Wallet.Infrastructure.ServiceBus.Search.SchemaBuilders;

internal class TransactionSchemaBuilder
{
    internal static MappingSchema BuildMappingSchema()
    {
        return new MappingSchema
        {
            SchemaName = "transactions",
            IndexName = "transactions",
            Settings = new SchemaSettings
            {
                NumberOfShards = 1,
                NumberOfReplicas = 1,
                RefreshInterval = "1s"
            },
            Fields = new Dictionary<string, FieldMapping>
            {
                // Common fields
                ["Id"] = new FieldMapping { Type = "keyword" },
                ["Type"] = new FieldMapping
                {
                    Type = "keyword",
                    Index = true
                },
                ["Description"] = new FieldMapping
                {
                    Type = "text",
                    Analyzer = "standard",
                    Fields = new Dictionary<string, FieldMapping>
                    {
                        ["Keyword"] = new FieldMapping { Type = "keyword" }
                    }
                },
                ["IsActive"] = new FieldMapping { Type = "boolean" },
                ["OwnerUserId"] = new FieldMapping { Type = "keyword" },
                ["CreatedAt"] = new FieldMapping
                {
                    Type = "date",
                    Format = "strict_date_optional_time||epoch_millis"
                },
                ["UpdatedAt"] = new FieldMapping
                {
                    Type = "date",
                    Format = "strict_date_optional_time||epoch_millis"
                },

                // BankAccount specific
                ["BankId"] = new FieldMapping { Type = "keyword" },
                ["BankName"] = new FieldMapping
                {
                    Type = "text",
                    Fields = new Dictionary<string, FieldMapping>
                    {
                        ["Keyword"] = new FieldMapping { Type = "keyword" }
                    }
                },
                ["CurrencyId"] = new FieldMapping { Type = "keyword" },
                ["CurrencyName"] = new FieldMapping { Type = "keyword" },
                ["AccountNumber"] = new FieldMapping { Type = "keyword" },
                ["IssuedOn"] = new FieldMapping { Type = "date" },
                ["IsClosed"] = new FieldMapping { Type = "boolean" },
                ["ClosedOn"] = new FieldMapping { Type = "date" },

                // CashFlow specific
                ["TransactedOn"] = new FieldMapping { Type = "date" },
                ["Amount"] = new FieldMapping { Type = "double" },

                // PeerTransfer specific
                ["CounterpartyId"] = new FieldMapping { Type = "keyword" },
                ["CounterpartyName"] = new FieldMapping
                {
                    Type = "text",
                    Fields = new Dictionary<string, FieldMapping>
                    {
                        ["Keyword"] = new FieldMapping { Type = "keyword" }
                    }
                },
                ["Status"] = new FieldMapping { Type = "keyword" },
                ["PeerTransferType"] = new FieldMapping { Type = "keyword" },

                // Nested transactions
                ["Transactions"] = new FieldMapping
                {
                    Type = "nested",
                    NestedProperties = new Dictionary<string, FieldMapping>
                    {
                        ["Id"] = new FieldMapping { Type = "keyword" },
                        ["TransactedOn"] = new FieldMapping { Type = "date" },
                        ["Description"] = new FieldMapping
                        {
                            Type = "text",
                            Fields = new Dictionary<string, FieldMapping>
                            {
                                ["Keyword"] = new FieldMapping { Type = "keyword" }
                            }
                        },
                        ["Amount"] = new FieldMapping { Type = "double" },
                        ["IsActive"] = new FieldMapping { Type = "boolean" },
                        ["CurrencyId"] = new FieldMapping { Type = "keyword" },
                        ["CurrencyName"] = new FieldMapping { Type = "keyword" },
                        ["IsTarget"] = new FieldMapping { Type = "boolean" },
                        ["IsInFlow"] = new FieldMapping { Type = "boolean" },
                        ["Name"] = new FieldMapping
                        {
                            Type = "text",
                            Fields = new Dictionary<string, FieldMapping>
                            {
                                ["Keyword"] = new FieldMapping { Type = "keyword" }
                            }
                        }
                    }
                },
                // Nested items
                ["Items"] = new FieldMapping
                {
                    Type = "nested",
                    NestedProperties = new Dictionary<string, FieldMapping>
                    {
                        ["Id"] = new FieldMapping { Type = "keyword" },
                        ["Description"] = new FieldMapping
                        {
                            Type = "text",
                            Fields = new Dictionary<string, FieldMapping>
                            {
                                ["Keyword"] = new FieldMapping { Type = "keyword" }
                            }
                        },
                        ["IsActive"] = new FieldMapping { Type = "boolean" },
                        ["Name"] = new FieldMapping
                        {
                            Type = "text",
                            Fields = new Dictionary<string, FieldMapping>
                            {
                                ["Keyword"] = new FieldMapping { Type = "keyword" }
                            }
                        }
                    }
                }
            },
            DynamicTemplates =
            [
                new DynamicTemplate
                {
                    Name = "ids_as_keywords",
                    Match = "*Id",
                    MatchMappingType = "string",
                    Mapping = new FieldMapping { Type = "keyword" }
                },
                new DynamicTemplate
                {
                    Name = "names_as_text_with_keyword",
                    Match = "*Name",
                    MatchMappingType = "string",
                    Mapping = new FieldMapping
                    {
                        Type = "text",
                        Fields = new Dictionary<string, FieldMapping>
                        {
                            ["keyword"] = new FieldMapping { Type = "keyword" }
                        }
                    }
                },
                new DynamicTemplate
                {
                    Name = "dates_as_date_type",
                    Match = "*On",
                    MatchMappingType = "string",
                    Mapping = new FieldMapping { Type = "date" }
                },
                new DynamicTemplate
                {
                    Name = "amounts_as_double",
                    Match = "*Amount",
                    MatchMappingType = "long",
                    Mapping = new FieldMapping { Type = "double" }
                }
            ]
        };
    }
}
