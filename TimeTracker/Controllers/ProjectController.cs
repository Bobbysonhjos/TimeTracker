using System.ComponentModel.DataAnnotations;
using TimeTracker.Data;
using TimeTracker.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Entities;



namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly CustomerContext _context;

        public ProjectController(CustomerContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get(int? customerid = null)
        {
            if (customerid is not null)
            {
                return Ok(_context.Projects.Include(x=>x.Customer).Where(x => x.Customer.Id == customerid).Select(p => new ProjectDTO
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    CustomerId = p.Customer.Id,

                }).ToList());
            }
            return Ok(_context.Projects.Select(p => new ProjectDTO
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                CustomerId = p.Customer.Id,

            }).ToList());
        }



        [HttpGet("{projectid}")]
        public IActionResult GetOne(int projectid)
        {
            var result = _context.Projects.Find(projectid);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        public class CreateProjectModel
        {
            [Required]
            public int CustomerId { get; set; }
            [Required]
            [MaxLength(50)]
            public string ProjectName { get; set; }
        }


        [HttpPost]
        public IActionResult Create(CreateProjectModel project)
        {
            var cus = _context.Customers.FirstOrDefault(e => e.Id == project.CustomerId);
            if (cus == null) return NotFound();
            var p = new Project
            {
                Customer = cus,
                ProjectName = project.ProjectName,
            };
            _context.Projects.Add(p);
            _context.SaveChanges();

            var pDto = new ProjectDTO
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                CustomerId = p.Customer.Id,
            };

            return CreatedAtAction(nameof(GetOne), new { projectid = p.ProjectId }, pDto);
        }

        public class EditProjectModel
        {
            [Required]
            [MaxLength(50)]
            public string ProjectName { get; set; }

        }


        [HttpPut("{projectid}")]
        public IActionResult Update(int projectid, EditProjectModel project)
        {
            var pro = _context.Projects.FirstOrDefault(t => t.ProjectId == projectid);
            if (pro == null) return NotFound();

            pro.ProjectName = project.ProjectName;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
