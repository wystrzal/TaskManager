using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TaskManager.API.Helpers.GenerateToken;
using TaskManager.API.Model;
using Xunit;

namespace TaskManager.API_Test
{
    public class TokenGeneratorTest
    {
        [Fact]
        public void GenerateJwtTokenTest()
        {
            //Arrange
            var configurationSection = new Mock<IConfigurationSection>();
            var configMock = new Mock<IConfiguration>();
            var user = new User { UserName = "test", Nickname = "test", Id = 1 };

            configurationSection.Setup(a => a.Value).Returns("VeryLongKeyForTest");
            configMock.Setup(a => a.GetSection("AppSettings:Token")).Returns(configurationSection.Object);

            //Act
            var act = TokenGenerator.GenerateJwtToken(user, configMock.Object);

            //Arrange
            Assert.NotNull(act);
        }
    }
}
