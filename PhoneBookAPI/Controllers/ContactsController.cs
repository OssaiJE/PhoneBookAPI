using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.Data;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactAPIDbContext dbContext;

        public ContactsController(ContactAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostContacts(AddContactRequest addContactRequest)
        {
            var contact = new ContactModel()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                PhoneNumber = addContactRequest.PhoneNumber
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            return Ok(contact);
        }

        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.SingleOrDefaultAsync(x => x.Id == id);

            if (contact == null)
            {
                return NotFound();
            }
            contact.Id = id;
            contact.Address = updateContactRequest.Address;
            contact.Email = updateContactRequest.Email;
            contact.FullName = updateContactRequest.FullName;
            contact.PhoneNumber = updateContactRequest.PhoneNumber;

            await dbContext.SaveChangesAsync();

            return Ok(contact);

        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FirstOrDefaultAsync(x => x.Id == id);
            if (contact == null)
            {
                return NotFound($"Contact with the ID {id} does not exist");
            }

            dbContext.Contacts.Remove(contact);
            await dbContext.SaveChangesAsync();

            
            return Ok(contact);
        }
    }
}
