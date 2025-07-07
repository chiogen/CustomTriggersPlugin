using CustomTriggersPlugin.Enums;
using CustomTriggersPlugin.Triggers;
using System.Collections.Generic;
using System.IO;

namespace CustomTriggersPlugin;

internal static class TriggerPresets
{

    internal static List<BasicTrigger> GetDeepDungeonTriggers()
    {
        List<BasicTrigger> triggers = [];
        string key = "DeepDungeon";
        ChatType systemChatType = (ChatType)2105;

        var add = (string name, string pattern, string soundData, TriggerMatchType matchType) => triggers.Add(new()
        {
            Key = key,
            Name = name,
            ChatType = systemChatType,
            MatchType = matchType,
            Pattern = pattern,
            SoundData = soundData
        });

        var addItem = (string itemName, string? soundData = null) =>
        {
            add(
                itemName,
                $"You return the pomander of {itemName} to the coffer. You cannot carry any more of that item.",
                soundData ?? itemName,
                TriggerMatchType.Equals
            );
            add(
                itemName,
                $"You return the protomander of {itemName} to the coffer. You cannot carry any more of that item.",
                soundData ?? itemName,
                TriggerMatchType.Equals
            );
        };

        add("Exit", "The (Pylon|Cairn|Beacon) of Passage is activated!", "Exit", TriggerMatchType.Regex);
        add("Safety", "All the traps on this floor have disappeared!", "Safety up", TriggerMatchType.Equals);

        addItem("affluence");
        addItem("alteration");
        addItem("flight");
        addItem("fortune");
        addItem("intuition");
        addItem("purity");
        addItem("raising");
        addItem("safety");
        addItem("serenity");
        addItem("sight");
        addItem("steel");
        addItem("strength");
        addItem("witching");
        addItem("dread");
        addItem("lethargy");
        addItem("storms");
        addItem("frailty");
        addItem("concealment", "conceal");
        addItem("petrification", "petri");
        addItem("lust");
        addItem("rage");
        addItem("resolution", "reso");

        add("Magicite", "You obtain a splinter of (Crag|Vortex|Elder|Inferno) magicite", "Magicite", TriggerMatchType.Regex);
        add("DreadBeast", "A dread beast stalks this floor", "Danger", TriggerMatchType.StartsWith);
        add("Unei", "You return the Unei demiclone to the coffer. You cannot carry any more of that item.", "Unei", TriggerMatchType.Equals);
        add("Doga", "You return the Unei demiclone to the coffer. You cannot carry any more of that item.", "Doga", TriggerMatchType.Equals);
        add("Onion Knight", "You return the onion knight demiclone to the coffer. You cannot carry any more of that item.", "Onion", TriggerMatchType.Equals);

        return triggers;
    }

}
