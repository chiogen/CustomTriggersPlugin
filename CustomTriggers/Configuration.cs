using Dalamud.Configuration;
using System;
using System.Collections.Generic;

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

    // Data
    public List<Trigger> Triggers { get; set; } = [];



    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
