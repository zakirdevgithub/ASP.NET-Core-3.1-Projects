using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApplication.Data;
using BookStoreApplication.Models;

namespace BookStoreApplication.Station
{
    public class BookStation
    {
        private readonly DataContext _context = null;
        public BookStation(DataContext context)
        {
            _context = context;
        }

        public async Task<int> SendBookToDatabase(BookModel model)
        {
            var books = new Book()
            {
                Id = model.Id,
                Title=model.Title,
                Author=model.Author,
                Description=model.Description,
                Category=model.Category,
                TotalPage=model.TotalPage,
                LanguageId=model.LanguageId,
                CoverImageUrl=model.CoverImageUrl,
                BookPdfURL=model.BookPdfURL
            };

            books.BookGallery = new List<BookGallery>();
            foreach (var file in model.Gallery)
            {
                books.BookGallery.Add(new BookGallery()
                {
                    Name=file.Name,
                    Url=file.Url
                });
            }

            await _context.Books.AddAsync(books);
            await _context.SaveChangesAsync();
            return books.Id;

        }

        public async Task<List<BookModel>> GetAllBooks()
        {
            var books = new List<BookModel>();
            var allBooks = await _context.Books.ToListAsync();
            if (allBooks?.Any() == true)
            {
                foreach (var book in allBooks)
                {
                    books.Add(new BookModel()
                    {
                        Id=book.Id,
                        Title=book.Title,
                        Author=book.Author,
                        Description=book.Description,
                        Category=book.Category,
                        TotalPage=book.TotalPage,
                        LanguageId=book.LanguageId,
                        CoverImageUrl=book.CoverImageUrl
                     
                    });
                }

                return books;
            }
            return null;
        }

        public async Task<BookModel> GetBookById(int id)
        {
            return await _context.Books.Where(x => x.Id == id).Select(book => new BookModel()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Category = book.Category,
                TotalPage = book.TotalPage,
                LanguageId = book.LanguageId,
                Language = book.Language.Name,
                Gallery = book.BookGallery.Select(x => new GalleryModel()
                {
                    Id=x.Id,
                    Name=x.Name,
                    Url= x.Url

                }).ToList(),
                BookPdfURL=book.BookPdfURL

            }).FirstOrDefaultAsync();
        }
    }
}
