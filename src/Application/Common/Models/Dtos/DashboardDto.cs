using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Dtos
{
    public class DashboardDto
    {
        public double RevenueInGivenMonth { get; set; }
        public double Revenue7Days { get; set; }
        public WorkspaceTypeDto? MostBookedWorkspaceType { get; set; }
    }
}
