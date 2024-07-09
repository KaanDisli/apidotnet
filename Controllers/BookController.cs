using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using api.Validations;
using api.Filters;
namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class BookController: ControllerBase
    {
        private BookRepositoryCache _bookRepositoryCache;
        private UserRepository _userRepository;
        private readonly ILogger<BookController> _logger;
        public BookController(BookRepositoryCache bookRepositoryCache,UserRepository userRepository,ILogger<BookController> logger){
            _bookRepositoryCache = bookRepositoryCache;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet("get/book/{bookID}")]
        //[FilterBookID]
        public async Task<IActionResult> getBook(int bookID){
            _logger.LogInformation($"Recieved request at get/book/{bookID}");
            try{
                var book = await _bookRepositoryCache.getBookByIdCache(bookID);
                return Ok(book);
            }catch(Exception ex){
                return BadRequest("Book not found");
            }
            
        }
        [HttpGet("get/category/{category}")]
        public async Task<IActionResult> getCategory(string Category){
            _logger.LogInformation($"Recieved request at get/category/{Category}");
            var books = await _bookRepositoryCache.getBookByCategoryCache(Category);
            if (books != null){
                return Ok(books);
            }
            else{
                return BadRequest("No books in this category");
            }
            
        }
        [HttpGet("get/serialNumber/{serialNumber}")]
        
        public async Task<IActionResult> getserialNumber(string serialNumber){
            _logger.LogInformation($"Recieved request at get/serialNumber/{serialNumber}");
            var book = await _bookRepositoryCache.getBookBySerialNumberCache(serialNumber);
            if (book != null){
                return Ok(book);
            }
            else{
                return BadRequest("Book with this serialNumber not found");
            }
        }

        [HttpPost("add/{userID}")]
        [ValidateBookAdd]
        public async Task<ActionResult> addBook(int userID, [FromBody] Book book){
            _logger.LogInformation($"Recieved request at add/{userID}");
            if (_userRepository.UserExists(userID)){
                await _bookRepositoryCache.addBookCache(book);
                return Ok("Book added");
            }
            else{
                return BadRequest("User does not exist");
            }
            
        }
        [HttpDelete ("delete/{bookID}/{userID}")]
        
        public async Task<ActionResult> deleteBook(int bookID, int userID){
            _logger.LogInformation($"Recieved request at delete/{bookID}/{userID}");
            if (_userRepository.UserExists(userID)){
                if (!_bookRepositoryCache.bookExistsCache(bookID)){
                    return BadRequest("Book does not exist");
                }
                await _bookRepositoryCache.DeleteBookCache(bookID);
                return Ok("Book deleted");
            }
            else{
                return BadRequest("User does not exist");
            }
            
        }
        [HttpPut("update/{bookID}/{userID}")]
        [FilterBookID]
        public string modifyBook(int bookID, int userID){
            _logger.LogInformation($"Recieved request at update/{bookID}/{userID}");
            return "ok";
        }
    }
}