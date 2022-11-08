using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questionnaireonnaire.Models;
using Questionnaireonnaire.ViewModels;

namespace Questionnaireonnaire.Controllers
{
    public class ApiController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly QuestionnaireContext _db;

        public ApiController(ILogger<HomeController> logger, QuestionnaireContext db)
        {
            _logger = logger;
            _db = db;
        }

        [Route("[Controller]/questions")]
        [HttpPost]
        public IActionResult Post([FromBody] Question question)
        {
            _db.Questions.Add(question);
            _db.SaveChanges();

            return Created("", null);
        }


        [Route("[Controller]/questions/disable/{id}")]
        [HttpPatch]
        public IActionResult Disable(int id)
        {
            Question? q = _db.Questions.Find(id);
            if(q == null) return NotFound();

            q.disabled = true;

            _db.Questions.Update(q);
            _db.SaveChanges();

            return Ok();
        }
    }
}
