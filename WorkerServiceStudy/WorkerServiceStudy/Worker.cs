namespace WorkerServiceStudy
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                File.AppendAllText("log.txt", $@"Worker running at: {DateTimeOffset.Now}{Environment.NewLine}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}