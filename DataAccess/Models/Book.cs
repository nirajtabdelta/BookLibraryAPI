using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    /// <summary>
    /// Books model data.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// it is unique value for book record.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// it is metion book name.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        ///  it is mention to writer/author name.
        /// </summary>
        [Required]
        public string Author { get; set; }

        /// <summary>
        /// it is mention to year of book publish.
        /// </summary>
        [Range(1000, 2100)]
        public int PublicationYear { get; set; }

        /// <summary>
        /// it is mantion genre values.
        /// </summary>
        [Required]
        public string Genre { get; set; }

        /// <summary>
        /// Indicates whether the book record is soft-deleted.
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;
    }
}
