using Charmed.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charmed.Container
{
	public sealed class Ioc
	{
		private static IContainer _container;
		private static object _lock = new object();

		public static IContainer Container
		{
			get
			{
				if (_container == null)
				{
					lock (_lock)
					{
						if (_container == null)
						{
							_container = new SimpleContainer();
						}
					}
				}

				return _container;
			}
			set
			{
				lock (_lock)
				{
					_container = value;
				}
			}
		}
	}
}
