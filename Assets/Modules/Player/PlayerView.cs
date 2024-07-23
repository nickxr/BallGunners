using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerView : NetworkBehaviour
    {
        [SerializeField] private MeshRenderer connorRenderer;
        [SerializeField] private MeshRenderer goalRenderer;

        public void SetColor(Color color)
        {
            Material material = new Material(connorRenderer.sharedMaterial);
            connorRenderer.sharedMaterial = material;
            goalRenderer.sharedMaterial = material;
            material.color = color;
        }
    }
}