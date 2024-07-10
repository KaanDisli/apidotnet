using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Models.Repositories
{
    public class BookRepositoryCache
    {

        private readonly IMemoryCache _cache;
        private readonly LibraryContext _context;
        private readonly string cacheKey = "Library_data";
        private  List<Book> books = new List<Book>();
        private readonly ILogger<BookRepository> _logger;
        private BookRepository _repository; 
        public BookRepositoryCache(IMemoryCache cache,BookRepository repository, LibraryContext context,ILogger<BookRepository> logger){
            _cache = cache;
            _repository = repository;
            _context = context;
            _logger = logger;
            
        }
        public async Task updateCache(){
                _logger.LogInformation($"Making a db request to fill cache ");
                var booklist = await _context.Books.ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(15));
                _cache.Set(cacheKey,booklist,cacheEntryOptions);    
                books = _cache.Get<List<Book>>(cacheKey) ?? new List<Book>();              
        }
        public async Task addBookCache(Book book){
            _logger.LogInformation($"Cache function calling db function to add Book");
            Console.WriteLine("TEST 4 ");
            await _repository.addBook(book);
        }
        public async Task<Book?> getBookByIdCache(int id){
            _logger.LogInformation($"Getting book from cache");
            if (books == null){
                await updateCache();
                if (books == null){
                    throw new Exception($"Book with id  {id} not found");
                }
            }
            var book =  books.Find((b)=>b.id == id);
            if (book != null){
                _logger.LogInformation($"Book found in cache");
                return book;
            }else{
                _logger.LogInformation($"Book with id {id} not found in cache. Calling updateCache()...");
                await updateCache();
                var book_ =  books.Find((b)=>b.id == id);
                if (book_ == null ){
                    throw new Exception($"Book with id  {id} not found");
                }
                return book_;
            }
        }
        public  async Task<IEnumerable<Book?>> getBookByCategoryCache(string category){
            await updateCache();
            _logger.LogInformation($"Getting books with category: {category} from cache");
            return books.Where((b)=>b.category == category);
        }
        public  async Task<Book> getBookBySerialNumberCache(string serialNumber){
            _logger.LogInformation($"Getting books by serialNumber");
            var book =  books.Find((b)=>b.serialNumber == serialNumber);
            if (book != null){
                return book;
            }else{
                await updateCache();
                var book_ =  books.Find((b)=>b.serialNumber == serialNumber);
                if (book_ == null ){
                    throw new Exception($"Book with serialNumber  {serialNumber} not found");
                }
                return book_;
            }
            
        }
        public  void ModifyBookCache(Book book){
            
        }

        public async Task DeleteBookCache(int id){
            _logger.LogInformation($"Deleting book with id {id}from cache");
            var book = await getBookByIdCache(id);
            if (book != null){
                books.Remove(book);
                _cache.Set(cacheKey, books);
            }
            try{
                await _repository.DeleteBook(id);
            }catch (Exception ex){
                Console.WriteLine($"Error deleting book with id {id}: {ex.Message}");
                throw; 
            }   
        }
        public async Task<bool> bookExistsCache(int? id){
            _logger.LogInformation($"Checking if book with id : {id} exists in cache");
            if(books == null){
                Console.WriteLine("TEST 21");
                await updateCache();
                if (books == null){
                    Console.WriteLine("TEST 22");
                    return false;
                }
            }
            Console.WriteLine("TEST 20");
            if (books.Any((b)=>b.id == id)){
                 return true;
            }
            else{
                await updateCache();
                return books.Any((b)=>b.id == id);
            }
            

           
           
        }

    }
}