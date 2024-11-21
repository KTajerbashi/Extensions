namespace Extensions.ChangeDataLog.Hamster.Options;

public sealed record ChangeDataLogHamsterOptions
{
    public List<string> PropertyForReject { get; set; } =
        [
            "CreatedByUserId",
            "CreatedDateTime",
            "ModifiedByUserId",
            "ModifiedDateTime"
        ];

    public string BusinessIdFieldName { get; set; } = "BusinessId";

}
