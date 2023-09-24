using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fileupload.Data;
using File = fileupload.Models.File;

public class FileRepository
{
    private readonly ApplicationDbContext _context;

    public FileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddFileAsync(fileupload.Models.File file)
    {
        _context.Files.Add(file);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<File> GetFiles()
    {
        return _context.Files.ToList();
    }
}