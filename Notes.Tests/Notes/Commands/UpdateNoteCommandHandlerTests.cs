using Microsoft.EntityFrameworkCore;
using Notes.Application.Common.Exceptions;
using Notes.Application.Notes.Commands.UpdateNote;
using Notes.Tests.Common;
using Xunit;

namespace Notes.Tests.Notes.Commands
{
    public class UpdateNoteCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UpdateNoteCommandHadnler_Success() 
        {
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);
            var updatedTitle = "new title";

            // Act
            await handler.Handle(new UpdateNoteCommand
            {
                Id = NotesContextFactory.NoteIdForUpdate,
                UserId = NotesContextFactory.UserBId,
                Title = updatedTitle
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(await Context.Notes.SingleOrDefaultAsync(note =>
                note.Id == NotesContextFactory.NoteIdForUpdate &&
                note.Title == updatedTitle));
        }

        [Fact]
        public async Task UpdateNoteCommandHadler_FailOnWrongId() 
        {
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await  handler.Handle(new UpdateNoteCommand 
                { 
                    Id = Guid.NewGuid(),
                    UserId = NotesContextFactory.UserAId,
                },
                CancellationToken.None));
        }

        [Fact]
        public async Task UpdateNoteCommandHadler_FailOnWrondUserId() 
        { 
            // Arrange
            var handler = new UpdateNoteCommandHandler(Context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(new UpdateNoteCommand
                {
                    Id = NotesContextFactory.NoteIdForUpdate,
                    UserId = NotesContextFactory.UserAId, // не new Guid.NewGuid() ?
                },
                CancellationToken.None));
        }
    }
}
