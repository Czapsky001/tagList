using Microsoft.AspNetCore.Mvc.Testing;
namespace TagProjectTests;


public class TagApiControllerTests
{
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        var factory = new WebApplicationFactory<TagList.Startup>();
        _client = factory.CreateClient();
    }

    [Test]
    public async Task GetAllTagsFromApi_ReturnsOkResult()
    {
        // Act
        var response = await _client.GetAsync("/TagApi");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Test]
    public async Task RefreshTagsFromApi_ReturnsOkResult()
    {
        // Act
        var response = await _client.PostAsync("/TagApi/refresh", new StringContent(""));

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}
