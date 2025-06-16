using SampleCodeForRaftLabs.Exceptions;
using SampleCodeForRaftLabs.Models;

namespace SampleCodeForRaftLabs.Services
{
    public class ExternalUserService
    {
        private readonly InternalUserService _internalUserService;

        public ExternalUserService(InternalUserService internalUserService)
        {
            _internalUserService = internalUserService ?? throw new ArgumentNullException(nameof(internalUserService));
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _internalUserService.GetUserByIdAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber)
        {
            var allUsers = new List<User>();
            int currentPage = pageNumber;
            PagedResult<User> pagedResult;

            try
            {
                pagedResult = await _internalUserService.GetUsersAsync(currentPage);
                if (pagedResult?.Data != null)
                {
                    allUsers.AddRange(pagedResult.Data);
                }

                if (pagedResult == null || pagedResult.Data == null || pagedResult.Data.Count == 0 || pagedResult.TotalPages <= 0)
                {
                    return allUsers;
                }

                for (currentPage += 1; currentPage <= pagedResult.TotalPages; currentPage++)
                {
                    var nextPageResult = await _internalUserService.GetUsersAsync(currentPage);
                    if (nextPageResult?.Data != null)
                    {
                        allUsers.AddRange(nextPageResult.Data);
                    }
                }
            }
            catch (ApiException)
            {
                throw;
            }

            return allUsers;
        }
    }
}
