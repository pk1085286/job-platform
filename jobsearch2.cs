// Create a controller for job search
public class JobSearchController : Controller {
    private readonly ApplicationDbContext _dbContext;

    public JobSearchController(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index(string query) {
        var jobListings = _dbContext.JobPostings
            .Where(jp => jp.Title.Contains(query) || jp.Description.Contains(query))
            .Select(jp => new JobListingViewModel {
                Id = jp.Id,
                Title = jp.Title,
                Description = jp.Description,
                SalaryRange = jp.SalaryRange,
                Location = jp.Location,
                ApplicationCount = jp.Applications.Count()
            })
            .ToList();

        var model = new JobSearchViewModel {
            Query = query,
            JobListings = jobListings
        };

        return View(model);
    }
}

// Create a view model for job search
public class JobSearchViewModel {
    public string Query { get; set; }
    public List<JobListingViewModel> JobListings { get; set; }
}

// Create a view for job search
@model JobSearchViewModel

<form asp-controller="JobSearch" asp-action="Index" method="get">
    <div class="input-group">
        <input type="text" class="form-control" name="query" placeholder="Search jobs" value="@Model.Query" />
        <div class="input-group-append">
            <button type="submit" class="btn btn-primary">Search</button>
        </div>
    </div>
</form>

@if (Model.JobListings.Count == 0) {
    <p>No jobs found.</p>
} else {
    <ul>
        @foreach (var jobListing in Model.JobListings) {
            <li>
                <h3><a asp-controller="JobPosting" asp-action="Details" asp-route-id="@jobListing.Id">@jobListing.Title</a></h3>
                <p>@jobListing.Description</p>
                <p>Salary Range: @jobListing.SalaryRange</p>
                <p>Location: @jobListing.Location</p>
                <p>Applications: @jobListing.ApplicationCount</p>
            </li>
        }
    </ul>
}
