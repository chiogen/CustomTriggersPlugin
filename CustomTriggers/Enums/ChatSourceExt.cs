using CustomTriggersPlugin.Enums;
using System;

namespace CustomTriggersPlugin.Enums;

internal static class ChatSourceExt
{
    internal const ChatSource All =
        ChatSource.Self
        | ChatSource.PartyMember
        | ChatSource.AllianceMember
        | ChatSource.Other
        | ChatSource.EngagedEnemy
        | ChatSource.UnengagedEnemy
        | ChatSource.FriendlyNpc
        | ChatSource.SelfPet
        | ChatSource.PartyPet
        | ChatSource.AlliancePet
        | ChatSource.OtherPet;

    internal static string Name(this ChatSource source) => source switch
    {
        ChatSource.Self => "Self",
        ChatSource.PartyMember => "PartyMember",
        ChatSource.AllianceMember => "AllianceMember",
        ChatSource.Other => "Other",
        ChatSource.EngagedEnemy => "EngagedEnemy",
        ChatSource.UnengagedEnemy => "UnengagedEnemy",
        ChatSource.FriendlyNpc => "FriendlyNpc",
        ChatSource.SelfPet => "SelfPet",
        ChatSource.PartyPet => "PartyPet",
        ChatSource.AlliancePet => "AlliancePet",
        ChatSource.OtherPet => "OtherPet",
        _ => throw new ArgumentOutOfRangeException(nameof(source), source, null),
    };
}
