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
    public class MediaRepository : Repository<Media>, IMediaRepository
    {
        public MediaRepository(ChatDpContext context) : base(context)
        {
        }
    }
}
