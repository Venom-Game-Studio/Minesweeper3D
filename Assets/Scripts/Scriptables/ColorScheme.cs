using System.Collections.Generic;
using UnityEngine;

namespace Fabwelt.Managers.Scriptable
{
    [CreateAssetMenu(fileName = "Color Palette", menuName = "Catalog/Color")]
    public class ColorScheme : ScriptableObject
    {
        public List<Color> colors = new List<Color>();
    }
}