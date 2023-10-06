using System.Collections.Generic;
using UnityEngine;

namespace StackTheBlockArslan
{
    public class ShapeSwitcher : MonoBehaviour
    {
        public enum ShapeType
        {
            Box,
            Square,
            loop,
            Triangle,
            Washel,
            BevelCube

        }

        public static ShapeSwitcher instance;
        [SerializeField]
        private ShapeType selectedShape = ShapeType.Box; // Default shape type

        [System.Serializable]
        public class ShapeInfo
        {
            public ShapeType shapeType;
            public Mesh shapeMesh;
        }

        [SerializeField]
        private List<ShapeInfo> shapeMeshes = new List<ShapeInfo>();

        [SerializeField]
        private MeshFilter targetMeshFilter;

        private void Start()
        {
            instance = this;
        }
        private ObjectPooler objectpooler;

        private void FixedUpdate()
        {
            if(objectpooler==null)
            {
                objectpooler = GameObject.FindObjectOfType<ObjectPooler>();
                ShapeSwitcher.ShapeType Type = objectpooler.ShapeType;
                SetShape(Type);
            }
        }



        public void SetShape(ShapeType shapeType)
        {
            // Update the selected shape type.
            selectedShape = shapeType;

            // Find the ShapeInfo associated with the selected shape type.
            ShapeInfo selectedShapeInfo = shapeMeshes.Find(shapeInfo => shapeInfo.shapeType == shapeType);

            // Set the mesh of the target MeshFilter.
            if (selectedShapeInfo != null && targetMeshFilter != null)
            {
                targetMeshFilter.mesh = selectedShapeInfo.shapeMesh;//.GetComponent<MeshFilter>().mesh;
                this.gameObject.GetComponent<MeshCollider>().sharedMesh = targetMeshFilter.mesh;

                this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(10f, 10f);
            }
        }
    }
}