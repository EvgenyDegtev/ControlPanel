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
            //TestRouteMatch("~/?page=2", "Agents", "Index");
            TestRouteMatch("~/Agents/Index/?page=2", "Agents", "Index");
            TestRouteMatch("~/Agents/Index/?page=2&searchString=1", "Agents", "Index");
            TestRouteMatch("~/Agents/Index", "Agents", "Index");
            TestRouteMatch("~/Agents/Create","Agents","Create");
            TestRouteMatch("~/Agents/Edit/29", "Agents", "Edit");
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

        }

        [TestMethod]
        public void TestGroupIncomingRoutes()
        {

        }

        [TestMethod]
        public void TestRouteIncomingRoutes()
        {

        }
    }
}
