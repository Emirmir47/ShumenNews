using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Models.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public bool IsBlocked { get; set; }
        public bool IsUnblocked { get; set; }
        public int? BlockTime { get; set; }
        public string Username => Email is not null ? Email.Split('@')[0] : "none";
        public bool HasUpdatedRoles { get; set; } = false;
        public IEnumerable<RoleViewModel> Roles { get; set; }
        public string RolesInStr { get; set; }
    }
}
