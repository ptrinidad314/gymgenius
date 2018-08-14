using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkoutApp.Domain
{
    public class AccountRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Email field required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(50, ErrorMessage = "Email field cannot be longer than 50 characters")]
        public string Email { get; set; }
        public bool Complete { get; set; } = false;
    }
}
