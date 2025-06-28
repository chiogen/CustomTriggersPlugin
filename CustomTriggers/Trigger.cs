using Dalamud.Game.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public XivChatType? ChatType { get; set; }
    public string Pattern { get; set; } = "";

    public string SoundData { get; set; } = "";

    public bool Match(string message)
    {
        if (Pattern.Length == 0)
            return false;

        // simple string match for now for testing
        if (message.Contains(Pattern))
            return true;

        return false;
    }

}
