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
    public bool UseDeepDungeonsPreset { get; set; } = false;
    public List<ITrigger> Triggers { get; set; } = [];

    // the below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
        OnSaved?.Invoke(this, EventArgs.Empty);
    }
}
