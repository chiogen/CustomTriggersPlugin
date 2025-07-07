using CustomTriggersPlugin.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTriggersPlugin.Triggers;

public interface ITrigger
{
    public string Key { get; set; }
    public string Name { get; set; }
    public ChatType? ChatType { get; set; }
    public TriggerMatchType MatchType { get; set; }
    public string Pattern { get; set; }
    public string SoundData { get; set; }
}

