using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.ViewModels
{
    public class BackstageCategoriesAnalyzeViewModel
    {
        public List<BackstageCategoriesAnalyzeViewModelItem> Experiences { get; set; }
        public List<BackstageCategoriesAnalyzeViewModelItem> Tags { get; set; }
    }

    public class BackstageCategoriesAnalyzeViewModelItem
    {
        public string Name { get; set; }
        public int Count { get; set; }

    }
}
