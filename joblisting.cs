// Create a view model for job listing
public class JobListingViewModel {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal SalaryRange { get; set; }
    public string Location { get; set; }
    public int ApplicationCount { get; set; }
}

// Create a controller for job listing
public class JobListingController : Controller {
    private readonly ApplicationDbContext _dbContext;

    public JobListingController(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index() {
        var jobListings = _dbContext.JobPostings
            .Select(jp => new JobListingViewModel {
                Id = jp.Id,
                Title = jp.Title,
                Description = jp.Description,
                SalaryRange = jp.SalaryRange,
                Location = jp.Location,
                Application
