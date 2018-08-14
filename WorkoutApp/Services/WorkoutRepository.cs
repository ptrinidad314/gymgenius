using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Data;
using WorkoutApp.Domain;

namespace WorkoutApp.Services
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private WorkoutContext _context;

        public WorkoutRepository(WorkoutContext context)
        {
            _context = context;
        }

        public void AddAccountRequest(AccountRequest accountRequest)
        {
            _context.AccountRequests.Add(accountRequest);
        }

        public void AddDay(Day day)
        {
            _context.Days.Add(day);
        }

        public void AddDayExercise(DayExercise dayExercise)
        {
            _context.DayExercises.Add(dayExercise);
        }

        public void AddDayExerciseForDay(int dayId, DayExercise dayExercise)
        {
            var day = GetDay(dayId);
            if (day != null)
                day.DayExercises.Add(dayExercise);
        }

        public void AddDayExerciseForExercise(int exerciseId, DayExercise dayExercise)
        {
            var exercise = GetExercise(exerciseId);
            if (exercise != null)
                exercise.DayExercises.Add(dayExercise);
        }

        public void AddDayForUser(int userId, Day day)
        {
            var user = GetUser(userId);
            if (user!=null)
                user.Days.Add(day);
        }

        public void AddExercise(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void AddWorkload(Workload workload)
        {
            _context.Workloads.Add(workload);
        }

        public AccountRequest GetAccountRequest(int accountRequestId)
        {
            return _context.AccountRequests.Where(a => a.Id == accountRequestId).FirstOrDefault();
        }

        public IEnumerable<AccountRequest> GetAccountRequests()
        {
            return _context.AccountRequests.OrderBy(a => a.Email).ToList();
        }

        public Day GetDay(int dayId)
        {
            return _context.Days.Include(d => d.DayExercises).Include(d => d.Workloads).Where(d => d.Id == dayId).FirstOrDefault();
        }

        public DayExercise GetDayExercise(int dayId, int exerciseId)
        {
            return _context.DayExercises.Where(d => d.DayId == dayId && d.ExerciseId == exerciseId).FirstOrDefault();
        }

        public IEnumerable<DayExercise> GetDayExercises()
        {
            return _context.DayExercises.ToList();
        }

        public IEnumerable<DayExercise> GetDayExercisesByDay(int dayId)
        {
            return _context.DayExercises.Where(d => d.DayId == dayId).ToList();
        }

        public IEnumerable<DayExercise> GetDayExercisesByExercise(int exerciseId)
        {
            return _context.DayExercises.Where(d => d.ExerciseId == exerciseId).ToList();
        }

        public IEnumerable<Day> GetDays()
        {
            return _context.Days.OrderBy(d => d.Date).ToList();
        }

        public IEnumerable<Day> GetDaysForUser(int userId)
        {
            return _context.Days.Include(d => d.DayExercises).Include(d=>d.Workloads).Where(d=>d.UserId == userId).ToList();
        }

        public Exercise GetExercise(int exerciseId)
        {
            return _context.Exercises.Include(e => e.DayExercises).Include(e => e.Workloads).Where(e => e.Id == exerciseId).FirstOrDefault();
        }

        public Exercise GetExerciseByName(string exerciseName)
        {
            return _context.Exercises.Include(e => e.DayExercises).Include(e => e.Workloads).Where(e => e.Name == exerciseName).FirstOrDefault();
        }

        public IEnumerable<Exercise> GetExercises()
        {
            return _context.Exercises.OrderBy(e => e.Name).ToList();
        }

        public IEnumerable<Exercise> GetExercisesByUser(int userId)
        {
            var user = _context.Users.Include(u=>u.Days).Where(u => u.Id == userId).FirstOrDefault();
            var exercises = new List<Exercise>();
            var exerciseIds = new List<int>();
            foreach(var d in user.Days)
            {

                var day = _context.Days.Include(x => x.DayExercises).Where(x => x.Id == d.Id).FirstOrDefault();

                foreach (var de in day.DayExercises)
                {
                    if (!exerciseIds.Contains(de.ExerciseId))
                    {
                        exerciseIds.Add(de.ExerciseId);
                        exercises.Add(GetExercise(de.ExerciseId));
                    }
                }
            }

            return exercises;
        }

        public User GetUser(int userId)
        {
            return _context.Users.Include(u => u.Days).Where(u => u.Id == userId).FirstOrDefault();
        }

        public User GetUserByAADObjectId(string objectId)
        {
            return _context.Users.Where(u => u.AADObjectId == objectId).FirstOrDefault();
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.OrderBy(u=>u.UserName).ToList();
        }

        public SortedList<DateTime, Workload> GetUserWorkloadsByExercise(int userId, int exerciseId)
        {
            SortedList<DateTime, Workload> DateWorkloads = new SortedList<DateTime, Workload>();

            var user = _context.Users.Include(u => u.Days).Where(u => u.Id == userId).FirstOrDefault();

            foreach(var d in user.Days)
            {
                var day = _context.Days.Include(x => x.Workloads).Where(x => x.Id == d.Id).FirstOrDefault();
                foreach(var w in day.Workloads)
                {
                    if (w.ExerciseId == exerciseId)
                        DateWorkloads[day.Date] = w;
                }
            }

            return DateWorkloads;
        }

        public Workload GetWorkload(int workloadId)
        {
            return _context.Workloads.Include(u=>u.Day).Where(w => w.Id == workloadId).FirstOrDefault();
        }

        public IEnumerable<Workload> GetWorkloads()
        {
            return _context.Workloads.OrderBy(w => w.Id).ToList();
        }

        public void ModifyAccountRequest(AccountRequest accountRequest)
        {
            _context.Attach(accountRequest).State = EntityState.Modified;
        }

        public void ModifyDay(Day day)
        {
            _context.Attach(day).State = EntityState.Modified;
        }

        public void ModifyDayExercise(DayExercise dayExercise)
        {
            _context.Attach(dayExercise).State = EntityState.Modified;
        }

        public void ModifyExercise(Exercise exercise)
        {
            _context.Attach(exercise).State = EntityState.Modified;
        }

        public void ModifyUser(User user)
        {
            _context.Attach(user).State = EntityState.Modified;
        }

        public void ModifyWorkload(Workload workload)
        {
            _context.Attach(workload).State = EntityState.Modified;
        }

        public void RemoveAccountRequest(AccountRequest accountRequest)
        {
            _context.AccountRequests.Remove(accountRequest);
        }

        public void RemoveDay(Day day)
        {
            _context.Days.Remove(day);
        }

        public void RemoveDayExercise(DayExercise dayExercise)
        {
            _context.DayExercises.Remove(dayExercise);
        }

        public void RemoveExercise(Exercise exercise)
        {
            _context.Exercises.Remove(exercise);
        }

        public void RemoveUser(User user)
        {
            _context.Users.Remove(user);
        }

        public void RemoveWorkload(Workload workload)
        {
            _context.Workloads.Remove(workload);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}
