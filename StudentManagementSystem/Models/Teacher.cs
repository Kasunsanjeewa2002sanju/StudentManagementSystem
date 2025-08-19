using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Required, StringLength(100)]
        public required string Subject { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
    }
}
