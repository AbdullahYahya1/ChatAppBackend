using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        INotificationRepository Notifications { get; }
        IConversationUserRepository ConversationUsers { get; }
        IConversationRepository Conversations { get; }
        IMessageRepository Messages { get; }
        IMediaRepository Media { get; }
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
