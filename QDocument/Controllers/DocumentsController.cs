using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QDocument.Data;
using QDocument.Data.Contracts;
using QDocument.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace QDocument.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private IRepositoryWrapper _repoWrapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DocumentsController(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IHostingEnvironment hostingEnvironment) //ApplicationDbContext context,  
        {
            //_context = context;
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _hostingEnvironment = hostingEnvironment;
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

        public IActionResult Import()
        {
            return View(new List<Document>());
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var filePath = Path.Combine(
                        _hostingEnvironment.WebRootPath, "uploads",
                        file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //List<Document> documents = new List<Document>();

            //Read the contents of CSV file.
            //string csvData = System.IO.File.ReadAllText(filePath, Encoding.GetEncoding("iso-8859-1"));
            string csvData = System.IO.File.ReadAllText(filePath, Encoding.Default);
            //return Content(csvData);
            //Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length)

            User user = await _userManager.GetUserAsync(User);
            Document document;

            if(csvData.Contains('\r'))
            {
                csvData = csvData.Replace("\r", "");
            }

            string[] rows = csvData.Split('\n');  // \r 0D  \n 0A
            if(rows.Length>0)
            {
                if(rows[0].Split(',').Length != 3 ||
                   rows[0].Split(',')[0].ToLower().Trim() != "title" ||
                   rows[0].Split(',')[1].ToLower().Trim() != "documenttype" ||
                   rows[0].Split(',')[2].ToLower().Trim() != "approvalscheme")
                {
                    return Content("Bad columns titles");
                }
                if(rows.Length == 1)
                {
                    return Content("No rows to import");
                }
            }

            IEnumerable<Job> jobList = await _repoWrapper.Job.GetAllJobsAsync();

            //Execute a loop over the rows.
            foreach (string row in rows.Skip(1))
            {
                if (!string.IsNullOrEmpty(row))
                {
                    //DocumentType documentType = Enum.TryParse(row.Split(',')[2], out documentType) ? documentType : DocumentType.Other;
                    string[] rowCols = row.Split(',');
                    if (rowCols.Length <= 3)
                    {
                        string sTitle = rowCols[0].Trim();
                        string sDocType = rowCols[1].Trim() ?? "";
                        document = new Document
                        {
                            //ID = Convert.ToInt32(row.Split(',')[0]),
                            Title = sTitle,
                            DocumentType = Enum.TryParse(sDocType, true, out DocumentType documentType) ? documentType : DocumentType.Other,
                            CreationUser = user.Id
                        };
                        List<int> approvalJobs = new List<int>();
                        if (rowCols.Length == 3)
                        {
                            string[] approvalScheme = rowCols[2].Split('|');

                            foreach (string approvalJob in approvalScheme)
                            {
                                Job job = jobList.FirstOrDefault(j => j.Title == approvalJob.Trim());
                                if (job != null)
                                {
                                    approvalJobs.Add(job.ID);
                                }
                            }
                        }
                        try
                        {
                            await _repoWrapper.Document.CreateDocumentAsync(document, approvalJobs.ToArray());
                        }
                        catch(Exception)
                        {

                        }
                    }
                }
            }
            return View(await _repoWrapper.Document.GetAllDocumentsAsync());
            /* 

                        string filePath = string.Empty;
                        if (postedFile != null)
                        {
                           string path = Server.MapPath("~/Uploads/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            filePath = path + Path.GetFileName(postedFile.FileName);
                            string extension = Path.GetExtension(postedFile.FileName);
                            postedFile.SaveAs(filePath);


                        }

                        return View(customers);*/
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
