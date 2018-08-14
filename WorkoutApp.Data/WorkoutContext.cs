using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutApp.Domain;

namespace WorkoutApp.Data
{
    public class WorkoutContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workload> Workloads { get; set; }
        public DbSet<DayExercise> DayExercises { get; set; }
        public DbSet<AccountRequest> AccountRequests { get; set; }

        public WorkoutContext(DbContextOptions<WorkoutContext> options):base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DayExercise>().HasKey(d => new { d.DayId, d.ExerciseId });
        }

    }
}
