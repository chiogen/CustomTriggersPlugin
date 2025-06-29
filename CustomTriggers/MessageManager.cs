using CustomTriggersPlugin.Enums;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Serilog;
using System;
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

    private void OnChatMessage(XivChatType _type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        try
        {
            ChatType chatType = (ChatType)_type;

            if (Plugin.Configuration.Debug)
                Log.Debug($"New message: {ChatTypeExt.Name(chatType)}({_type}) {sender.TextValue} {message.TextValue}");

            ProcessMessage(chatType, message.TextValue);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString(), ex);
        }
    }

    public void ProcessMessage(ChatType chatType, string message)
    {
        // No need to do anything, if we got no triggers
        if (Plugin.Configuration.Triggers.Count == 0)
            return;

        foreach (Trigger trigger in Plugin.Configuration.Triggers)
        {
            if (trigger.ChatType != null && trigger.ChatType != chatType)
                continue;

            if (Regex.IsMatch(message, trigger.Pattern))
                Plugin.TextToSpeechService.Speak(trigger.SoundData ?? message);
        }
    }


}

