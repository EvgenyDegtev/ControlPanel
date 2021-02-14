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
using System.Collections.Generic;

namespace ControlPanel.UnitTests
{
    [TestClass]
    public class AgentTests
    {
        [TestMethod]
        public async Task CanPaginate()
        {
            //Arrange
            Mock<IAgentRepository> mock = new Mock<IAgentRepository>();

            mock.Setup(agentsRepo => agentsRepo.GetAgentsIncludeGroupAsync()).ReturnsAsync(new List<Agent>
                {
                    new Agent {Id=1, Login="Login1", Name="Agent1" },
                    new Agent {Id=2, Login="Login2", Name="Agent2" },
                    new Agent {Id=3, Login="Login3", Name="Agent3" },
                    new Agent {Id=4, Login="Login4", Name="Agent4" },
                    new Agent {Id=5, Login="Login5", Name="Agent5" },
                    new Agent {Id=6, Login="Login6", Name="Agent6" },
                    new Agent {Id=7, Login="Login7", Name="Agent7" }
                });

            mock.Setup(agentsRepo => agentsRepo.GetGroupsAsync()).ReturnsAsync(new List<Group>
                {
                    new Group {Id=1, Name="Group1" },
                    new Group {Id=2, Name="Group2" }
                });

            AgentsController controller = new AgentsController(mock.Object);

            AgentsIndexViewModel agentsIndexModel = new AgentsIndexViewModel
            {
                Page = 2
            };


            //Act
            var resultView = await controller.Index(agentsIndexModel) as ViewResult;
            AgentsIndexViewModel result = (AgentsIndexViewModel)resultView.Model;

            //Accert
            Assert.IsTrue(result.PagedAgents.Count == 2);
            Assert.AreEqual("Login6", result.PagedAgents.ToList()[0].Login);
            Assert.AreEqual("Agent7", result.PagedAgents.ToList()[1].Name);
        }
    }
}
