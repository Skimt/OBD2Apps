using Management.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Pages.Inherited
{
	public class ManagementBase : ComponentBase
	{

		/// <summary>
		/// Create new model, set 'marked as' type and returns it. 
		/// </summary>
		public T SetMarkedAs<T>(MarkedType type) where T : IModel, new()
		{
			T model = new T
			{
				Marked = type
			};
			return model;
		}

		/// <summary>
		/// Set the model's 'marked as' type and returns it. 
		/// </summary>
		public T SetMarkedAs<T>(T model, MarkedType type) where T : IModel
		{
			model.Marked = type;
			return model;
		}

	}
}
