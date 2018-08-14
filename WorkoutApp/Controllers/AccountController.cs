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
    public class AccountController : Controller
    {
        private IWorkoutRepository _workoutRepository;

        public AccountController(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public IActionResult ViewAccountRequests()
        {
            var accountRequests = _workoutRepository.GetAccountRequests();
            var model = new AccountRequestDto();
            model.AccountRequests = accountRequests;

            return View(model);
        }

        public IActionResult ViewAccountRequest(int id)
        {
            var accountRequest = _workoutRepository.GetAccountRequest(id);
            var model = new AccountRequestDto();
            model.Email = accountRequest.Email;
            model.Complete = accountRequest.Complete;

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult CreateAccountRequest()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccountRequest(AccountRequestDto accountRequestDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("CreateAccountRequest", "Account");

            var accountRequest = new AccountRequest();
            accountRequest.Email = accountRequestDto.Email;
            accountRequest.Complete = false;

            _workoutRepository.AddAccountRequest(accountRequest);

            if (!_workoutRepository.Save())
                return RedirectToAction("CreateAccountRequest", "Account");

            return RedirectToAction("SuccessfulAccountRequest", "Account");
        }

        public IActionResult EditAccountRequest(int id)
        {
            var request = _workoutRepository.GetAccountRequest(id);

            if (request == null)
                return RedirectToAction("ViewAccountRequests", "Account");

            var model = new AccountRequestDto();
            model.id = request.Id;
            model.Email = request.Email;
            model.Complete = request.Complete;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccountRequest(AccountRequestDto accountRequestDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Home");

            var request = _workoutRepository.GetAccountRequest(accountRequestDto.id);

            if (request == null)
                return RedirectToAction("Index", "Home");

            request.Email = accountRequestDto.Email;
            request.Complete = accountRequestDto.Complete;
            _workoutRepository.ModifyAccountRequest(request);

            if (!_workoutRepository.Save())
                return RedirectToAction("Index", "Home");

            return RedirectToAction("ViewAccountRequests", "Account");
        }

        [AllowAnonymous]
        public IActionResult SuccessfulAccountRequest()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
