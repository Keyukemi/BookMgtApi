#nullable enable
using System;
using BookMgtApi.Models;
using Microsoft.AspNetCore.Mvc;
using BookMgtApi.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BookMgtApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookApiController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public BookApiController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        //Get all the books in the database

        [HttpGet]
        [AllowAnonymous]
        public  ActionResult<List<Book>> GetAll() 
        {
            List<Book> books = _dbContext.Books.ToList();
            if (books == null)
            {
                NotFound("Not item Found");
            }
            return Ok(books);
        }

        //Get a single book from the database

        [HttpGet("{id}")]
        public ActionResult<Book> SingleBook(int id)
        {
            var book = _dbContext.Books.Include(ctx => ctx.Author).Where(book => book.Id == id);
            if (book.Count() == 0 || id <= 0)
            {
               return  NotFound("No record Found");
            }
            return Ok(book);
        }

        //Add books to the database

        [HttpPost]
        public ActionResult<List<Book>> Add(List<Book> books)
        {
            ObjectResult response = Ok("");
            books.ForEach(book => {
                var author = _dbContext.Authors.Where(author => author.Name == book.Author.Name).ToList();
                if(author.Count == 0)
                {
                    _dbContext.Books.Add(book);
                    _dbContext.SaveChanges();
                    response = Created("Existing author", book);
                }
                else
                {
                    Book newBook = new Book();
                    newBook.AuthorId = author[0].Id;
                    newBook.ISBN = book.ISBN;
                    newBook.PublishedDate = book.PublishedDate;
                    newBook.Title =  book.Title;
                    newBook.Price =  book.Price;
                    newBook.Genre =  book.Genre;
                    _dbContext.Books.Add(newBook);
                    _dbContext.SaveChanges();
                    response = Created("New Author", newBook);
                }
               
            });
            return response;
        }

        //Edit book details in the database

        [HttpPut("{id}")]
        public ActionResult<Book> EditBook(int id, [FromBody] Book book)
        {
            Book? dbBook = _dbContext.Books.Find(id);
            if (dbBook == null || id <= 0)
            {
               return  NotFound("No record Found");
            }
            dbBook.Title = book.Title;
            dbBook.ISBN = book.ISBN;
            dbBook.Price =  book.Price;
            dbBook.Genre =  book.Genre;
            dbBook.PublishedDate = book.PublishedDate;
            _dbContext.Books.Update(dbBook);
            _dbContext.SaveChanges();
            return Ok(_dbContext.Books.Include(ctx => ctx.Author).Where( book => book.Id == id));
        }

        //Delete a book in the database
        [HttpDelete("{id}")]
        public ActionResult<Book> Delete(int id)
        {
            Book? dbBook = _dbContext.Books.Find(id);
            if (dbBook == null || id <= 0)
            {
               return  NotFound("No record Found");
            }
            _dbContext.Books.Remove(dbBook);
            _dbContext.SaveChanges();
            return Ok("Book successfully deleted");
        }
    }
}