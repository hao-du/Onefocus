namespace Onefocus.Common.Configurations;

public interface IEmbeddingSettings
{
    const string SettingName = "Embedding";

    string BaseAddress { get; }
}

public class EmbeddingSettings : IEmbeddingSettings
{
    public string BaseAddress { get; init; } = "http://localhost:15601";
}

