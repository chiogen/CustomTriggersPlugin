using CustomTriggersPlugin.Enums;
using System.Collections.Generic;

namespace CustomTriggersPlugin;

public class Trigger
{

    public static List<Trigger> GetDebugTriggers()
    {
        List<Trigger> triggers = [];


        return triggers;
    }

    public string Key { get; set; } = "custom";
    public string Name { get; set; } = "";
    public ChatType? ChatType { get; set; }
    public string Pattern { get; set; } = "";

    public string SoundData { get; set; } = "";

}
