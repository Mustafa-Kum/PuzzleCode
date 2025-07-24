using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Booster
{
    [CreateAssetMenu(fileName = "BoosterUnlockLevels", menuName = "ThisGame/Booster/BoosterUnlockLevels", order = 0)]
    public class BoosterUnlockLevels : ScriptableObject
    {
        [System.Serializable]
        public class BoosterLevelMapping
        {
            public BoosterType boosterType;
            public int unlockLevel;
            public Sprite boosterSprite;
            public Sprite boosterSpriteLocked;
        }

        public List<BoosterLevelMapping> boosterLevelMappings;
    }
}