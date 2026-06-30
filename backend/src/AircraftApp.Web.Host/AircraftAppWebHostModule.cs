using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AircraftApp.EntityFrameworkCore;

namespace AircraftApp.Web.Host
{
    [DependsOn(typeof(AircraftAppApplicationModule), typeof(AircraftAppEntityFrameworkCoreModule), typeof(AbpAspNetCoreModule))]
    public class AircraftAppWebHostModule : AbpModule
    {
        public override void PreInitialize()
        {
            // Expose all AppServices as dynamic API controllers
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(AircraftAppApplicationModule).GetAssembly(),
                    moduleName: "app",
                    useConventionalHttpVerbs: true
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AircraftAppWebHostModule).GetAssembly());
        }
    }
}
