using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersManagement.WebApi.Controllers.TokensControllers;

// Add to controllers
[Authorize(Policy = "RequireAdmin")]
public class TokenAdminController : Controller { /* ... */ }
