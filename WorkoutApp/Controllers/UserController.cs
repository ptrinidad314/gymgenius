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
    public class UserController:Controller
    {
        private IWorkoutRepository _workoutRepository;
        private IUtilities _utilities;

        public UserController(IWorkoutRepository workoutRepository, IUtilities utilities)
        {
            _workoutRepository = workoutRepository;
            _utilities = utilities;
        }

        public IActionResult ViewMyProfile(int id)
        {
            var user = _workoutRepository.GetUser(id);

            if (user == null)
                return RedirectToAction("Index", "Home");

            if (!_utilities.IsCurrentUsersAccount(user.AADObjectId))
                return RedirectToAction("Index", "Home");

            var model = _utilities.MapUserToUserDto(user);

            return View(model);
        }

        public IActionResult UpdateMyProfile(int id)
        {
            var user = _workoutRepository.GetUser(id);

            if (user == null)
                return RedirectToAction("Index", "Home");

            if (!_utilities.IsCurrentUsersAccount(user.AADObjectId))
                return RedirectToAction("Index", "Home");

            var model = _utilities.MapUserToUserDto(user);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateMyProfile(UserDto userDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home");

            var user = _utilities.MapUserDtoToUser(userDto);

            if (!_utilities.IsCurrentUsersAccount(user.AADObjectId))
                return RedirectToAction("Index", "Home");

            _workoutRepository.ModifyUser(user);

            if (!_workoutRepository.Save())
                return RedirectToAction("UpdateMyProfile", "User", new { id=userDto.Id}); 


            return RedirectToAction("ViewMyProfile", "User", new { id = userDto.Id });
        }

       
        public IActionResult SelectExerciseToTrack(int id)
        {
            var user = _workoutRepository.GetUser(id);

            if (user == null)
                return RedirectToAction("Index", "Home");

            if (!_utilities.IsCurrentUsersAccount(user.AADObjectId))
                return RedirectToAction("Index", "Home");

            var exercises = _workoutRepository.GetExercisesByUser(id);

            var model = new SelectExerciseDto();
            model.Exercises = exercises;
            model.UserId = id;

            return View(model);
        }

        public IActionResult TrackResults(SelectExerciseDto selectExerciseDto)
        {
            var exercise = _workoutRepository.GetExerciseByName(selectExerciseDto.SelectedExerciseName);

            if (exercise == null)
                return RedirectToAction("SelectExerciseToTrack", "User", new { id=selectExerciseDto.UserId});

            var DateWorkloads = _workoutRepository.GetUserWorkloadsByExercise(selectExerciseDto.UserId, exercise.Id);

            var trackExercise = new TrackExercise();
            trackExercise.ExerciseId = exercise.Id;
            trackExercise.ExerciseName = exercise.Name;
            trackExercise.DateWorkloads = DateWorkloads;

            var trackExerciseList = new List<TrackExercise>();
            trackExerciseList.Add(trackExercise);

            var model = new TrackResultsDto();
            model.UserId = selectExerciseDto.UserId;
            model.TrackExercises = trackExerciseList;
            
            model.Labels = GetChartLabels(trackExercise);
            
            model.ExerciseName = "\"" + exercise.Name + "\"";
            
            
            if(exercise.Type == ExerciseType.cardio)
            {
                model.ChartTitle = "\"" + exercise.Name + " Time (in minutes)\"";
                model.Data = GetChartDataCardio(trackExercise);
            }
            else
            {
                model.ChartTitle = "\"" + exercise.Name + " weight (in lbs.)\"";
                model.Data = GetChartData(trackExercise);
            }


            return View(model);
        }

        private string GetChartLabels(TrackExercise trackExercise)
        {
            string labels = "[";

            foreach(var d in trackExercise.DateWorkloads)
            {
                labels += "\"" + d.Key.Month.ToString() + "/" + d.Key.Day.ToString() + "/" + d.Key.Year.ToString() + "\",";
            }

            labels += "]";

            return labels;
        }

        private string GetChartData(TrackExercise trackExercise)
        {
            string data = "[";

            foreach(var d in trackExercise.DateWorkloads)
            {
                data += d.Value.Weight.ToString() + ",";
            }

            data += "]";

            return data;
        }

        private string GetChartDataCardio(TrackExercise trackExercise)
        {
            string data = "[";

            foreach(var d in trackExercise.DateWorkloads)
            {
                data += d.Value.Duration.ToString() + ",";
            }

            data += "]";

            return data;
        }
    }
}
