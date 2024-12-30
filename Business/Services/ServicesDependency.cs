using AutoMapper;
using Business.IServices;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ServicesDependency<T> : IServicesDependency<T>
    {
        public IUnitOfWork UnitOfWork { get; }
        public IConfiguration Configuration { get; }
        public IMapper Mapper { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        //public ILogger<T> Logger { get; }

        public ServicesDependency(
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<T> logger
        )
        {
            UnitOfWork = unitOfWork;
            Configuration = configuration;
            Mapper = mapper;
            HttpContextAccessor = httpContextAccessor;
            //Logger = logger;
        }
        public string GetUserId()
        {
            return HttpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
        }
        public string GetUserRole()
        {
            return HttpContextAccessor?.HttpContext?.User?.FindFirst("UserType")?.Value;
        }
    }

}
