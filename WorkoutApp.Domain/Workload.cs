using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkoutApp.Domain
{
    public class Workload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int Duration { get; set; }
        public int Weight { get; set; }
        public int Distance { get; set; }
        [MaxLength(100, ErrorMessage = "Workload Notes field cannot be longer than 100 characters")]
        public string Notes { get; set; }
        [ForeignKey("DayId")]
        public Day Day { get; set; }
        public int DayId { get; set; }
        [ForeignKey("ExerciseId")]
        public Exercise Exercise { get; set; }
        public int ExerciseId { get; set; }
    }
}
