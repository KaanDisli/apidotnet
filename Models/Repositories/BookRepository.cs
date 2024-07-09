using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace api.Models.Repositories
{
    //Task<Book> means an async task that returns a book
    //FindAsync can be used for primary keys, for non primary keys we use FirstOrDefaultAsync
    //Task<IEnumerable<Book>> is used for an async task that returns a collection of books, use .Where method for multiple books
    public class BookRepository
    {
        private readonly LibraryContext _context;
        private readonly ILogger<BookRepository> _logger;
        public BookRepository(LibraryContext context,ILogger<BookRepository> logger){
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Book>> AllBooks(){
            _logger.LogInformation("Getting AllBooks()");
            return await _context.Books.ToListAsync();
        }
        public async Task addBook(Book book){
            _logger.LogInformation("adding book to the database");
            Console.WriteLine("TEST 2 ");
            int id;
            try{
                Console.WriteLine("TEST 5 ");
                if (!_context.Books.Any()){
                    Console.WriteLine("TEST 6 ");
                    id = 1 ;
                    _logger.LogInformation("First book");
                }else{
                    Console.WriteLine("TEST 7 ");
                    id = _context.Books.Max(x=>x.id);
                    id = id + 1;
                }
                book.id = id;
                Console.WriteLine("TEST 3 ");
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex){
                Console.WriteLine("error handling book");
                 _logger.LogError(ex, $"Error occurred while fetching the book");
                throw;
            }

        }
        public async Task<Book?> getBookById(int id){
            _logger.LogInformation("Making a db request to get book by id");
            return await _context.Books.FindAsync(id);
        }
        public async Task<IEnumerable<Book>> getBookByCategory(string category){
            _logger.LogInformation("Making a db request to get book by category");
            return await _context.Books.Where(b => b.category == category).ToListAsync();
        }
        public async Task<Book?> getBookBySerialNumber(string serialNumber){
            _logger.LogInformation("Making a db request to get book by serialNumber");
            return await _context.Books.FirstOrDefaultAsync(b => b.serialNumber == serialNumber);
        }
        public async Task ModifyBook(Book book){
            _logger.LogInformation("Making a db request to modify book");
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(int id){
            _logger.LogInformation("Making a db request to delete book");
            var book = await _context.Books.FindAsync(id);
            if (book != null){
                _context.Books.Remove(book); 
                await _context.SaveChangesAsync();
            }
            else{
                throw new Exception($"Book with id {id} not found.");
            } 
        }
        public async Task<bool> bookExists(int? id){
            _logger.LogInformation("Making a request to verify book exists");
            if (id == null){
                return false;
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null){
                return false;
            }
            else{
                return true;
            }
        }
    }
}