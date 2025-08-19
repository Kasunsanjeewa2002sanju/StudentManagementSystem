using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required, StringLength(100)]
        public required string Name { get; set; }

        [Range(5, 100)]
        public int Age { get; set; }

        [Required, StringLength(10)]
        public required string Grade { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
    }
}
