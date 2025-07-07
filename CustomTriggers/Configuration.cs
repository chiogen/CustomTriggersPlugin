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

    // General options
    public bool Debug { get; set; } = false;

    // Settings
    public int Volume { get; set; } = 50;
    public bool EnableSoundQueue { get; set; } = false;

    // Triggers
    public List<BasicTrigger> Triggers { get; set; } = [];
    public void AddTrigger(BasicTrigger trigger)
    {
        Triggers.Add(trigger);
        Save();
    }
    public void DeleteTrigger(BasicTrigger trigger)
    {
        if (Triggers.Remove(trigger))
            Save();
    }
    public void DeleteTriggers(List<BasicTrigger> triggersToDelete)
    {
        Triggers.RemoveAll(trigger => triggersToDelete.Contains(trigger));
        Save();
    }


    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
