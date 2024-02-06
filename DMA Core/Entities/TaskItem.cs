using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; } = string.Empty;

        [JsonProperty("due_on")]
        public DateTime? DueOn { get; set; }

        [JsonProperty("modified_at")]
        public DateTime? ModifiedAt { get; set; }

        [JsonProperty("name")]
        [Required(ErrorMessage = "The task name is required")]
        public string TaskName { get; set; }

        [JsonProperty("notes")]
        public string? Notes { get; set; } = string.Empty;

        [JsonProperty("permalink_url")]
        public string? PermaLink { get; set; }

        [JsonProperty("memberships")]
        [NotMapped]
        public List<Membership> Memberships { get; set; }

        [JsonProperty("gid")]
        public string? Gid { get; set; }

        public ICollection<ProjectTask> ProjectTasks { get; set; }
    }

    public class TaskItemDetail
    {
        [JsonProperty("data")]
        public TaskItem data { get; set; }
    }

    public class Membership
    {
        public ProjectItem project { get; set; }
        public Section section { get; set; }
    }

    public class ProjectItem
    {
        public string gid { get; set; }
        public string name { get; set; }
        public string resource_type { get; set; }
    }

    public class Section
    {
        public string gid { get; set; }
        public string name { get; set; }
        public string resource_type { get; set; }
    }

    public class TaskInfo
    {
        [JsonProperty("gid")]
        public string Gid { get; set; }
    }

    public class TaskInfoResponse
    {
        public List<TaskInfo> Data { get; set; }
    }
}
