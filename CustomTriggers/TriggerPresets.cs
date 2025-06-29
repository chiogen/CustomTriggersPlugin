using CustomTriggersPlugin.Enums;
using System.Collections.Generic;

namespace CustomTriggersPlugin;

internal static class TriggerPresets
{

    internal static List<Trigger> GetDeepDungeonTriggers()
    {
        List<Trigger> triggers = [];

        var add = (string name, string pattern, string soundData) => triggers.Add(new()
        {
            Key = "DeepDungeon",
            Name = name,
            ChatType = (ChatType)2105,
            Pattern = pattern,
            SoundData = soundData
        });

        var addItem = (string itemName, string? soundData) => add(
            itemName,
            $"You return the (protomander|pomander) of {itemName} to the coffer. You cannot carry any more of that item.",
            soundData ?? itemName
        );

        add("Exit", "The (Pylon|Cairn|Beacon) of Passage is activated\\!", "Exit");
        add("Safety", "All the traps on this floor have disappeared\\!", "Safety up");

        addItem("affluence", null);
        addItem("alteration", null);
        addItem("flight", null);
        addItem("fortune", null);
        addItem("intuition", null);
        addItem("purity", null);
        addItem("raising", null);
        addItem("safety", null);
        addItem("serenity", null);
        addItem("sight", null);
        addItem("steel", null);
        addItem("strength", null);
        addItem("witching", null);
        addItem("dread", null);
        addItem("lethargy", null);
        addItem("storms", null);
        addItem("frailty", null);
        addItem("concealment", "conceal");
        addItem("petrification", "petri");
        addItem("lust", null);
        addItem("rage", null);
        addItem("resolution", "reso");

        add("Magicite", "You obtain a splinter of (Crag|Vortex|Elder|Inferno) magicite\\.", "Magicite");
        add("DreadBeast", "A dread beast stalks this floor\\.\\.\\.", "Danger");

        return triggers;
    }

}
