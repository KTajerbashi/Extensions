using SoftwarepartDetector.DataModel;

namespace SoftwarepartDetector.Publishers;

public interface ISoftwarePartPublisher
{
    Task PublishAsync(SoftwarePart softwarePart);
}