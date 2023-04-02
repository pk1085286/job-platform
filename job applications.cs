// Create a job application model
public class JobApplication {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Resume { get; set; }
    public string CoverLetter { get; set; }
    public DateTime CreatedAt { get; set; }

    // Foreign key for job posting
    public int JobPostingId { get; set; }
    public JobPosting JobPosting { get; set; }
}

// Create a job application form
public class JobApplicationForm {
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public IFormFile Resume { get; set; }
    public IFormFile CoverLetter { get; set; }
}

// Create a controller for job applications
public class JobApplicationsController : Controller {
    private readonly ApplicationDbContext _dbContext;

    public JobApplicationsController(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Create(int id) {
        var jobPosting = _dbContext.JobPostings.Find(id);
        if (jobPosting == null) {
            return NotFound();
        }

        var form = new JobApplicationForm {
            Name = User.Identity.Name,
            Email = User.FindFirstValue(ClaimTypes.Email)
        };

        return View(form);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int id, JobApplicationForm form) {
        var jobPosting = _dbContext.JobPostings.Find(id);
        if (jobPosting == null) {
            return NotFound();
        }

        if (!ModelState.IsValid) {
            return View(form);
        }

        var resume = await SaveFileAsync(form.Resume);
        var coverLetter = await SaveFileAsync(form.CoverLetter);

        var jobApplication = new JobApplication {
            Name = form.Name,
            Email = form.Email,
            Resume = resume,
            CoverLetter = coverLetter,
            CreatedAt = DateTime.Now,
            JobPostingId = id
        };

        _dbContext.JobApplications.Add(jobApplication);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveFileAsync(IFormFile file) {
        if (file == null) {
            return null;
        }

        var fileName = Path.GetFileName(file.FileName);
        var filePath = Path.Combine("wwwroot", "uploads", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create)) {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }
}
