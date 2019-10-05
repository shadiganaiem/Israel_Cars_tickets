using System;
using System.Threading.Tasks;

namespace ISRLCARS.Services
{
    public interface IQrScanningService
    {
        Task<string> scanAsync();
    }
}
