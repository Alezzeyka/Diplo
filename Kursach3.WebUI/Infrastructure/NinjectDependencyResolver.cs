using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;
using Kursach3Domain.Concrete;
using Kursach3.WebUI.Infrastructure.Abstract;
using Kursach3.WebUI.Infrastructure.Concrete;
namespace Kursach3.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<ITestRepository>().To<EFTestRepository>();
            kernel.Bind<IQuestionRepository>().To<EFQuestionRepository>();
            kernel.Bind<IAnswersRepository>().To<EFAnswerRepository>();
            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }
    }
}