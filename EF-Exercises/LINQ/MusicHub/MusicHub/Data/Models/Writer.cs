using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models;

public class Writer
{
    public Writer()
    {
        this.Songs = new List<Song>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(GlobalConstants.WriterNameMaxLength)]
    public string Name { get; set; } = null!;

    public string? Pseudonym { get; set; }

    public virtual ICollection<Song> Songs { get; set; }
}