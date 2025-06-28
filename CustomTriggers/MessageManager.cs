using CustomTriggersPlugin.Enums;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            if (Plugin.Configuration.Debug)
                Log.Debug($"New message: {ChatTypeExt.Name((ChatType)type)}({type}) {sender.TextValue} {message.TextValue}");

            ProcessMessage(type, message.TextValue);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString(), ex);
        }
    }

    public void ProcessMessage(XivChatType chatType, string message)
    {
        // No need to do anything, if we got no triggers
        if (Plugin.Configuration.Triggers.Count == 0)
            return;

        foreach (Trigger trigger in Plugin.Configuration.Triggers)
        {
            if (trigger.ChatType != null && trigger.ChatType != chatType)
                continue;

            if (trigger.Match(message))
                Plugin.TextToSpeechService.Speak(message);
        }
    }


}

