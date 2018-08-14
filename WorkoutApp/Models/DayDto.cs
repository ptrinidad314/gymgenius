using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;

namespace WorkoutApp.Models
{
    public class DayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public IEnumerable<Exercise> Exercises { get; set; }
        public ICollection<Workload> Workloads { get; set; }
        public IEnumerable<string> ExerciseNames { get; set; }
        public int UserId { get; set; }
    }
}
