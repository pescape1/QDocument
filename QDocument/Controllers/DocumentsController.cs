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
using QDocument.Models;

namespace QDocument.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<User> userManager;
        private IRepositoryWrapper _repoWrapper;

        public DocumentsController(UserManager<User> userMgr, IRepositoryWrapper repoWrapper) //ApplicationDbContext context, 
        {
            //_context = context;
            userManager = userMgr;
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

            var document = await _repoWrapper.Document.GetDocumentByIdAsync((int) id);

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
        public async Task<IActionResult> Create([Bind("ID,Title,DocumentType,CreationDate")] Document document)
        {
            if (ModelState.IsValid)
            {
                await _repoWrapper.Document.CreateDocumentAsync(document);

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

            var document = await _repoWrapper.Document.GetDocumentByIdAsync((int) id);

            if (document == null)
            {
                return NotFound();
            }
            await PopulateJobsDropDownList();
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,DocumentType,CreationDate")] Document document)
        {
            if (id != document.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repoWrapper.Document.UpdateDocumentAsync(document);
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
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _repoWrapper.Document.GetDocumentByIdAsync((int) id);

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
            var document = await _repoWrapper.Document.GetDocumentByIdAsync(id);
            await _repoWrapper.Document.DeleteDocumentAsync(document);

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateJobsDropDownList()
        {
            IEnumerable<Job> jobsQuery = await _repoWrapper.Job.GetAllJobsAsync();
            ViewBag.JobList = new SelectList(jobsQuery, "ID", "Title");
        }
    }
}
