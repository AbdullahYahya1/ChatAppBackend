using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}
