using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Persistance;

namespace Notes.Tests.Common
{
    public class NotesContextFactory
    {
        public static Guid UserAId = Guid.NewGuid();
        public static Guid UserBId = Guid.NewGuid();

        public static Guid NoteIdForDelete = Guid.NewGuid();
        public static Guid NoteIdForUpdate = Guid.NewGuid();

        public static NotesDbContext Create() 
        { 
            var options = new DbContextOptionsBuilder<NotesDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new NotesDbContext(options);
            context.Database.EnsureCreated();
            context.Notes.AddRange(
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details1",
                    EditDate = null,
                    Id = Guid.Parse("9D7346DB-7264-4C5A-BDF4-763FEB3D75E1"),
                    Title = "Title1",
                    UserId = UserAId
                },
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details2",
                    EditDate = null,
                    Id = Guid.Parse("386512FD-1DE9-419E-A7DC-D808D60B1554"),
                    Title = "Title2",
                    UserId = UserBId
                },
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details3",
                    EditDate = null,
                    Id = NoteIdForDelete,
                    Title = "Title3",
                    UserId = UserAId
                }, 
                new Note
                {
                    CreationDate = DateTime.Today,
                    Details = "Details4",
                    EditDate = null,
                    Id = NoteIdForUpdate,
                    Title = "Title4",
                    UserId = UserBId
                });
            context.SaveChanges();
            return context;
        }

        public static void Destroy(NotesDbContext context) 
        { 
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
