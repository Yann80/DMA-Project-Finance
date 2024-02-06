using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }

        [JsonProperty("name")]
        [Required(ErrorMessage = "The project name is required")]
        public string ProjectName { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }

        [JsonProperty("completed_at")]
        public DateTime? CompletedAt { get; set; }

        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }

        [JsonProperty("notes")]
        public string? Notes { get; set; } = string.Empty;

        [JsonProperty("permalink_url")]
        public string? PermaLink { get; set; } = string.Empty;

        [JsonProperty("gid")]
        public string? Gid { get; set; }

        public ICollection<ProjectTask> ProjectTasks { get; set; }
    }

    public class ProjectJsonResponse
    {
        public List<Project> Data { get; set; }
    }
}
