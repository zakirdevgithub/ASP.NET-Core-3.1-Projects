using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStoreApplication.Models;
using BookStoreApplication.Station;

namespace BookStoreApplication.Controllers
{
    public class BookController : Controller
    {
        private readonly BookStation _bookStation = null;
        private readonly LanguageStation _languageStation = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(BookStation bookStation,
            LanguageStation languageStation,
            IWebHostEnvironment webHostEnvironment)
        {
            _bookStation = bookStation;
            _languageStation = languageStation;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<ViewResult> AddNewBook(bool IsSuccess = false, int bookId = 0)
        {
            var languages = new SelectList(await _languageStation.GetLanguage(), "Id", "Name");
            ViewBag.Languages = languages;
            ViewBag.IsSuccess = IsSuccess;
            ViewBag.BookId = bookId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBook(BookModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CoverPhoto != null)
                {
                    string folder = "books/cover/";
                    model.CoverImageUrl= await UploadImages(folder, model.CoverPhoto);
                    
                }

                if (model.GalleryFiles != null)
                {
                    string folder = "books/Gallery/";
                    model.Gallery = new List<GalleryModel>();
                    foreach (var file in model.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            Name= file.FileName,
                            Url= await UploadImages(folder, file)
                    };
                        model.Gallery.Add(gallery);
                        
                    }

                    
                }

                if (model.BookPdf != null)
                {
                    string folder = "books/pdf/";
                    model.BookPdfURL = await UploadImages(folder, model.BookPdf);
                }


                var id = await _bookStation.SendBookToDatabase(model);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewBook), new { IsSuccess = true, bookId = id });
                }
            }

            var languages = new SelectList(await _languageStation.GetLanguage(), "Id", "Name");
            ViewBag.Languages = languages;
          
            return View();
        }

        private async Task<string> UploadImages(string folderPath, IFormFile file)
        {
           
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            
            return "/" + folderPath;
        }

        public async Task<ViewResult> ShowBooks()
        {
            var data = await _bookStation.GetAllBooks();
            return View(data);
        }

        public async Task<ViewResult> ShowBookById(int id)
        {
            var data = await _bookStation.GetBookById(id);
            return View(data);
        }
    }
}
