﻿using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApplicationController : ControllerBase {}