using System;

namespace Puzzle.Advertisements
{
    public class VideoPlacement : Placement
    {
        public override string ID => "video";

        public VideoPlacement(Action finished, Action skipped, Action failed) : base(finished, skipped, failed)
        { }
    }
}