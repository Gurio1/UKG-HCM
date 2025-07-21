using HCM.Domain.Persons;
using HCM.Infrastructure;
using HCM.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace HCM.Shared.Persons;

public sealed class PersonCreator
{
    private readonly ApplicationDbContext dbContext;
    private readonly ILogger<PersonCreator> logger;
    
    public PersonCreator(ApplicationDbContext dbContext, ILogger<PersonCreator> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }
    
    public async Task<Result<Person>> Create(Person person, CancellationToken cancellationToken)
    {
        dbContext.Persons.Add(person);
        
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<Person>.Success(person);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "An error occurred while creating a person.");
            return Result<Person>.Failure("An error occurred while creating the user");
        }
    }
}
