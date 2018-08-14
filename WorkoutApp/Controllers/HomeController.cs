using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkoutApp.Models;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Authorize]
    public class HomeController:Controller
    {
        private IWorkoutRepository _workoutRepository;
        private IUtilities _utilities;

        public HomeController(IWorkoutRepository workoutRepository, IUtilities utilities)
        {
            _workoutRepository = workoutRepository;
            _utilities = utilities;
        }

        [AllowAnonymous]
        public IActionResult Index(bool isChecked = false)
        {
            var model = new UserDto();

            if (User.Identity.IsAuthenticated && isChecked == false)
            {
                var objectId = _utilities.GetCurrentAADObjectId();
                var userEntity = _workoutRepository.GetUserByAADObjectId(objectId);

                if (userEntity == null)
                {
                    return RedirectToAction("Index", "Home", new { isChecked = true});
                }
                else
                {
                    model.ObjectId = objectId;
                    model.Id = userEntity.Id;
                    model.hasAccount = true;

                    return View(model);
                }
            }

            model.hasAccount = false;

            return View(model);
        }

      
    }
}
