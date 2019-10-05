using System;
using System.Threading.Tasks;
using ISRLCARS.Services;
using ZXing.Mobile;

namespace ISRLCARS.Droid.Services
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> scanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionCustom = new MobileBarcodeScanningOptions();
            var scanner = new MobileBarcodeScanner()
            {
                TopText = "TopText",
                BottomText = "bottom"
            };

            var scanResult = await scanner.Scan(optionCustom);

            return scanResult.Text;

        }
    }
}
