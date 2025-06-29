using System;
using System.Linq;
using System.Numerics;
using CustomTriggersPlugin.Enums;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace CustomTriggersPlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration { get; }

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("Settings")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(232, 200);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        Flags &= ~ImGuiWindowFlags.NoMove;
    }

    public override void Draw()
    {
        // can't ref a property, so use a local copy
        bool debugValue = Configuration.Debug;
        if (ImGui.Checkbox("Debug Mode", ref debugValue))
        {
            Configuration.Debug = debugValue;
            Configuration.Save();
        }

        //
        int volume = Configuration.Volume;
        if (ImGui.SliderInt("Volume", ref volume, 0, 100))
        {
            Configuration.Volume = volume;
            Configuration.Save();
        }

        // 
        bool hasDeepDungeonTriggers = Configuration.Triggers.Any(t => t.Key == "DeepDungeon");
        if (hasDeepDungeonTriggers)
            ImGui.BeginDisabled();
        if (ImGui.Button("Add DeepDungeon triggers"))
            AddDeepDungeonTriggers();
        if (hasDeepDungeonTriggers)
            ImGui.EndDisabled();

        //
        if (ImGui.Button("Clear Triggers"))
            ClearTriggers();

    }

    private void AddDeepDungeonTriggers()
    {
        var add = (string name, string pattern, string soundData) => Configuration.Triggers.Add(new()
        {
            Key = "DeepDungeon",
            Name = name,
            ChatType = (ChatType)2105,
            Pattern = pattern,
            SoundData = soundData
        });

        var addItem = (string itemName, string? soundData) => add(itemName, $"You return the (protomander|pomander) of {itemName} to the coffer. You cannot carry any more of that item.", soundData ?? itemName);

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
        addItem("beast", null);
        addItem("lethargy", null);
        addItem("storms", null);
        addItem("frailty", null);
        addItem("concealment", "conceal");
        addItem("petrification", "petri");
        addItem("lust", null);
        addItem("rage", null);
        addItem("resolution", "reso");

        add("Magicite", "You obtain a splinter of (Crag|Vortex|Elder|Inferno) magicite\\.", "Magicite");
        add("Dread", "A dread beast stalks this floor\\.\\.\\.", "Dread");

        Configuration.Save();

    }

    private void ClearTriggers()
    {
        Configuration.Triggers.Clear();
        Configuration.Save();
    }

}
