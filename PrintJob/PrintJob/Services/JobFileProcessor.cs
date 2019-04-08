using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using System.Collections.Async;

namespace PrintJob.Services
{
    public interface IJobFileProcessor
    {
       Task<IList<Job>> ProcessInputFile();
        Task GenerateInvoices(IList<Job> jobs);
        Task GenerateInvoice(Job job);
    }

    public class JobFileProcessor : IJobFileProcessor
    {
        private string _outputDirectory;
        private AppSettings _appSettings;

        public JobFileProcessor(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _appSettings.InvoiceOutputFolder);
        }

        public async Task<IList<Job>> ProcessInputFile()
        {
            var lines = await File.ReadAllLinesAsync(_appSettings.JobInputFile);
            var jobs = new List<Job>();
            Job job = null;
            foreach(string line in lines)
            {
                if (!String.IsNullOrEmpty(line.Trim()))
                {
                    if (line.Contains("Job", StringComparison.CurrentCultureIgnoreCase))
                    {
                        job = new Job(_appSettings.Margin, _appSettings.ExtraMargin)
                        {
                            Name = line.Trim(':'),
                            Items = new List<JobItem>()
                        };
                        jobs.Add(job);
                    }
                    else if (line.Equals("extra-margin", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (job != null)
                        {
                            job.HasExtraMargin = true;
                        }
                        else
                        {
                            //throw incorrect format exception
                        }
                    }
                    else
                    {
                        var itemDetails = line.Split(" ");
                        if(itemDetails?.Count() > 0)
                        {
                            var isExempt = itemDetails.Any(detail => detail.Equals("exempt", StringComparison.CurrentCultureIgnoreCase));
                            job.Items.Add(new JobItem(itemDetails[0], Convert.ToDouble(itemDetails[1]), isExempt, _appSettings.SalesTax));
                        }
                    }
                }
                else
                {
                    job = null;
                }
            }
            return jobs;
        }

        public async Task GenerateInvoices(IList<Job> jobs)
        {
            if (Directory.Exists(_outputDirectory))
            {
                Directory.Delete(_outputDirectory, true);
            }

            Directory.CreateDirectory(_outputDirectory);
            await jobs.ParallelForEachAsync(async (job) =>
            {
                await GenerateInvoice(job);
            }, maxDegreeOfParalellism:3);
        }

        public async Task GenerateInvoice(Job job)
        {
            StringBuilder jobOutput = new StringBuilder();

            jobOutput.AppendLine(job.Name);
            foreach(JobItem item in job.Items)
            {
                jobOutput.AppendLine($"{item.Name}: {item.Price:$0.00}");
            }
            jobOutput.AppendLine($"total: {job.FinalTotalToEvenCent}");

            // Write the specified text asynchronously to a new file named "WriteTextAsync.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_outputDirectory, $"{job.Name.Replace(" ", "-")}.txt")))
            {
                await outputFile.WriteAsync(jobOutput.ToString());
            }
        }
    }
}