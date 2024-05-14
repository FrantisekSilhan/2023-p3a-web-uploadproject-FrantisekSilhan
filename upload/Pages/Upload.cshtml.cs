using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System.Security.Claims;
using upload.Data;
using upload.Models;

namespace upload.Pages {
    public class UploadModel : PageModel {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly int _squareSize;
        private readonly int _sameAspectRatioHeigth;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public ICollection<IFormFile> Upload { get; set; }

        public UploadModel(ApplicationDbContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
            if (Int32.TryParse(_configuration["Thumbnails:SquareSize"], out _squareSize) == false) _squareSize = 64; // získej data z konfigurave nebo použij 64
            if (Int32.TryParse(_configuration["Thumbnails:SameAspectRatioHeigth"], out _sameAspectRatioHeigth) == false) _sameAspectRatioHeigth = 128;
        }

        public void OnGet() {
        }

        public async Task<IActionResult> OnPostAsync() {
            int successfulProcessing = 0;
            int failedProcessing = 0;
            foreach (var uploadedFile in Upload) {
                try {
                    StoredFile file = new StoredFile {
                        OriginalName = uploadedFile.FileName,
                        MimeType = uploadedFile.ContentType,
                        Blob = new byte[uploadedFile.Length],
                        AuthorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                        CreatedAt = DateTime.Now
                    };

                    if (uploadedFile.ContentType.StartsWith("image")) // je soubor obrázek?
                    {
                        file.Thumbnails = new List<Thumbnail>();
                        MemoryStream ims = new MemoryStream(); // proud pro příchozí obrázek
                        MemoryStream oms1 = new MemoryStream(); // proud pro čtvercový náhled
                        MemoryStream oms2 = new MemoryStream(); // proud pro obdélníkový náhled
                        uploadedFile.CopyTo(ims); // vlož obsah do vstupního proudu
                        IImageFormat format; // zde si uložíme formát obrázku (JPEG, GIF, ...), budeme ho potřebovat při ukládání
                        using (Image image = Image.Load(ims.ToArray())) // vytvoříme čtvercový náhled
                        {
                            format = image.Configuration.ImageFormats.First();
                            int largestSize = Math.Max(image.Height, image.Width); // jaká je orientace obrázku?
                            if (image.Width > image.Height) // podle orientace změníme velikost obrázku
                            {
                                image.Mutate(x => x.Resize(0, _squareSize));
                            } else {
                                image.Mutate(x => x.Resize(_squareSize, 0));
                            }
                            image.Mutate(x => x.Crop(new Rectangle((image.Width - _squareSize) / 2, (image.Height - _squareSize) / 2, _squareSize, _squareSize)));
                            // obrázek ořízneme na čtverec
                            image.Save(oms1, format); // vložíme ho do výstupního proudu
                            file.Thumbnails.Add(new Thumbnail { Type = ThumbnailType.Square, Blob = oms1.ToArray() }); // a uložíme do databáze jako pole bytů
                        }
                        using (Image image = Image.Load(ims.ToArray())) // obdélníkový náhled začíná zde
                        {
                            format = image.Configuration.ImageFormats.First();
                            image.Mutate(x => x.Resize(0, _sameAspectRatioHeigth)); // stačí jen změnit jeho velikost
                            image.Save(oms2, format); // a přes proud ho uložit do databáze
                            file.Thumbnails.Add(new Thumbnail { Type = ThumbnailType.SameAspectRatio, Blob = oms2.ToArray() });
                        }
                    }

                    using (var memoryStream = new MemoryStream()) {
                        await uploadedFile.CopyToAsync(memoryStream);
                        file.Blob = memoryStream.ToArray();
                    }

                    _context.StoredFiles.Add(file);

                    await _context.SaveChangesAsync();

                    successfulProcessing++;
                } catch {
                    failedProcessing++;
                }
                if (failedProcessing == 0) {
                    SuccessMessage = "All files has been uploaded successfuly.";
                } else {
                    ErrorMessage = "There were <b>" + failedProcessing + "</b> errors during uploading and processing of files.";
                }
            }
            return RedirectToPage("/Index");
        }
    }
}
