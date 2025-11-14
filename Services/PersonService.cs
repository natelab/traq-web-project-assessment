using Microsoft.EntityFrameworkCore;
using traq_web_project_assessment.Data;
using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _context;

        public PersonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            return await _context.Persons
                .Include(p => p.Accounts)
                .ThenInclude(a => a.Status)
                .OrderBy(p => p.Surname)
                .ToListAsync();
        }

        public async Task<Person?> GetPersonByIdAsync(int id)
        {
            return await _context.Persons
                .Include(p => p.Accounts)
                .ThenInclude(a => a.Status)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Person?> GetPersonByIdNumberAsync(string idNumber)
        {
            return await _context.Persons
                .Include(p => p.Accounts)
                .FirstOrDefaultAsync(p => p.IdNumber == idNumber);
        }

        public async Task<IEnumerable<Person>> SearchPersonsBySurnameAsync(string surname)
        {
            return await _context.Persons
                .Include(p => p.Accounts)
                .Where(p => p.Surname.Contains(surname))
                .OrderBy(p => p.Surname)
                .ToListAsync();
        }

        public async Task<bool> CreatePersonAsync(Person person)
        {
            try
            {
                
                if (await IdNumberExistsAsync(person.IdNumber)) // Checking if ID Number already exists
                {
                    return false;
                }

                _context.Persons.Add(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdatePersonAsync(Person person)
        {
            try
            {
                // Check if ID Number already exists for another person
                if (await IdNumberExistsAsync(person.IdNumber, person.Id))
                {
                    return false;
                }

                _context.Persons.Update(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            try
            {
                // Check if person can be deleted
                if (!await CanDeletePersonAsync(id))
                {
                    return false;
                }

                var person = await _context.Persons
                    .Include(p => p.Accounts) // Important: Include accounts
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (person == null)
                {
                    return false;
                }

                // If person has closed accounts, delete them first
                if (person.Accounts.Any())
                {
                    _context.Accounts.RemoveRange(person.Accounts);
                }

                // Then delete the person
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the error 
                return false;
            }
        }

        //Deletion can only happen if there are no accounts or
        //All accounts have been closed
        public async Task<bool> CanDeletePersonAsync(int id)
        {
            var person = await _context.Persons
                .Include(p => p.Accounts)
                .ThenInclude(a => a.Status)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                return false;
            }

            // Can delete if no accounts exist
            if (!person.Accounts.Any())
            {
                return true;
            }

            // Can delete if ALL the accounts have a StatusId equal to 2 (Closed)
            bool allAccountsClosed = person.Accounts.All(a => a.StatusId == 2);

            return allAccountsClosed;
        }

        public async Task<bool> IdNumberExistsAsync(string idNumber, int? excludePersonId = null)
        {
            if (excludePersonId.HasValue)
            {
                return await _context.Persons
                    .AnyAsync(p => p.IdNumber == idNumber && p.Id != excludePersonId.Value);
            }

            return await _context.Persons
                .AnyAsync(p => p.IdNumber == idNumber);
        }
    }
}