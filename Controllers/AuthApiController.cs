#nullable enable
using System.Collections.Generic;
using System.Linq;
using BookMgtApi.Data;
using BookMgtApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookMgtApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthApiController: ControllerBase
        {
            
            private readonly AppDbContext _dbContext;

            public AuthApiController(AppDbContext dbContext){
                _dbContext = dbContext;
            }


            //Get all the Authors in the database
            [HttpGet]
            public ActionResult<List<Author>> GetAll() 
            {
                List<Author> authors = _dbContext.Authors.ToList();
                if (authors == null)
                {
                return NotFound("No record found");
                }
                return Ok(authors);
            }

            //Add author to the database
            [HttpPost]
            public ActionResult<List<Author>> Add(List<Author> authors)
            {
                ObjectResult response = Ok("");
                authors.ForEach(author => {
                    var result = _dbContext.Authors.Where(dbAuthor => dbAuthor.Name == author.Name).ToList();
                    if(result.Count != 0)
                    {
                    response = Ok("Record already exist");
                    }
                    else
                    {
                        _dbContext.Authors.Add(author);
                        _dbContext.SaveChanges();
                        response = Created("created", author);
                    }
                
                });
                return response;
            }

            //Get Author by ID
            [HttpGet("{id}")]
            public ActionResult<Author> SingleAuthor(int id) 
            {
                Author? author = _dbContext.Authors.Find(id);
                if (author == null || id <= 0)
                {
                return  NotFound("No record");
                }
                return Ok(author);
            }

            //Edit Author detais in the in the database
            [HttpPut("{id}")]
            public ActionResult<Author> EditAuthor(int id, [FromBody]Author author)
            {
                Author? dbAuthor = _dbContext.Authors.Find(id);
                if (dbAuthor == null || id <= 0)
                {
                return  NotFound("No record");
                }
                dbAuthor.Age = author.Age;
                dbAuthor.Name = author.Name;
                _dbContext.Authors.Update(dbAuthor);
                _dbContext.SaveChanges();
                return Ok(dbAuthor);
            }
            
            //Delete Author and Associated books from the database
            [HttpDelete("{id}")]
            public ActionResult Delete(int id)
            {
                Author? dbAuthor = _dbContext.Authors.Find(id);
                if (dbAuthor == null || id <= 0)
                {
                return  NotFound("No record Found");
                }
                _dbContext.Authors.Remove(dbAuthor);
                _dbContext.Books.RemoveRange(_dbContext.Books.Where(book => book.AuthorId == dbAuthor.Id));
                _dbContext.SaveChanges();
                return Ok("Author record and associated books successfully deleted");
            }    
        }
}

