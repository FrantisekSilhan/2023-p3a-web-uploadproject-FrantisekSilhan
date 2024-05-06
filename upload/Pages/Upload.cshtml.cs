using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using upload.Data;
using upload.Models;

namespace upload.Pages {
    public class UploadModel : PageModel {
        private readonly ApplicationDbContext _context;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public ICollection<IFormFile> Upload { get; set; }

        public UploadModel(ApplicationDbContext context) {
            _context = context;
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
