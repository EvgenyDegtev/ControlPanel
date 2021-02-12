using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Routing;
using Moq;
using System.Reflection;
using ControlPanel.Models;
using ControlPanel.Controllers;
using ControlPanel.Abstract;
using System.Linq;
using ControlPanel.ViewModels;
using System.Web.Mvc;
using System.Threading.Tasks;
using PagedList;

namespace ControlPanel.UnitTests
{
    [TestClass]
    public class AgentTests
    {
        [TestMethod]
        public async Task Can_Paginate()
        {
            //Arrange
            Mock<IAgentRepository> mock = new Mock<IAgentRepository>();

            mock.Setup(m => m.Agents).Returns(new Agent[]
                {
                    new Agent {Id=1, Login="Login1", Name="Agent1" },
                    new Agent {Id=2, Login="Login2", Name="Agent2" },
                    new Agent {Id=3, Login="Login3", Name="Agent3" },
                    new Agent {Id=4, Login="Login4", Name="Agent4" },
                    new Agent {Id=5, Login="Login5", Name="Agent5" },
                    new Agent {Id=6, Login="Login6", Name="Agent6" },
                    new Agent {Id=7, Login="Login7", Name="Agent7" }
                }.AsQueryable());

            AgentsController controller = new AgentsController(mock.Object);

            AgentsIndexViewModel agentsIndexModel = new AgentsIndexViewModel
            {
                SelectedSortProperty = "Login",
                SortOrder="asc",
                Page = 2
            };
            //Act
            //AgentsIndexViewModel result = (AgentsIndexViewModel)(await controller.Index(agentsIndexModel)).Model;

            var qq = await controller.Index(agentsIndexModel) as ViewResult;
            AgentsIndexViewModel result = (AgentsIndexViewModel)qq.Model;

            //Accert
            int agentCount = result.PagedAgents.Count;
            Assert.IsTrue(agentCount == 2);
        }
    }
}
