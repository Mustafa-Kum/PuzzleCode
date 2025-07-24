using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid_Object
{
    public class PlanePathAction : MonoBehaviour
    {
        private void OnEnable()
        {
            //EventManager.CubixPathEvents.OnPathFound += PathAction;
        }
        
        private void OnDisable()
        {
            //EventManager.CubixPathEvents.OnPathFound -= PathAction;
        }

        private void PathAction(List<GridTile> arg0)
        {
            List<Material> materials = new List<Material>();
            
            GetComponent<MeshRenderer>().GetMaterials(materials);
            
            materials[0].DOFloat(1f, "_AdvancedDissolveCutoutStandardClip", 3f);
        }
    }
}