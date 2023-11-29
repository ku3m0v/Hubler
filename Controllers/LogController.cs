using Microsoft.AspNetCore.Mvc;
using Hubler.DAL.Interfaces;
using Hubler.DAL.Models;
using System.Text;
using System.Globalization;

namespace Hubler.Controllers;

[ApiController]
[Route("api/logs")]
public class LogController : ControllerBase
{
    private readonly ILogDAL _logDAL;

    public LogController(ILogDAL logDAL)
    {
        _logDAL = logDAL;
    }

    // GET: api/logs/list
    [HttpGet("list")]
    public ActionResult<IEnumerable<Log>> GetAll()
    {
        var logs = _logDAL.GetAll();
        return Ok(logs);
    }

    // GET: api/logs/download
    [HttpGet("download")]
    public ActionResult DownloadCsv()
    {
        var logs = _logDAL.GetAll();
        var stringBuilder = new StringBuilder();
        var info = CultureInfo.InvariantCulture;

        // Writing the header
        stringBuilder.AppendLine("Id,Timestamp,Level,Message");

        // Writing the data
        foreach (var log in logs)
        {
            stringBuilder.AppendLine($"{log.tabulka},{log.operace},{log.cas},{log.uzivatel}");
        }

        byte[] buffer = Encoding.UTF8.GetBytes(stringBuilder.ToString());
        string csvName = $"logs-{DateTime.UtcNow.ToString("yyyyMMddHHmmss", info)}.csv";

        return File(buffer, "text/csv", csvName);
    }
}