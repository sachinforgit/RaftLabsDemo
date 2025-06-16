using SampleCodeForRaftLabs.Exceptions;
using SampleCodeForRaftLabs.Models;
using SampleCodeForRaftLabs.Services;
using System.Configuration;

namespace SampleAppForRaftLabs
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();

            var apiBaseUrl = ConfigurationManager.AppSettings["APIBASEURL"];
            var internalUserService = new InternalUserService(httpClient, apiBaseUrl);
            var userService = new ExternalUserService(internalUserService);

            Console.WriteLine("--- Demo: Get User By ID (Exists) ---");
            try
            {
                int userIdToFetch = 2;
                Console.WriteLine($"Fetching user with ID {userIdToFetch}...");
                User? user = await userService.GetUserByIdAsync(userIdToFetch);
                if (user != null)
                {
                    Console.WriteLine($"Found user: ID: {user.Id}, Name: {user.FirstName} {user.LastName}, Email: {user.Email}");
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
            catch (ApiException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"API Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.ResetColor();
            }
            Console.WriteLine(new string('-', 50));
            Console.WriteLine();

            Console.WriteLine("--- Demo: Get User By ID (Does Not Exist) ---");
            try
            {
                int userIdToFetchNonExistent = 23;
                Console.WriteLine($"Fetching user with ID {userIdToFetchNonExistent}...");
                User? user = await userService.GetUserByIdAsync(userIdToFetchNonExistent);
                if (user != null)
                {
                    Console.WriteLine($"Found user: {user.FirstName} {user.LastName} ({user.Email})");
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Successfully caught expected error: {ex.Message}");
                Console.ResetColor();
            }
            catch (ApiException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"API Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.ResetColor();
            }
            Console.WriteLine(new string('-', 50));
            Console.WriteLine();

            Console.WriteLine("--- Demo: Get All Users ---");
            try
            {
                Console.WriteLine("Fetching all users...");
                IEnumerable<User> allUsers = await userService.GetAllUsersAsync(1);
                int count = 0;
                foreach (var u in allUsers)
                {
                    if (u != null)
                    {
                        Console.WriteLine($"- ID: {u.Id}, Name: {u.FirstName} {u.LastName}, Email: {u.Email}");
                        count++;
                    }
                }
                Console.WriteLine($"Total users fetched: {count}");
            }
            catch (ApiException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"API Error fetching all users: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.ResetColor();
            }
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}