using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PrintJob.Services;

namespace PrintJob
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly AppSettings _appSettings;
        private readonly IJobFileProcessor _jobFileProcessor;
        private readonly IJobCalculationService _jobCalculationService;

        public App(IOptions<AppSettings> appSettings, ILogger<App> logger, IJobFileProcessor jobFileProcessor, IJobCalculationService jobCalculationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _jobFileProcessor = jobFileProcessor;
            _jobCalculationService = jobCalculationService;
        }

        public async Task Run()
        {
            var jobs = await _jobFileProcessor.ProcessFile();
        }
    }

    public class AppSettings
    {
        public double SalesTax { get; set; }
        public double Margin { get; set; }
        public double ExtraMargin { get; set; }
    }
}
