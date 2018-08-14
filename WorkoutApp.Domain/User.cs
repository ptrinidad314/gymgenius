using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkoutApp.Domain
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Object ID Required")]
        [MaxLength(38, ErrorMessage ="Object ID cannot be greater than 38 characters")]
        public string AADObjectId { get; set; }
        [Required(ErrorMessage = "User Name field Required")]
        [MaxLength(50, ErrorMessage = "User Name field cannot be greater than 50 characters")]
        public string UserName { get; set; }
        //[Required(ErrorMessage = "Last Name field required")]
        //[MaxLength(50, ErrorMessage = "Last Name field cannot be greater than 50 characters")]
        //public string LastName { get; set; }
        //[Required(ErrorMessage = "Email field required")]
        [MaxLength(50, ErrorMessage = "Email address cannot be greater than 50 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public ICollection<Day> Days { get; set; } = new List<Day>();
    }
}
