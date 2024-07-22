using UnityEngine;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer connorRenderer;
        [SerializeField] private MeshRenderer goalRenderer;

        public void SetColor(Color color)
        {
            if (connorRenderer.sharedMaterial == goalRenderer.sharedMaterial)
            {
                connorRenderer.sharedMaterial.color = color;
            }
            else
            {
                Debug.Log("Warning! Not equal sharedMaterials.");
                connorRenderer.material.color = color;
                goalRenderer.material.color = color;
            }
        }
    }
}