using Business.Context;
using Business.Entities;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ChatDpContext _context;
        public UserRepository(ChatDpContext context) : base(context)
        {
            _context = context;
        }

 

        public async Task<User> GetUserByEmail(string email)
        {
            var User =await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return User;
        }


    }
}
