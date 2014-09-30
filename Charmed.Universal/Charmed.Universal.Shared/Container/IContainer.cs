using System;

namespace Charmed.Container
{
	public interface IContainer
	{
		void Register<TClass>();
		void Register<TService, TClass>() where TClass : TService;
		void RegisterInstance<TService>(TService instance);

		TService Resolve<TService>();
		object Resolve(Type type);
	}
}
