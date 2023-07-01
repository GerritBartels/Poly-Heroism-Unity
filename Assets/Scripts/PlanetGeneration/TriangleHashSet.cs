using System.Collections.Generic;
using UnityEngine;

namespace PlanetGeneration
{
    public class TriangleHashSet : HashSet<MeshTriangle>
    {
        public TriangleHashSet() {}
        public TriangleHashSet(TriangleHashSet source) : base(source) {}
        public int IterationIndex = -1;
    
        public BorderHashSet CreateBoarderHashSet()
        {
            BorderHashSet boarderSet = new BorderHashSet();
            foreach (MeshTriangle triangle in this)
            {
                foreach (MeshTriangle neighbor in triangle.Neighbours)
                {
                    if (this.Contains(neighbor))
                    {
                        continue;
                    }
                    TriangleBorder boarder = new TriangleBorder(triangle, neighbor);
                    boarderSet.Add(boarder);
                }
            }
            return boarderSet;
        }
    
        public List<int> RemoveDublicates()
        {
            List<int> vertices = new List<int>();
            foreach (MeshTriangle triangle in this)
            {
                foreach (int vertexIndex in triangle.VertexIndices)
                {
                    if (!vertices.Contains(vertexIndex))
                    {
                        vertices.Add(vertexIndex);
                    }      
                }
            }
            return vertices;
        }
    
        public void ApplyColor(Color _color)
        {
            foreach (MeshTriangle triangle in this)
                triangle.Color = _color;
        }
    }
}

