using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;

namespace WorkoutApp.Models
{
    public class TrackExercise
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public SortedList<DateTime, Workload> DateWorkloads { get; set; }
    }
}
