using Microsoft.EntityFrameworkCore;
using TagList.DatabaseConnector;
using TagList.Model;

namespace TagList.Repositories.TagRepo
{
    public class TagRepository : ITagRepository
    {
        private readonly DatabaseContext dbContext;

        private readonly ILogger<TagRepository> logger;

        public TagRepository(DatabaseContext dbContext, ILogger<TagRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public async Task<bool> AddAsync(Tag tag)
        {
            try
            {
                await dbContext.AddAsync(tag);
                await dbContext.SaveChangesAsync();
                return true;

            }catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Tag tag)
        {
            try
            {
                dbContext.Remove(tag);
                await dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            try
            {
                return await dbContext.Tags.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new List<Tag>();
            }
        }


        public async Task<Tag> GetByNameAsync(string name)
        {
            try
            {
                return await dbContext.Tags.FirstOrDefaultAsync(t => t.Name == name);
            }catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Tag tag)
        {
            try
            {
                dbContext.Tags.Update(tag);
                await dbContext.SaveChangesAsync();
                return true;

            }catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
