using CustomTriggersPlugin.Triggers;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CustomTriggersPlugin;

internal class TriggersJson
{
    public int Version { get; set; } = 1;
    public List<Trigger> Triggers { get; set; } = [];
}

internal class TriggersManager : IDisposable
{
    private static string fileName = "triggers.json";

    private Plugin Plugin { get; init; }
    private readonly Configuration config;

    public List<Trigger> Triggers { get; private set; } = [];
    private readonly List<Trigger> deepDungeonTriggers;
    private readonly List<Trigger> fateTriggers;



    internal TriggersManager(Plugin plugin)
    {
        Plugin = plugin;
        config = plugin.Configuration;
        deepDungeonTriggers = TriggerPresets.GetDeepDungeonTriggers();
        fateTriggers = TriggerPresets.GetFateTriggers();
        Load();
    }

    public void Dispose()
    {
        deepDungeonTriggers.Clear();
    }

    public void Load()
    {
        string filePath = Path.Combine(Plugin.PluginInterface.ConfigDirectory.FullName, fileName);
        if (!File.Exists(filePath))
            return;

        try
        {
            string importData = File.ReadAllText(filePath);

            TriggersJson? import = JsonSerializer.Deserialize<TriggersJson>(importData);
            if (import == null)
                return;

            // maybe in future make a non-deserialized parse here. at least the verison number
            Triggers = import.Triggers;

            Log.Verbose($"Successfully imported {fileName}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed loading {fileName}");
        }
    }
    public void Save()
    {
        try
        {
            TriggersJson export = new();
            export.Triggers = Triggers;

            string exportData = JsonSerializer.Serialize(export);

            string filePath = Path.Combine(Plugin.PluginInterface.ConfigDirectory.FullName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            File.WriteAllText(filePath, exportData);
            Log.Verbose($"Successfully exported {fileName}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed saving triggers to {fileName}");
        }
    }


    public IEnumerable<Trigger> IterateTriggers()
    {
        if (config.UseDeepDungeonsPreset)
            foreach (var trigger in deepDungeonTriggers)
                yield return trigger;

        if (config.UseFateTriggersPreset)
            foreach (var trigger in fateTriggers)
                yield return trigger;

        foreach (var trigger in Triggers)
            yield return trigger;
    }


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
        int nRemoved = Triggers.RemoveAll(trigger => triggersToDelete.Contains(trigger));
        if (nRemoved > 0)
            Save();
    }
}
