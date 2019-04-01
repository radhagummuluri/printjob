
using Microsoft.Extensions.Options;
using System;

namespace PrintJob.Services
{
    public interface IJobCalculationService
    {
    }

    public class JobCalculationService : IJobCalculationService
    {
        private object _appSettings;

        public JobCalculationService(IOptions<AppSettings> appSettings)
        {

        }
    }
}
