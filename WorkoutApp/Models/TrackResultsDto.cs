using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;

namespace WorkoutApp.Models
{
    public class TrackResultsDto
    {
        public int UserId { get; set; }
        public ICollection<TrackExercise> TrackExercises { get; set; }
        public string Labels { get; set; }
        public string Data { get; set; }
        public string ExerciseName { get; set; }
        public string ChartTitle { get; set; }
    }
}
