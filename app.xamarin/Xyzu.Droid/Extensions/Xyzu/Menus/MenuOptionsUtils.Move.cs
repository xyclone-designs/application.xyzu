#nullable enable

using System.IO;
using System.Linq;

using Xyzu.Library.Models;
using Xyzu.Player.Enums;

namespace Xyzu.Menus
{
	public partial class MenuOptionsUtils
	{
		public static void MoveDown(VariableContainer variables)
		{
			MoveDown(variables.Songs.ToArray());
		}
		public static void MoveDown(params ISong[] songs)
		{
			foreach (ISong song in songs)
				XyzuPlayer.Instance.Player.Queue.Move(song.Id, QueuePositions.DownOne);
		}
		public static void MoveToBottom(VariableContainer variables)
		{
			MoveToBottom(variables.Songs.ToArray());
		}
		public static void MoveToBottom(params ISong[] songs)
		{
			foreach (ISong song in songs)
				XyzuPlayer.Instance.Player.Queue.Move(song.Id, QueuePositions.Last);
		}
		public static void MoveToTop(VariableContainer variables)
		{
			MoveToTop(variables.Songs.ToArray());
		}
		public static void MoveToTop(params ISong[] songs)
		{
			foreach (ISong song in songs)
				XyzuPlayer.Instance.Player.Queue.Move(song.Id, QueuePositions.First);
		}
		public static void MoveUp(VariableContainer variables)
		{
			MoveUp(variables.Songs.ToArray());
		}
		public static void MoveUp(params ISong[] songs)
		{
			foreach (ISong song in songs)
				XyzuPlayer.Instance.Player.Queue.Move(song.Id, QueuePositions.UpOne);
		}
	}
}