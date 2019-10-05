using System;
using System.Threading.Tasks;
using ISRLCARS.Services;
using Xamarin.Forms;
using ZXing.Mobile;


[assembly: Dependency(typeof(ISRLCARS.iOS.Services.QrScanningService))]
namespace ISRLCARS.iOS.Services
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> scanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionCustom = new MobileBarcodeScanningOptions();
            var scanner = new MobileBarcodeScanner()
            {
                TopText = "סרוק ברקוד",
                BottomText = "bottom"
            };

            var scanResult = await scanner.Scan(optionCustom);

            return scanResult.Text;

        }
    }
}
