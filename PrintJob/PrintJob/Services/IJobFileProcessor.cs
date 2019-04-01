using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace PrintJob.Services
{
    public interface IJobFileProcessor
    {
       Task<IList<Job>> ProcessFile();
    }

    public class JobFileProcessor : IJobFileProcessor
    {
        public async Task<IList<Job>> ProcessFile()
        {
            var lines = await File.ReadAllLinesAsync("jobInput.txt");
            var jobs = new List<Job>();
            Job job = null;
            foreach(string line in lines)
            {
                if (!String.IsNullOrEmpty(line.Trim()))
                {
                    if (line.Contains("Job", StringComparison.CurrentCultureIgnoreCase))
                    {
                        job = new Job()
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
                            job.Items.Add(new JobItem(itemDetails[0], Convert.ToDouble(itemDetails[1]), isExempt));
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
    }
}
