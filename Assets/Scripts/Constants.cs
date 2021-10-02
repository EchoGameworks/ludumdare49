using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public enum ElementTypes { Fire, Ice, Earth, Poison, Electricity }
    public enum DamagedAmount { None, Small, Large }

    public static class Helpers
    {
        public static Color GetElementColor(ElementTypes et)
        {
            switch (et)
            {
                case ElementTypes.Fire:
                    return new Color(180f / 255f, 82f / 255f, 82f / 255f);
                case ElementTypes.Ice:
                    return new Color(75f / 255f, 128f / 255f, 202f / 255f);
                case ElementTypes.Earth:
                    return new Color(138f / 255f, 176f / 255f, 96f / 255f);
                case ElementTypes.Poison:
                    return new Color(106f / 255f, 83f / 255f, 110f / 255f);
                case ElementTypes.Electricity:
                    return new Color(237f / 255f, 225f / 255f, 158f / 255f);
                default:
                    return new Color(242f / 255f, 240f / 255f, 229f / 255f);
            }

        }
    }
}