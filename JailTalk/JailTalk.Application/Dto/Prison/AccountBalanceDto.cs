﻿namespace JailTalk.Application.Dto.Prison;

public class AccountBalanceDto
{
    public Guid PrisonerId { get; set; }
    public float? AccountBalanceAmount { get; set; }
    public string TalkTimeLeft { get; set; }
}
