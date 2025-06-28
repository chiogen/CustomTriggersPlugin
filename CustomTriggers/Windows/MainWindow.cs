using System;
using System.Numerics;
using CustomTriggersPlugin.Enums;
using Dalamud.Game.Text;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.Sheets;
using Serilog;

namespace CustomTriggersPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private string GoatImagePath { get; }
    private Plugin Plugin { get; }

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, string goatImagePath)
        : base("Triggers", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        GoatImagePath = goatImagePath;
        Plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        // Do not use .Text() or any other formatted function like TextWrapped(), or SetTooltip().
        // These expect formatting parameter if any part of the text contains a "%", which we can't
        // provide through our bindings, leading to a Crash to Desktop.
        // Replacements can be found in the ImGuiHelpers Class

        if (ImGui.Button("Show Settings"))
        {
            Plugin.ToggleConfigUI();
        }

        ImGui.Spacing();

        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().
        using var child = ImRaii.Child("TriggerTableContainer", Vector2.Zero, true);
        // Check if this child is drawing
        if (child.Success)
        {
            if (ImGui.BeginTable("TriggerTable", 6))
            {
                ImGui.TableHeadersRow();
                ImGui.TableNextColumn();
                ImGui.Text("#");
                ImGui.TableNextColumn();
                ImGui.Text("Key");
                ImGui.TableNextColumn();
                ImGui.Text("ChatType");
                ImGui.TableNextColumn();
                ImGui.Text("Pattern");
                ImGui.TableNextColumn();
                ImGui.Text("SoundData");
                ImGui.TableNextColumn();
                uint index = 0;

                foreach (Trigger trigger in Plugin.Configuration.Triggers)
                {
                    ImGui.TableNextRow();
                    // # index
                    ImGui.TableNextColumn();
                    ImGui.Text(index.ToString());
                    // # Key
                    ImGui.TableNextColumn();
                    ImGui.Text(trigger.Key);
                    // # ChatType
                    ImGui.TableNextColumn();
                    if (trigger.ChatType != null)
                        ImGui.Text(ChatTypeExt.Name((ChatType)trigger.ChatType));
                    else
                        ImGui.Text("None");
                    // # Pattern
                    ImGui.TableNextColumn();
                    ImGui.Text(trigger.Pattern);
                    // # SoundText
                    ImGui.TableNextColumn();
                    ImGui.Text(trigger.SoundData ?? "");
                    // # Buttons
                    ImGui.TableNextColumn();
                    if (trigger.SoundData == null || trigger.SoundData.Length == 0)
                        ImGui.BeginDisabled();
                    if (ImGui.Button($"Play##{index}"))
                    {
                        if (trigger.SoundData != null)
                            Plugin.TextToSpeechService.Speak(trigger.SoundData);
                        else
                            Log.Debug("trigger has no sounddata to play");
                    }
                    if (trigger.SoundData == null || trigger.SoundData.Length == 0)
                        ImGui.EndDisabled();
                    index++;
                }


                ImGui.EndTable();
            }
        }
    }
}
