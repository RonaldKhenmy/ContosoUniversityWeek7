using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using ContosoUniversityIntegrationTest;
using ContosoUniversity;

namespace ContosoUniversityIntegrationTest.IntegrationTests
{
    public class BasicTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public BasicTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Students")]
        [InlineData("/About")]
        [InlineData("/Privacy")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)

        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.NotNull(response);
            response.EnsureSuccessStatusCode(); //Status Code 200-299, probably.
            Assert.NotNull(response.Content?.Headers?.ContentType);
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}
