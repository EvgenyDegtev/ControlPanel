using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ControlPanel
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //Agent Routes
            routes.MapRoute(
                name: "EditAgentSkill",
                url: "Agents/{agentId}/EditSkill/{skillId}",
                new { controller="Agents", action = "EditSkill" }
                );

            routes.MapRoute(
                name: "RemoveAgentSkill",
                url: "Agents/{agentId}/RemoveSkill/{skillId}",
                new { controller = "Agents", action = "RemoveSkill" }
                );

            routes.MapRoute(
                name: "AddAgentSkillConfirmation",
                url: "Agents/{agentId}/AddSkill/{skillId}",
                new { controller = "Agents", action = "SkillAddConfirmation" }
                );

            routes.MapRoute(
                name: "AddAgentSkill",
                url: "Agents/{id}/AddSkill",
                new { controller = "Agents", action = "AddSkill" }
                );

            routes.MapRoute(
                name: "AgentSkills",
                url: "{controller}/{id}/Skills",
                new { action = "AgentSkills" }
                );

            //Group Routes
            routes.MapRoute(
                name: "RemoveAgentFromGroup",
                url: "Groups/{groupId}/RemoveAgent/{agentId}",
                new { controller = "Groups", action = "RemoveAgent" }
                );

            routes.MapRoute(
                name: "GroupAgents",
                url: "{controller}/{id}/Agents",
                new { action = "GroupAgents" }
                );

            //Skill Routes
            routes.MapRoute(
                name: "RemoveRouteFromSkill",
                url: "Skills/{skillId}/RemoveRoute/{routeId}",
                new { controller="Skills", action="RemoveRoute" }
                );

            routes.MapRoute(
                name: "SkillRoutes",
                url: "{controller}/{id}/Routes",
                new { action = "SkillRoutes" }
                );

            //Default Route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Agents", action = "Index", id = UrlParameter.Optional }
            );           
        }
    }
}
