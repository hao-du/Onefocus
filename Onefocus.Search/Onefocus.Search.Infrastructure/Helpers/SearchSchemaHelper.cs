using Microsoft.Extensions.Logging;
using OpenSearch.Client;
using DynamicTemplate = Onefocus.Common.Search.Schema.DynamicTemplate;
using FieldMapping = Onefocus.Common.Search.Schema.FieldMapping;

namespace Onefocus.Search.Infrastructure.Helpers
{
    internal class SearchSchemaHelper
    {
        public static PropertiesDescriptor<dynamic> ApplyFieldMapping(PropertiesDescriptor<dynamic> descriptor, string fieldName, FieldMapping mapping, ILogger logger)
        {
            switch (mapping.Type.ToLower())
            {
                case "keyword":
                    descriptor = descriptor.Keyword(k => k.Name(fieldName));
                    break;

                case "text":
                    descriptor = descriptor.Text(t =>
                    {
                        var textDesc = t.Name(fieldName);

                        if (!string.IsNullOrEmpty(mapping.Analyzer))
                        {
                            textDesc = textDesc.Analyzer(mapping.Analyzer);
                        }

                        if (mapping.Fields != null && mapping.Fields.Count != 0)
                        {
                            textDesc = textDesc.Fields(f =>
                            {
                                var fieldsDesc = f;
                                foreach (var subField in mapping.Fields)
                                {
                                    if (subField.Value.Type == "keyword")
                                    {
                                        fieldsDesc = fieldsDesc.Keyword(k => k.Name(subField.Key));
                                    }
                                }
                                return fieldsDesc;
                            });
                        }

                        return textDesc;
                    });
                    break;

                case "date":
                    descriptor = descriptor.Date(d =>
                    {
                        var dateDesc = d.Name(fieldName);

                        if (!string.IsNullOrEmpty(mapping.Format))
                        {
                            dateDesc = dateDesc.Format(mapping.Format);
                        }

                        return dateDesc;
                    });
                    break;

                case "boolean":
                    descriptor = descriptor.Boolean(b => b.Name(fieldName));
                    break;

                case "long":
                    descriptor = descriptor.Number(n => n.Name(fieldName).Type(NumberType.Long));
                    break;

                case "integer":
                    descriptor = descriptor.Number(n => n.Name(fieldName).Type(NumberType.Integer));
                    break;

                case "double":
                    descriptor = descriptor.Number(n => n.Name(fieldName).Type(NumberType.Double));
                    break;

                case "float":
                    descriptor = descriptor.Number(n => n.Name(fieldName).Type(NumberType.Float));
                    break;

                case "nested":
                    descriptor = descriptor.Nested<dynamic>(n =>
                    {
                        var nestedDesc = n.Name(fieldName).Dynamic(true);

                        if (mapping.NestedProperties != null && mapping.NestedProperties.Count != 0)
                        {
                            nestedDesc = nestedDesc.Properties(p =>
                            {
                                var propDesc = p;
                                foreach (var nestedField in mapping.NestedProperties)
                                {
                                    propDesc = ApplyFieldMapping(propDesc, nestedField.Key, nestedField.Value, logger);
                                }
                                return propDesc;
                            });
                        }

                        return nestedDesc;
                    });
                    break;

                default:
                    logger?.LogWarning("Unknown field type: {Type} for field: {FieldName}", mapping.Type, fieldName);
                    break;
            }

            return descriptor;
        }

        public static DynamicTemplateContainerDescriptor<dynamic> ApplyDynamicTemplate(DynamicTemplateContainerDescriptor<dynamic> descriptor, DynamicTemplate template)
        {
            return descriptor.DynamicTemplate(template.Name, dt =>
            {
                var templateDesc = dt;

                if (!string.IsNullOrEmpty(template.Match))
                {
                    templateDesc = templateDesc.Match(template.Match);
                }

                if (!string.IsNullOrEmpty(template.UnMatch))
                {
                    templateDesc = templateDesc.Unmatch(template.UnMatch);
                }

                if (!string.IsNullOrEmpty(template.MatchMappingType))
                {
                    templateDesc = templateDesc.MatchMappingType(template.MatchMappingType);
                }

                if (!string.IsNullOrEmpty(template.PathMatch))
                {
                    templateDesc = templateDesc.PathMatch(template.PathMatch);
                }

                templateDesc = templateDesc.Mapping(m =>
                {
                    return template.Mapping.Type.ToLower() switch
                    {
                        "keyword" => m.Keyword(k => k),
                        "text" => m.Text(t =>
                                                    {
                                                        if (template.Mapping.Fields?.ContainsKey("keyword") == true)
                                                        {
                                                            return t.Fields(f => f.Keyword(k => k.Name("keyword")));
                                                        }
                                                        return t;
                                                    }),
                        "date" => m.Date(d => d),
                        "long" => m.Number(n => n.Type(NumberType.Long)),
                        "double" => m.Number(n => n.Type(NumberType.Double)),
                        _ => m.Keyword(k => k),
                    };
                });

                return templateDesc;
            });
        }
    }
}
