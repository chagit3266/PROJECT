using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public static class ExtentionRepository
    {
        public static IServiceCollection AddRepository(this IServiceCollection service)
        {
            service.AddScoped<IRepository<User,int>, UserRepository>();
            service.AddScoped<IRepository<Node,long>, NodeRepository>();
            service.AddScoped<IRepository<Way,string>, WayRepository>();
            service.AddScoped<IRepository<UserWay,int>, UserWayRepository>();
            return service;
        }
    }
}
