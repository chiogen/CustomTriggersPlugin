using CustomTriggersPlugin.Util;
using Dalamud.Game.Config;
using System.Collections.Generic;

namespace CustomTriggersPlugin.Enums;

internal static class ChatTypeExt
{

    internal static string Name(this ChatType type)
    {
        return type switch
        {
            ChatType.Debug => "Debug",
            ChatType.Urgent => "Urgent",
            ChatType.Notice => "Notice",
            ChatType.Say => "Say",
            ChatType.Shout => "Shout",
            ChatType.TellOutgoing => "TellOutgoing",
            ChatType.TellIncoming => "TellIncoming",
            ChatType.Party => "Party",
            ChatType.Alliance => "Alliance",
            ChatType.Linkshell1 => "Linkshell1",
            ChatType.Linkshell2 => "Linkshell2",
            ChatType.Linkshell3 => "Linkshell3",
            ChatType.Linkshell4 => "Linkshell4",
            ChatType.Linkshell5 => "Linkshell5",
            ChatType.Linkshell6 => "Linkshell6",
            ChatType.Linkshell7 => "Linkshell7",
            ChatType.Linkshell8 => "Linkshell8",
            ChatType.FreeCompany => "FreeCompany",
            ChatType.NoviceNetwork => "NoviceNetwork",
            ChatType.CustomEmote => "CustomEmotes",
            ChatType.StandardEmote => "StandardEmotes",
            ChatType.Yell => "Yell",
            ChatType.CrossParty => "CrossWorldParty",
            ChatType.PvpTeam => "PvpTeam",
            ChatType.CrossLinkshell1 => "CrossLinkshell1",
            ChatType.Damage => "Damage",
            ChatType.Miss => "Miss",
            ChatType.Action => "Action",
            ChatType.Item => "Item",
            ChatType.Healing => "Healing",
            ChatType.GainBuff => "GainBuff",
            ChatType.GainDebuff => "GainDebuff",
            ChatType.LoseBuff => "LoseBuff",
            ChatType.LoseDebuff => "LoseDebuff",
            ChatType.Alarm => "Alarm",
            ChatType.Echo => "Echo",
            ChatType.System => "System",
            ChatType.BattleSystem => "BattleSystem",
            ChatType.GatheringSystem => "GatheringSystem",
            ChatType.Error => "Error",
            ChatType.NpcDialogue => "NpcDialogue",
            ChatType.LootNotice => "LootNotice",
            ChatType.Progress => "Progress",
            ChatType.LootRoll => "LootRoll",
            ChatType.Crafting => "Crafting",
            ChatType.Gathering => "Gathering",
            ChatType.NpcAnnouncement => "NpcAnnouncement",
            ChatType.FreeCompanyAnnouncement => "FreeCompanyAnnouncement",
            ChatType.FreeCompanyLoginLogout => "FreeCompanyLoginLogout",
            ChatType.RetainerSale => "RetainerSale",
            ChatType.PeriodicRecruitmentNotification => "PeriodicRecruitmentNotification",
            ChatType.Sign => "Sign",
            ChatType.RandomNumber => "RandomNumber",
            ChatType.NoviceNetworkSystem => "NoviceNetworkSystem",
            ChatType.Orchestrion => "Orchestrion",
            ChatType.PvpTeamAnnouncement => "PvpTeamAnnouncement",
            ChatType.PvpTeamLoginLogout => "PvpTeamLoginLogout",
            ChatType.MessageBook => "MessageBook",
            ChatType.GmTell => "GmTell",
            ChatType.GmSay => "GmSay",
            ChatType.GmShout => "GmShout",
            ChatType.GmYell => "GmYell",
            ChatType.GmParty => "GmParty",
            ChatType.GmFreeCompany => "GmFreeCompany",
            ChatType.GmLinkshell1 => "GmLinkshell1",
            ChatType.GmLinkshell2 => "GmLinkshell2",
            ChatType.GmLinkshell3 => "GmLinkshell3",
            ChatType.GmLinkshell4 => "GmLinkshell4",
            ChatType.GmLinkshell5 => "GmLinkshell5",
            ChatType.GmLinkshell6 => "GmLinkshell6",
            ChatType.GmLinkshell7 => "GmLinkshell7",
            ChatType.GmLinkshell8 => "GmLinkshell8",
            ChatType.GmNoviceNetwork => "GmNoviceNetwork",
            ChatType.CrossLinkshell2 => "CrossLinkshell2",
            ChatType.CrossLinkshell3 => "CrossLinkshell3",
            ChatType.CrossLinkshell4 => "CrossLinkshell4",
            ChatType.CrossLinkshell5 => "CrossLinkshell5",
            ChatType.CrossLinkshell6 => "CrossLinkshell6",
            ChatType.CrossLinkshell7 => "CrossLinkshell7",
            ChatType.CrossLinkshell8 => "CrossLinkshell8",
            ChatType.ExtraChatLinkshell1 => "ExtraChatLinkshell1",
            ChatType.ExtraChatLinkshell2 => "ExtraChatLinkshell2",
            ChatType.ExtraChatLinkshell3 => "ExtraChatLinkshell3",
            ChatType.ExtraChatLinkshell4 => "ExtraChatLinkshell4",
            ChatType.ExtraChatLinkshell5 => "ExtraChatLinkshell5",
            ChatType.ExtraChatLinkshell6 => "ExtraChatLinkshell6",
            ChatType.ExtraChatLinkshell7 => "ExtraChatLinkshell7",
            ChatType.ExtraChatLinkshell8 => "ExtraChatLinkshell8",
            _ => type.ToString(),
        };
    }

    internal static uint? DefaultColor(this ChatType type)
    {
        switch (type)
        {
            case ChatType.Debug:
                return ColourUtil.ComponentsToRgba(204, 204, 204);
            case ChatType.Urgent:
                return ColourUtil.ComponentsToRgba(255, 127, 127);
            case ChatType.Notice:
                return ColourUtil.ComponentsToRgba(179, 140, 255);

            case ChatType.Say:
            case ChatType.GmSay:
                return ColourUtil.ComponentsToRgba(247, 247, 247);
            case ChatType.Shout:
            case ChatType.GmShout:
                return ColourUtil.ComponentsToRgba(255, 166, 102);
            case ChatType.TellIncoming:
            case ChatType.TellOutgoing:
            case ChatType.GmTell:
                return ColourUtil.ComponentsToRgba(255, 184, 222);
            case ChatType.Party:
            case ChatType.CrossParty:
            case ChatType.GmParty:
                return ColourUtil.ComponentsToRgba(102, 229, 255);
            case ChatType.Alliance:
                return ColourUtil.ComponentsToRgba(255, 127, 0);
            case ChatType.NoviceNetwork:
            case ChatType.NoviceNetworkSystem:
            case ChatType.GmNoviceNetwork:
                return ColourUtil.ComponentsToRgba(212, 255, 125);
            case ChatType.Linkshell1:
            case ChatType.Linkshell2:
            case ChatType.Linkshell3:
            case ChatType.Linkshell4:
            case ChatType.Linkshell5:
            case ChatType.Linkshell6:
            case ChatType.Linkshell7:
            case ChatType.Linkshell8:
            case ChatType.CrossLinkshell1:
            case ChatType.CrossLinkshell2:
            case ChatType.CrossLinkshell3:
            case ChatType.CrossLinkshell4:
            case ChatType.CrossLinkshell5:
            case ChatType.CrossLinkshell6:
            case ChatType.CrossLinkshell7:
            case ChatType.CrossLinkshell8:
            case ChatType.GmLinkshell1:
            case ChatType.GmLinkshell2:
            case ChatType.GmLinkshell3:
            case ChatType.GmLinkshell4:
            case ChatType.GmLinkshell5:
            case ChatType.GmLinkshell6:
            case ChatType.GmLinkshell7:
            case ChatType.GmLinkshell8:
                return ColourUtil.ComponentsToRgba(212, 255, 125);
            case ChatType.StandardEmote:
                return ColourUtil.ComponentsToRgba(186, 255, 240);
            case ChatType.CustomEmote:
                return ColourUtil.ComponentsToRgba(186, 255, 240);
            case ChatType.Yell:
            case ChatType.GmYell:
                return ColourUtil.ComponentsToRgba(255, 255, 0);
            case ChatType.Echo:
                return ColourUtil.ComponentsToRgba(204, 204, 204);
            case ChatType.System:
            case ChatType.GatheringSystem:
            case ChatType.PeriodicRecruitmentNotification:
            case ChatType.Orchestrion:
            case ChatType.Alarm:
            case ChatType.RetainerSale:
            case ChatType.Sign:
            case ChatType.MessageBook:
                return ColourUtil.ComponentsToRgba(204, 204, 204);
            case ChatType.NpcAnnouncement:
            case ChatType.NpcDialogue:
                return ColourUtil.ComponentsToRgba(171, 214, 71);
            case ChatType.Error:
                return ColourUtil.ComponentsToRgba(255, 74, 74);
            case ChatType.FreeCompany:
            case ChatType.FreeCompanyAnnouncement:
            case ChatType.FreeCompanyLoginLogout:
            case ChatType.GmFreeCompany:
                return ColourUtil.ComponentsToRgba(171, 219, 229);
            case ChatType.PvpTeam:
                return ColourUtil.ComponentsToRgba(171, 219, 229);
            case ChatType.PvpTeamAnnouncement:
            case ChatType.PvpTeamLoginLogout:
                return ColourUtil.ComponentsToRgba(171, 219, 229);
            case ChatType.Action:
            case ChatType.Item:
            case ChatType.LootNotice:
                return ColourUtil.ComponentsToRgba(255, 255, 176);
            case ChatType.Progress:
                return ColourUtil.ComponentsToRgba(255, 222, 115);
            case ChatType.LootRoll:
            case ChatType.RandomNumber:
                return ColourUtil.ComponentsToRgba(199, 191, 158);
            case ChatType.Crafting:
            case ChatType.Gathering:
                return ColourUtil.ComponentsToRgba(222, 191, 247);
            case ChatType.Damage:
                return ColourUtil.ComponentsToRgba(255, 125, 125);
            case ChatType.Miss:
                return ColourUtil.ComponentsToRgba(204, 204, 204);
            case ChatType.Healing:
                return ColourUtil.ComponentsToRgba(212, 255, 125);
            case ChatType.GainBuff:
            case ChatType.LoseBuff:
                return ColourUtil.ComponentsToRgba(148, 191, 255);
            case ChatType.GainDebuff:
            case ChatType.LoseDebuff:
                return ColourUtil.ComponentsToRgba(255, 138, 196);
            case ChatType.BattleSystem:
                return ColourUtil.ComponentsToRgba(204, 204, 204);
            default:
                return null;
        }
    }

    internal static bool IsGm(this ChatType type) => type switch
    {
        ChatType.GmTell => true,
        ChatType.GmSay => true,
        ChatType.GmShout => true,
        ChatType.GmYell => true,
        ChatType.GmParty => true,
        ChatType.GmFreeCompany => true,
        ChatType.GmLinkshell1 => true,
        ChatType.GmLinkshell2 => true,
        ChatType.GmLinkshell3 => true,
        ChatType.GmLinkshell4 => true,
        ChatType.GmLinkshell5 => true,
        ChatType.GmLinkshell6 => true,
        ChatType.GmLinkshell7 => true,
        ChatType.GmLinkshell8 => true,
        ChatType.GmNoviceNetwork => true,
        _ => false,
    };

    internal static bool IsExtraChatLinkshell(this ChatType type) => type switch
    {
        ChatType.ExtraChatLinkshell1 => true,
        ChatType.ExtraChatLinkshell2 => true,
        ChatType.ExtraChatLinkshell3 => true,
        ChatType.ExtraChatLinkshell4 => true,
        ChatType.ExtraChatLinkshell5 => true,
        ChatType.ExtraChatLinkshell6 => true,
        ChatType.ExtraChatLinkshell7 => true,
        ChatType.ExtraChatLinkshell8 => true,
        _ => false,
    };

    public static UiConfigOption ToConfigEntry(this ChatType type) => type switch
    {
        ChatType.Say => UiConfigOption.ColorSay,
        ChatType.Shout => UiConfigOption.ColorShout,
        ChatType.TellOutgoing => UiConfigOption.ColorTell,
        ChatType.Party => UiConfigOption.ColorParty,
        ChatType.Linkshell1 => UiConfigOption.ColorLS1,
        ChatType.Linkshell2 => UiConfigOption.ColorLS2,
        ChatType.Linkshell3 => UiConfigOption.ColorLS3,
        ChatType.Linkshell4 => UiConfigOption.ColorLS4,
        ChatType.Linkshell5 => UiConfigOption.ColorLS5,
        ChatType.Linkshell6 => UiConfigOption.ColorLS6,
        ChatType.Linkshell7 => UiConfigOption.ColorLS7,
        ChatType.Linkshell8 => UiConfigOption.ColorLS8,
        ChatType.FreeCompany => UiConfigOption.ColorFCompany,
        ChatType.NoviceNetwork => UiConfigOption.ColorBeginner,
        ChatType.CustomEmote => UiConfigOption.ColorEmoteUser,
        ChatType.StandardEmote => UiConfigOption.ColorEmote,
        ChatType.Yell => UiConfigOption.ColorYell,
        ChatType.GainBuff => UiConfigOption.ColorBuffGive,
        ChatType.GainDebuff => UiConfigOption.ColorDebuffGive,
        ChatType.System => UiConfigOption.ColorSysMsg,
        ChatType.NpcDialogue => UiConfigOption.ColorNpcSay,
        ChatType.LootRoll => UiConfigOption.ColorLoot,
        ChatType.FreeCompanyAnnouncement => UiConfigOption.ColorFCAnnounce,
        ChatType.PvpTeamAnnouncement => UiConfigOption.ColorPvPGroupAnnounce,
        _ => UiConfigOption.ColorSay,
    };

    internal static bool HasSource(this ChatType type) => type switch
    {
        // Battle
        ChatType.Damage => true,
        ChatType.Miss => true,
        ChatType.Action => true,
        ChatType.Item => true,
        ChatType.Healing => true,
        ChatType.GainBuff => true,
        ChatType.LoseBuff => true,
        ChatType.GainDebuff => true,
        ChatType.LoseDebuff => true,

        // Announcements
        ChatType.System => true,
        ChatType.BattleSystem => true,
        ChatType.Error => true,
        ChatType.LootNotice => true,
        ChatType.Progress => true,
        ChatType.LootRoll => true,
        ChatType.Crafting => true,
        ChatType.Gathering => true,
        ChatType.FreeCompanyLoginLogout => true,
        ChatType.PvpTeamLoginLogout => true,
        _ => false,
    };
}
