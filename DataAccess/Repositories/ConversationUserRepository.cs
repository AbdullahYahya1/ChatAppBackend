using Business.Context;
using Business.Entities;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ConversationUserRepository : Repository<ConversationUser>, IConversationUserRepository
    {
        public ConversationUserRepository(ChatDpContext context) : base(context)
        {
        }
    }
}
