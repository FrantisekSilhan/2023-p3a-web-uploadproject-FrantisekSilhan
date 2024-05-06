using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace upload.Models {
    public class StoredFile {
        public Guid StoredFileId { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;

        public Guid AuthorId { get; set; } = default!;
        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; } = default!;

        [Required]
        public string OriginalName { get; set; } = default!;

        [Required]
        public string MimeType { get; set; } = default!;

        public ICollection<Thumbnail>? Thumbnails { get; set; } = default!;
        [Required]
        public byte[] Blob { get; set; } = default!;
    }
}
