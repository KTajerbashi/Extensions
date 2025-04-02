using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersManagement.WebApi.Controllers.SessionsControllers;

// Add to controllers
[Authorize(Policy = "RequireAdmin")]
public class AdminController : Controller { /* ... */ }
