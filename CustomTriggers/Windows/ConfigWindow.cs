using System;
using System.Linq;
using System.Numerics;
using System.Speech.Synthesis;
using CustomTriggersPlugin.Enums;
using CustomTriggersPlugin.Triggers;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace CustomTriggersPlugin.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Plugin Plugin { get; }
    private Configuration Configuration { get; }

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("Settings")
    {
        Flags = ImGuiWindowFlags.NoCollapse;

        //Size = new Vector2(232, 200);
        SizeCondition = ImGuiCond.Always;

        Plugin = plugin;
        Configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        Flags &= ~ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize;
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
        DrawTextToSpeachConfig();
        DrawPresetsConfig();

        //
        if (ImGui.Button("Clear Triggers##clearAllTriggers"))
            ClearTriggers();

    }

    private void DrawTextToSpeachConfig()
    {
        var tts = Plugin.TextToSpeechService;

        if (ImGui.CollapsingHeader("Text To Speech"))
        {
            string selectedVoice = tts.Voice;
            if (ImGui.BeginCombo($"Voice##SelectVoice", selectedVoice))
            {
                foreach (InstalledVoice voice in Plugin.TextToSpeechService.InstalledVoices)
                {
                    bool isSelected = voice.VoiceInfo.Id == selectedVoice;
                    if (ImGui.Selectable($"{voice.VoiceInfo.Name}##{voice.VoiceInfo.Id}", isSelected))
                    {
                        Configuration.Voice = voice.VoiceInfo.Name;
                        Configuration.Save();
                    }
                    if (isSelected)
                        ImGui.SetItemDefaultFocus();
                }

                ImGui.EndCombo();
            }

            int volume = Configuration.Volume;
            if (ImGui.SliderInt("Volume##volumeSlider", ref volume, 0, 100))
            {
                Configuration.Volume = volume;
                Configuration.Save();
            }

            bool enableSoundQueue = Configuration.EnableSoundQueue;
            if (ImGui.Checkbox("Enable SoundQueue", ref enableSoundQueue))
            {
                Configuration.EnableSoundQueue = enableSoundQueue;
                Configuration.Save();
            }


            if (ImGuiToolkit.Button("Reload installed voices"))
                tts.Refresh();
        }
    }

    private void DrawPresetsConfig()
    {
        if (ImGui.CollapsingHeader("Presets"))
        {
            bool useDeepDungeonsPreset = Configuration.UseDeepDungeonsPreset;
            if (ImGui.Checkbox("Deep Dungeons", ref useDeepDungeonsPreset))
            {
                Configuration.UseDeepDungeonsPreset = useDeepDungeonsPreset;
                Configuration.Save();
            }
        }
    }

    private void ClearTriggers()
    {
        Configuration.Triggers.Clear();
        Configuration.Save();
    }

}
