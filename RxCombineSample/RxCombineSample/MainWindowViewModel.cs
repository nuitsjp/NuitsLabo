using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;

namespace RxCombineSample
{
    public class MainWindowViewModel
    {
        public ReactiveCollection<Business> Businesses { get; } = new ReactiveCollection<Business>();
        private List<UnitCategory> allUnitCategories = new List<UnitCategory>();
        public ReactiveCollection<UnitCategory> UnitCategories { get; }
        private List<Report> allReports = new List<Report>();
        public ReactiveCollection<Report> Reports { get; }

        public MainWindowViewModel()
        {
            Enumerable.Range(1, 10)
                .Select(x => new Business
                {
                    BusinessId = x,
                    Name = $"業務{x}"
                }).ToList().ForEach(x => Businesses.Add(x));

            foreach (var business in Businesses)
            {
                Enumerable.Range(1, 5).ToList().ForEach(x =>
                {
                    allUnitCategories.Add(new UnitCategory
                    {
                        Name = $"分類{x}"
                    });

                });
            }
        }
    }

    public class Business
    {
        public int BusinessId { get; set; }
        public string Name { get; set; }
    }

    public class UnitCategory
    {
        public string Name { get; set; }
        public IList<int> ReportIds { get; } = new List<int>();
    }

    public class Report
    {
        public int ReportId { get; set; }
        public string Name { get; set; }
        public int BusinessId { get; set; }
    }
}