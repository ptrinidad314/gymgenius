using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;

namespace WorkoutApp.Models
{
    public class SelectExerciseDto
    {
        public int UserId { get; set; }
        public IEnumerable<Exercise> Exercises { get; set; }
        public string SelectedExerciseName { get; set; }
    }
}
