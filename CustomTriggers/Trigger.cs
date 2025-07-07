using CustomTriggersPlugin.Enums;
using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CustomTriggersPlugin;

public enum TriggerMatchType
{
    Regex = 0,
    Equals = 1,
    StartsWith = 2,
    Contains = 3,
    EndsWith = 4
}

[Serializable]
public class Trigger
{
    public string Key { get; set; } = "custom";
    public string Name { get; set; } = "";
    public ChatType? ChatType { get; set; }
    public TriggerMatchType MatchType { get; set; } = TriggerMatchType.Regex;
    public string Pattern { get; set; } = "";
    public string SoundData { get; set; } = "";

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Regex CompiledPattern { get; private set; }

    public Trigger()
    {
        CompiledPattern = new Regex(Pattern, RegexOptions.Compiled);
    }

    [OnDeserialized]
    public void OnDeserialized(StreamingContext context)
    {
        CompiledPattern = new Regex(Pattern, RegexOptions.Compiled);
    }

    public void UpdatePattern(string pattern)
    {
        Pattern = pattern;
        CompiledPattern = new Regex(Pattern, RegexOptions.Compiled);
    }

}
