using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerFediverseJam.cs
{
    public struct GameStats
    {
        public int GoodPersonIndex { get; set; }
        public int WateredPlantTimes { get; set; }
        public bool KilledRoommatePlant { get; set; }
        public bool UsedRoommatesPlantToOpenDoor { get; set; }
        public bool RoommateTrustedYouAndOpenedDoorForYou { get; set; }
        public int MundaneObjectsLookedAt { get; set; }
        public bool IsBadRoommateEnding { get; set; }
        public bool IsFunParallelDimensionTimeWRommate { get; set; }
        public bool IsRoommateInParty { get; set; }
        public bool HasGun { get; set; }
        public int HP { get; set; }
        public int Lvl { get; set; }
        public int Exp { get; set; }
        public List<MonsterOption> MonstersVanquished { get; set; }
        public List<MonsterOption> MonstersAquianted { get; set; }
        public bool IsOutOfCirculation(MonsterOption option)
        {
            return MonstersVanquished.Contains(option) || MonstersAquianted.Contains(option);
        }
        public int MonstersOutOfCirculation
        {
            get
            {
                return MonstersAquianted.Count + MonstersVanquished.Count;
            }
        }
        public int Score
        {
            get
            {
                var score = 0;
                // Watering the plant more than once causes a negative score modifier
                // unless you use it to unlock the door, and then, it becomes a positive modifier
                if (WateredPlantTimes > 1 && !UsedRoommatesPlantToOpenDoor) {
                    score -= WateredPlantTimes;
                } else {
                    score += WateredPlantTimes;
                }
                if (IsBadRoommateEnding)
                {
                    score -= 1;
                }
                // simillarly, you get a bonus for the amount of mundane things you look at, until you look at all of them, and it becomes a negative modifier
                if (MundaneObjectsLookedAt < 5)
                {
                    score += MundaneObjectsLookedAt;
                } else {
                    score -= MundaneObjectsLookedAt;
                }
                if (IsRoommateInParty)
                {
                    score += 5;
                }
                score += GoodPersonIndex;
                score += MonstersAquianted.Count;
                score -= MonstersVanquished.Count;
                return score;
            }
        }
    }
}
