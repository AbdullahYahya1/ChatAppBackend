﻿using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IMessageRepository:IRepository<Message>
    {
        Task<ICollection<Message>> GetMessaegsByConversationID(int conversationID, int currentUserI, int? lastMessageId = null,int pageSize = 10);
        Task UpdateMessagesReadStatus(int conversationID, int currentUserId);
    }
}
