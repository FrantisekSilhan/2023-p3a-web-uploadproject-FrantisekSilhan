using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using upload.Data;
using upload.Models;

namespace upload.Pages {
    public class IndexModel : PageModel {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public List<StoredFile> Files { get; set; } = new List<StoredFile>();

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context) {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGet() {
            Files = await _context.StoredFiles.ToListAsync();
            return Page();
        }

        public IActionResult OnGetDownload(string filename) {
            var file = _context.StoredFiles.FirstOrDefault(f => f.OriginalName == filename);
            if (file != null) {
                return File(file.Blob, file.MimeType, file.OriginalName);
            } else {
                ErrorMessage = "There is no such file.";
                return RedirectToPage();
            }
        }
    }
}
