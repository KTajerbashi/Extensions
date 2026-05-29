using LoggerWebApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace LoggerWebApi.Controllers
{
    [ApiController]
    [Route("api/exceptions")]
    public class ExceptionController : ControllerBase
    {
        private readonly ILogger<ExceptionController> _logger;

        public ExceptionController(ILogger<ExceptionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Ok-200")]
        public IActionResult Index()
        {
            return Ok("Ok-200");
        }

        /// <summary>
        /// Throws a domain exception.
        /// </summary>
        [HttpGet("domain")]
        public IActionResult ThrowDomainException()
        {
            _logger.LogWarning("Throwing DomainException");

            using (LogContext.PushProperty("UserId", 100))
            {
                _logger.LogInformation("User Updated");
            }
            throw new DomainException(
                "Business rule violated.");
        }

        /// <summary>
        /// Throws an application exception.
        /// </summary>
        [HttpGet("application")]
        public IActionResult ThrowApplicationException()
        {
            _logger.LogWarning("Throwing AppException");

            throw new AppException(
                "Application processing failed.");
        }

        /// <summary>
        /// Throws a database exception.
        /// </summary>
        [HttpGet("database")]
        public IActionResult ThrowDatabaseException()
        {
            _logger.LogError("Throwing DataBaseException");

            throw new DataBaseException(
                "Database operation failed.");
        }

        /// <summary>
        /// Throws an external API exception.
        /// </summary>
        [HttpGet("api")]
        public IActionResult ThrowApiException()
        {
            _logger.LogError("Throwing ApiException");

            throw new ApiException(
                "External API call failed.");
        }

        /// <summary>
        /// Generic test exception.
        /// </summary>
        [HttpGet("unknown")]
        public IActionResult ThrowUnknownException()
        {
            _logger.LogCritical("Throwing unknown exception");

            throw new Exception(
                "Unexpected system error occurred.");
        }
    }
}