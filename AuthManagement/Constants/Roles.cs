namespace AuthManagement.Constants;

/// <summary>
/// Centralized role constants for consistent role references throughout the application.
/// These role names must match exactly with the roles defined in the database seed data.
/// </summary>
public static class UIRoles
{
    // System Administration Roles
    public const string SuperAdmin = "SuperAdmin";
    
    // Finance Department Roles
    public const string FinanceAdmin = "FinanceAdmin";
    public const string FinanceManager = "FinanceManager";
    public const string FinanceAnalyst = "FinanceAnalyst";
    public const string FinanceStaff = "FinanceStaff";
    
    // HR Department Roles
    public const string HRAdmin = "HRAdmin";
    public const string HRManager = "HRManager";
    public const string HRExecutive = "HRExecutive";
    public const string HRStaff = "HRStaff";
    
    // Legacy/Generic Roles
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Staff = "Staff";
    public const string PendingUser = "PendingUser";
    public const string Accountant = "Accountant";
    public const string Auditor = "Auditor";
}

/// <summary>
/// Department name constants for consistent department references.
/// </summary>
public static class Departments
{
    public const string Finance = "Finance";
    public const string HR = "HR";
}

/// <summary>
/// Permission name constants for consistent permission checking.
/// </summary>
public static class PermissionNames
{
    public const string Create = "Create";
    public const string Edit = "Edit";
    public const string View = "View";
    public const string Delete = "Delete";
    public const string List = "List";
}

