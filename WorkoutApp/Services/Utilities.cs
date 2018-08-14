using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;
using WorkoutApp.Models;

namespace WorkoutApp.Services
{
    public class Utilities : IUtilities
    {
        private IWorkoutRepository _workoutRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;

        public Utilities(IWorkoutRepository workoutRepository, IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration)
        {
            _workoutRepository = workoutRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public Day CreateDayEntity(string name, DateTime date, int userId) 
        {
            var day = new Day();
            if (name != null && name != "")
                day.Name = name;

            day.Date = date;
            day.UserId = userId;
            _workoutRepository.AddDay(day);
            return day;
        }

        public DayExercise CreateDayExerciseEntity(int dayId, int exerciseId)
        {
            var dayExercise = new DayExercise();
            dayExercise.DayId = dayId;
            dayExercise.ExerciseId = exerciseId;
            _workoutRepository.AddDayExercise(dayExercise);
            return dayExercise;
        }

        public Exercise CreateExerciseEntity(string exerciseName)
        {
            var exercise = new Exercise();
            exercise.Name = exerciseName;
            exercise.Type = 0;
            _workoutRepository.AddExercise(exercise);

            return exercise;
        }

        public Workload CreateWorkloadEntity(int dayId, int exerciseId)
        {
            var workload = new Workload();
            workload.Sets = 0;
            workload.Reps = 0;
            workload.Weight = 0;
            workload.Duration = 0;
            workload.Distance = 0;
            workload.Notes = "";
            workload.DayId = dayId;
            workload.ExerciseId = exerciseId;
            _workoutRepository.AddWorkload(workload);
            return workload;
        }

        public string GetCurrentAADObjectId()
        {
            return _httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").FirstOrDefault().Value;
        }

        public bool IsCurrentUsersAccount(string AADObjectId)
        {
            return (GetCurrentAADObjectId() == AADObjectId);
        }

        public bool IsCurrentUsersAccount(int userId)
        {
            var user = _workoutRepository.GetUser(userId);

            if (user == null)
                return false;

            return (GetCurrentAADObjectId() == user.AADObjectId);
        }

        public bool IsAdminAccount()
        {
            return (_configuration["AzureAd:AdminObjectId"] == GetCurrentAADObjectId());
        }

        public UserDto MapUserToUserDto(User user)
        {
            var userDto = new UserDto();

            userDto.Id = user.Id;
            userDto.UserName = user.UserName;
            userDto.Email = user.Email;
            userDto.ObjectId = user.AADObjectId;
            userDto.hasAccount = true;
            userDto.Height = user.Height;
            userDto.Weight = user.Weight;
            userDto.Days = user.Days;
            //userDto.Workloads = user.work

            return userDto;
        }

        public User MapUserDtoToNewUser(UserDto userDto)
        {
            var user = new User();

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.AADObjectId = userDto.ObjectId;
            user.Height = userDto.Height;
            user.Weight = userDto.Weight;

            return user;
        }

        public User MapUserDtoToUser(UserDto userDto)
        {
            var user = _workoutRepository.GetUser(userDto.Id);

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.AADObjectId = userDto.ObjectId;
            user.Height = userDto.Height;
            user.Weight = userDto.Weight;
            if(userDto.Days!=null)
                user.Days = userDto.Days;

            return user;
        }

        public DayDto MapDayToDayDto(Day day)
        {
            var dayDto = new DayDto();

            dayDto.Id = day.Id;
            dayDto.Name = day.Name;
            dayDto.Date = day.Date;
            dayDto.Notes = day.Notes;
            dayDto.Workloads = day.Workloads;
            dayDto.UserId = day.UserId;

            return dayDto;
        }

        public Day MapDayDtoToDay(DayDto dayDto)
        {
            var day = _workoutRepository.GetDay(dayDto.Id);

            day.Name = dayDto.Name;
            day.Date = dayDto.Date;
            day.Notes = dayDto.Notes;
            if(dayDto.Workloads != null)
                day.Workloads = dayDto.Workloads;
            day.UserId = dayDto.UserId;

            return day;
        }

        public ExerciseDto MapExerciseToExerciseDto(Exercise exercise)
        {
            var exerciseDto = new ExerciseDto();

            exerciseDto.Id = exercise.Id;
            exerciseDto.Name = exercise.Name;
            exerciseDto.Type = exercise.Type;

            return exerciseDto;
        }

        public Exercise MapExerciseDtoToExercise(ExerciseDto exerciseDto)
        {
            var exercise = _workoutRepository.GetExercise(exerciseDto.Id);

            exercise.Name = exerciseDto.Name;
            exercise.Type = exerciseDto.Type;

            return exercise;
        }

        public Exercise MapExerciseDtoToNewExercise(ExerciseDto exerciseDto)
        {
            var exercise = new Exercise();

            exercise.Name = exerciseDto.Name;
            exercise.Type = exerciseDto.Type;

            return exercise;
        }

        public WorkloadDto MapWorkloadToWorkloadDto(Workload workload)
        {
            var workloadDto = new WorkloadDto();
            workloadDto.workloadId = workload.Id;
            workloadDto.sets = workload.Sets;
            workloadDto.reps = workload.Reps;
            workloadDto.weight = workload.Weight;
            workloadDto.duration = workload.Duration;
            workloadDto.distance = workload.Distance;
            workloadDto.Notes = workload.Notes;
            workloadDto.exerciseId = workload.ExerciseId;
            workloadDto.dayId = workload.DayId;

            return workloadDto;
        }

        public Workload MapWorkloadDtoToWorkload(WorkloadDto workloadDto)
        {
            var workload = _workoutRepository.GetWorkload(workloadDto.workloadId);

            workload.Sets = workloadDto.sets;
            workload.Reps = workloadDto.reps;
            workload.Weight = workloadDto.weight;
            workload.Duration = workloadDto.duration;
            workload.Distance = workloadDto.distance;
            workload.Notes = workloadDto.Notes;

            return workload;
        }

    }
}
