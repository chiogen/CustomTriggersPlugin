using System;
using System.Numerics;
using CustomTriggersPlugin.Enums;
using Dalamud.Interface.Utility;
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

        RenderTopBar();
        ImGui.Spacing();
        RenderTriggersTable();
    }
    private void RenderTopBar()
    {
        // Some logic to move button to the right
        string showSettingsLabel = "Show Settings";
        float settingsButtonWidth = ImGui.CalcTextSize(showSettingsLabel).X + (ImGui.GetStyle().FramePadding.X * 2);
        float availableSpace = ImGui.GetContentRegionAvail().X;
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + availableSpace - settingsButtonWidth);

        if (ImGui.Button(showSettingsLabel))
            Plugin.ToggleConfigUI();
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
                ImGui.TableSetupColumn("SoundData", ImGuiTableColumnFlags.WidthFixed, 200f);
                ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 100f);
                ImGui.TableHeadersRow();
                uint rowIndex = 0;

                foreach (Trigger trigger in Plugin.Configuration.Triggers)
                    RenderTriggerRow(trigger, rowIndex++);

                RenderNewEntryRow(rowIndex++);

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
        ImGuiHelpers.SafeTextWrapped(rowIndex.ToString());

        // # Key
        ImGui.TableNextColumn();
        ImGuiHelpers.SafeTextWrapped(trigger.Key);

        // # ChatType
        ImGui.TableNextColumn();
        ChatType? chatType = trigger.ChatType;
        if (RenderChatTypeDropDown(rowIndex, ref chatType, trigger.Key != "custom"))
        {
            trigger.ChatType = chatType;
            Plugin.Configuration.Save();
        }

        // # Pattern
        ImGui.TableNextColumn();
        ImGuiHelpers.SafeTextWrapped(trigger.Pattern);

        // # SoundText
        ImGui.TableNextColumn();
        ImGui.SetNextItemWidth(-1);
        string soundData = trigger.SoundData;
        if (ImGui.InputText($"##updateSoundData{rowIndex}", ref soundData, 20))
        {
            trigger.SoundData = soundData;
            Plugin.Configuration.Save();
        }

        // # Buttons Cell (render all buttons into 1 cell)
        ImGui.TableNextColumn();
        // # Play button
        if (ImGui.Button($"Play##playbtn-{rowIndex}"))
        {
            if (trigger.SoundData != null)
                Plugin.TextToSpeechService.Speak(trigger.SoundData);
            else
                Log.Debug("trigger has no sounddata to play");
        }
        // Delete Button
        ImGui.SameLine();
        if (ImGuiToolkit.Button($"Del##btnDel-{rowIndex}", trigger.Key != "custom"))
        {
            Plugin.Configuration.Triggers.Remove(trigger);
            Plugin.Configuration.Save();
        }

    }

    private Trigger draftTrigger = new();
    private void RenderNewEntryRow(uint rowIndex)
    {
        ImGui.TableNextRow();

        // # no
        ImGui.TableNextColumn();
        ImGuiHelpers.SafeTextWrapped("#");

        // # key
        ImGui.TableNextColumn();
        ImGui.Text("custom");

        // # ChatType
        ImGui.TableNextColumn();
        ChatType? chatType = draftTrigger.ChatType;
        if (RenderChatTypeDropDown(rowIndex, ref chatType, false))
            draftTrigger.ChatType = chatType;

        // # Pattern
        ImGui.TableNextColumn();
        ImGui.SetNextItemWidth(-1);
        string pattern = draftTrigger.Pattern;
        if (ImGui.InputText("##inputPattern", ref pattern, 20))
            draftTrigger.Pattern = pattern;

        // # SoundData
        ImGui.TableNextColumn();
        ImGui.SetNextItemWidth(-1);
        string soundData = draftTrigger.SoundData;
        if (ImGui.InputText("##inputSoundData", ref soundData, 20))
            draftTrigger.SoundData = soundData;

        // # Add Button
        ImGui.TableNextColumn();
        bool dataValid = pattern.Length > 0 && soundData.Length > 0;
        if (ImGuiToolkit.Button("+##btnAddEntry", !dataValid))
        {
            Plugin.Configuration.Triggers.Add(draftTrigger);
            draftTrigger = new();
            Plugin.Configuration.Save();
        }


    }
    private bool RenderChatTypeDropDown(uint rowIndex, ref ChatType? currentValue, bool disabled)
    {
        bool valueChanged = false;

        string currentValueText = "None";
        if (currentValue != null)
            currentValueText = ChatTypeExt.Name(currentValue.Value);

        ImGui.SetNextItemWidth(-1);

        if (disabled)
            ImGui.BeginDisabled();

        if (ImGui.BeginCombo($"##cbox-{rowIndex}", currentValueText))
        {

            bool noneSelected = currentValue == null;
            if (ImGui.Selectable($"None##clear-{rowIndex}", noneSelected))
            {
                currentValue = null;
                valueChanged = true;
            }
            if (currentValue == null)
                ImGui.SetItemDefaultFocus();

            foreach (ChatType value in Enum.GetValues<ChatType>())
            {
                bool isSelected = value == currentValue;
                if (ImGui.Selectable(ChatTypeExt.Name(value), isSelected))
                {
                    currentValue = value;
                    valueChanged = true;
                }
                if (isSelected)
                    ImGui.SetItemDefaultFocus();
            }

            ImGui.EndCombo();
        }

        if (disabled)
            ImGui.EndDisabled();

        return valueChanged;
    }

}
