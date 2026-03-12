using DinkToPdf.Contracts;
using DinkToPdf;

namespace Identity_Login.Services
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            // Load native DLL from wwwroot
            var context = new CustomAssemblyLoadContext();
            var dllPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "libwkhtmltox.dll");
            context.LoadUnmanagedLibrary(dllPath);

            _converter = converter;
        }

        public byte[] GeneratePdf(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                 Margins = new MarginSettings { Top = 0, Bottom = 0, Left = 0, Right = 0 } // No margins
   
            },
                //    Objects = {
                //    new ObjectSettings() {
                //        HtmlContent = htmlContent
                //    }
                //}
                //};
                Objects = {
        new ObjectSettings() {
            PagesCount = true,
            HtmlContent = htmlContent,
            WebSettings = {
                DefaultEncoding = "utf-8",
                LoadImages = true,
                PrintMediaType = true
            },
            //HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
            //FooterSettings = { FontSize = 9, Line = false, Center = "" }
        }
    }
            };

            return _converter.Convert(doc);
        }
    }

    // Required helper class
    public class CustomAssemblyLoadContext : System.Runtime.Loader.AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllPath)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllPath);
        }
    }
}
