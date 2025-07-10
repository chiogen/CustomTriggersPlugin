using CustomTriggersPlugin.Triggers;
using Dalamud.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CustomTriggersPlugin;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    // Events
    public event EventHandler? OnSaved;

    // Debug options
    public bool Debug { get; set; } = false;
    public ushort? DebugTestChatType { get; set; }
    public string DebugTestMessage { get; set; } = "";

    // Settings
    public int Volume { get; set; } = 50;
    public bool EnableSoundQueue { get; set; } = false;

    // Triggers
    public List<Trigger> Triggers { get; set; } = [];
    public void AddTrigger(Trigger trigger)
    {
        Triggers.Add(trigger);
        Save();
    }
    public void DeleteTrigger(Trigger trigger)
    {
        if (Triggers.Remove(trigger))
            Save();
    }
    public void DeleteTriggers(List<Trigger> triggersToDelete)
    {
        Triggers.RemoveAll(trigger => triggersToDelete.Contains(trigger));
        Save();
    }


    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
        OnSaved?.Invoke(this, EventArgs.Empty);
    }
}
