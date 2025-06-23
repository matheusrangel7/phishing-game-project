using Game.API.Data;
using Game.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Game.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmailsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Email>>> GetEmails()
    {
        return await _context.Emails.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Email>> GetEmail(int id)
    {
        var email = await _context.Emails.FindAsync(id);

        if (email == null)
        {
            return NotFound();
        }

        return email;
    }
}