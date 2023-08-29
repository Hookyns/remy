using RJDev.Tyml.Core.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RJDev.Tyml.Core
{
	public class TymlContextBuilder
	{
		private readonly List<Type> taskTypes = new();
		private string? workingDirectory;
		private readonly Dictionary<string, object> baseVariables = new();

		/// <summary>
		/// Return instance of TymlContext.
		/// </summary>
		/// <returns></returns>
		public TymlContext Build()
		{
			if (workingDirectory == null)
			{
				throw new InvalidOperationException($"Working directory not specified in {nameof(TymlContext)}");
			}

			Dictionary<string, TaskInfo> tasks = GetTasks(taskTypes);
			return new TymlContext(tasks, workingDirectory, baseVariables);
		}

		/// <summary>
		/// Add task allowed for execution.
		/// </summary>
		/// <param name="taskType"></param>
		/// <returns></returns>
		public TymlContextBuilder AddTask(Type taskType)
		{
			taskTypes.Add(taskType);
			return this;
		}

		/// <summary>
		/// Add tasks allowed for execution.
		/// </summary>
		/// <param name="taskTypes"></param>
		/// <returns></returns>
		// ReSharper disable once ParameterHidesMember
		public TymlContextBuilder AddTasks(params Type[] taskTypes)
		{
			this.taskTypes.AddRange(taskTypes);
			return this;
		}

		/// <summary>
		/// Set working directory of processing context.
		/// </summary>
		/// <param name="workingDirectory"></param>
		/// <returns></returns>
		// ReSharper disable once ParameterHidesMember
		public TymlContextBuilder UseWorkingDirectory(string workingDirectory)
		{
			this.workingDirectory = workingDirectory;
			return this;
		}

		/// <summary>
		/// Set base variable.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <remarks>Replace existing variable or add new variable.</remarks>
		public TymlContextBuilder WithBaseVariable(string name, object value)
		{
			baseVariables[name] = value;
			return this;
		}

		/// <summary>
		/// Set base variables.
		/// </summary>
		/// <param name="baseVariables"></param>
		/// <returns></returns>
		/// <remarks>Replace existing variables or add new variables.</remarks>
		// ReSharper disable once ParameterHidesMember
		public TymlContextBuilder WithBaseVariables(Dictionary<string, object> baseVariables)
		{
			foreach ((string name, object value) in baseVariables)
			{
				this.baseVariables[name] = value;
			}

			return this;
		}

		/// <summary>
		/// Return dictionary of YamlTask types.
		/// </summary>
		/// <param name="taskTypes"></param>
		/// <returns></returns>
		private static Dictionary<string, TaskInfo> GetTasks(IEnumerable<Type> taskTypes)
		{
			return taskTypes
				.Where(x => x.IsClass && !x.IsAbstract)
				.Select(t => new
				{
					Type = t,
					Attribute = (TymlTaskAttribute?)t.GetCustomAttributes(typeof(TymlTaskAttribute), true).FirstOrDefault()
				})
				.Where(x => x.Attribute != null)
				.ToDictionary(x => x.Attribute!.Name.ToLower(), x => new TaskInfo(x.Type, x.Attribute!));
		}
	}
}