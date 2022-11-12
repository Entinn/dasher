using System.Linq;
using UnityEngine;

namespace Dasher
{
    internal class SkinColorChanger : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] changeColorMaterials;

        private Color[] startMaterialsColors;

        private void Start()
        {
            startMaterialsColors = changeColorMaterials.Select(x => x.material.color).ToArray();
        }

        public void SetColor(Color color)
        {
            foreach (var changeColorMaterial in changeColorMaterials)
                changeColorMaterial.material.color = color;
        }

        public void ReturnToStartColor()
        {
            for (var i = 0; i < changeColorMaterials.Length; i++)
                changeColorMaterials[i].material.color = startMaterialsColors[i];
        }
    }
}