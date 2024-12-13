using System.ComponentModel.DataAnnotations;

namespace Repo_UnitofWork.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(1, 100)]
        public int Age { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }
    }
}
