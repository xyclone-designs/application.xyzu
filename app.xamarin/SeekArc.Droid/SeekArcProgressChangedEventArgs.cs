using System;

namespace SeekArc.Droid
{
    public class SeekArcProgressChangedEventArgs : EventArgs
    {
        public SeekArcView SeekArc { get; set; }
        public int Progress { get; set; }
        public bool FromUser { get; set; }

        public SeekArcProgressChangedEventArgs(SeekArcView seekArc, int progress, bool fromUser)
        {
            SeekArc = seekArc;
            Progress = progress;
            FromUser = fromUser;
        }
    }
}