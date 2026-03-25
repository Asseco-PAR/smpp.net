using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
namespace JulMar.Smpp.Utility
{
    /// <summary>
    /// Punkt integracji biblioteki z logowaniem aplikacji hostującej.
    /// </summary>
    public static class SmppLogging
    {
        private static ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;

        public static ILoggerFactory LoggerFactory
        {
            get => _loggerFactory;
            set => _loggerFactory = value ?? NullLoggerFactory.Instance;
        }

        public static ILogger CreateLogger<T>() =>
            _loggerFactory.CreateLogger(typeof(T).FullName ?? typeof(T).Name);
    }
}
