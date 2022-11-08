using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Questionnaireonnaire.Models;
using Questionnaireonnaire.ViewModels;
using System.Diagnostics;

namespace Questionnaireonnaire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QuestionnaireContext _db;

        public HomeController(ILogger<HomeController> logger, QuestionnaireContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login()
        {
            string? email = Request.Form["email"];
            if (email == null) email = "";

            UserData? user;
            try
            {
                if(_db.Users.Count(u => u.Email == email) == 0)
                {
                    if (_db.Questions.Count(q => q.disabled == false) == 0)
                    {
                        user = new UserData
                        {
                            Email = email,
                        };
                        _db.Users.Add(user);
                    }
                    else
                    {
                        user = new UserData
                        {

                            Email = email,
                            NextQuestion = _db.Questions.OrderBy(q => q.Id).First(q => q.disabled == false).Id,
                            LastQuestion = _db.Questions.OrderBy(q => q.Id).Last(q => q.disabled == false).Id,
                        };
                        _db.Users.Add(user);
                    }
                    _db.SaveChanges();
                }
                else
                {
                    user = _db.Users.First(u => u.Email == email);

                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }


            if (user.NextQuestion == null)
            {
                return RedirectToAction("Get", "Users", new { id = user.Id });
            }
            else
            {
                return RedirectToAction("Questions", "Users", new { userId = user.Id, questionId = user.NextQuestion });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}