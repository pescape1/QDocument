using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QDocument.Data;
using QDocument.Data.Contracts;
using QDocument.Data.Models;

namespace QDocument.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private IRepositoryWrapper _repoWrapper;

        public DocumentsController(IRepositoryWrapper repoWrapper, UserManager<User> userManager) //ApplicationDbContext context,  
        {
            //_context = context;
            _userManager = userManager;
            _repoWrapper = repoWrapper;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            return View(await _repoWrapper.Document.GetAllDocumentsAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _repoWrapper.Document.GetDocumentAsync(d => d.ID == id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public async Task<IActionResult> Create()
        {
            await PopulateJobsDropDownList();
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int[] jobList, [Bind("Title,DocumentType")] Document document)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.GetUserAsync(User);
                document.CreationUser = user.Id;
                await _repoWrapper.Document.CreateDocumentAsync(document, jobList);

                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _repoWrapper.Document.GetDocumentWithApprovalAsync((int) id);

            if (document == null)
            {
                return NotFound();
            }
            await PopulateJobsDropDownList(document.ApprovedBy.Select(a => a.JobID).ToArray());
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int[] jobList, [Bind("ID,Title,DocumentType,CreationDate")] Document document)
        {
            if (id != document.ID)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _repoWrapper.Document.UpdateDocumentAsync(document, jobList);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repoWrapper.Document.DocumentExists(document.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateJobsDropDownList(jobList);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _repoWrapper.Document.GetDocumentAsync(d => d.ID == id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _repoWrapper.Document.GetDocumentAsync(d => d.ID == id);
            await _repoWrapper.Document.DeleteDocumentAsync(document);

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateJobsDropDownList(params int[] selectedValues)
        {
            var jobsQuery = await _repoWrapper.Job.GetAllJobsAsync();
            ViewBag.JobList = new MultiSelectList(jobsQuery, "ID", "Title", selectedValues);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> DoesTitleExistAsync(string Title, int ID)
        {
            var document = await _repoWrapper.Document.GetDocumentAsync(d => d.Title == Title && d.ID != ID);

            return Json(document.Title == null);
        }
    }
}
