using System;

namespace Puzzle.Advertisements
{
    public class RewardedVideoPlacement : Placement
    {
        public RewardedVideoPlacement(Action finished, Action skipped, Action failed) : base(finished, skipped, failed)
        { }

        public override string ID => "rewardedVideo";
    }
}