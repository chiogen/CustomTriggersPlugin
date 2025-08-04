using CustomTriggersPlugin.Enums;
using System.Collections.Generic;

namespace CustomTriggersPlugin.Triggers;

internal static class TriggerPresets
{

    internal static List<Trigger> GetFateTriggers()
    {
        List<Trigger> triggers = [];

        var systemChatType = (ChatType)2105;

        void add(string name, string pattern, string soundData, TriggerMatchType matchType) => triggers.Add(new()
        {
            IsPreset = true,
            Name = name,
            ChatType = systemChatType,
            MatchType = matchType,
            Pattern = pattern,
            SoundData = soundData
        });

        add("Fate|Maiden", "The smell of death has drawn a Forlorn maiden to the battlefield!", "Maiden", TriggerMatchType.Equals);


        return triggers;
    }

    internal static List<Trigger> GetDeepDungeonTriggers()
    {
        List<Trigger> triggers = [];
        var systemChatType = (ChatType)2105;

        void add(string name, string pattern, string soundData, TriggerMatchType matchType) => triggers.Add(new()
        {
            IsPreset = true,
            Name = name,
            ChatType = systemChatType,
            MatchType = matchType,
            Pattern = pattern,
            SoundData = soundData
        });

        void addItem(string itemName, string? soundData = null)
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
        }

        // Add Exists
        foreach (var exitName in new[] { "Pylon", "Cairn", "Beacon" })
            add("Exit", $"The {exitName} of Passage is activated!", "Exit", TriggerMatchType.Equals);

        add("Safety", "All the traps on this floor have disappeared!", "Safety up", TriggerMatchType.Equals);
        add("Mimic", "The coffer...bares its fangs!", "Mimic", TriggerMatchType.Equals);

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

        // Add Magicites
        foreach (var name in new[] { "Crag", "Vortex", "Elder", "Inferno" })
            add($"Magicite|{name}", $"You return the splinter of {name} magicite to the coffer. You cannot carry any more of that item.", "Magicite", TriggerMatchType.Equals);

        // Add EO specials
        add("DreadBeast", "A dread beast stalks this floor", "Danger", TriggerMatchType.StartsWith);
        add("Unei", "You return the Unei demiclone to the coffer. You cannot carry any more of that item.", "Unei", TriggerMatchType.Equals);
        add("Doga", "You return the Doga demiclone to the coffer. You cannot carry any more of that item.", "Doga", TriggerMatchType.Equals);
        add("Onion Knight", "You return the onion knight demiclone to the coffer. You cannot carry any more of that item.", "Onion", TriggerMatchType.Equals);

        return triggers;
    }

}
