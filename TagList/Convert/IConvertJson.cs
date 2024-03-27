using TagList.Model;

namespace TagList.Convert
{
    public interface IConvertJson
    {
        public List<Tag> DeserializeObject(string data);
    }
}
