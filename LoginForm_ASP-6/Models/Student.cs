using System;
using System.Collections.Generic;

namespace LoginForm_ASP_6.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public int Age { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
