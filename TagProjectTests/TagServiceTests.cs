using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TagList.DatabaseConnector;
using TagList.Model;
using TagList.Repositories.TagRepo;
using TagList.Services;

namespace TagProjectTests
{
    public class TagServiceTests
    {
        private TagService _tagService;
        private DatabaseContext _dbContext;
        private Mock<ITagRepository> _tagRepositoryMock;
        private Mock<ILogger<TagService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            _dbContext = new DatabaseContext(options);
            _tagRepositoryMock = new Mock<ITagRepository>();
            _loggerMock = new Mock<ILogger<TagService>>();
            _tagService = new TagService(_tagRepositoryMock.Object, _loggerMock.Object);
            AddTagsToDbContext();
        }

        private async Task AddTagsToDbContext()
        {
            var tags = new List<Tag>
            {
                new Tag { Id = 1, Name = "tag1", HasSynonyms = false, IsModeratorOnly = false, Count = 5.42, IsRequired = false },
                new Tag { Id = 2, Name = "tag2", HasSynonyms = false, IsModeratorOnly = false, Count = 5.2, IsRequired = false },
                new Tag { Id = 3, Name = "tag3", HasSynonyms = false, IsModeratorOnly = false, Count = 5.12, IsRequired = false }
            };

            _dbContext.Tags.AddRange(tags);
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task GetAllTags_ReturnsTags()
        {

            // Arrange
            _tagRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_dbContext.Tags.ToList());

            // Act
            var result = await _tagService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task AddNoExistElementToDb_ReturnsTrue()
        {

            var tag = new Tag {Name = "tag5", HasSynonyms = false, IsModeratorOnly = true, Count = 5.12, IsRequired = false };
            _tagRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Tag>())).ReturnsAsync(true);

            // Act
            var result = await _tagService.AddAsync(tag);

            // Assert
            _tagRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Tag>()), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AddExistingElementToDb_ThrowsException_ReturnsFalse()
        {
            var tag = new Tag { Name = "tag5", HasSynonyms = false, IsModeratorOnly = false, Count = 5.12, IsRequired = false };
            _tagRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Tag>())).ThrowsAsync(new Exception("Some error message"));

            var result = await _tagService.AddAsync(tag);

            Assert.IsFalse(result);

        }

        [Test]
        public async Task DeleteExistingElementFromDb_ReturnsTrue()
        {

            var tagToDelete = _dbContext.Tags.First();
            _tagRepositoryMock.Setup(repo => repo.DeleteAsync(tagToDelete)).ReturnsAsync(true);


            var result = await _tagService.DeleteAsync(tagToDelete);


            _tagRepositoryMock.Verify(repo => repo.DeleteAsync(tagToDelete), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetByNameAsync_ExistingName_ReturnsTag()
        {

            var tagName = "tag1";
            var expectedTag = _dbContext.Tags.First(t => t.Name == tagName);
            _tagRepositoryMock.Setup(repo => repo.GetByNameAsync(tagName)).ReturnsAsync(expectedTag);


            var result = await _tagService.GetByNameAsync(tagName);


            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTag, result);
        }

        [Test]
        public async Task GetByNameAsync_NoExistingName_ReturnsNull()
        {

            var tagName = "nonexistingtag";
            _tagRepositoryMock.Setup(repo => repo.GetByNameAsync(tagName)).ReturnsAsync((Tag)null);


            var result = await _tagService.GetByNameAsync(tagName);


            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateExistingElementInDb_ReturnsTrue()
        {

            var tagToUpdate = _dbContext.Tags.First();
            _tagRepositoryMock.Setup(repo => repo.UpdateAsync(tagToUpdate)).ReturnsAsync(true);


            var result = await _tagService.UpdateAsync(tagToUpdate);


            _tagRepositoryMock.Verify(repo => repo.UpdateAsync(tagToUpdate), Times.Once);
            Assert.IsTrue(result);
        }

    }
}
