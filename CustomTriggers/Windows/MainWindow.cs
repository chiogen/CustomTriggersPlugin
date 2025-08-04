using System;
using System.Collections.Generic;
using System.Numerics;
using CustomTriggersPlugin.Enums;
using CustomTriggersPlugin.Triggers;
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
        RenderTestRow();
        RenderTriggersTable();

        ImGui.Spacing();

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
                ImGui.TableSetupColumn("ChatType", ImGuiTableColumnFlags.WidthFixed, 150f);
                ImGui.TableSetupColumn("MatchType", ImGuiTableColumnFlags.WidthFixed, 150f);
                ImGui.TableSetupColumn("Pattern", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("SoundData", ImGuiTableColumnFlags.WidthFixed, 200f);
                ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 100f);
                ImGui.TableHeadersRow();
                uint rowIndex = 0;

                // Store triggers to delete in a list here, and delete them after the render completes
                List<Trigger> triggersToDelete = [];

                foreach (Trigger trigger in Plugin.TriggersManager.IterateTriggers())
                {
                    RenderTriggerRow(trigger, rowIndex++, out bool delete);
                    if (delete)
                        triggersToDelete.Add(trigger);
                }

                RenderNewEntryRow(rowIndex++);

                ImGui.EndTable();
                ImGui.PopStyleVar();


                if (triggersToDelete.Count > 0)
                {
                    Plugin.TriggersManager.DeleteTriggers(triggersToDelete);
                    triggersToDelete.Clear();
                }
            }
        }

    }
    private void RenderTriggerRow(Trigger trigger, uint rowIndex, out bool delete)
    {
        delete = false;
        bool editDisabled = trigger.IsPreset;

        // # Start row
        ImGui.TableNextRow();

        // # index
        ImGui.TableNextColumn();
        ImGuiHelpers.SafeTextWrapped(rowIndex.ToString());

        // # ChatType
        ImGui.TableNextColumn();
        ChatType? chatType = trigger.ChatType;
        ImGui.SetNextItemWidth(-1);
        if (RenderChatTypeDropDown($"cboxChatType|{rowIndex}", ref chatType, editDisabled))
        {
            trigger.ChatType = chatType;
            Plugin.TriggersManager.Save();
        }

        // # MatchType
        ImGui.TableNextColumn();
        TriggerMatchType matchType = trigger.MatchType;
        if (RenderMatchTypeDropDown($"cboxMatchType|{rowIndex}", ref matchType, editDisabled))
        {
            trigger.MatchType = matchType;
            Plugin.TriggersManager.Save();
        }

        // # Pattern
        ImGui.TableNextColumn();
        if (editDisabled)
        {
            ImGuiHelpers.SafeTextWrapped(trigger.Pattern);
        }
        else
        {
            string pattern = trigger.Pattern;
            ImGui.SetNextItemWidth(-1);
            if (ImGui.InputText($"##pattern{rowIndex}", ref pattern, 2000))
            {
                trigger.Pattern = pattern;
                Plugin.TriggersManager.Save();
            }
        }

        // # SoundText
        ImGui.TableNextColumn();
        ImGui.SetNextItemWidth(-1);
        string soundData = trigger.SoundData;
        if (editDisabled)
        {
            ImGuiHelpers.SafeTextWrapped(trigger.SoundData);
        }
        else
        {
            if (ImGui.InputText($"##soundData{rowIndex}", ref soundData, 20))
            {
                trigger.SoundData = soundData;
                Plugin.TriggersManager.Save();
            }
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
        if (ImGuiToolkit.Button($"Del##btnDel-{rowIndex}", editDisabled))
            delete = true;

    }

    private Trigger draftTrigger = new();
    private void RenderNewEntryRow(uint rowIndex)
    {
        ImGui.TableNextRow();

        // # no
        ImGui.TableNextColumn();
        ImGuiHelpers.SafeTextWrapped("#");

        // # ChatType
        ImGui.TableNextColumn();
        ChatType? chatType = draftTrigger.ChatType;
        ImGui.SetNextItemWidth(-1);
        if (RenderChatTypeDropDown($"cboxChatType|{rowIndex}", ref chatType, disabled: false))
            draftTrigger.ChatType = chatType;

        // # MatchType
        ImGui.TableNextColumn();
        TriggerMatchType matchType = draftTrigger.MatchType;
        if (RenderMatchTypeDropDown($"cboxMatchType|{rowIndex}", ref matchType, false))
            draftTrigger.MatchType = matchType;

        // # Pattern
        ImGui.TableNextColumn();
        ImGui.SetNextItemWidth(-1);
        string pattern = draftTrigger.Pattern;
        if (ImGui.InputText("##inputPattern", ref pattern, 255))
            draftTrigger.Pattern = pattern;

        // # SoundData
        ImGui.TableNextColumn();
        ImGui.SetNextItemWidth(-1);
        string soundData = draftTrigger.SoundData;
        if (ImGui.InputText("##inputSoundData", ref soundData, 25))
            draftTrigger.SoundData = soundData;

        // # Add Button
        ImGui.TableNextColumn();
        bool dataValid = pattern.Length > 0 && soundData.Length > 0;
        if (ImGuiToolkit.Button("+##btnAddEntry", !dataValid))
        {
            draftTrigger.UpdatePattern(draftTrigger.Pattern);
            Plugin.TriggersManager.AddTrigger(draftTrigger);
            draftTrigger = new();
        }

    }
    private static bool RenderChatTypeDropDown(string key, ref ChatType? currentValue, bool disabled)
    {
        bool valueChanged = false;

        string currentValueText = "None";
        if (currentValue != null)
            currentValueText = ChatTypeExt.Name(currentValue.Value);

        if (disabled)
            ImGui.BeginDisabled();

        if (ImGui.BeginCombo($"##{key}", currentValueText))
        {

            bool noneSelected = currentValue == null;
            if (ImGui.Selectable($"None##{key}|none", noneSelected))
            {
                currentValue = null;
                valueChanged = true;
            }
            if (currentValue == null)
                ImGui.SetItemDefaultFocus();

            ushort customValue = (ushort)(currentValue ?? 0);
            if (ImGuiToolkit.InputUShort("##{key}|InputInt", ref customValue))
            {
                currentValue = (ChatType)customValue;
                valueChanged = true;
            }

            foreach (ChatType value in Enum.GetValues<ChatType>())
            {
                bool isSelected = value == currentValue;
                if (ImGui.Selectable($"{ChatTypeExt.Name(value)}##{key}|{value}", isSelected))
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
    private static bool RenderMatchTypeDropDown(string key, ref TriggerMatchType currentValue, bool disabled)
    {
        bool valueChanged = false;

        ImGui.SetNextItemWidth(-1);

        if (disabled)
            ImGui.BeginDisabled();

        if (ImGui.BeginCombo($"##{key}", currentValue.ToString()))
        {
            foreach (TriggerMatchType value in Enum.GetValues<TriggerMatchType>())
            {
                bool isSelected = value == currentValue;
                if (ImGui.Selectable($"{value}##{key}|{value}", isSelected))
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

    private void RenderTestRow()
    {
        if (ImGui.BeginTable("TestArea", 3))
        {
            ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(10, 2)); // X and Y padding

            ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 150f);
            ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("", ImGuiTableColumnFlags.WidthFixed, 100f);

            ImGui.TableNextColumn();
            ChatType? chatType = (ChatType?)Plugin.Configuration.DebugTestChatType;
            ImGui.SetNextItemWidth(-1);
            if (RenderChatTypeDropDown("Test|ChatType", ref chatType, false))
            {
                Plugin.Configuration.DebugTestChatType = (ushort?)chatType;
                Plugin.Configuration.Save();
            }

            ImGui.TableNextColumn();
            ImGui.SetNextItemWidth(-1);
            string message = Plugin.Configuration.DebugTestMessage;
            if (ImGui.InputText("##testInput", ref message, 255))
            {

                Plugin.Configuration.DebugTestMessage = message;
                Plugin.Configuration.Save();
            }

            ImGui.TableNextColumn();
            ImGui.SetNextItemWidth(-1);
            if (ImGui.Button("Send##Test|sendMessage"))
                Test.Message(Plugin, chatType ?? ChatType.Echo, message);

            ImGui.EndTable();
            ImGui.PopStyleVar();
        }
    }

}
