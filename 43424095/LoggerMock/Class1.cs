using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PosInformatique.Logging.Assertions;
using Xunit;

namespace LoggerMock
{
    public class Class1
    {

    }

    public class BlogController : Controller
    {
        private IDAO<Blog> _blogDAO;
        private readonly ILogger<BlogController> _logger;

        public BlogController(ILogger<BlogController> logger, IDAO<Blog> blogDAO)
        {
            this._blogDAO = blogDAO;
            this._logger = logger;
        }
        public IActionResult Index()
        {
            var blogs = this._blogDAO.GetMany();
            this._logger.LogInformation("Index page say hello", new object[0]);
            return View(blogs);
        }
    }

    public interface IDAO<T>
    {
        IReadOnlyList<Blog> GetMany(string id = null);
    }

    public class Blog
    {

    }

public class BlogControllerTest
{
    [Fact]
    public void Index_ReturnAViewResult_WithAListOfBlog()
    {
        var mockRepo = new Mock<IDAO<Blog>>();
        mockRepo.Setup(repo => repo.GetMany(null)).Returns(GetListBlog());

        var logger = new LoggerMock<BlogController>();
        logger.SetupSequence()
            .LogInformation("Index page say hello");

        var controller = new BlogController(logger.Object, mockRepo.Object);

        var result = controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Blog>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());

        logger.VerifyLogs();
    }
}
}