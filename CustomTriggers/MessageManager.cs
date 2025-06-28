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
        if (Plugin.Configuration.Debug)
            Log.Debug($"New message: {type} {sender.TextValue} {message}");
    }

}

