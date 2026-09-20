using System;
using System.Collections.Generic;
using InventoryAPI.Helpers;
using InventoryAPI.Models;
using InventoryAPI.Repositories;
using InventoryAPI.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace InventoryAPI.Tests.Services
{
    public class UserServiceTests
    {
        private IConfiguration CreateJwtConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?> {
                { "Jwt:Key", "test_secret_which_is_long_enough" },
                { "Jwt:Issuer", "testissuer" },
                { "Jwt:Audience", "testaud" },
                { "Jwt:DurationInMinutes", "60" }
            };
            return new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }

        [Fact]
        public void Authenticate_ValidUser_ReturnsToken()
        {
            var userRepoMock = new Mock<IUserRepo>();
            var user = new User { Username = "testuser", Role = "Admin" };
            userRepoMock.Setup(r => r.GetUser("u", "p")).Returns(user);

            var configuration = CreateJwtConfiguration();
            var jwtHelper = new JwtHelper(configuration);

            var service = new UserService(userRepoMock.Object, jwtHelper);

            var response = service.Authenticate(new LoginRequest { Username = "u", Password = "p" });

            Assert.NotNull(response);
            Assert.False(string.IsNullOrWhiteSpace(response.Token));
            Assert.True(response.Expiration > DateTime.UtcNow);
            userRepoMock.Verify(r => r.GetUser("u", "p"), Times.Once);
        }

        [Fact]
        public void Authenticate_InvalidUser_ReturnsNull()
        {
            var userRepoMock = new Mock<IUserRepo>();
            userRepoMock.Setup(r => r.GetUser(It.IsAny<string>(), It.IsAny<string>())).Returns((User?)null);

            var configuration = CreateJwtConfiguration();
            var jwtHelper = new JwtHelper(configuration);
            var service = new UserService(userRepoMock.Object, jwtHelper);

            var response = service.Authenticate(new LoginRequest { Username = "no", Password = "no" });

            Assert.Null(response);
            userRepoMock.Verify(r => r.GetUser("no", "no"), Times.Once);
        }
    }
}
