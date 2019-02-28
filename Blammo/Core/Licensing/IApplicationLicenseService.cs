using System.Threading.Tasks;

namespace Agent.Core.Licensing
{
    public interface IApplicationLicenseService
    {
        bool IsLicensed { get; }
        
        Task<bool> ApplyLicense(string licenseCode);

        ILicenseInfo GetLicenseInfo();
    }
} 