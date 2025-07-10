using CustomTriggersPlugin.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTriggersPlugin
{
    internal class TriggersManager
    {
        private readonly Configuration config;

        public List<ITrigger> Triggers { get; } = [];


        internal TriggersManager(Plugin plugin)
        {
            config = plugin.Configuration;
            config.OnSaved += OnConfigSaved;
            TryReadFromConfig();
        }

        private bool TryReadFromConfig()
        {
            try
            {
                foreach (ITrigger tr in config.Triggers)
                    Triggers.Add((Trigger)tr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void OnConfigSaved(object? sender, EventArgs e)
        {
            // TODO Update triggers list
        }

    }
}
