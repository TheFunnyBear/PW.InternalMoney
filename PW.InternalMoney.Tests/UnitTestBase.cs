using Microsoft.Practices.Unity;
using SpecFlow.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace PW.InternalMoney.Tests
{

    public static class TestDependencies
    {
        [ScenarioDependencies]
        public static IUnityContainer CreateContainer()
        {
            // create container with the runtime dependencies
            var container = Dependencies.CreateContainer();

            // Registers the build steps, this gives us dependency resolution using the container.
            // NB If you need named parameters into the steps you should override specific registrations
            container.RegisterTypes(typeof(TestDependencies).Assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(BindingAttribute))),
                                    WithMappings.FromMatchingInterface,
                                    WithName.Default,
                                    WithLifetime.ContainerControlled);

            return container;
        }
    }

    public static class Dependencies
    {
        public static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<ITestsConstants, TestsConstants>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILinkCliker, LinkCliker>(new ContainerControlledLifetimeManager());
            container.RegisterType<IBrowserNavigator, BrowserNavigator>(new ContainerControlledLifetimeManager());
            container.RegisterType<IButtonCliker, ButtonCliker>(new ContainerControlledLifetimeManager());
            return container;
        }
    }

    public abstract class UnitTestBase
    {

        static public IUnityContainer Container { get; set; }

        public UnitTestBase()
        {
        }

        public virtual void TestInitialize()
        {
            Container = TestDependencies.CreateContainer();
            Container.Resolve<BrowserNavigator>();
            Container.Resolve<TestsConstants>();
            Container.Resolve<LinkCliker>();
            Container.Resolve<ButtonCliker>();
        }

        public virtual void TestCleanup()
        {

        }

    }
}
