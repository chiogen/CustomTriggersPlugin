using CustomTriggersPlugin.Enums;
using Dalamud.Bindings.ImGui;
using Serilog;
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

    internal static bool InputUShort(string label, ref ushort value, bool disabled = false)
    {
        bool valueChanged = false;

        if (disabled)
            ImGui.BeginDisabled();

        try
        {
            int intValue = (int)value;
            if (ImGui.InputInt("##{key}|InputInt", ref intValue))
            {
                if (intValue > ushort.MinValue && intValue < ushort.MaxValue)
                {
                    value = (ushort)intValue;
                    valueChanged = true;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "imgui ushort input helper has thrown an error.");
        }

        if (disabled)
            ImGui.EndDisabled();

        return valueChanged;
    }
}
