
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TimeTracker.Data;
using TimeTracker.DTO;
using TimeTracker.Entities;


namespace TimeTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerContext _context;

        public CustomerController(CustomerContext context)
        {
            _context = context;
        }




        [HttpGet]
        
        public IActionResult Get()
        {
            return Ok(_context.Customers.Select(c => new CustomerDTO
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList());
        }

       

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var result = _context.Customers.Find(id);
            if (result == null)
                return NotFound();


            return Ok( new CustomerDTO
            {
                Id = result.Id,
                Name = result.Name,
            });
        }

        public class CreateCustomerModel
        {
            [Required]
            [MaxLength(50)]
            public string Name { get; set; }
        }

        [HttpPost]
        public IActionResult Create(CreateCustomerModel customer)
        {

            var c = new Customer
            {
                Name = customer.Name,
            };

            _context.Customers.Add(c);
            _context.SaveChanges();

            var cDto = new CustomerDTO
            {
                Id = c.Id,
                Name = c.Name,
                
            };
                

            return CreatedAtAction(nameof(GetOne), new { id = c.Id }, cDto);


        }
        public class EditCustomerModel
        {
            [Required]
            [MaxLength(50)]
            public string Name { get; set; }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, EditCustomerModel customer)
        {
            var cus = _context.Customers.FirstOrDefault(e => e.Id == id);
            if (cus == null) return NotFound();


            cus.Name = customer.Name;
            

            _context.SaveChanges();
            return NoContent();
        }









    }
}
