using FileManagerApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FileManagerApi.Test
{
    public class FilesControllerTest
    {
        [Fact]
        public void GetJsonTest()
        {
            // Arrange
            var controller = new FilesController();
            // Act
            var result = controller.Get();
            // Assert
            Assert.IsAssignableFrom<JsonResult>(result);
        }

        [Fact]
        public void GetNotFoundTest()
        {
            // Arrange
            var controller = new FilesController();
            // Act
            var result = controller.Get();
            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }
    }
}