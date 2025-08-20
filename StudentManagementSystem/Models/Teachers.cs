using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    [Table("Teachers", Schema = "myschema")] // Must match the table name in SQL
    public class Teacher
    {
        [Key]
        public int TeacherID { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }
        public required string Department { get; set; }
        public DateTime HireDate { get; set; }

        [Required]
        public required string Status { get; set; }
    }

}