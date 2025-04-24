using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace EasyTravel.Application.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SessionService> _logger;

        public SessionService(IHttpContextAccessor httpContextAccessor, ILogger<SessionService> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("Attempted to get a session value with an empty or null key.");
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            }

            try
            {
                if (_httpContextAccessor.HttpContext?.Session.TryGetValue(key, out byte[] bytes) == true)
                {
                    _logger.LogInformation("Successfully retrieved session value for key: {Key}", key);
                    return Encoding.UTF8.GetString(bytes);
                }

                _logger.LogInformation("No session value found for key: {Key}", key);
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the session value for key: {Key}", key);
                throw new InvalidOperationException($"An error occurred while getting the session value for key: {key}.", ex);
            }
        }

        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("Attempted to remove a session value with an empty or null key.");
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            }

            try
            {
                _httpContextAccessor.HttpContext?.Session.Remove(key);
                _logger.LogInformation("Successfully removed session value for key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the session value for key: {Key}", key);
                throw new InvalidOperationException($"An error occurred while removing the session value for key: {key}.", ex);
            }
        }

        public void SetString(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("Attempted to set a session value with an empty or null key.");
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
            }

            if (value == null)
            {
                _logger.LogWarning("Attempted to set a session value with a null value for key: {Key}", key);
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");
            }

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                _httpContextAccessor.HttpContext?.Session.Set(key, bytes);
                _logger.LogInformation("Successfully set session value for key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while setting the session value for key: {Key}", key);
                throw new InvalidOperationException($"An error occurred while setting the session value for key: {key}.", ex);
            }
        }
    }
}