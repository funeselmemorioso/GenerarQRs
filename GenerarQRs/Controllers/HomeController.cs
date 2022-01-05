using GenerarQRs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using ZXing;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;

namespace GenerarQRs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private Bitmap generar()
        {
            string level = "q";
            //string texto = "http://www.europ-assistance.com.ar"; // url comun browser
            string texto  = "https://wa.me/12025550172"; // wapp
            Bitmap foto = null;

            QRCodeGenerator.ECCLevel eccLevel 
                = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData 
                    = qrGenerator.CreateQrCode(texto, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {
                        foto = qrCode.GetGraphic(50);
                    }
                }
            }

            return foto;
        }

        public ActionResult GenerateImage()
        {
            FileContentResult result;
            Bitmap b = generar();
            using (var memStream = new MemoryStream())
            {
                b.Save(memStream, ImageFormat.Jpeg);
                result = this.File(memStream.GetBuffer(), "image/jpeg");
            }

            return result;
        }
        public IActionResult Index()
        {
            return View();
        }       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
