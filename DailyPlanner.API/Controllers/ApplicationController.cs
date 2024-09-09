using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class ApplicationController : ControllerBase {}