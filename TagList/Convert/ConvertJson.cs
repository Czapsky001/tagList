using System.Collections.Generic;
using System.Text.Json;
using TagList.Model;

namespace TagList.Convert
{
    public class ConvertJson : IConvertJson
    {
        public List<Tag> DeserializeObject(string data)
        {
            JsonElement root = JsonSerializer.Deserialize<JsonElement>(data);
            JsonElement items = root.GetProperty("items");
            List<Tag> tags = new List<Tag>();
            foreach (JsonElement item in items.EnumerateArray())
            {
                Tag tag = new Tag
                {
                    Name = item.GetProperty("name").GetString(),
                    HasSynonyms = item.GetProperty("has_synonyms").GetBoolean(),
                    IsModeratorOnly = item.GetProperty("is_moderator_only").GetBoolean(),
                    Count = item.GetProperty("count").GetInt32(),
                    IsRequired = item.GetProperty("is_required").GetBoolean()
                };
                tags.Add(tag);
            }
            return tags;
        }
    }
}
