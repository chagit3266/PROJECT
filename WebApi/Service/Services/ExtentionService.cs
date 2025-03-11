using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
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
            //service.AddScoped<IService<User>, UserService>();
            //service.AddScoped<IService<Point>, PointService>();
            //service.AddScoped<IService<Route>, RouteService>();

            return service;
        }
    }
}
