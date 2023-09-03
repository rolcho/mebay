using MeBay.Models;
using MeBay.Test;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MyApp.Tests
{
    [TestFixture]
    public class UserEndpointsTestsAsAdmin
    {
        private FactoryOverride _factory;
        private User _defaultUser;
        private User _defaultAdmin;

        [SetUp]
        public void Setup()
        {
            _factory = new FactoryOverride();
            _defaultUser = new User
            {
                Name = "Tim Apple",
                Password = "TimsSecret123",
                Email = "tim@apple.com",
                Role = "User"
            };
            _defaultAdmin = new User
            {
                Name = "Bill Microsoft",
                Password = "Gates123",
                Email = "gates@microsoft.com",
                Role = "User"
            };
        }

        [Test]
        public async Task FirstRegistration_IsAdmin()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            var login = await client.PostAsJsonAsync("user/login", _defaultAdmin);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.GetAsync("user/1");
            var content = await response.Content.ReadAsStringAsync();
            var userDto = JsonConvert.DeserializeObject<UserResponseDto>(content);
            // Assert
            Assert.That(userDto!.Role, Is.EqualTo("Admin"));
        }

        [Test]
        public async Task AdminGetAllUser_ReturnsUserList()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultAdmin);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.GetAsync("/user/list");
            var content = await response.Content.ReadAsStringAsync();
            var userList = JsonConvert.DeserializeObject<List<UserResponseDto>>(content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(userList!, Has.Count.EqualTo(2));
            ;
        }

        [Test]
        public async Task AdminGetAnotherUserById_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultAdmin);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.GetAsync("/user/2");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task AdminUpdatesAnotherUserBy_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultAdmin);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var user = _defaultUser;
            user.Name = "XXXX";
            var response = await client.PutAsJsonAsync("/user/2", user);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task AdminDeleteAnotherUser_ReturnsNoContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultAdmin);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.DeleteAsync("/user/2");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
