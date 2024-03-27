using TagList.Model;

namespace TagList.Repositories.TagRepo
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<bool> AddAsync(Tag tag);
        Task<bool> DeleteAsync(Tag tag);
        Task<Tag> GetByNameAsync(string name);
        Task<bool> UpdateAsync(Tag tag);

    }
}
