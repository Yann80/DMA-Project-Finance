using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProjectTask
    {
        public int ProjectTaskId { get; set; }

        public int? ProjectId { get; set; }
        public int? TaskItemId { get; set; }

        public decimal? Billing { get; set; }

        public Project Project { get; set; }
        public TaskItem Task { get; set; }
    }
}
