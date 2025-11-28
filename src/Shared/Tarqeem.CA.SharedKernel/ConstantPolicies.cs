namespace Tarqeem.CA.SharedKernel;

public static class ConstantPolicies
{
    public const string DynamicPermission = nameof(DynamicPermission);

    public static readonly List<DynamicPermission> TeacherPermissions =
    [
        SharedKernel.DynamicPermission.CanTakeAttendance,
    ];
}

public enum DynamicPermission
{
    CanAddUsers,
    IsManager,
    CanManageRooms,
    CanTakeAttendance,
    CanEditAttendance,
    CanManageStudents,
}
