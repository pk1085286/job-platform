// Create a controller for user dashboard
public class UserDashboardController : Controller {
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserDashboardController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index() {
        var user = await _userManager.GetUserAsync(User);
        var jobApplications = _dbContext.JobApplications
            .Where(ja => ja.Email == user.Email)
            .Select(ja => new JobApplicationViewModel {
                Id = ja.Id,
                JobTitle = ja.JobPosting.Title,
                CreatedAt = ja.CreatedAt,
                Status = ja.Status
            })
            .ToList();

        var model = new UserDashboardViewModel {
            JobApplications = jobApplications
        };

        return View(model);
    }
}

// Create a view model for job application
public class JobApplicationViewModel {
    public int Id { get; set; }
    public string JobTitle { get; set; }
    public DateTime CreatedAt { get; set; }
    public JobApplicationStatus Status { get; set; }
}

// Create a view model for user dashboard
public class UserDashboardViewModel {
   
