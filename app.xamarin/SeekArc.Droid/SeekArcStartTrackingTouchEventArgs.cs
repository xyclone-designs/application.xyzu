using System;

namespace SeekArc.Droid
{
    public class SeekArcTrackingTouchEventArgs : EventArgs
    {
        public SeekArcView SeekArc { get; set; }

        public SeekArcTrackingTouchEventArgs(SeekArcView seekArc)
        {
            SeekArc = seekArc;
        }
    }
}