using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;
using WorkoutApp.Models;

namespace WorkoutApp.Services
{
    public interface IUtilities
    {
        Exercise CreateExerciseEntity(string exerciseName);
        DayExercise CreateDayExerciseEntity(int dayId, int exerciseId);
        Workload CreateWorkloadEntity(int dayId, int exerciseId);
        Day CreateDayEntity(string name, DateTime date, int userId);
        string GetCurrentAADObjectId();
        bool IsCurrentUsersAccount(string AADObjectId);
        bool IsCurrentUsersAccount(int userId);
        bool IsAdminAccount();

        UserDto MapUserToUserDto(User user);
        User MapUserDtoToNewUser(UserDto userDto);
        User MapUserDtoToUser(UserDto userDto);

        DayDto MapDayToDayDto(Day day);
        Day MapDayDtoToDay(DayDto dayDto);

        ExerciseDto MapExerciseToExerciseDto(Exercise exercise);
        Exercise MapExerciseDtoToExercise(ExerciseDto exerciseDto);
        Exercise MapExerciseDtoToNewExercise(ExerciseDto exerciseDto);

        WorkloadDto MapWorkloadToWorkloadDto(Workload workload);
        Workload MapWorkloadDtoToWorkload(WorkloadDto workloadDto);
    }
}
