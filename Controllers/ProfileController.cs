using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;

namespace Hubler.Controllers;


[Route("api/profile")]
[ApiController]
[Authorize] // Ensure the controller is accessible only by authenticated users
public class ProfileController : ControllerBase
{
    private readonly IEmployeeDAL _employeeDAL;
    private readonly ISupermarketDAL _supermarketDAL;
    private readonly IBinaryContentDAL _binaryContentDAL;

    public ProfileController(IEmployeeDAL employeeDAL, ISupermarketDAL supermarketDAL, IBinaryContentDAL binaryContentDAL)
    {
        _employeeDAL = employeeDAL;
        _supermarketDAL = supermarketDAL;
        _binaryContentDAL = binaryContentDAL;
    }

    // GET: api/profile
    [HttpGet]
    public IActionResult GetProfile()
    {
        var userId = this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        int id = int.Parse(userId);

        var employee = _employeeDAL.GetById(id);
        if (employee == null) return NotFound("Profile not found.");

        var supermarket = _supermarketDAL.GetById(employee.SupermarketId);

        var profile = new ProfileModel
        {
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            CreatedDate = employee.CreatedDate,
            SupermarketName = supermarket?.Title,
        };

        return Ok(profile);
    }

    // PUT: api/profile
    [HttpPut]
    public IActionResult UpdateProfile([FromBody] ProfileModel model)
    {
        var userId = this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value;
        int id = int.Parse(userId);

        var employee = _employeeDAL.GetById(id);
        if (employee == null) return NotFound("Profile not found.");

        employee.FirstName = model.FirstName;
        employee.LastName = model.LastName;

        _employeeDAL.Update(employee);

        return Ok("Profile updated successfully.");
    }
    
    [HttpPost("upload-photo")]
    public async Task<IActionResult> UploadPhoto()
    {
        try
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var userId = this.User.Claims.First(i => i.Type.Equals(ClaimTypes.NameIdentifier)).Value;
                int id = int.Parse(userId);

                var employee = _employeeDAL.GetById(id);
                if (employee == null) return NotFound("Employee not found.");

                // Delete the old content if it exists
                if (employee.ContentId != 0) // Assuming 0 is default/non-existing value
                {
                    _binaryContentDAL.Delete(employee.ContentId);
                }

                // Save the new content
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();

                    var binaryContent = new BinaryContent
                    {
                        FileName = file.FileName,
                        FileType = "profile-photo",
                        FileExtension = Path.GetExtension(file.FileName),
                        Content = fileBytes,
                        UploadDate = DateTime.UtcNow
                    };

                    _binaryContentDAL.Insert(binaryContent);
                    employee.ContentId = binaryContent.Id;
                }

                _employeeDAL.Update(employee);

                return Ok("Photo uploaded successfully.");
            }
            else
            {
                return BadRequest("No file received.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }
}