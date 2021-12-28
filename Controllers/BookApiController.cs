using System;
using Microsoft.AspNetCore.Mvc;
using BookMgtApi.Data;

namespace BookMgtApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookApiController(AppDbContext context)
        {
            _context = context;
        }

        //get all Database Item and return to view
        [HttpGet]
        
        
        //take input from user and post to database
        [HttpPost]
        
        
        //get singele item
        [HttpGet("{ID}")]
        

        //take Edited input from user and Update to database

        [HttpPut("{ID}")]
        

        //Delete Data from the Database
        [HttpDelete("{ID}")]
    }
}