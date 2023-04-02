// Create a job posting model
public class JobPosting {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal SalaryRange { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Create a job posting form
public class JobPostingForm {
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal SalaryRange { get; set; }
    [Required]
    public string Location { get; set; }
}

// Create a controller for job postings
public class JobPostingsController : Controller {
    private readonly ApplicationDbContext _dbContext;

    public JobPostingsController(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(JobPostingForm form) {
        if (!ModelState.IsValid) {
            return View(form);
        }

        var jobPosting = new JobPosting {
            Title = form.Title,
            Description = form.Description,
            SalaryRange = form.SalaryRange,
            Location = form.Location,
            CreatedAt = DateTime.Now
        };

        _dbContext.JobPostings.Add(jobPosting);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
