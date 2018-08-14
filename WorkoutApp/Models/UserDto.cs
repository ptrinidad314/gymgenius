using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;

namespace WorkoutApp.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ObjectId { get; set; }
        public bool hasAccount { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public ICollection<Day> Days { get; set; }
        public ICollection<Workload> Workloads { get; set; }
    }
}
