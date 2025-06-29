using CustomTriggersPlugin.Enums;
using System;

namespace CustomTriggersPlugin;

[Serializable]
public class Trigger
{
    public string Key { get; set; } = "custom";
    public string Name { get; set; } = "";
    public ChatType? ChatType { get; set; }
    public string Pattern { get; set; } = "";
    public string SoundData { get; set; } = "";
}
