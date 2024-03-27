using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TagList.Model;
using TagList;

namespace TagProjectTests;

public class TagFromDatabaseControllerTests
{
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        var appFactory = new WebApplicationFactory<Startup>();
        _client = appFactory.CreateClient();
    }

    [Test]
    public async Task GetTags_WithPagination_ReturnsPaginatedData()
    {
        // Arrange
        var page = 1;
        var pageSize = 10;

        // Act
        var response = await _client.GetAsync($"/TagFromDatabase?page={page}&pageSize={pageSize}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        var tags = JsonConvert.DeserializeObject<List<Tag>>(content);
        Assert.IsNotNull(tags);
        Assert.AreEqual(pageSize, tags.Count);
    }
}
