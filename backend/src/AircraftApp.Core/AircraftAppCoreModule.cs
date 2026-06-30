using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AircraftApp
{
    public class AircraftAppCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = AircraftAppConsts.ConnectionStringName;
            // SMTP: ABP reads email settings from AbpSettings table by default.
            // Override via ISmtpEmailSenderConfiguration registration if needed.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AircraftAppCoreModule).GetAssembly());
        }
    }
}
