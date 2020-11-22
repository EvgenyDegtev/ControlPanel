using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Routing;
using Moq;
using System.Reflection;

namespace ControlPanel.UnitTests
{
    [TestClass]
    public class RouteTests
    {
        private HttpContextBase CreateHttpContext (string targetUrl=null, string httpMethod="GET")
        {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath)
                .Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>()))
                .Returns<string>(s => s);

            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }

        private void TestRouteMatch(string url, string controller, string action, object routeProperties=null, string httpMethod="GET")
        {
            //Arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //Act
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result,controller,action,routeProperties));
        }

        private void TestRouteFail(string url)
        {
            //Arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //Act
            RouteData result = routes.GetRouteData(CreateHttpContext(url));

            //Assert
            Assert.IsTrue(result == null || result.Route == null);
        }

        private bool TestIncomingRouteResult(RouteData routeResult, string controller, string action, object propertySet=null)
        {
            Func<object, object, bool> valCompare = (v1, v2) =>
                {
                    return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
                };


            bool result = valCompare(routeResult.Values["controller"], controller)
                && valCompare(routeResult.Values["action"], action);

            if(propertySet!=null)
            {
                PropertyInfo[] propertyInfos = propertySet.GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if(!(routeResult.Values.ContainsKey(propertyInfo.Name)&&valCompare(routeResult.Values[propertyInfo.Name],propertyInfo.GetValue(propertySet,null))))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        [TestMethod]
        public void TestAgentsIncomingRoutes()
        {
            TestRouteMatch("~/", "Agents", "Index");
            TestRouteMatch("~/Agents", "Agents", "Index");
            //TestRouteMatch("~/?page=2", "Agents", "Index");
            TestRouteMatch("~/Agents/Index", "Agents", "Index");
            TestRouteMatch("~/Agents/Index/?page=2", "Agents", "Index");
            TestRouteMatch("~/Agents/Index/?page=2&searchString=1", "Agents", "Index");
            TestRouteMatch("~/Agents/Create","Agents","Create");
            TestRouteMatch("~/Agents/Edit/22", "Agents", "Edit");
            TestRouteMatch("~/Agents/Delete/29", "Agents", "Delete");
            TestRouteMatch("~/Agents/22/Skills", "Agents", "AgentSkills");
            TestRouteMatch("~/Agents/22/EditSkill/4", "Agents", "EditSkill");
            TestRouteMatch("~/Agents/22/RemoveSkill/4", "Agents", "RemoveSkill");
            TestRouteMatch("~/Agents/22/AddSkill", "Agents", "AddSkill");
            TestRouteMatch("~/Agents/22/AddSkill/4", "Agents", "SkillAddConfirmation");
        }

        [TestMethod]
        public void TestSkillIncomingRoutes()
        {
            TestRouteMatch("~/Skills", "Skills", "Index");
            TestRouteMatch("~/Skills/Index", "Skills", "Index");
            TestRouteMatch("~/Skills/Index/?page=2", "Skills", "Index");
            TestRouteMatch("~/Skills/Index/?page=2&searchString=1", "Skills", "Index");
            TestRouteMatch("~/Skills/Create", "Skills", "Create");
            TestRouteMatch("~/Skills/Edit/29", "Skills", "Edit");
            TestRouteMatch("~/Skills/Delete/29", "Skills", "Delete");
            TestRouteMatch("~/Skills/4/Routes", "Skills", "SkillRoutes");
        }

        [TestMethod]
        public void TestGroupIncomingRoutes()
        {
            TestRouteMatch("~/Groups", "Groups", "Index");
            TestRouteMatch("~/Groups/Index", "Groups", "Index");
            TestRouteMatch("~/Groups/Index/?page=2", "Groups", "Index");
            TestRouteMatch("~/Groups/Index/?page=2&searchString=1", "Groups", "Index");
            TestRouteMatch("~/Groups/Create", "Groups", "Create");
            TestRouteMatch("~/Groups/Edit/29", "Groups", "Edit");
            TestRouteMatch("~/Groups/Delete/29", "Groups", "Delete");
            TestRouteMatch("~/Groups/14/Agents", "Groups", "GroupAgents");
            TestRouteMatch("~/Groups/14/RemoveAgent/55", "Groups", "RemoveAgent");
        }

        [TestMethod]
        public void TestRouteIncomingRoutes()
        {
            TestRouteMatch("~/Routes", "Routes", "Index");
            TestRouteMatch("~/Routes/Index", "Routes", "Index");
            TestRouteMatch("~/Routes/Index/?page=2", "Routes", "Index");
            TestRouteMatch("~/Routes/Index/?page=2&searchString=1", "Routes", "Index");
            TestRouteMatch("~/Routes/Create", "Routes", "Create");
            TestRouteMatch("~/Routes/Edit/29", "Routes", "Edit");
            TestRouteMatch("~/Routes/Delete/29", "Routes", "Delete");
        }

        [TestMethod]
        public void TestReportIncomingRoutes()
        {
            TestRouteMatch("~/Reports", "Reports", "Index");
            TestRouteMatch("~/Reports/Index", "Reports", "Index");
        }
    }
}
