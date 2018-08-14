using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;

namespace WorkoutApp.Services
{
    public interface IWorkoutRepository
    {
        User GetUser(int userId);
        User GetUserByAADObjectId(string email);
        IEnumerable<User> GetUsers();
        void AddUser(User user);
        void ModifyUser(User user);
        void RemoveUser(User user);
        bool Save();

        IEnumerable<Day> GetDays();
        Day GetDay(int dayId);
        void AddDay(Day day);
        void ModifyDay(Day day);
        void RemoveDay(Day day);
        void AddDayForUser(int userId, Day day);
        IEnumerable<Day> GetDaysForUser(int userId);

        IEnumerable<Exercise> GetExercises();
        IEnumerable<Exercise> GetExercisesByUser(int userId);
        Exercise GetExercise(int exerciseId);
        Exercise GetExerciseByName(string exerciseName);
        void AddExercise(Exercise exercise);
        void ModifyExercise(Exercise exercise);
        void RemoveExercise(Exercise exercise);

        IEnumerable<Workload> GetWorkloads();
        Workload GetWorkload(int workloadId);
        void AddWorkload(Workload workload);
        void ModifyWorkload(Workload workload);
        void RemoveWorkload(Workload workload);
        SortedList<DateTime, Workload> GetUserWorkloadsByExercise(int userId, int exerciseId);

        IEnumerable<DayExercise> GetDayExercises();
        DayExercise GetDayExercise(int dayId, int exerciseId);
        IEnumerable<DayExercise> GetDayExercisesByDay(int dayId);
        IEnumerable<DayExercise> GetDayExercisesByExercise(int exerciseId);
        void AddDayExercise(DayExercise dayExercise);
        void ModifyDayExercise(DayExercise dayExercise);
        void RemoveDayExercise(DayExercise dayExercise);
        void AddDayExerciseForDay(int dayId, DayExercise dayExercise);
        void AddDayExerciseForExercise(int exerciseId, DayExercise dayExercise);

        AccountRequest GetAccountRequest(int accountRequestId);
        IEnumerable<AccountRequest> GetAccountRequests();
        void AddAccountRequest(AccountRequest accountRequest);
        void ModifyAccountRequest(AccountRequest accountRequest);
        void RemoveAccountRequest(AccountRequest accountRequest);
    }
}
