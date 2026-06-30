using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AircraftApp
{
    [DependsOn(typeof(AircraftAppCoreModule), typeof(AbpAutoMapperModule))]
    public class AircraftAppApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.AddMaps(typeof(AircraftAppApplicationModule).GetAssembly());
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AircraftAppApplicationModule).GetAssembly());
        }
    }
}
