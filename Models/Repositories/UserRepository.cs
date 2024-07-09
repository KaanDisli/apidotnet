using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models.Repositories
{
    public class UserRepository
    {
        private readonly LibraryContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(LibraryContext context, ILogger<UserRepository> logger){
            _context = context;
            _logger = logger;
        }
        public async Task addUser(User user){
            _logger.LogInformation($"Making a db request to add User");
            int id;
            if (!_context.Users.Any()){
                 id = 1 ;
            }else{
                id = _context.Users.Max(x=>x.id);
                id = id + 1;
            }
            user.id = id;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public bool UserExists(int id){
            _logger.LogInformation($"Making a db request to check if User exists");
            return _context.Users.Any(u => u.id == id);
        }
    }
}