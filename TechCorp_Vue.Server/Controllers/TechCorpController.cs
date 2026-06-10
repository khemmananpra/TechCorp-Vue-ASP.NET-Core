using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechCorp_Vue.Server.Models;

namespace TechCorp_Vue.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechCorpController : ControllerBase
    {
        private readonly TechCorpContext _context;

        public TechCorpController(TechCorpContext context)
        {
            _context = context;
        }



        [HttpGet("Employee")]
        public IActionResult GetEmp()
        {
            List<Employee> employees = _context.Employees
                                .AsNoTracking()
                                .Include(e => e.Department)
                                .Include(e => e.IdfileNavigation)
                                .Include(e => e.Position)
                                .Include(e => e.Signature)
                                .Include(e => e.InverseManager)
                                .Where(e => e.Isdeleted != true)
                                .ToList();


            if (employees.Count == 0)
                return NotFound("No employees found");

            return Ok(employees);
        }

        [HttpGet("Employee/{id}/FullHierarchy")]
        public ActionResult<Employee> GetEmployeeHierarchy(int id)
        {
            // root
            Employee? employee = _context.Employees
            .Include(e => e.Position)
            .Include(e => e.IdfileNavigation)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
                return NotFound();

            // ดึงลูกหลานทั้งหมดแบบ flat list
            List<Employee>? allSubordinates = _context.Employees
            .Include(e => e.Position)
            .Include(e => e.IdfileNavigation)
                .Where(e => e.Isdeleted != true)
                .ToList();

            // สร้าง tree
            employee.InverseManager = BuildHierarchy(allSubordinates, employee.Id);
            return employee;
        }

        // Recursive function
        private ICollection<Employee> BuildHierarchy(List<Employee> flatList, int parentId)
        {
            List<Employee>? children = flatList.Where(e => e.Managerid == parentId).ToList();
            foreach (Employee child in children)
            {
                child.InverseManager = BuildHierarchy(flatList, child.Id);
            }
            return children;
        }

        [HttpPost("Employee")]
        public ActionResult<Employee> CreateEmp([FromBody] Employee emp)
        {
            emp.Create(_context);
            return CreatedAtAction(nameof(GetEmp), new { id = emp.Id }, emp);
        }

        [HttpPut("Employee/{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee emp)
        {

            Employee EmpUpdate = emp.Update(_context);
            return Ok(EmpUpdate);

        }

        [HttpGet("files/{id}")]
        public async Task<ActionResult<Filemodel>> GetFile(int id)
        {
            Filemodel? file = await _context.Filemodels.Include(f => f.Employees).FirstOrDefaultAsync(f => f.Id == id);
            if (file == null) return NotFound();

            return file;
        }


        [HttpPost("files")]
        public ActionResult<Filemodel> PostFile(IFormFile coverPhotoFile)
        {
            if (coverPhotoFile == null || coverPhotoFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            Filemodel? fileEntity = Filemodel.CreateFile(_context, coverPhotoFile);
            if (fileEntity == null) return StatusCode(500, "Failed to create file.");
            // return JSON
            return CreatedAtAction(nameof(GetFile), new { id = fileEntity.Id }, fileEntity);
        }

        [HttpPut("files/{id}")]
        public ActionResult<Filemodel> UpdateFile(int id, IFormFile? newFile)
        {
            if (newFile == null) return BadRequest("No file uploaded");

            var files = new List<IFormFile> { newFile };
            var fileEntity = Filemodel.UpdateFile(_context, id, files);

            if (fileEntity == null) return NotFound();
            return Ok(fileEntity);
        }


        [HttpDelete("Employee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            Employee? employee = _context.Employees
                .Include(e => e.InverseManager)
                .Include(e => e.IdfileNavigation)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
                return NotFound();

            employee.DeleteA(_context);

            Filemodel.DeleteFilesOfEmployeeTree(employee, _context);

            return NoContent();
        }

        [HttpGet("Departments")]
        public IActionResult GetDepartments()
        {
            var deps = _context.Departments
                .Where(d => d.Isdeleted == false)
                .Select(d => new { d.Id, d.Name })
                .ToList();
            return Ok(deps);
        }

        [HttpGet("Positions")]
        public IActionResult GetPositions()
        {
            var pos = _context.Positions
                .Where(d => d.Isdeleted == false)
                .Select(d => new { d.Id, d.Title })
                .ToList();
            return Ok(pos);
        }


        //Base64 string จาก frontend -> Backend จะ decode Base64 → กลายเป็นไฟล์จริง
        [HttpPost("Signature")]
        public async Task<ActionResult> SaveSignature([FromBody] Signature model)
        {
            await model.CreateSign(_context);
            return Ok(new { id = model.Id, filePath = model.Signature1 });
        }
    }
}
