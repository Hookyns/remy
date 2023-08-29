using RJDev.Outputter;

namespace RJDev.Tyml.Core.Tasks
{
	public sealed class TaskResult
	{
		/// <summary>
		/// Display name of task
		/// </summary>
		public string? DisplayName { get; }

		/// <summary>
		/// Name identifier of task
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Task's completion status.
		/// </summary>
		public TaskCompletionStatus Status { get; }

		/// <summary>
		/// Reader of tasks output
		/// </summary>
		public OutputReader Output { get; }

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="name"></param>
		/// <param name="displayName"></param>
		/// <param name="taskCompletionStatus"></param>
		/// <param name="outputReader"></param>
		public TaskResult(
			string name,
			string? displayName,
			TaskCompletionStatus taskCompletionStatus,
			OutputReader outputReader
		)
		{
			Name = name;
			DisplayName = displayName;
			Status = taskCompletionStatus;
			Output = outputReader;
		}
	}
}