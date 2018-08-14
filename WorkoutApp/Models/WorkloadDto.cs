using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;

namespace WorkoutApp.Models
{
    public class WorkloadDto
    {
        public int workloadId { get; set; }
        public int sets { get; set; }
        public int reps { get; set; }
        public int weight { get; set; }
        public int duration { get; set; }
        public int distance { get; set; }
        public string Notes { get; set; }
        public string exerciseName { get; set; }
        public ExerciseType exerciseType { get; set; }
        public int exerciseId { get; set; }
        public int dayId { get; set; }
    }
}
