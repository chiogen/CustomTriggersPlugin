using CustomTriggersPlugin.Enums;
using CustomTriggersPlugin.Triggers;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CustomTriggersPlugin;

internal class MessageManager : IDisposable
{
    private Plugin Plugin { get; }


    internal unsafe MessageManager(Plugin plugin)
    {
        Plugin = plugin;
        Plugin.ChatGui.ChatMessage += OnChatMessage;
    }

    public void Dispose()
    {
        Plugin.ChatGui.ChatMessage -= OnChatMessage;
    }

    private void OnChatMessage(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        try
        {
            ProcessMessage((ChatType)type, message.TextValue);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString(), ex);
        }
    }

    public void ProcessMessage(ChatType chatType, string message)
    {
        bool debug = Plugin.Configuration.Debug;

        // No need to do anything, if we got no triggers
        if (Plugin.Configuration.Debug)
            Log.Debug($"New Message | {ChatTypeExt.Name(chatType)}({chatType}) | {message}");

        // Check for chat types to ignore
        switch (chatType)
        {
            case ChatType.Debug:
                return;
        }

        foreach (ITrigger trigger in Plugin.TriggersManager.IterateTriggers())
        {
            if (trigger.ChatType != null && trigger.ChatType != chatType)
                continue;
            if (!MatchMessageOnTrigger(trigger, message))
                continue;

            if (trigger.Pattern.Length == 0)

                if (debug)
                    Log.Debug($"Match: ChatType={chatType.ToString() ?? null}|{trigger.ChatType?.ToString() ?? "null"} Pattern=\"{trigger.Pattern}\" Message=\"{message}\"");

            Plugin.TextToSpeechService.Speak(trigger.SoundData ?? message);
        }
    }

    public bool MatchMessageOnTrigger(ITrigger trigger, string message)
    {
        if (message.Length == 0)
            return false;

        return trigger.MatchType switch
        {
            TriggerMatchType.Regex => GetTriggerCompiledPattern(trigger).IsMatch(message),
            TriggerMatchType.Equals => message.Equals(trigger.Pattern, StringComparison.OrdinalIgnoreCase),
            TriggerMatchType.StartsWith => message.StartsWith(trigger.Pattern, StringComparison.OrdinalIgnoreCase),
            TriggerMatchType.EndsWith => message.EndsWith(trigger.Pattern, StringComparison.OrdinalIgnoreCase),
            TriggerMatchType.Contains => message.Contains(trigger.Pattern, StringComparison.OrdinalIgnoreCase),
            _ => false,
        };
    }

    private static readonly Dictionary<ITrigger, Regex> CompiledRegexStore = [];
    private static Regex GetTriggerCompiledPattern(ITrigger trigger)
    {
        if (trigger is Trigger basicTrigger)
            return basicTrigger.GetCompiledPattern();

        if (CompiledRegexStore.TryGetValue(trigger, out Regex? cachedCompiledRegex))
            if (cachedCompiledRegex != null)
                return cachedCompiledRegex;

        Regex compiledRegex = new Regex(trigger.Pattern, RegexOptions.Compiled);
        CompiledRegexStore.Add(trigger, compiledRegex);
        return compiledRegex;
    }


}

