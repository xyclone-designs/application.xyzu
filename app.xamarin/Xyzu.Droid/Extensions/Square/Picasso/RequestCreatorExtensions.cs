#nullable enable

using System;

using Xyzu.Images.Enums;

namespace Square.Picasso
{
	public static class RequestCreatorExtensions
	{
		public static RequestCreator PerformOperations(this RequestCreator requestcreator, Action<Operations, RequestCreator>? onoperate, params Operations[] operations)
		{
			foreach (Operations operation in operations)
				onoperate?.Invoke(operation, requestcreator);

			return requestcreator;
		}
	}
}