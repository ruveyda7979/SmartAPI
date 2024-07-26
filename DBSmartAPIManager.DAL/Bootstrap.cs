using DBSmartAPIManager.DAL.Context;
using DBSmartAPIManager.DAL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DBSmartAPIManager.DAL
{
    public static class Bootstrap
    {
        public static void RepositoryInitialize(this IServiceCollection service)
        {
            service.AddScoped(typeof(DBSmartAPIManagerContext));
            service.AddScoped(typeof(ProjectService));
            service.AddScoped(typeof(ProjectFileService));
            service.AddScoped(typeof(ProjectJsonService));
            service.AddScoped(typeof(UserService));
        }
    }
}
