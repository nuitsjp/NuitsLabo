using System;
using Microsoft.Extensions.Logging;

namespace WindowsServiceStudy.Application
{
    public class Processor : IProcessor
    {
        private readonly ILogger<IProcessor> _logger;

        public Processor(ILogger<IProcessor> logger)
        {
            _logger = logger;
        }

        public void DoSomeWork()
        {
            _logger.LogInformation("DoSomeWork!");
        }
    }
}