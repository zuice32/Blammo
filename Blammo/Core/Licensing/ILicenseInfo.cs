namespace Agent.Core.Licensing
{
    public interface ILicenseInfo
    {
        string CompanyName { get; }
    }

    public class LicenseInfo : ILicenseInfo
    {
        public string CompanyName { get; set; }
    }
}