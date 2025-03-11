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
            service.AddScoped<IRepository<User>, UserRepository>();
            service.AddScoped<IRepository<Point>, PointRepository>();
            service.AddScoped<IRepository<Route>, RouteRepository>();

            return service;
        }
    }
}
