namespace TagList.Model;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool HasSynonyms { get; set; }
    public bool IsModeratorOnly { get; set; }
    public double Count { get; set; }
    public bool IsRequired { get; set; }
}
