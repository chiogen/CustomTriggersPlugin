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
        if (ImGui.Checkbox("Debug Mode##chkDebugMode", ref debugValue))
        {
            Configuration.Debug = debugValue;
            Configuration.Save();
        }

        //
        int volume = Configuration.Volume;
        if (ImGui.SliderInt("Volume##volumeSlider", ref volume, 0, 100))
        {
            Configuration.Volume = volume;
            Configuration.Save();
        }

        // 
        bool hasDeepDungeonTriggers = Configuration.Triggers.Any(t => t.Key == "DeepDungeon");
        if (hasDeepDungeonTriggers)
        {
            if (ImGui.Button("Delete DeepDungeon triggers##deleteDDTriggers"))
                DeleteDeepDungeonTriggers();
        }
        else
        {
            if (ImGui.Button("Add DeepDungeon triggers##addDDTriggers"))
                AddDeepDungeonTriggers();
        }

        //
        if (ImGui.Button("Clear Triggers##clearAllTriggers"))
            ClearTriggers();

    }

    private void AddDeepDungeonTriggers()
    {
        Configuration.Triggers.AddRange(TriggerPresets.GetDeepDungeonTriggers());
        Configuration.Save();
    }

    private void DeleteDeepDungeonTriggers()
    {
        bool dataChanged = false;

        Configuration.Triggers.RemoveAll(trigger => trigger.Key == "DeepDungeon");

        if (dataChanged)
            Configuration.Save();
    }

    private void ClearTriggers()
    {
        Configuration.Triggers.Clear();
        Configuration.Save();
    }

}
