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
            
            routes.MapRoute(
                name: "EditAgentSkill",
                url: "Agents/{agentId}/EditSkill/{skillId}",
                new { controller="Agents", action = "EditSkill" }
                );

            routes.MapRoute(
                name: "RemoveAgentSkill",
                url: "Agents/{agentId}/RemoveSkill/{skillId}",
                new { controller = "Agents", action = "EditSkill" }
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

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Agents", action = "Index", id = UrlParameter.Optional }
            );           
        }
    }
}
