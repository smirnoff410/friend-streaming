namespace Translation;

public class UrlSettings
{
    public static readonly string SectionName = "UrlSettings";

    public string RelayGrpcUrl { get; set; }
    public int UdpPort { get; set; }
}