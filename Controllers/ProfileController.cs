using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using Hubler.Models;

namespace Hubler.Controllers;


[Route("api/profile")]
[ApiController]
[Authorize]
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
                
                if (employee.Content_Id != 0)
                {
                    int? contentId = employee.Content_Id;
                    employee.Content_Id = null;
                    _employeeDAL.Update(employee);
                    _binaryContentDAL.Delete(contentId);
                }
                
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

                    int contentID = _binaryContentDAL.Insert(binaryContent);
                    employee.Content_Id = contentID;
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
    
    [HttpGet("profile-picture")]
    public IActionResult GetProfilePicture()
    {
        try
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            if (userIdClaim == null)
            {
                return Unauthorized("User not found.");
            }

            int userId = int.Parse(userIdClaim.Value);
            
            var employee = _employeeDAL.GetById(userId);
            if (employee == null || employee.Content_Id == 0)
            {
                return NotFound("Profile or profile picture not found.");
            }
            
            
            var binaryContent = _binaryContentDAL.GetById(employee.Content_Id);
            if (binaryContent == null || binaryContent.Content == null || binaryContent.Content.Length == 0)
            {
                return NotFound("Image not found.");
            }
            
            return File(binaryContent.Content, "image/jpeg");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }

}