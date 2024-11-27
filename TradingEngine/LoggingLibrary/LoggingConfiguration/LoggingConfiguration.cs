using System;
namespace TradingEngineServer.Logging.LoggingConfiguration
{
	public class LoggerConfiguration
    {

		public LoggerType LoggerType { get; set; }

		public TextLoggerConfiguration TextLoggerConfiguration { get; set; }
		public ConsoleLoggerConfiguration ConsoleLoggerConfiguration { get; set; }
    }

	public class TextLoggerConfiguration
	{
		public string Directory { get; set; }

        public string Filename { get; set; }

        public string FileExtension { get; set; }

    }

    public class ConsoleLoggerConfiguration
    {
        
    }
}

