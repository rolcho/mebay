using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace MyApp.Tests
{
    public class UserEndpointsTests
    {
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [Test]
        public async Task Login_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/login", null);
            var content = await response.Content.ReadFromJsonAsync<dynamic>();

            // Assert
            Assert.That(response.IsSuccessStatusCode);
            Assert.That(content?.GetProperty("action").GetString(), Is.EqualTo("login"));
        }

        [Test]
        public async Task Register_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/register", null);
            var content = await response.Content.ReadFromJsonAsync<dynamic>();

            // Assert
            Assert.That(response.IsSuccessStatusCode);
            Assert.That(content?.GetProperty("action").GetString(), Is.EqualTo("register"));
        }

        [Test]
        public async Task ListUsers_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/users");
            var content = await response.Content.ReadFromJsonAsync<dynamic>();

            // Assert
            Assert.That(response.IsSuccessStatusCode);
            Assert.That(content?.GetProperty("action").GetString(), Is.EqualTo("list"));
        }

        [Test]
        public async Task GetUserById_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userId = 1;

            // Act
            var response = await client.GetAsync($"/user/{userId}");
            var content = await response.Content.ReadFromJsonAsync<dynamic>();

            // Assert
            Assert.That(response.IsSuccessStatusCode);
            Assert.That(content?.GetProperty("action").GetString(), Is.EqualTo("get"));
            Assert.That(content?.GetProperty("userId").GetInt32(), Is.EqualTo(userId));
        }

        [Test]
        public async Task GetUserById_WithoutId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/user");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UpdateUserById_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userId = 1;

            // Act
            var response = await client.PutAsync($"/user/{userId}", null);
            var content = await response.Content.ReadFromJsonAsync<dynamic>();

            // Assert
            Assert.That(response.IsSuccessStatusCode);
            Assert.That(content?.GetProperty("action").GetString(), Is.EqualTo("update"));
            Assert.That(content?.GetProperty("userId").GetInt32(), Is.EqualTo(userId));
        }

        [Test]
        public async Task UpdateUserById_WithoutId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PutAsync("/user", null);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task DeleteUserById_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            var userId = 1;

            // Act
            var response = await client.DeleteAsync($"/user/{userId}");
            var content = await response.Content.ReadFromJsonAsync<dynamic>();

            // Assert
            Assert.That(response.IsSuccessStatusCode);
            Assert.That(content?.GetProperty("action").GetString(), Is.EqualTo("delete"));
            Assert.That(content?.GetProperty("userId").GetInt32(), Is.EqualTo(userId));
        }

        [Test]
        public async Task DeleteUserById_WithoutId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync("/user");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
