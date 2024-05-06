using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace upload.Models {
    public class Thumbnail {
        public Guid StoredFileId { get; set; } = default!;
        [ForeignKey(nameof(StoredFileId))]
        public StoredFile StoredFile { get; set; } = default!;
        [Key]
        public ThumbnailType Type { get; set; } = default!;
        public byte[] Blob { get; set; } = default!;
    }

    public enum ThumbnailType {
        Square,
        SameAspectRatio
    }
}
