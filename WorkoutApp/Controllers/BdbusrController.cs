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
    [Authorize(Policy = "RequireElevatedRights")]
    public class BdbusrController:Controller
    {
        private IWorkoutRepository _workoutRepository;
        private IUtilities _utilities;

        public BdbusrController(IWorkoutRepository workoutRepository, IUtilities utilities)
        {
            _workoutRepository = workoutRepository;
            _utilities = utilities;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewUsers()
        {
            var users = _workoutRepository.GetUsers();

            if (users == null)
                return NotFound();

            var model = new List<UserDto>();

            foreach (var u in users)
            {
                var userDto = _utilities.MapUserToUserDto(u);
               
                model.Add(userDto);
            }

            return View(model);
        }

        public IActionResult ViewUser(int id)
        {
            var user = _workoutRepository.GetUser(id);

            if (user == null)
                return RedirectToAction("index", "bdbusr");

            var model = _utilities.MapUserToUserDto(user);

            return View(model);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("CreateUser", "bdbusr");

            var user = _utilities.MapUserDtoToNewUser(userDto);

            //var user = new User();
            //user.AADObjectId = userDto.ObjectId;
            //user.UserName = userDto.UserName;

            //if (userDto.Email.Length > 0)
            //    user.Email = userDto.Email;

            _workoutRepository.AddUser(user);

            if (!_workoutRepository.Save())
            {
                return RedirectToAction("CreateUser", "bdbusr");
            }

            return RedirectToAction("ViewUser", "Bdbusr", new { id = user.Id });
        }

        public IActionResult UpdateUser(int id)
        {
            var user = _workoutRepository.GetUser(id);

            if (user == null)
                return RedirectToAction("ViewUser", "bdbusr", new { id = id });

            var model = _utilities.MapUserToUserDto(user);

            //var model = new UserDto();
            //model.Id = id;
            //model.ObjectId = user.AADObjectId;
            //model.UserName = user.UserName;
            //model.Height = user.Height;
            //model.Weight = user.Weight;
            //model.Email = user.Email;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("updateuser", "bdbusr", new { id=userDto.Id});

            var user = _utilities.MapUserDtoToUser(userDto);

            //var user = _workoutRepository.GetUser(userDto.Id);

            //if (user == null)
            //    return NotFound();

            //user.UserName = userDto.UserName;
            //user.Email = userDto.Email;
            //if (userDto.Height > 0)
            //    user.Height = userDto.Height;
            //if (userDto.Weight > 0)
            //    user.Weight = userDto.Weight;

            _workoutRepository.ModifyUser(user);

            if (!_workoutRepository.Save())
            {
                return RedirectToAction("UpdateUser", "bdbusr");
            }

            return RedirectToAction("ViewUser", "bdbusr", new { id = user.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            var user = _workoutRepository.GetUser(id);

            if (user == null)
                return RedirectToAction("ViewUser","bdbusr", new { id=id});

            _workoutRepository.RemoveUser(user);

            if (!_workoutRepository.Save())
                return RedirectToAction("ViewUser", "bdbusr", new { id=id});

            return RedirectToAction("ViewUsers", "bdbusr");
        }


        public IActionResult ViewDays()
        {
            var days = _workoutRepository.GetDays();

            List<DayDto> model = new List<DayDto>();

            foreach(var d in days)
            {
                var dayDto = _utilities.MapDayToDayDto(d);

                model.Add(dayDto);
            }

            return View(model);
        }

        public IActionResult ViewDay(int Id)
        {
            var day = _workoutRepository.GetDay(Id);

            if (day == null)
                return RedirectToAction("ViewDays", "bdbusr");

            var model = _utilities.MapDayToDayDto(day);

            //var model = new DayDto();
            //model.Id = day.Id;
            //model.Name = day.Name;
            //model.Notes = day.Notes;
            //model.Date = day.Date;
            //model.Workloads = day.Workloads;
            //model.UserId = day.UserId;

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

        public IActionResult EditDay(int id)
        {
            var day = _workoutRepository.GetDay(id);

            if (day == null)
                return RedirectToAction("viewday", "bdbusr");

            var model = _utilities.MapDayToDayDto(day);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDay(DayDto dayDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("EditDay", "bdbusr", new { id = dayDto.Id });

            var day = _utilities.MapDayDtoToDay(dayDto);

            _workoutRepository.ModifyDay(day);

            if (!_workoutRepository.Save())
                return RedirectToAction("EditDay", "bdbusr", new { id = dayDto.Id });

            return RedirectToAction("ViewDay", "bdbusr", new { id = dayDto.Id });

        }

        public IActionResult CreateDay(int Id)
        {
           
            var model = new DayDto();
            model.UserId = Id;
            model.Date = DateTime.Now;
            model.Exercises = _workoutRepository.GetExercises();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDay(DayDto dayDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("CreateDay", "bdbusr", new { id = dayDto.UserId });

            var day = _utilities.CreateDayEntity(dayDto.Name, dayDto.Date, dayDto.UserId);
            if (!_workoutRepository.Save() || day == null)
                return RedirectToAction("CreateDay", "bdbusr", new { id = dayDto.UserId });

            foreach (var e in dayDto.ExerciseNames)
            {
                if (e != "")
                {
                    var exercise = _workoutRepository.GetExerciseByName(e);
                    if (exercise == null)
                    {
                        exercise = _utilities.CreateExerciseEntity(e);
                        if (!_workoutRepository.Save())
                            return RedirectToAction("CreateDay", "bdbusr", new { id = dayDto.UserId });
                    }

                    var dayExercise = _utilities.CreateDayExerciseEntity(day.Id, exercise.Id);
                    if (!_workoutRepository.Save() || dayExercise == null)
                        return RedirectToAction("CreateDay", "bdbusr", new { id = dayDto.UserId });

                    var workload = _utilities.CreateWorkloadEntity(day.Id, exercise.Id);
                }
            }

            _workoutRepository.AddDayForUser(dayDto.UserId, day);
            if (!_workoutRepository.Save())
                return RedirectToAction("CreateDay", "bdbusr", new { id = dayDto.UserId });

            return RedirectToAction("ViewDay", "bdbusr", new { id = day.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDay(int id)
        {
            var day = _workoutRepository.GetDay(id);

            if (day == null)
                return NotFound();

            var userId = day.UserId;

            _workoutRepository.RemoveDay(day);

            if (!_workoutRepository.Save())
                return RedirectToAction("ViewDay", "bdbusr", new { id = id });

            return RedirectToAction("ViewUser", "bdbusr", new { id = userId });
        }


        public IActionResult ViewExercise(int id)
        {
            var exercise = _workoutRepository.GetExercise(id);

            var model = _utilities.MapExerciseToExerciseDto(exercise);

            //var model = _workoutRepository.GetExercise(id);

            //if (model == null)
            //    return RedirectToAction("Index", "Home");

            return View(model);
        }

        
        public IActionResult ViewExercises()
        {
            
            var exercises = _workoutRepository.GetExercises();
            List<ExerciseDto> model = new List<ExerciseDto>();

            foreach(var e in exercises)
            {
                var exerciseDto = _utilities.MapExerciseToExerciseDto(e);

                model.Add(exerciseDto);
            }


            //var model = _workoutRepository.GetExercises();

            return View(model);
        }

        public IActionResult UpdateExercise(int id)
        {
            var exercise = _workoutRepository.GetExercise(id);

            var model = _utilities.MapExerciseToExerciseDto(exercise);

            //var model = _workoutRepository.GetExercise(id);

            //if (model == null)
            //    return RedirectToAction("Index", "Home");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateExercise(ExerciseDto exerciseDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("UpdateExercise", "bdbusr", new { id = exerciseDto.Id });

            var exercise = _utilities.MapExerciseDtoToExercise(exerciseDto);

            //var exercise = _workoutRepository.GetExercise(exerciseDto.Id);
            //if (exercise == null)
            //    return RedirectToAction("UpdateExercise", "Exercise", new { id = exerciseDto.Id });

            //exercise.Name = exerciseDto.Name;
            //exercise.Type = exerciseDto.Type;

            _workoutRepository.ModifyExercise(exercise);

            if (!_workoutRepository.Save())
                return RedirectToAction("UpdateExercise", "bdbusr", new { id = exerciseDto.Id });

            return RedirectToAction("ViewExercise", "bdbusr", new { id = exerciseDto.Id });

        }

        public IActionResult CreateExercise()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateExercise(ExerciseDto exerciseDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("CreateExercise", "bdbusr");

            var exercise = _utilities.MapExerciseDtoToNewExercise(exerciseDto);

            //var exercise = new Exercise();

            //exercise.Name = exerciseDto.Name;
            //exercise.Type = exerciseDto.Type;

            _workoutRepository.AddExercise(exercise);

            if (!_workoutRepository.Save())
                return RedirectToAction("CreateExercise", "bdbusr");

            return RedirectToAction("ViewExercise", "bdbusr", new { id = exercise.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteExercise(int id)
        {
            var exercise = _workoutRepository.GetExercise(id);

            if (exercise == null)
                return RedirectToAction("ViewExercise", "bdbusr", new { id = id });

            _workoutRepository.RemoveExercise(exercise);

            if (!_workoutRepository.Save())
                return RedirectToAction("ViewExercise", "bdbusr", new { id = id });

            return RedirectToAction("ViewExercises", "bdbusr");
        }

        public IActionResult UpdateWorkload(int id)
        {
            var workload = _workoutRepository.GetWorkload(id);
            var exercise = _workoutRepository.GetExercise(workload.ExerciseId);

            if (workload == null)
                return RedirectToAction("Index", "Home");

            var model = _utilities.MapWorkloadToWorkloadDto(workload);
            model.exerciseName = exercise.Name;
  
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateWorkload(WorkloadDto workloadDto)
        {
            var workload = _utilities.MapWorkloadDtoToWorkload(workloadDto);
            
            _workoutRepository.ModifyWorkload(workload);

            if (!_workoutRepository.Save())
                return RedirectToAction("UpdateWorkload", "bdbusr", new { id = workloadDto.workloadId });

            return RedirectToAction("ViewDay", "bdbusr", new { id = workloadDto.dayId });

        }

        public IActionResult AddExerciseToDay(int id)
        {
            var day = _workoutRepository.GetDay(id);

            if (day == null)
                return RedirectToAction("Index", "Home");

            var model = _utilities.MapDayToDayDto(day);
            model.Exercises = _workoutRepository.GetExercises();

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddExerciseToDay(WorkloadDto workloadDto)
        {
            var exercise = _workoutRepository.GetExerciseByName(workloadDto.exerciseName);

            if (exercise == null)
            {
                exercise = _utilities.CreateExerciseEntity(workloadDto.exerciseName);
                exercise.Type = workloadDto.exerciseType;
                if (!_workoutRepository.Save())
                    return RedirectToAction("ViewDay", "bdbusr", new { id = workloadDto.dayId });                 
            }

            var dayExercise = _utilities.CreateDayExerciseEntity(workloadDto.dayId, exercise.Id);
            if (!_workoutRepository.Save() || dayExercise == null)
                return RedirectToAction("ViewDay", "bdbusr", new { id = workloadDto.dayId });
                


            var workload = _utilities.CreateWorkloadEntity(workloadDto.dayId, exercise.Id);
            if (!_workoutRepository.Save())
                return RedirectToAction("ViewDay", "bdbusr", new { id = workloadDto.dayId });

            workloadDto.workloadId = workload.Id;
            workload = _utilities.MapWorkloadDtoToWorkload(workloadDto);

            if(!_workoutRepository.Save())
                return RedirectToAction("ViewDay", "bdbusr", new { id = workloadDto.dayId });

            return RedirectToAction("ViewDay", "bdbusr", new { id = workloadDto.dayId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteExerciseFromDay(int id)
        {
            var workload = _workoutRepository.GetWorkload(id);
            if (workload == null)
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


            return RedirectToAction("ViewDay", "bdbusr", new { id = dayId });

           
        }

    }
}
