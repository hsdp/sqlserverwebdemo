using Steeltoe.Extensions.Configuration.CloudFoundry;


namespace SqlServerWebDemo.ViewModels.Home
{
    public class CloudFoundryViewModel
    {
        public CloudFoundryViewModel(CloudFoundryApplicationOptions appOptions, CloudFoundryServicesOptions serviceOptions)
        {
            CloudFoundryServices = serviceOptions;
            CloudFoundryApplication = appOptions;
        }
        public CloudFoundryServicesOptions CloudFoundryServices { get;}
        public CloudFoundryApplicationOptions CloudFoundryApplication { get;}
    }
}