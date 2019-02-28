using System.Threading.Tasks;

namespace Agent.Core.Licensing
{
    public class NullLicenseService : IApplicationLicenseService
    {
        public bool IsLicensed
        {
            get { return true; }
        }

#pragma warning disable 1998
        public async Task<bool> ApplyLicense(string licenseCode)
#pragma warning restore 1998
        {
            return true;
        }

        public ILicenseInfo GetLicenseInfo()
        {
            return null;
        }
    }
}