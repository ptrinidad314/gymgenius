using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;
using WorkoutApp.Models;
using WorkoutApp.Services;
using WorkoutApp.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace WorkoutApp.Controllers
{
    [Authorize]
    public class ExerciseController:Controller
    {
        private IWorkoutRepository _workoutRepository;
        private IUtilities _utilities;

        public ExerciseController(IWorkoutRepository workoutRepository, IUtilities utilities)
        {
            _workoutRepository = workoutRepository;
            _utilities = utilities;
        }


        public IActionResult UpdateMyWorkload(int id)
        {
            var workload = _workoutRepository.GetWorkload(id);
            var exercise = _workoutRepository.GetExercise(workload.ExerciseId);

            if (!_utilities.IsCurrentUsersAccount(workload.Day.UserId) || workload == null)
                return RedirectToAction("Index","Home");

            var model = _utilities.MapWorkloadToWorkloadDto(workload);
            model.exerciseName = exercise.Name;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateMyWorkload(WorkloadDto workloadDto)
        {
            var workload = _utilities.MapWorkloadDtoToWorkload(workloadDto);
            
            _workoutRepository.ModifyWorkload(workload);

            if (!_workoutRepository.Save())
                return RedirectToAction("UpdateMyWorkload", "Exercise", new { id=workloadDto.workloadId});

            return RedirectToAction("ViewMyDay", "Day", new { id = workloadDto.dayId });

        }

        public IActionResult AddExerciseToMyDay(int id)
        {
            var day = _workoutRepository.GetDay(id);

            if (day == null)
                return RedirectToAction("Index","Home");

            var model = _utilities.MapDayToDayDto(day);
            model.Exercises = _workoutRepository.GetExercises();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddExerciseToMyDay(WorkloadDto workloadDto)
        {
            var exercise = _workoutRepository.GetExerciseByName(workloadDto.exerciseName);

            if(exercise == null)
            {
                exercise = _utilities.CreateExerciseEntity(workloadDto.exerciseName);
                exercise.Type = workloadDto.exerciseType;
                if(!_workoutRepository.Save())
                    return RedirectToAction("ViewMyDay", "Day", new { id = workloadDto.dayId });

            }

            var dayExercise = _utilities.CreateDayExerciseEntity(workloadDto.dayId, exercise.Id);
            if (!_workoutRepository.Save() || dayExercise == null)
                return RedirectToAction("ViewMyDay", "Day", new { id = workloadDto.dayId });


            var workload = _utilities.CreateWorkloadEntity(workloadDto.dayId, exercise.Id);
            if (!_workoutRepository.Save())
                return RedirectToAction("ViewMyDay", "Day", new { id = workloadDto.dayId});

            workloadDto.workloadId = workload.Id;
            workload = _utilities.MapWorkloadDtoToWorkload(workloadDto);
            if (!_workoutRepository.Save())
                return RedirectToAction("ViewMyDay", "Day", new { id=workloadDto.dayId});

            return RedirectToAction("ViewMyDay", "Day", new { id = workloadDto.dayId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteExerciseFromMyDay(int id)
        {
            var workload = _workoutRepository.GetWorkload(id);
            if (workload == null)
                return RedirectToAction("Index", "Home");

            if (!_utilities.IsCurrentUsersAccount(workload.Day.UserId) && !_utilities.IsAdminAccount())
                return RedirectToAction("Index", "Home");

            var dayId = workload.DayId;

            var dayExercise = _workoutRepository.GetDayExercise(workload.DayId, workload.ExerciseId);
            if (dayExercise == null)
                return RedirectToAction("Index", "Home");

            _workoutRepository.RemoveWorkload(workload);
            if (!_workoutRepository.Save())
                return RedirectToAction("Index", "Home");

            _workoutRepository.RemoveDayExercise(dayExercise);
            if (!_workoutRepository.Save())
                return RedirectToAction("Index", "Home");


            if(_utilities.IsAdminAccount())
                return RedirectToAction("ViewDay", "Day", new { id=dayId});
            else
                return RedirectToAction("ViewMyDay", "Day", new { id = dayId });


        }
        
    }
}
