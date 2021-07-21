using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.ViewModels
{
    public class ExpInCardViewModel : ExperienceViewModel
    {
        public bool ShowPosition { get; set; }
        public bool ShowFeedback { get; set; }
    }
}
