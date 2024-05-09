
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StudentController : ControllerBase
{
    private readonly StudentRepository _studentRepository;
    
    public StudentController(StudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    [HttpDelete]
    [Route("{id}")]
    
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (!await _studentRepository.doesStudentExist(id))
        {
            return NotFound("Student with given id does not exist");
        }

        await _studentRepository.deleteStudent(id);
        return Ok("Student deleted");
    }
}