namespace DailyPlanner.Domain.Enum;

public enum ErrorCodes
{
    None = -1,
    Unknown = 0,
    ReportsNotFound = 1,
    ReportNotFound = 2,
    ReportAlreadyExists = 3,
    InternalServerError = 10,
    UserNotFound = 11,
    UserAlreadyExists = 12,
    UserUnauthorizedAccess = 13,
    PasswordsNotMatch = 21,
    PasswordIsNotCorrect = 22,
}