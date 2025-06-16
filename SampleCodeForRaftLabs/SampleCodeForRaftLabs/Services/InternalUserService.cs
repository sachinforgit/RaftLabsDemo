using SampleCodeForRaftLabs.Exceptions;
using SampleCodeForRaftLabs.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace SampleCodeForRaftLabs.Services
{
    public class InternalUserService
    {
        private readonly HttpClient _httpClient;

        public InternalUserService(HttpClient httpClient, string apiBaseUrl = "https://reqres.in/api/")
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            if (!string.IsNullOrWhiteSpace(apiBaseUrl))
            {
                _httpClient.BaseAddress = new Uri(apiBaseUrl);
            }
            else // Default in case of url is not provided
            {
                _httpClient.BaseAddress = new Uri("https://reqres.in/api/");
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"users/{userId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new UserNotFoundException(userId);
                }

                response.EnsureSuccessStatusCode();

                try
                {
                    var singleUserResponse = await response.Content.ReadFromJsonAsync<SingleUserResponse>();
                    if (singleUserResponse == null || singleUserResponse.Data == null)
                    {
                        throw new ApiException("Failed to deserialize user data or user data is null.");
                    }
                    return singleUserResponse.Data;
                }
                catch (JsonException ex)
                {
                    throw new ApiException("Failed to deserialize the response from the API.", ex);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("An error occurred while making the HTTP request.", ex);
            }
        }

        public async Task<PagedResult<User>> GetUsersAsync(int pageNumber)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be a positive integer.");
            }

            try
            {
                var response = await _httpClient.GetAsync($"users?page={pageNumber}");
                response.EnsureSuccessStatusCode();

                try
                {
                    var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<User>>();
                    if (pagedResult == null)
                    {
                        throw new ApiException("Failed to deserialize paged result or the result is null.");
                    }
                    return pagedResult;
                }
                catch (JsonException ex)
                {
                    throw new ApiException("Failed to deserialize the paginated response from the API.", ex);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("An error occurred while making the HTTP request.", ex);
            }
        }
    }
}
