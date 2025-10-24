using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly AppDbContext _context;
    
    public SubjectRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> ExistsById(int id)
    {
        return await _context.Subjects.AnyAsync(u => u.Id == id);
    }
}