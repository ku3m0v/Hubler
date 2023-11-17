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
        var content = _binaryContentDAL.GetById(employee.ContentId);

        var profile = new ProfileModel
        {
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            CreatedDate = employee.CreatedDate,
            SupermarketName = supermarket?.Title,
            PhotoBase64 = content != null ? Convert.ToBase64String(content.Content) : null
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

        if (!string.IsNullOrEmpty(model.PhotoBase64))
        {
            // Delete the old content
            if (employee.ContentId.HasValue)
            {
                _binaryContentDAL.Delete(employee.ContentId);
            }

            // Create new content
            var newContent = new BinaryContent
            {
                FileName = "profile_picture", // or derive from other sources
                FileType = "image", // or derive from other sources
                FileExtension = ".jpg", // or derive from the base64 string
                Content = Convert.FromBase64String(model.PhotoBase64),
                UploadDate = DateTime.UtcNow
            };

            _binaryContentDAL.Insert(newContent);

            // Update the employee's content reference
            employee.ContentId = newContent.Id;
        }

        _employeeDAL.Update(employee);

        return Ok("Profile updated successfully.");
    }
}