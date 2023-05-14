using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using System.Text.Json.Serialization;

namespace SerilogStudy.Http.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Log([FromBody] List<LogEvent> logEvents)
        {
            // ÉçÉOÇï€ë∂Ç∑ÇÈèàóù
            return Ok();
        }
    }
}

public record LogEvent(
    DateTime Timestamp,
    string Level,
    string? MessageTemplate,
    string? RenderedMessage,
    Dictionary<string, object>? Properties,
    Dictionary<string, LogEventRendering[]>? Renderings,
    string? Exception)
{
}

public record LogEventRendering(
    string Format,
    string Rendering);
