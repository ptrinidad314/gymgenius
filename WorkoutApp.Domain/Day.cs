using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WorkoutApp.Domain
{
    public class Day
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "Day Name field cannot be longer than 50 characters")]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = "Day Notes field cannot be longer than 100 characters")]
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public ICollection<DayExercise> DayExercises { get; set; } = new List<DayExercise>();
        public ICollection<Workload> Workloads { get; set; } = new List<Workload>();
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
