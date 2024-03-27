using Azure;
using TagList.Model;
using TagList.Repositories.TagRepo;

namespace TagList.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository tagRepository;
        private readonly ILogger<TagService> logger;

        public TagService(ITagRepository tagRepository, ILogger<TagService> logger)
        {
            this.tagRepository = tagRepository;
            this.logger = logger;
        }
        public async Task<bool> AddAsync(Tag tag)
        {
            try
            {
                await tagRepository.AddAsync(tag);
                return true;
            }catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Tag tag)
        {
            try
            {
                await tagRepository.DeleteAsync(tag);
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
                return await tagRepository.GetAllAsync();
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
                return await tagRepository.GetByNameAsync(name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new Tag();
            }
        }

        public async Task<bool> UpdateAsync(Tag tag)
        {
            try
            {
                return await tagRepository.UpdateAsync(tag);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
