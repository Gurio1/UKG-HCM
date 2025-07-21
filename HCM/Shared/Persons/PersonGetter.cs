using HCM.Domain.Persons;
using HCM.Infrastructure;
using HCM.Shared.Results;
using Microsoft.EntityFrameworkCore;


//Actually i dont think that i have to extract this logic,because for update i need to check only target department,
//but i dont have time to polish the project
namespace HCM.Shared.Persons;

public sealed class PersonGetter
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<PersonGetter> logger;
    
    public PersonGetter(ApplicationDbContext context, ILogger<PersonGetter> logger)
    {
        this.context = context;
        this.logger = logger;
    }
    
    public async Task<Result<Person>> GetByIdAsync(Guid personId, CancellationToken cancellationToken = default)
    {
        try
        {
            var person = await context.Persons.AsNoTracking().FirstOrDefaultAsync(p => p.Id == personId, cancellationToken);
            return person is null
                ? Result<Person>.NotFound("Person not found")
                : Result<Person>.Success(person);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting person by id");
            return Result<Person>.Failure("Failed to retrieve person");
        }
    }
}
