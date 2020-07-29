using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace BookStoreApplication.Models
{
    public class BookModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Book Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Author Name")]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Book Description")]
        [StringLength(500, MinimumLength =5)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Category (Example: Programming, Framework, Story)")]
        public string Category { get; set; }
        [Required]
        [Display(Name = "Total Page")]
        public int TotalPage { get; set; }
        [Required]
        [Display(Name = "Language of This Book")]
        public int LanguageId { get; set; }
        public string Language { get; set; }
        [Required]
        [Display(Name = "Upload a Cover Photo")]
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }
        [Required]
        [Display(Name = "Upload Some Photo of Book")]
        public IFormFileCollection GalleryFiles { get; set; }
        public List<GalleryModel> Gallery { get; set; }

        [Required]
        [Display(Name = "Upload your PDF Book")]
        public IFormFile BookPdf { get; set; }
        public string  BookPdfURL{ get; set; }

    }
}
