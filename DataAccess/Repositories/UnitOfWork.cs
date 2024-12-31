using Business.Context;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDpContext _db;
        public IUserRepository Users { get; }
        public INotificationRepository Notifications { get; }
        public IConversationUserRepository ConversationUsers { get; }
        public IConversationRepository Conversations { get; }
        public IMessageRepository Messages { get; }
        public IMediaRepository Media { get; }
        public UnitOfWork(
            ChatDpContext context,
            IUserRepository userRepository,
            INotificationRepository notificationRepository,
            IConversationRepository conversationRepository,
            IConversationUserRepository conversationUserRepository,
            IMessageRepository messageRepository,
            IMediaRepository mediaRepository
            )
        {
            _db = context;
            Users = userRepository;
            Notifications = notificationRepository;
            ConversationUsers = conversationUserRepository;
            Messages = messageRepository;
            Media = mediaRepository;
            Conversations = conversationRepository;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }
    }

}
