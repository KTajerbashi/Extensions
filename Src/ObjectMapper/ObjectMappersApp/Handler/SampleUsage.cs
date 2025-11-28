namespace ObjectMappersApp.Handler;

public class SampleUsage
{
    private readonly ServiceHandler _handler;

    public SampleUsage(ServiceHandler handler)
    {
        _handler = handler;
    }

    public void Run()
    {
        // Example UserEntity
        var userEntity = new UserEntity();

        // 1) Convert Entity → DTO
        UserDTO dto = _handler.ConvertToDto(userEntity);

        // 2) Convert DTO → View
        UserView view = _handler.ConvertToView(dto);

        // Example list of roles
        var roles = new List<RoleEntity>
        {
            new RoleEntity(),
            new RoleEntity()
        };

        // 3) Convert list of Entity → list of View
        List<RoleView> roleViews = _handler.ConvertRoleToViews(roles);
    }
}
