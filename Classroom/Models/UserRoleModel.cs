namespace Classroom.Models
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleType { get; set; } = string.Empty;
    }
}
