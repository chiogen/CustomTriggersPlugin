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

        var addItem = (string itemName, string? soundData = null) => add(
            itemName,
            $"You return the (protomander|pomander) of {itemName} to the coffer. You cannot carry any more of that item.",
            soundData ?? itemName
        );

        add("Exit", "The (Pylon|Cairn|Beacon) of Passage is activated\\!", "Exit");
        add("Safety", "All the traps on this floor have disappeared\\!", "Safety up");

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

        add("Magicite", "You obtain a splinter of (Crag|Vortex|Elder|Inferno) magicite\\.", "Magicite");
        add("DreadBeast", "A dread beast stalks this floor\\.\\.\\.", "Danger");
        //add("Unei", "You return the Unei demiclone to the coffer\\. You cannot carry any more of that item\\.", "Unei");
        //add("Doga", "You return the Unei demiclone to the coffer\\. You cannot carry any more of that item\\.", "Doga");
        //add("Onion Knight", "You return the onion knight demiclone to the coffer\\. You cannot carry any more of that item\\.", "Onion");

        return triggers;
    }

}
