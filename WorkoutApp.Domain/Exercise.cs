using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkoutApp.Domain
{
    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Exercise Name field required")]
        [MaxLength(50, ErrorMessage = "Exercise Name field cannont be longer than 50 characters")]
        public string Name { get; set; }
        public ExerciseType Type { get; set; }
        public ICollection<DayExercise> DayExercises { get; set; } = new List<DayExercise>();
        public ICollection<Workload> Workloads { get; set; } = new List<Workload>();
    }
}
