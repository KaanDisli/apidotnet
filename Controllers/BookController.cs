using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using api.Validations;
using api.Filters;
using Prometheus;
using System.Diagnostics;
namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class BookController: ControllerBase
    {
        private BookRepositoryCache _bookRepositoryCache;
        private UserRepository _userRepository;
        private readonly ILogger<BookController> _logger;
        private static Counter requestCounter = Metrics.CreateCounter("dotnet_requests_total","Total amount of requests recieved.");
        private static Counter errorCounter = Metrics.CreateCounter("dotnet_error_total","Total amount of errors.");
        private static Histogram requestDuration = Metrics.CreateHistogram("dotnet_request_time","Total time it takes to process a request");
        public BookController(BookRepositoryCache bookRepositoryCache,UserRepository userRepository,ILogger<BookController> logger){
            _bookRepositoryCache = bookRepositoryCache;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet("get/book/{bookID}")]
        //[FilterBookID]
        public async Task<IActionResult> getBook(int bookID){
             requestCounter.Inc();
            _logger.LogInformation($"Recieved request at get/book/{bookID}");
            var sw = Stopwatch.StartNew();
            try{
                var book = await _bookRepositoryCache.getBookByIdCache(bookID);
                return Ok(book);
            }catch(Exception){
                errorCounter.Inc();
                return BadRequest("Book not found");
                
            }
            finally{
                sw.Stop();
                requestDuration.Observe(sw.Elapsed.TotalSeconds);
            }
            
        }
        [HttpGet("get/category/{category}")]
        public async Task<IActionResult> getCategory(string Category){
            var sw = Stopwatch.StartNew();
            try{
                requestCounter.Inc();
                
                _logger.LogInformation($"Recieved request at get/category/{Category}");
                var books = await _bookRepositoryCache.getBookByCategoryCache(Category);
                if (books != null){
                    return Ok(books);
                }
                else{
                    errorCounter.Inc();
                    return BadRequest("No books in this category");
                }
            }catch(Exception){
                errorCounter.Inc();
                return BadRequest("error");
            }
            finally{
                sw.Stop();
                requestDuration.Observe(sw.Elapsed.TotalSeconds);
            }
        }
        [HttpGet("get/serialNumber/{serialNumber}")]
        
        public async Task<IActionResult> getserialNumber(string serialNumber){
            var sw = Stopwatch.StartNew();
            try{
                requestCounter.Inc();
                _logger.LogInformation($"Recieved request at get/serialNumber/{serialNumber}");
                var book = await _bookRepositoryCache.getBookBySerialNumberCache(serialNumber);
                if (book != null){
                    return Ok(book);
                }
                else{
                    errorCounter.Inc();
                    return BadRequest("Book with this serialNumber not found");
                }
            }
            catch(Exception){
                errorCounter.Inc();
                return BadRequest("Book with this serialNumber not found");
            }
            finally{
                sw.Stop();
                requestDuration.Observe(sw.Elapsed.TotalSeconds);
            }
        }

        [HttpPost("add/{userID}")]
        [ValidateBookAdd]
        public async Task<ActionResult> addBook(int userID, [FromBody] Book book){
            requestCounter.Inc();
            _logger.LogInformation($"Recieved request at add/{userID}");
            var sw = Stopwatch.StartNew();
            try{
                if (await _userRepository.UserExists(userID)){

                    await _bookRepositoryCache.addBookCache(book);
                    return Ok("Book added");
                }
                else{
                    errorCounter.Inc();
                    return BadRequest("User does not exist");
                }
            }
            catch(Exception){
                errorCounter.Inc();
                return BadRequest("Error while adding book");
            }
            finally{
                sw.Stop();
                requestDuration.Observe(sw.Elapsed.TotalSeconds);
            }
        }
        [HttpDelete ("delete/{bookID}/{userID}")]
        
        public async Task<ActionResult> deleteBook(int bookID, int userID){
            var sw = Stopwatch.StartNew();
            try{
                requestCounter.Inc();
                _logger.LogInformation($"Recieved request at delete/{bookID}/{userID}");
                if (await _userRepository.UserExists(userID)){
                    if (!await _bookRepositoryCache.bookExistsCache(bookID)){
                        return BadRequest("Book does not exist");
                    }
                    await _bookRepositoryCache.DeleteBookCache(bookID);
                    return Ok("Book deleted");
                }
                else{
                    errorCounter.Inc();
                    return BadRequest("User does not exist");
                }
            }catch(Exception){
                errorCounter.Inc();
                return BadRequest("error");
            }
            finally{
                sw.Stop();
                requestDuration.Observe(sw.Elapsed.TotalSeconds);
            }
        }
        [HttpPut("update/{bookID}/{userID}")]
        
        public async Task<IActionResult> modifyBook(int bookID, int userID,[FromBody] Dictionary<string, object> val){
            
            _logger.LogInformation($"Recieved request at update/{bookID}/{userID}");
            var sw = Stopwatch.StartNew();
            try{
                requestCounter.Inc();
                if (await _userRepository.UserExists(userID) ){
                    await _bookRepositoryCache.ModifyBookCache(val ,bookID);
                     _logger.LogInformation($"Book modified in cache");
                     return Ok("Book succesfully updated");
                }
                else{
                    errorCounter.Inc();
                    return BadRequest("User doesn't exist");
                }
                    
            }catch(Exception){
                errorCounter.Inc();
                return BadRequest("Error updating book");
            }
            finally{
                sw.Stop();
                requestDuration.Observe(sw.Elapsed.TotalSeconds);
            }
            

        }
    }
}