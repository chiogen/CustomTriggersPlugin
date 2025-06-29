using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTriggersPlugin;

internal static class ImGuiToolkit
{
    internal static bool Button(string label, bool disabled = false)
    {
        bool result = false;

        if (disabled)
            ImGui.BeginDisabled();

        result = ImGui.Button(label);

        if (disabled)
            ImGui.EndDisabled();

        return result;
    }
}
