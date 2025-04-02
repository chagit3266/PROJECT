using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public static class ExtentionService
    {
        public static IServiceCollection AddService(this IServiceCollection service)
        {
            service.AddRepository();
            service.AddScoped<IService<Node,long>, NodeService>();
            service.AddScoped<IService<Way,string>, WayService>();
            service.AddScoped<IService<UserWay,int>, UserRoutesService>();
            //service.AddScoped<IUserService, UserService>();
            service.AddScoped<IAlgorithem,AlgorithmService>();
            return service;
        }
    }
}
