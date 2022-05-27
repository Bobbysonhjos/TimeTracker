using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.DTO;
using TimeTracker.Entities;



namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeTrackerController : ControllerBase
    {

        private readonly CustomerContext _context;

        public TimeTrackerController(CustomerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(int? projectid = null)
        {
            if (projectid is not null)
            {

                var project = _context.Projects.Include(x => x.TimeTrackings)
                    .FirstOrDefault(x => x.ProjectId == projectid);
                return Ok(project.TimeTrackings.Select(x => new TimeTrackingDTO()
                {
                    Id = x.Id,
                    Date = x.Date,
                    NumberofMinutes = x.NumberofMinutes,
                    Description = x.Description,
                    ProjectId = x.Project.ProjectId,

                }).ToList());


            }
            return Ok(_context.TimeTracking.Select(x => new TimeTrackingDTO
            {
               Id = x.Id,
               Date = x.Date,
               NumberofMinutes = x.NumberofMinutes,
               Description = x.Description,
               ProjectId = x.Project.ProjectId,

            }).ToList());
            
        }

        [HttpGet("{timeTrackingid}")]
        public IActionResult GetOne(Guid timeTrackingid)
        {
            var result = _context.TimeTracking.Include(x=>x.Project).FirstOrDefault(x=>x.Id==timeTrackingid);
            if (result == null)
                return NotFound();
            var timeTrackingDTO = new TimeTrackingDTO
            {
                Id = result.Id,
                Date = result.Date,
                NumberofMinutes = result.NumberofMinutes,
                Description = result.Description,
                ProjectId = result.Project.ProjectId,
            };
            return Ok(timeTrackingDTO);

        }

        public class CreateTimeTrackingModel
        {
            [Required]
            [Range(1,int.MaxValue)]
            public int NumberofMinutes { get; set; }
            [Required]
            [MaxLength(50)]
            public string Description { get; set; }
            [Required]
            
            public int ProjectId { get; set; }
            [Required]
            [DataType(DataType.Date)]
            public DateTime Date { get; set; }
            
        }

        [HttpPost]
        public IActionResult Create(CreateTimeTrackingModel timeTracking)
        {
            var project = _context.Projects.Find(timeTracking.ProjectId);
            if (project is null)
                return NotFound();
            var time = new TimeTracking
            {
                
                Date = timeTracking.Date,
                Description = timeTracking.Description,
                Project = project,
                NumberofMinutes = timeTracking.NumberofMinutes,

            };

            _context.TimeTracking.Add(time);
            _context.SaveChanges();


            var timeTrackingDTO = new TimeTrackingDTO
            {
                Id = time.Id,
                Date = timeTracking.Date,
                Description = timeTracking.Description,
                ProjectId = timeTracking.ProjectId,
                NumberofMinutes = timeTracking.NumberofMinutes
            };
            
            return CreatedAtAction(nameof(GetOne), new{ timeTrackingid = time.Id}, timeTrackingDTO);

                
        }


        public class EditTimetrackingModel
        {
            [Required]
            [Range(1, int.MaxValue)]
            public int NumberofMinutes { get; set; }
            [Required]
            [MaxLength(50)]
            public string Description { get; set; }
            [Required]

            public int ProjectId { get; set; }
            [Required]
            [DataType(DataType.Date)]
            public DateTime Date { get; set; }

        }

        [HttpPut("{timeTrackingid}")]
        public IActionResult Update(Guid timeTrackingid, EditTimetrackingModel timeTracking)
        {
            var time = _context.TimeTracking.FirstOrDefault(a => a.Id == timeTrackingid);
            if (time == null) return NotFound();


            time.Date = timeTracking.Date;
            time.Description = timeTracking.Description;
            time.NumberofMinutes = timeTracking.NumberofMinutes;

            _context.SaveChanges();
            return NoContent();

        }





    }
}
