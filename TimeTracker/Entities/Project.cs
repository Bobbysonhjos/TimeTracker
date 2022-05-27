namespace TimeTracker.Entities
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Customer Customer { get; set; }

        public List<TimeTracking> TimeTrackings { get; set; }=new List<TimeTracking>();

    }
}
