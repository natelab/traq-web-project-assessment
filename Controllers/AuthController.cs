﻿using Microsoft.AspNetCore.Mvc;
using traq_web_project_assessment.Services;
using traq_web_project_assessment.ViewModels;

namespace traq_web_project_assessment.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to home
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _authService.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Store user info in session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("FullName", user.FullName ?? user.Username);

            TempData["SuccessMessage"] = $"Welcome back, {user.FullName ?? user.Username}!";

            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["InfoMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }

        // GET: /Auth/Register (Optional - for creating new users)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword, string fullName)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return View();
            }

            if (await _authService.UsernameExistsAsync(username))
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return View();
            }

            var success = await _authService.RegisterAsync(username, password, fullName);

            if (success)
            {
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
            return View();
        }
    }
}