﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using ControlPanel.Abstract;
using ControlPanel.Concrete;
using ControlPanel.Infastructure.Abstract;
using ControlPanel.Infastructure.Concrete;

namespace ControlPanel.Infastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance (RequestContext requestContext, Type controllerType)
        {
            IController controllerInstance = (controllerType == null) ? null : (IController)ninjectKernel.Get(controllerType);
            return controllerInstance;
        }

        public void AddBindings ()
        {
            ninjectKernel.Bind<ISkillRepository>().To<EFSkillRepository>();
            ninjectKernel.Bind<IRouteRepository>().To<EFRouteRepository>();
            ninjectKernel.Bind<IAgentRepository>().To<EFAgentRepository>();
            ninjectKernel.Bind<IGroupRepository>().To<EFGroupRepository>();
            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }

    }
}