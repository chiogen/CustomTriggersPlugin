using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomTriggersPlugin.Enums;

namespace CustomTriggersPlugin;

internal static class Test
{
    public static void Message(Plugin plugin, ChatType chatType, string message)
    {
        try
        {
            plugin.MessageManager.ProcessMessage(chatType, message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Testing message failed.");
        }
    }
}
