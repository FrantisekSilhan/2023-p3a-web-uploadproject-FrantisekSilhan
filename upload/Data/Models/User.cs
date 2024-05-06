using Microsoft.AspNetCore.Identity;

namespace upload.Models {
    public class User : IdentityUser<Guid> {
        public string? FullName { get; set; } = default!;
        public ICollection<StoredFile>? StoredFiles { get; set; } = default!;
    }
}