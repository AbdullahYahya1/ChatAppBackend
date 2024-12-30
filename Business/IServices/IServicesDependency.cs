using AutoMapper;
using DataAccess.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IServicesDependency<T>
    {
        IUnitOfWork UnitOfWork { get; }
        IConfiguration Configuration { get; }
        IMapper Mapper { get; }
        IHttpContextAccessor HttpContextAccessor { get; }
        //ILogger<T> Logger { get; }
        string GetUserId();
        string GetUserRole();
    }
}
