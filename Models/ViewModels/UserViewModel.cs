﻿using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Models.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Username => Email.Split('@')[0];
        public IEnumerable<RoleViewModel> Roles { get; set; }
    }
}
