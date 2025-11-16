using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Common.Configurations;

public interface ISearchSettings
{
    const string SettingName = "Search";

    string DefaultIndexName { get; }
    string Url { get; }
}

public class SearchSettings : ISearchSettings
{
    public string DefaultIndexName { get; init; } = "index_default";
    public string Url { get; init; } = "http://localhost:9200";
}

