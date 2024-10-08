using EProjectFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EProjectFinal.Controllers
{
    public class ManagerController : Controller
    {
        public ApplicationDbContext _Context { get; }
        public ManagerController(ApplicationDbContext Db)
        {
            this._Context = Db;
        }

        private bool IsManagerLoggedIn()
        {
            return HttpContext.Session.GetString("Manager") == "True";
        }
        public IActionResult Dashboard()
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            return View();
        }

        public IActionResult ViewTests()
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            return View(_Context.Tests.ToList());
        }

        public IActionResult CreateTest()
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTest(Test test)
        {
            if (ModelState.IsValid)
            {
                _Context.Tests.Add(test);

                await _Context.SaveChangesAsync();
                TempData["Message"]="Test Added Successfully";
                return RedirectToAction("ViewTests");
            }

            return View(test);
        }

        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = await _Context.Tests.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            _Context.Tests.Remove(test);
            await _Context.SaveChangesAsync();
                TempData["Message"]="Test Deleted Successfully";

            return RedirectToAction("ViewTests");
        }

        public async Task<IActionResult> EditTest(int id)
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            var test = await _Context.Tests.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }
        [HttpPost]
        public async Task<IActionResult> EditTest(Test test)
        {
            if (ModelState.IsValid)
            {
                _Context.Tests.Update(test);
                await _Context.SaveChangesAsync();
                TempData["Message"] = "Test Edit Successfully";
                return RedirectToAction("ViewTests");
            }

            return View(test);
        }

        public IActionResult ViewQuestions()
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            return View(_Context.Questions.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> AddQuestion()
        {
            ViewBag.Tests = await _Context.Tests
                .Select(t => new SelectListItem
                {
                    Value = t.TestId.ToString(),  
                    Text = t.TestName
                })
                .ToListAsync();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddQuestion(Question question)
        {

            if (!ModelState.IsValid)
            {
                // Log or inspect errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                   Console.WriteLine(error.ErrorMessage);
                }

                // Reload tests list for dropdown
                ViewBag.Tests = await _Context.Tests
                    .Select(t => new SelectListItem
                    {
                        Value = t.TestId.ToString(),
                        Text = t.TestName
                    })
                    .ToListAsync();

                return View(question); // Return view with errors
            }

            _Context.Questions.Add(question);
            await _Context.SaveChangesAsync();
            TempData["Message"] = "Question Added Successfully";

            return RedirectToAction("ViewQuestions", new { testId = question.TestId });
        }
        [HttpGet]
        public async Task<IActionResult> EditQuestion(int id)
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            // Fetch the question by ID
            var question = await _Context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            // Fetch list of tests for dropdown
            ViewBag.Tests = await _Context.Tests
                .Select(t => new SelectListItem
                {
                    Value = t.TestId.ToString(),
                    Text = t.TestName
                })
                .ToListAsync();

            return View(question);
        }
        [HttpPost]
        public async Task<IActionResult> EditQuestion(Question question)
        {
            if (ModelState.IsValid)
            {
                _Context.Questions.Update(question);
                await _Context.SaveChangesAsync();
                TempData["Message"] = "Question Edit Successfully";
                return RedirectToAction("ViewQuestions", new { testId = question.TestId });
            }

            ViewBag.Tests = await _Context.Tests
                .Select(t => new SelectListItem
                {
                    Value = t.TestId.ToString(),
                    Text = t.TestName
                })
                .ToListAsync();

            return View(question);
        }

        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _Context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            _Context.Questions.Remove(question);
            await _Context.SaveChangesAsync();
            TempData["Message"] = "Question Deleted Successfully";

            return RedirectToAction("ViewQuestions");
        }

        public async Task<IActionResult> ViewCandidates()
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            // Fetch all candidates from the database
            var candidates = await _Context.Candidates.ToListAsync();
            return View(candidates);
        }

        [HttpGet]
        public IActionResult AddCandidate()
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCandidate(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                candidate.CreatedAt = DateTime.Now;
                _Context.Candidates.Add(candidate);
                await _Context.SaveChangesAsync();

                TempData["Message"] = "Candidate Added Successfully";
                return RedirectToAction("ViewCandidates");
            }

            return View(candidate);
        }
        [HttpGet]
        public async Task<IActionResult> EditCandidate(int id)
        {
            if (!IsManagerLoggedIn())
            {
                return RedirectToAction("ManagerLogin", "Account");
            }
            // Fetch the candidate by ID
            var candidate = await _Context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }
        [HttpPost]
        public async Task<IActionResult> EditCandidate(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                // Update the candidate in the database
                _Context.Candidates.Update(candidate);
                await _Context.SaveChangesAsync();

                // Redirect back to the list of candidates after successful edit
                TempData["Message"] = "Candidate Edit Successfully";
                return RedirectToAction("ViewCandidates");
            }

            // If validation fails, return the form with error messages
            return View(candidate);
        }
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var candidate = await _Context.Candidates.FindAsync(id);

            if (candidate == null)
            {
                return NotFound();
            }

            _Context.Candidates.Remove(candidate);
            await _Context.SaveChangesAsync();
            TempData["Message"] = "Candidate Deleted Successfully";

            return RedirectToAction("ViewCandidates");
        }
    }
}
