using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;

namespace WorkoutApp.Models
{
    public class AccountRequestDto
    {
        public IEnumerable<AccountRequest> AccountRequests { get; set; }
        public string Email { get; set; }
        public bool Complete { get; set; }
        public int id { get; set; }
    }
}
