using System.Collections.Generic;
using UnityEngine;

namespace PlanetGeneration
{
    public class BorderHashSet : HashSet<TriangleBorder>
    {
        public void Seperate(List<int> _originalVertices, List<int> _addedVertices)
        {
            foreach(TriangleBorder border in this)
            {
                for(int i = 0; i < 2; i++)
                {
                    border.InnerVertices[i] = _addedVertices[_originalVertices.IndexOf(border.OuterVertices[i])];
                }
            }
        }

        public List<int> RemoveDublicates()
        {
            List<int> vertices = new List<int>();
            foreach (TriangleBorder border in this)
            {
                foreach (int vertexIndex in border.OuterVertices)
                {
                    if (!vertices.Contains(vertexIndex))
                    {
                        vertices.Add(vertexIndex);
                    }
                }
            }
            return vertices;
        }

        public Dictionary<int, Vector3> GetInwardDirections(List<Vector3> vertexPositions)
        {
            Dictionary<int,Vector3> inwardDirections = new Dictionary<int, Vector3>();
            Dictionary<int,int> numItems = new Dictionary<int, int>();

            foreach(TriangleBorder border in this)
            {
                Vector3 innerVertexPosition = vertexPositions[border.InwardDirectionVertex];
                Vector3 borderPosA   = vertexPositions[border.InnerVertices[0]];
                Vector3 borderPosB   = vertexPositions[border.InnerVertices[1]];
                Vector3 borderCenter = Vector3.Lerp(borderPosA, borderPosB, 0.5f);
                Vector3 innerVector = (innerVertexPosition - borderCenter).normalized;

                for(int i = 0; i < 2; i++)
                {
                    int borderVertex = border.InnerVertices[i];
                    if (inwardDirections.ContainsKey(borderVertex))
                    {
                        inwardDirections[borderVertex] += innerVector;
                        numItems[borderVertex]++;
                    }
                    else
                    {
                        inwardDirections.Add(borderVertex, innerVector);
                        numItems.Add(borderVertex, 1);
                    }
                }
            }

            foreach(KeyValuePair<int, int> kvp in numItems)
            {
                int vertexIndex               = kvp.Key;
                int contributionsToThisVertex = kvp.Value;
                inwardDirections[vertexIndex] = (inwardDirections[vertexIndex] / contributionsToThisVertex).normalized;
            }

            return inwardDirections;
        }   
    }
}
