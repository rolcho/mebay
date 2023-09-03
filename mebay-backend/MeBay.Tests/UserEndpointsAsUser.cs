using MeBay.Models;
using MeBay.Test;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MyApp.Tests
{
    [TestFixture]
    public class UserEndpointsTestsAsUser
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
        public async Task RegisterWithMissingData_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.CreateClient();
            _defaultUser.Email = null!;

            // Act
            var response = await client.PostAsJsonAsync("user/register", _defaultUser);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task RegisterWithExistingEmail_ReturnsConflict()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultUser);

            // Act
            var registerAgain = await client.PostAsJsonAsync("user/register", _defaultUser);

            // Assert
            Assert.That(registerAgain.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }

        [Test]
        public async Task RegisterWithAllData_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("user/register", _defaultUser);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public async Task LoginWithWrongUserEmail_ReturnsNotfound()
        {
            // Arrange
            var client = _factory.CreateClient();
            var user = new { Name = "Tim Apple", Password = "TimsSecret123", };

            // Act
            var response = await client.PostAsJsonAsync("user/register", user);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task LoginWithWrongPassword_ReturnsUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();

            await client.PostAsJsonAsync("user/register", _defaultUser);
            var user = _defaultUser;
            user.Password = "xxxxxx";
            // Act
            var response = await client.PostAsJsonAsync("user/login", user);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task LoginWithValidUser_ReturnsToken()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultUser);

            // Act
            var response = await client.PostAsJsonAsync("user/login", _defaultUser);
            var responseObject = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseObject!.Token, Is.Not.Null);
        }

        [Test]
        public async Task UserGetAllUser_ReturnsForbidden()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.GetAsync("/user/list");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task UserGetAnotherUserById_ReturnsForbidden()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.GetAsync("/user/1");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task UserGetUserById_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
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
        public async Task UserUpdatesAnotherUserBy_ReturnsForbidden()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.PutAsJsonAsync("/user/1", _defaultUser);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task UserUpdatesInvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.PutAsJsonAsync("/user/99", _defaultUser);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserUpdatesExistingEmail_ReturnsConflict()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            _defaultUser.Email = _defaultAdmin.Email;
            var response = await client.PutAsJsonAsync("/user/2", _defaultUser);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }

        [Test]
        public async Task UserDeleteSelf_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
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

        [Test]
        public async Task UserDeleteNoneExist_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.DeleteAsync("/user/3");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task UserDeleteAnotherUser_ReturnsForbidden()
        {
            // Arrange
            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("user/register", _defaultAdmin);
            await client.PostAsJsonAsync("user/register", _defaultUser);
            var login = await client.PostAsJsonAsync("user/login", _defaultUser);
            var loginResponse = await login.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                loginResponse!.Token
            );

            // Act
            var response = await client.DeleteAsync("/user/1");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
