using Microsoft.AspNetCore.Mvc;

namespace Lab4.Web.Controllers;

[ApiController]
[Route("api/history")]
public class HistoryController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllHistories()
    {
        var histories = HistoryManager.GetAllHistories();
        if (histories == null)
        {
            return Ok("History is empty");
        }
        return Ok(histories);
    }
    
    [HttpGet("{key}")]
    public IActionResult GetHistoryByKey(string key)
    {
        var history = HistoryManager.GetHistoryByKey(key);
        if (history == null)
        {
            return Ok("History not found");
        }
        return Ok(history);
    }
}