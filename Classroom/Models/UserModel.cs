namespace Classroom.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Email { get; set; }

        public IEnumerable<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();
    }
}
