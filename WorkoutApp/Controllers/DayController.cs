using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Domain;
using WorkoutApp.Models;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Authorize]
    public class DayController:Controller
    {
        private IWorkoutRepository _workoutRepository;
        private IUtilities _utilities;

        public DayController(IWorkoutRepository workoutRepository, IUtilities utilities)
        {
            _workoutRepository = workoutRepository;
            _utilities = utilities;
        }

        public IActionResult EditMyDay(int id)
        {
            var day = _workoutRepository.GetDay(id);

            if (day == null)
                return RedirectToAction("ViewMyDay","Day");

            if (!_utilities.IsCurrentUsersAccount(day.UserId))
                return RedirectToAction("Index", "Home");

            var model = _utilities.MapDayToDayDto(day);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMyDay(DayDto dayDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("EditDay", "Day", new { id = dayDto.Id });

            var day = _utilities.MapDayDtoToDay(dayDto);

            _workoutRepository.ModifyDay(day);

            if (!_workoutRepository.Save())
                return RedirectToAction("EditDay", "Day", new { id = dayDto.Id });

            return RedirectToAction("ViewMyDay", "Day", new { id = dayDto.Id });

        }

       
        public IActionResult CreateMyDay(int Id)
        {
            
            if (!_utilities.IsCurrentUsersAccount(Id))
                return RedirectToAction("Index", "Home");

            var model = new DayDto();
            model.UserId = Id;
            model.Date = DateTime.Now;
            model.Exercises = _workoutRepository.GetExercises();

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMyDay(DayDto dayDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("CreateDay","Day", new { id=dayDto.UserId});

            var day = _utilities.CreateDayEntity(dayDto.Name, dayDto.Date, dayDto.UserId);
            if (!_workoutRepository.Save() || day == null)
                return RedirectToAction("CreateDay", "Day", new { id = dayDto.UserId });

            foreach (var e in dayDto.ExerciseNames)
            {
                if (e != "")
                {
                    var exercise = _workoutRepository.GetExerciseByName(e);
                    if(exercise == null)
                    {
                        exercise = _utilities.CreateExerciseEntity(e);
                        if (!_workoutRepository.Save())
                            return RedirectToAction("CreateDay", "Day", new { id = dayDto.UserId });
                    }

                    var dayExercise = _utilities.CreateDayExerciseEntity(day.Id, exercise.Id);
                    if (!_workoutRepository.Save() || dayExercise == null)
                        return RedirectToAction("CreateDay", "Day", new { id=dayDto.UserId});

                    var workload = _utilities.CreateWorkloadEntity(day.Id, exercise.Id);
                }
            }

            _workoutRepository.AddDayForUser(dayDto.UserId, day);
            if (!_workoutRepository.Save())
                return RedirectToAction("CreateDay", "Day", new { id=dayDto.UserId});

            return RedirectToAction("ViewMyDay","Day",new { id=day.Id});
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMyDay(int id)
        {
            var day = _workoutRepository.GetDay(id);

            if (day == null)
                return NotFound();

            if (!_utilities.IsCurrentUsersAccount(day.UserId))
                return RedirectToAction("Index", "Home");

            _workoutRepository.RemoveDay(day);

            if (!_workoutRepository.Save())
                return RedirectToAction("ViewDays", "Day", new { id=day.UserId});

            return RedirectToAction("ViewMyDays", "Day", new { id = day.UserId });
        }

        public IActionResult ViewMyDays(int id)
        {
            var user = _workoutRepository.GetUser(id);

            if (user == null)
                return RedirectToAction("Index", "Home");

            if (!_utilities.IsCurrentUsersAccount(user.AADObjectId))
                return RedirectToAction("Index", "Home");

            var model = _utilities.MapUserToUserDto(user);

            return View(model);
        }

        public IActionResult ViewMyDay(int id)
        {
            var day = _workoutRepository.GetDay(id);

            if (day == null)
                return RedirectToAction("Index", "Home");

            if (!_utilities.IsCurrentUsersAccount(day.UserId))
                return RedirectToAction("Index","Home");

            var model = _utilities.MapDayToDayDto(day);

            var exerciseList = new List<Exercise>();

            foreach (var e in day.DayExercises)
            {
                var exercise = _workoutRepository.GetExercise(e.ExerciseId);
                if (exercise != null)
                    exerciseList.Add(exercise);
            }

            model.Exercises = exerciseList;

            return View(model);
        }

    }
}
