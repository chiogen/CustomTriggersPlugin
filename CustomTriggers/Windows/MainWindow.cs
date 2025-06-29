using System;
using System.Numerics;
using CustomTriggersPlugin.Enums;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Serilog;

namespace CustomTriggersPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin { get; }

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin)
        : base("Triggers", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

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
        RenderTriggersTable();
    }

    private void RenderTriggersTable()
    {

        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().
        using var child = ImRaii.Child("TriggerTableContainer", Vector2.Zero, true);
        // Check if this child is drawing
        if (child.Success)
        {
            ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(10, 2)); // X and Y padding

            if (ImGui.BeginTable("TriggerTable", 6))
            {
                ImGui.TableSetupColumn("#", ImGuiTableColumnFlags.WidthFixed, 30f);
                ImGui.TableSetupColumn("Key", ImGuiTableColumnFlags.WidthFixed, 150f);
                ImGui.TableSetupColumn("ChatType", ImGuiTableColumnFlags.WidthFixed, 150f);
                ImGui.TableSetupColumn("Pattern", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("SoundData", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 100f);
                ImGui.TableHeadersRow();
                uint rowIndex = 0;

                foreach (Trigger trigger in Plugin.Configuration.Triggers)
                    RenderTriggerRow(trigger, rowIndex++);

                ImGui.EndTable();
                ImGui.PopStyleVar();
            }
        }

    }
    private void RenderTriggerRow(Trigger trigger, uint rowIndex)
    {
        // # Start row
        ImGui.TableNextRow();

        // # index
        ImGui.TableNextColumn();
        ImGui.Text(rowIndex.ToString());

        // # Key
        ImGui.TableNextColumn();
        ImGui.Text(trigger.Key);

        // # ChatType
        ImGui.TableNextColumn();
        RenderChatTypeDropDown(trigger, rowIndex);

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
        if (ImGui.Button($"Play##playbtn-{rowIndex}"))
        {
            if (trigger.SoundData != null)
                Plugin.TextToSpeechService.Speak(trigger.SoundData);
            else
                Log.Debug("trigger has no sounddata to play");
        }
        if (trigger.SoundData == null || trigger.SoundData.Length == 0)
            ImGui.EndDisabled();

    }
    private void RenderChatTypeDropDown(Trigger trigger, uint rowIndex)
    {
        string currentValue = trigger.ChatType == null
            ? "None"
            : ChatTypeExt.Name((ChatType)trigger.ChatType);

        ChatType? selectedValue = trigger.ChatType;

        ImGui.SetNextItemWidth(-1);

        if (ImGui.BeginCombo($"##cbox-{rowIndex}", currentValue))
        {

            bool noneSelected = trigger.ChatType == null;
            if (ImGui.Selectable($"None##clear-{rowIndex}", noneSelected))
                selectedValue = null;
            if (trigger.ChatType == null)
                ImGui.SetItemDefaultFocus();

            foreach (ChatType value in Enum.GetValues<ChatType>())
            {
                bool isSelected = value == trigger.ChatType;
                if (ImGui.Selectable(ChatTypeExt.Name(value), isSelected))
                    selectedValue = value;
                if (isSelected)
                    ImGui.SetItemDefaultFocus();
            }

            ImGui.EndCombo();
        }

        // Update config when value has changed
        if (selectedValue != trigger.ChatType)
        {
            trigger.ChatType = selectedValue;
            Plugin.Configuration.Save();
        }
    }
}
