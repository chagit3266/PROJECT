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
            service.AddScoped<IService<Node>, PointService>();
            service.AddScoped<IService<Way>, RouteService>();
            service.AddScoped<IService<UserRoutes>, UserRoutesService>();
            //service.AddScoped<IUserService, UserService>();
            return service;
        }
    }
}
