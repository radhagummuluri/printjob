using System.Collections.Generic;

namespace PrintJob
{
    public class Job
    {
        public string Name { get; set; }
        public bool HasExtraMargin { get; set; }
        public List<JobItem> Items { get; set; }
    }

    public class JobItem
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsExempt { get; set; }

        public JobItem(string name, double price, bool isExempt)
        {
            Name = name;
            Price = price;
            IsExempt = isExempt;
        }
    }
}
