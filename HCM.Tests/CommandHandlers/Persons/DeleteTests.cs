using HCM.Features.Persons.Delete;
using Microsoft.Extensions.Logging.Abstractions;

namespace HCM.Tests.CommandHandlers.Persons;

public sealed class DeleteTests
{
    [Fact]
    public async Task DeletePerson_ReturnsNotFound_WhenMissing()
    {
        await using var context = App.CreateContext();
        var handler = new DeletePersonCommandHandler(context, NullLogger<DeletePersonCommandHandler>.Instance);

        var result = await handler.Handle(new DeletePersonCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(404, result.Error.Code);
    }
}