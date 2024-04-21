using System;

using Xyzu.Images.Enums;
using Xyzu.Library;

namespace Xyzu.Images
{
	public interface IImages
	{
		ILibrary.IMisc? LibraryMisc { get; set; }

		public static class DefaultOperations
		{
			public static readonly Operations[] Blur = new Operations[] { Operations.Blur };
			public static readonly Operations[] BlurDownsample = new Operations[] { Operations.Downsample, Operations.Blur, };
			public static readonly Operations[] Circularise = new Operations[] { Operations.Circularise };
			public static readonly Operations[] CirculariseDownsample = new Operations[] { Operations.Downsample, Operations.Circularise, };	   
			public static readonly Operations[] Downsample = new Operations[] { Operations.Downsample, };
			public static readonly Operations[] None = Array.Empty<Operations>();
			public static readonly Operations[] Rounded = new Operations[] { Operations.Rounded };
			public static readonly Operations[] RoundedDownsample = new Operations[] { Operations.Downsample, Operations.Rounded, };
		}

		public class Default : IImages
		{
			public ILibrary.IMisc? LibraryMisc { get; set; }
		}
	}
}