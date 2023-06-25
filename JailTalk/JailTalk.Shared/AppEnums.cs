namespace JailTalk.Shared;

public enum ErrorType : short
{
    Warning = 1,
    Error = 2
}

public enum PhoneBalanceReason : short
{
    Recharge = 1,
    RegularCall = 2
}

public enum CallEndReason : short
{
    CallEnded = 1,

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
    Country = 2
}