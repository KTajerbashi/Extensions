namespace Extensions.Logger.Abstractions;

//_logger.LogError(new EventId(AppLogEventId.DomainError), ex, "Domain error occurred");
public static class AppLogEventId
{
    // ==========================================================
    // DOMAIN (10000 - 19999)
    // ==========================================================
    public const int DomainError = 10001;
    public const int DomainStateException = 10002;
    public const int DomainValidationException = 10003;
    public const int DomainLogicException = 10004;
    public const int DomainObjectException = 10005;

    // Domain Informational
    public const int DomainEventRaised = 11001;
    public const int DomainEntityCreated = 11002;
    public const int DomainEntityUpdated = 11003;

    // ==========================================================
    // APPLICATION (20000 - 29999)
    // ==========================================================
    public const int ApplicationError = 20001;
    public const int ApplicationValidationError = 20002;
    public const int ApplicationHandlerError = 20003;

    // Application Informational
    public const int ApplicationCommandHandled = 21001;
    public const int ApplicationQueryHandled = 21002;
    public const int ApplicationWorkflowStarted = 21003;

    // ==========================================================
    // INFRASTRUCTURE (30000 - 39999)
    // ==========================================================
    public const int InfrastructureError = 30001;
    public const int DatabaseError = 30002;
    public const int ExternalServiceError = 30003;
    public const int CacheError = 30004;
    public const int FileSystemError = 30005;

    // Infrastructure Informational
    public const int DatabaseOperation = 31001;
    public const int ExternalServiceCalled = 31002;

    // ==========================================================
    // WEB API (40000 - 49999)
    // ==========================================================
    public const int WebApiError = 40001;
    public const int WebApiValidationError = 40002;
    public const int WebApiUnauthorized = 40003;
    public const int WebApiForbidden = 40004;

    // WebApi Informational
    public const int WebAppInfo = 41001;
    public const int WebApiRequestReceived = 41002;
    public const int WebApiResponseSent = 41003;

    // ==========================================================
    // WEB APPLICATION (BLAZOR, RAZOR) (50000 - 59999)
    // ==========================================================
    public const int WebAppError = 50001;
    public const int WebAppNavigationError = 50002;
    public const int WebAppUiError = 50003;
    public const int WebAppWarning = 50004;

    // WebApp Informational
    public const int WebAppPageLoaded = 51001;
    public const int WebAppUserAction = 51002;
    public const int WebAppComponentError = 51003;
}
