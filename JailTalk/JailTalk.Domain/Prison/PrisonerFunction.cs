using JailTalk.Domain.System;

namespace JailTalk.Domain.Prison;
public class PrisonerFunction
{
    public long Id { get; set; }
    public Guid PrisonerId { get; set; }

    /// <summary>
    /// Previous jail of the prisoner.
    /// </summary>
    public int? LastAssociatedJailId { get; set; }

    /// <summary>
    /// When the prisoner was released from jail
    /// </summary>
    public DateTimeOffset? LastReleasedOn { get; set; }

    /// <summary>
    /// If a prisoner is punished then this field can be assigned a value. If current time is greater than
    /// this value then the prisoner is considered to be not under punishment.
    /// </summary>
    public DateTimeOffset? PunishmentEndsOn { get; set; }

    /// <summary>
    /// This field can be used to let a prisoner to talk any number of times till the given date.
    /// Usually, this value should be 1 day.
    /// </summary>
    public DateTimeOffset? UnlimitedCallsEndsOn { get; set; }

    /// <summary>
    /// Attachment to be shown as prisoner's display picture
    /// </summary>
    public int? DpAttachmentId { get; set; }

    public Prisoner Prisoner { get; set; }
    public Jail LastAssociatedJail { get; set; }
    public Attachment DpAttachment { get; set; }
}
