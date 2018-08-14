using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutApp.Domain
{
    public class DayExercise
    {
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; }
    }
}
