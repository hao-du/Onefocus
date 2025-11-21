namespace Onefocus.Search.Infrastructure.MachineLearning.Models;

public class TokenizerOutput
{
    public required long[] InputIds { get; set; }
    public required long[] AttentionMask { get; set; }
    public required int[] TokenTypeIds { get; set; }
}
