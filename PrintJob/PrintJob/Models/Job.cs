using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintJob
{
    public class Job
    {
        public Job(double margin, double extraMargin)
        {
            Margin = margin;
            ExtraMargin = extraMargin;
        }
        public string Name { get; set; }
        public bool HasExtraMargin { get; set; }
        public List<JobItem> Items { get; set; }
        public double Margin { get; private set; }
        public double ExtraMargin { get; private set; }
        public double TotalMargin => Margin + (HasExtraMargin ? ExtraMargin : 0);
        public double JobBaseTotal => Items?.Count > 0 ? Items.Sum(item => item.Price) : 0;
        public double JobBaseTotalWithMargin => JobBaseTotal + (JobBaseTotal * TotalMargin / 100);
        public double FinalTotal => JobBaseTotalWithMargin + (Items?.Count > 0 ? Items.Sum(item => item.PriceWithSalesTax - item.Price) : 0);
        public string FinalTotalToEvenCent => FinalTotal.ToEvenCent();
    }

    public class JobItem
    {
        public JobItem(string name, double price, bool isExempt, double salesTax)
        {
            Name = name;
            Price = price;
            IsExempt = isExempt;
            SalesTax = salesTax;
        }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool IsExempt { get; set; }
        public double SalesTax { get; private set; }
        public double PriceWithSalesTax => (!IsExempt ? Math.Round(Price + Price * SalesTax / 100, 2): Price);
    }
}
