using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Questionnaireonnaire.Models;
using Questionnaireonnaire.ViewModels;

namespace Questionnaireonnaire.Controllers
{
    public class UsersController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly QuestionnaireContext _db;

        public UsersController(ILogger<HomeController> logger, QuestionnaireContext db)
        {
            _logger = logger;
            _db = db;
        }

        [Route("[Controller]/{id}")]
        public IActionResult Get(int id)
        {
            UserData? user = _db.Users
                                .Include(u => u.Responses)
                                .ThenInclude(r => r.Answer)
                                .Include(u => u.Responses)
                                .ThenInclude(r => r.Question)
                                .ThenInclude(q => q.Answers)
                                .First(u => u.Id == id);

            if (user == null) return RedirectToAction("Index", "Home");
            if (user.NextQuestion != null) return RedirectToAction("Questions", new { userId = id, questionId = user.NextQuestion });

            /*_db.Entry(user).Collection(u => u.Responses).Load();
            foreach(Response r in user.Responses)
            {
                _db.Entry(r).Reload();
            }*/
            return View(user);
        }

        [Route("[Controller]/{userId}/questions/{questionId}")]
        public IActionResult Questions(int userId, int questionId)
        {
            Question? question = _db.Questions.Find(questionId);
            _db.Entry(question).Collection(q => q.Answers).Load();
            UserData? user = _db.Users.Find(userId);
            if (question == null || user == null || user.NextQuestion != questionId)
            {
                return RedirectToAction("Index", "Home");
            }
            QuestionData data = new QuestionData { Question = question, UserId = userId };
            return View(data);
        }

        [HttpPost]
        [Route("[Controller]/Respond")]
        public IActionResult Respond()
        {
            Question? q = _db.Questions.Find(int.Parse(Request.Form["QuestionId"]));
            Answer? a = _db.Answers.Find(int.Parse(Request.Form["AnswerId"]));
            UserData? user = _db.Users.Find(int.Parse(Request.Form["UserDataId"]));
            if(q == null || a == null || user == null || user.NextQuestion != q.Id)
            {
                return BadRequest();
            }
            Response response = new Response
            {
                Question = q,
                Answer = a,
            };
            user.Responses.Add(response);

            if(user.NextQuestion == user.LastQuestion)
            {
                user.NextQuestion = null;
            }
            else
            {
                try
                {
                    Question? next = _db.Questions.OrderBy(q => q.Id).First(question => question.Id > user.NextQuestion && !question.disabled);
                    if (next == null)
                    {
                        user.NextQuestion = null;
                    }
                    else
                    {
                        user.NextQuestion = next.Id;
                    }
                }
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }

            }

            _db.Users.Update(user);
            _db.Responses.Add(response);
            _db.SaveChanges();

            if (user.NextQuestion == null) return RedirectToAction("Get", "Users", new { id = user.Id });
            return RedirectToAction("Questions", "Users", new { userId = user.Id, questionId = user.NextQuestion });
        }
    }
}
