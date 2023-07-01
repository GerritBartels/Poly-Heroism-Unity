using System.Collections.Generic;
using UnityEngine;

namespace PlanetGeneration
{
    public class PlanetGenerator : MonoBehaviour
    {
        [Header("Planet Settings:")]
        public float Radius;
        public int IcosphereSubdivisions;
        public Material PlanetMaterial;
        public bool SmoothNormals;

        [Header("Continents:")]
        public int MaxAmountOfContinents;
        public float ContinentsMinSize;
        public float ContinentsMaxSize;
        public float MinLandExtrusionHeight;
        public float MaxLandExtrusionHeight;

        private List<MeshTriangle> MeshTriangles = new List<MeshTriangle>();
        private List<Vector3> Vertices = new List<Vector3>();
        
        private GameObject planetGameObject;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private Mesh planetMesh;
        
        private TriangleHashSet continents;
        private TriangleHashSet continents2;
        
        private Color Land1Color;
        private Color Land2Color;
        
        private VegetationSpawner _vegetationSpawner;
        
        
        public void Start()
        {
            StartGeneration();

            GameObject treeSpawnerObject = GameObject.Find("TreeSpawner");
            _vegetationSpawner = treeSpawnerObject.GetComponent<VegetationSpawner>();
            _vegetationSpawner.SpawnPrefabs(this.gameObject, Radius, Land1Color, Land2Color);
            
            GameObject stoneSpawnerObject = GameObject.Find("StoneSpawner");
            _vegetationSpawner = stoneSpawnerObject.GetComponent<VegetationSpawner>();
            _vegetationSpawner.SpawnPrefabs(this.gameObject, Radius, Land1Color, Land2Color);
        }

        public void StartGeneration()
        {
            planetGameObject = this.gameObject;
            planetGameObject.transform.parent = transform;
            
            meshRenderer = planetGameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = PlanetMaterial;

            meshFilter = planetGameObject.AddComponent<MeshFilter>();

            planetMesh = new Mesh();
            GenerateIcosphere();
            CalculateNeighbors();

            AddPrimaryLandmass();
            AddSecondaryLandmass();
            AddBedrock();
            GenerateMesh();
                
            planetGameObject.AddComponent<MeshCollider>();
        }
        
        private void AddPrimaryLandmass()
        {
            continents = new TriangleHashSet();

            for(int i = 0; i < MaxAmountOfContinents; i++)
            {
                float continentSize = Random.Range(ContinentsMinSize, ContinentsMaxSize);
                TriangleHashSet addedLandmass = GetTriangles(Random.onUnitSphere, continentSize, MeshTriangles);

                continents.UnionWith(addedLandmass);
            }
            
            Land1Color = SampleRandomColor();
            continents.ApplyColor(Land1Color);

            Extrude(continents, Random.Range(MinLandExtrusionHeight, MaxLandExtrusionHeight));
            
        }
        
        private void AddSecondaryLandmass()
        {
            continents2 = new TriangleHashSet();

            foreach (MeshTriangle triangle in MeshTriangles)
            {
                if (!continents.Contains(triangle))
                    continents2.Add(triangle);
            }

            TriangleHashSet addedLandmass2 = new TriangleHashSet(continents2);
            
            Land2Color = SampleRandomColor();
            addedLandmass2.ApplyColor(Land2Color);
        }

        private void AddBedrock()
        {
            // Create empty GameObject
            GameObject bedrock = new GameObject("Bedrock");
            bedrock.tag = "Bedrock";
            bedrock.transform.parent = transform;
            bedrock.AddComponent<SphereCollider>().radius = Radius-0.5f;
            
        }
        
        private Color SampleRandomColor()
        {
            float red = Random.Range(0f, 1f);
            float green = Random.Range(0f, 1f);
            float blue = Random.Range(0f, 1f);
            float alpha = Random.Range(0f, 1f);

            Color randomColor = new Color(red, green, blue, alpha);
            return randomColor;
        }
        
        public void GenerateIcosphere()
        {
            this.transform.localScale = Vector3.one * Radius;
            MeshTriangles = new List<MeshTriangle>();
            Vertices = new List<Vector3>();

            float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

            Vertices.Add(new Vector3(-1,  t,  0).normalized);
            Vertices.Add(new Vector3( 1,  t,  0).normalized);
            Vertices.Add(new Vector3(-1, -t,  0).normalized);
            Vertices.Add(new Vector3( 1, -t,  0).normalized);
            Vertices.Add(new Vector3( 0, -1,  t).normalized);
            Vertices.Add(new Vector3( 0,  1,  t).normalized);
            Vertices.Add(new Vector3( 0, -1, -t).normalized);
            Vertices.Add(new Vector3( 0,  1, -t).normalized);
            Vertices.Add(new Vector3( t,  0, -1).normalized);
            Vertices.Add(new Vector3( t,  0,  1).normalized);
            Vertices.Add(new Vector3(-t,  0, -1).normalized);
            Vertices.Add(new Vector3(-t,  0,  1).normalized);

            MeshTriangles.Add(new MeshTriangle( 0, 11,  5));
            MeshTriangles.Add(new MeshTriangle( 0,  5,  1));
            MeshTriangles.Add(new MeshTriangle( 0,  1,  7));
            MeshTriangles.Add(new MeshTriangle( 0,  7, 10));
            MeshTriangles.Add(new MeshTriangle( 0, 10, 11));
            MeshTriangles.Add(new MeshTriangle( 1,  5,  9));
            MeshTriangles.Add(new MeshTriangle( 5, 11,  4));
            MeshTriangles.Add(new MeshTriangle(11, 10,  2));
            MeshTriangles.Add(new MeshTriangle(10,  7,  6));
            MeshTriangles.Add(new MeshTriangle( 7,  1,  8));
            MeshTriangles.Add(new MeshTriangle( 3,  9,  4));
            MeshTriangles.Add(new MeshTriangle( 3,  4,  2));
            MeshTriangles.Add(new MeshTriangle( 3,  2,  6));
            MeshTriangles.Add(new MeshTriangle( 3,  6,  8));
            MeshTriangles.Add(new MeshTriangle( 3,  8,  9));
            MeshTriangles.Add(new MeshTriangle( 4,  9,  5));
            MeshTriangles.Add(new MeshTriangle( 2,  4, 11));
            MeshTriangles.Add(new MeshTriangle( 6,  2, 10));
            MeshTriangles.Add(new MeshTriangle( 8,  6,  7));
            MeshTriangles.Add(new MeshTriangle( 9,  8,  1));

            Subdivide();
        }

        public void Subdivide()
        {
            var midPointCache = new Dictionary<int, int>();

            for (int i = 0; i < IcosphereSubdivisions; i++)
            {
                var newPolys = new List<MeshTriangle>();
                foreach (var poly in MeshTriangles)
                {
                    int a = poly.VertexIndices[0];
                    int b = poly.VertexIndices[1];
                    int c = poly.VertexIndices[2];

                    int ab = GetMidPointIndex(midPointCache, a, b);
                    int bc = GetMidPointIndex(midPointCache, b, c);
                    int ca = GetMidPointIndex(midPointCache, c, a);

                    newPolys.Add(new MeshTriangle(a, ab, ca));
                    newPolys.Add(new MeshTriangle(b, bc, ab));
                    newPolys.Add(new MeshTriangle(c, ca, bc));
                    newPolys.Add(new MeshTriangle(ab, bc, ca));
                }
                
                MeshTriangles = newPolys;
            }
        }
        
        public int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
        {
            int smallerIndex = Mathf.Min(indexA, indexB);
            int greaterIndex = Mathf.Max(indexA, indexB);
            int key = (smallerIndex << 16) + greaterIndex;

            // If a midpoint is already defined, just return it.

            int ret;
            if (cache.TryGetValue(key, out ret))
                return ret;

            // If we're here, it's because a midpoint for these two
            // vertices hasn't been created yet. Let's do that now!

            Vector3 p1 = Vertices[indexA];
            Vector3 p2 = Vertices[indexB];
            Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

            ret = Vertices.Count;
            Vertices.Add(middle);

            // Add our new midpoint to the cache so we don't have
            // to do this again. =)

            cache.Add(key, ret);
            return ret;
        }
        
        public void GenerateMesh()
        {
            int vertexCount = MeshTriangles.Count * 3;

            int[] indices = new int[vertexCount];

            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals  = new Vector3[vertexCount];
            Color32[] colors   = new Color32[vertexCount];
            Vector2[] uvs      = new Vector2[vertexCount];

            for (int i = 0; i < MeshTriangles.Count; i++)
            {
                var poly = MeshTriangles[i];

                indices[i * 3 + 0] = i * 3 + 0;
                indices[i * 3 + 1] = i * 3 + 1;
                indices[i * 3 + 2] = i * 3 + 2;

                vertices[i * 3 + 0] = Vertices[poly.VertexIndices[0]];
                vertices[i * 3 + 1] = Vertices[poly.VertexIndices[1]];
                vertices[i * 3 + 2] = Vertices[poly.VertexIndices[2]];

                uvs[i * 3 + 0] = poly.UVs[0];
                uvs[i * 3 + 1] = poly.UVs[1];
                uvs[i * 3 + 2] = poly.UVs[2];

                colors[i * 3 + 0] = poly.Color;
                colors[i * 3 + 1] = poly.Color;
                colors[i * 3 + 2] = poly.Color;

                if(SmoothNormals)
                {
                    normals[i * 3 + 0] = Vertices[poly.VertexIndices[0]].normalized;
                    normals[i * 3 + 1] = Vertices[poly.VertexIndices[1]].normalized;
                    normals[i * 3 + 2] = Vertices[poly.VertexIndices[2]].normalized;
                }
                else
                {
                    Vector3 ab = Vertices[poly.VertexIndices[1]] - Vertices[poly.VertexIndices[0]];
                    Vector3 ac = Vertices[poly.VertexIndices[2]] - Vertices[poly.VertexIndices[0]];

                    Vector3 normal = Vector3.Cross(ab, ac).normalized;

                    normals[i * 3 + 0] = normal;
                    normals[i * 3 + 1] = normal;
                    normals[i * 3 + 2] = normal;
                }
            }

            planetMesh.vertices = vertices;
            planetMesh.normals  = normals;
            planetMesh.colors32 = colors;
            planetMesh.uv       = uvs;

            planetMesh.SetTriangles(indices, 0);

            meshFilter.mesh = planetMesh;

        }
        
        public TriangleHashSet GetTriangles(Vector3 center, float radius, IEnumerable<MeshTriangle> source)
        {
            TriangleHashSet newSet = new TriangleHashSet();

            foreach(MeshTriangle p in source)
            {
                foreach(int vertexIndex in p.VertexIndices)
                {
                    float distanceToSphere = Vector3.Distance(center, Vertices[vertexIndex]);

                    if (distanceToSphere <= radius)
                    {
                        newSet.Add(p);
                        break;
                    }
                }
            }

            return newSet;
        }
        
        public List<int> CloneVertices(List<int> old_verts)
        {
            List<int> new_verts = new List<int>();
            foreach (int old_vert in old_verts)
            {
                Vector3 cloned_vert = Vertices[old_vert];
                new_verts.Add(Vertices.Count);
                Vertices.Add(cloned_vert);
            }
            return new_verts;
        }
        
        public TriangleHashSet StitchPolys(TriangleHashSet polys, out BorderHashSet stitchedEdge)
        {
            TriangleHashSet stichedPolys = new TriangleHashSet();

            stichedPolys.IterationIndex = Vertices.Count;

            stitchedEdge      = polys.CreateBoarderHashSet();
            var originalVerts = stitchedEdge.RemoveDublicates();
            var newVerts      = CloneVertices(originalVerts);

            stitchedEdge.Seperate(originalVerts, newVerts);

            foreach (TriangleBorder edge in stitchedEdge)
            {
                // Create new polys along the stitched edge. These
                // will connect the original poly to its former
                // neighbor.

                var stitch_poly1 = new MeshTriangle(edge.OuterVertices[0],
                                               edge.OuterVertices[1],
                                               edge.InnerVertices[0]);
                var stitch_poly2 = new MeshTriangle(edge.OuterVertices[1],
                                               edge.InnerVertices[1],
                                               edge.InnerVertices[0]);
                // Add the new stitched faces as neighbors to
                // the original Polys.
                edge.InnerTriangle.UpdateNeighbour(edge.OuterTriangle, stitch_poly2);
                edge.OuterTriangle.UpdateNeighbour(edge.InnerTriangle, stitch_poly1);

                MeshTriangles.Add(stitch_poly1);
                MeshTriangles.Add(stitch_poly2);

                stichedPolys.Add(stitch_poly1);
                stichedPolys.Add(stitch_poly2);
            }

            //Swap to the new vertices on the inner polys.
            foreach (MeshTriangle poly in polys)
            {
                for (int i = 0; i < 3; i++)
                {
                    int vert_id = poly.VertexIndices[i];
                    if (!originalVerts.Contains(vert_id))
                        continue;
                    int vert_index = originalVerts.IndexOf(vert_id);
                    poly.VertexIndices[i] = newVerts[vert_index];
                }
            }
            
            return stichedPolys;
        }
        
        public TriangleHashSet Extrude(TriangleHashSet polys, float height)
        {
            BorderHashSet stitchedEdge;
            TriangleHashSet stitchedPolys = StitchPolys(polys, out stitchedEdge);
            List<int> verts = polys.RemoveDublicates();

            // Take each vertex in this list of polys, and push it
            // away from the center of the Planet by the height
            // parameter.

            foreach (int vert in verts)
            {
                Vector3 v = Vertices[vert];
                v = v.normalized * (v.magnitude + height);
                Vertices[vert] = v;
            }

            return stitchedPolys;
        }
        
        public void CalculateNeighbors()
        {
            foreach (MeshTriangle poly in MeshTriangles)
            {
                foreach (MeshTriangle other_poly in MeshTriangles)
                {
                    if (poly == other_poly)
                        continue;

                    if (poly.IsNeighbouring(other_poly))
                        poly.Neighbours.Add(other_poly);
                }
            }
        }
    }
}
