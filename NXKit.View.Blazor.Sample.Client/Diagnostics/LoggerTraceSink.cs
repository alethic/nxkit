using System;

using Cogito.Autofac;

using Microsoft.Extensions.Logging;

using NXKit.Diagnostics;

namespace NXKit.View.Blazor.Sample.Client.Diagnostics
{

    [RegisterAs(typeof(ITraceSink))]
    [RegisterSingleInstance]
    public class LoggerTraceSink : ITraceSink
    {

        readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="logger"></param>
        public LoggerTraceSink(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Debug(object data)
        {
            logger.LogDebug("{Data}", data);
        }

        public void Debug(string message)
        {
            logger.LogDebug(message);
        }

        public void Debug(string format, params object[] args)
        {
            logger.LogDebug(format, args);
        }

        public void Error(object data)
        {
            logger.LogError("{Data}", data);
        }

        public void Error(string message)
        {
            logger.LogError(message);
        }

        public void Error(string format, params object[] args)
        {
            logger.LogError(format, args);
        }

        public void Information(object data)
        {
            logger.LogInformation("{Data}", data);
        }

        public void Information(string message)
        {
            logger.LogInformation(message);
        }

        public void Information(string format, params object[] args)
        {
            logger.LogInformation(format, args);
        }

        public void Warning(object data)
        {
            logger.LogWarning("{Data}", data);
        }

        public void Warning(string message)
        {
            logger.LogWarning(message);
        }

        public void Warning(string format, params object[] args)
        {
            logger.LogWarning(format, args);
        }

    }

}
