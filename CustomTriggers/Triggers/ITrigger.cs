using CustomTriggersPlugin.Enums;

namespace CustomTriggersPlugin.Triggers;

public interface ITrigger
{
    bool IsPreset { get; init; }
    string Name { get; set; }
    ChatType? ChatType { get; set; }
    TriggerMatchType MatchType { get; set; }
    string Pattern { get; set; }
    string SoundData { get; set; }
}

