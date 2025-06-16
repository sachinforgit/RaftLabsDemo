namespace SampleCodeForRaftLabs.Exceptions
{
    public class UserNotFoundException : ApiException
    {
        public UserNotFoundException(int userId) : base($"User with ID {userId} not found.")
        {
        }
    }
}
