using TagList.Model;

namespace TagList.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<bool> AddAsync(Tag tag);
        Task<bool> DeleteAsync(Tag tag);
        Task<Tag> GetByNameAsync(string name);
        Task<bool> UpdateAsync(Tag tag);
    }
}
