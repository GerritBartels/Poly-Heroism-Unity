using System.Collections.Generic;
using UnityEngine;

namespace PlanetGeneration
{
    public class MeshTriangle
    {
        public List<int> VertexIndices;
        public List<Vector2> UVs;
        public List<MeshTriangle> Neighbours;
        public Color Color;

        public MeshTriangle(int vertexIndexA, int vertexIndexB, int vertexIndexC)
        {
            VertexIndices = new List<int>() { vertexIndexA, vertexIndexB, vertexIndexC };
            UVs = new List<Vector2> { Vector2.zero, Vector2.zero, Vector2.zero };
            Neighbours = new List<MeshTriangle>();
        }

        public bool IsNeighbouring(MeshTriangle other)
        {
            int sharedVertices = 0;
            foreach (int index in VertexIndices)
            {
                if (other.VertexIndices.Contains(index))
                {
                    sharedVertices++;
                }
            }

            return sharedVertices > 1;
        }

        public void UpdateNeighbour(MeshTriangle initialNeighbour, MeshTriangle newNeighbour)
        {
            for (int i = 0; i < Neighbours.Count; i++)
            {
                if (initialNeighbour == Neighbours[i])
                {
                    Neighbours[i] = newNeighbour;
                    return;
                }
            }
        }
    }
}