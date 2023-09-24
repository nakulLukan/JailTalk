namespace JailTalk.Shared;

public enum ErrorType : short
{
    Warning = 1,
    Error = 2
}

public enum PhoneBalanceReason : short
{
    Recharge = 1,
    RegularCall = 2,
    Released = 3
}

public enum CallEndReason : short
{
    CallerRegularCut = 1,

    CallConference = 2,

    RecieverRegularCut = 3,

    InsufficientBalance = 4,

    NetworkError = 5
}

public enum States : int
{
    Kerala = 1
}

public enum Country : int
{
    India = 1
}

public enum LookupMasters : int
{
    State = 1,
    Country = 2,
    Relationship = 3,
    IdProof = 4
}

public enum Gender : int
{
    Male = 1,
    Female = 2,
    Transgender = 3,
}

/// <summary>
/// Enum to indicate the unlimited call access action.
/// </summary>
public enum UnlimitedCallAction : short
{
    Allow = 1,
    Revoke = 2
}

public enum ApplicationSettings : int
{
    MaxAllowedActiveContacts = 1,
    MaxAllowedCallDurationMale = 2,
    MaxAllowedCallDurationFemale = 3,
    MaxAllowedCallDurationOthers = 4,
    CallPricePerMinute = 5,
    MaxAllowedCallTimeInRupeesPerMonth = 6,
    AllowAccessToCallRecording = 7,
    DailyCallTimeWindow = 8
}