﻿namespace JailTalk.Shared;

public static class AppClaims
{
    public const string PrisonId = "PrisonId";
}

public static class HttpHeader
{
    public const string SessionToken = "SessionToken";
}

public static class CommonExceptionMessages
{
    public const string UserNotFound = "Unknown user";
    public const string PrisonerNotFound = "Unknown prisoner";
    public const string ItemNotFound = "Not Found";
    public const string DeviceNotFound = "Device Not Found";
    public const string PermissionDenied = "Permission Denied";
}

public static class AppRoleNames
{
    public const string SuperAdmin = "super-admin";
    public const string Admin = "admin";
}

public static class AppStringConstants
{
    public const string GridNoDataIndication = "-";
}