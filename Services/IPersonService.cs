using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Services
{
    public interface IPersonService
    { 
        Task<IEnumerable<Person>> GetAllPersonsAsync();

        Task<Person?> GetPersonByIdAsync(int id);

        Task<Person?> GetPersonByIdNumberAsync(string idNumber);
        
        Task<IEnumerable<Person>> SearchPersonsBySurnameAsync(string surname); // Search for a person using their surname


        Task<bool> CreatePersonAsync(Person person);
		
        Task<bool> UpdatePersonAsync(Person person);

        
        Task<bool> DeletePersonAsync(int id);

        
        Task<bool> CanDeletePersonAsync(int id); // Checking if person can be deleted

         
        Task<bool> IdNumberExistsAsync(string idNumber, int? excludePersonId = null); // Checking if ID Number already exists
    }
}