﻿using System;

using Xyzu.Images.Enums;

namespace Xyzu.Images
{
	public interface IImages
	{
		public static class DefaultOperations
		{
			public static readonly Operations[] Blur = new Operations[] { Operations.Blur };
			public static readonly Operations[] BlurDownsample = new Operations[] { Operations.Downsample, Operations.Blur, };
			public static readonly Operations[] Circularise = new Operations[] { Operations.Circularise };
			public static readonly Operations[] CirculariseDownsample = new Operations[] { Operations.Downsample, Operations.Circularise, };
			public static readonly Operations[] Downsample = new Operations[] { Operations.Downsample, };
			public static readonly Operations[] Merge = new Operations[] { Operations.Merge, };
			public static readonly Operations[] MergeDownsample = new Operations[] { Operations.Downsample, Operations.Merge, };
			public static readonly Operations[] MergeRounded = new Operations[] { Operations.Merge, Operations.Rounded, };
			public static readonly Operations[] MergeRoundedDownsample = new Operations[] { Operations.Downsample, Operations.Merge, Operations.Rounded, };
			public static readonly Operations[] None = Array.Empty<Operations>();
			public static readonly Operations[] Rounded = new Operations[] { Operations.Rounded };
			public static readonly Operations[] RoundedDownsample = new Operations[] { Operations.Downsample, Operations.Rounded, };
		}

		public class Parameters
		{
			public Parameters()
			{
				Operations = DefaultOperations.None;
			}

			public Operations[] Operations { get; set; }
		}

		public class Default : IImages { }
	}
}