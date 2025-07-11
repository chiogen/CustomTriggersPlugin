using CustomTriggersPlugin.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CustomTriggersPlugin
{
    internal class TriggersManager : IDisposable
    {
        private readonly Configuration config;
        private readonly List<Trigger> deepDungeonTriggers;


        internal TriggersManager(Plugin plugin)
        {
            config = plugin.Configuration;
            deepDungeonTriggers = TriggerPresets.GetDeepDungeonTriggers();
        }

        public void Dispose()
        {
            deepDungeonTriggers.Clear();
        }


        public IEnumerable<ITrigger> IterateTriggers()
        {
            if (config.UseDeepDungeonsPreset)
                foreach (var trigger in deepDungeonTriggers)
                    yield return trigger;

            foreach (var trigger in config.Triggers)
                yield return trigger;
        }


        public void AddTrigger(ITrigger trigger)
        {
            config.Triggers.Add(trigger);
            config.Save();
        }
        public void DeleteTrigger(ITrigger trigger)
        {
            if (config.Triggers.Remove(trigger))
                config.Save();
        }
        public void DeleteTriggers(List<ITrigger> triggersToDelete)
        {
            int nRemoved = config.Triggers.RemoveAll(trigger => triggersToDelete.Contains(trigger));
            if (nRemoved > 0)
                config.Save();
        }
    }
}
