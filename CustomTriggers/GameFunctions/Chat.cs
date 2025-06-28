using CustomTriggersPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTriggersPlugin.GameFunctions
{
    internal sealed unsafe class Chat : IDisposable
    {

        private Plugin Plugin { get; }

        internal Chat(Plugin plugin)
        {
            Plugin = plugin;

        }

        public void Dispose()
        {

        }

    }
}
