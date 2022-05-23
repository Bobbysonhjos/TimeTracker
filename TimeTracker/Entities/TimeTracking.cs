using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTracker.Entities
{
    public class TimeTracking
    {
       
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int NumberofMinutes { get; set; }
        public string Description { get; set; }
        public int  ProjectId { get; set; }

       

        



    }
}
