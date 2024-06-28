using IdentityModel.Client;

namespace SoftwarepartDetector.Authentications;

public interface ISoftwarePartAuthentication
{
    Task<TokenResponse> LoginAsync();
}