namespace Core.Entities
{
    public class Import
    {
        public int ImportId { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<TaskItem> TaskItems { get; set; }
    }
}
