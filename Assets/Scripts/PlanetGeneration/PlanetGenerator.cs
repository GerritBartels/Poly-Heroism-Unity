using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlanetGeneration
{
    public class PlanetGenerator : MonoBehaviour
    {
        [FormerlySerializedAs("Radius")] [Header("Planet Settings:")]
        public float radius;

        [FormerlySerializedAs("IcosphereSubdivisions")]
        public int icosphereSubdivisions;

        [FormerlySerializedAs("PlanetMaterial")]
        public Material planetMaterial;

        [FormerlySerializedAs("SmoothNormals")]
        public bool smoothNormals;

        [FormerlySerializedAs("MaxAmountOfContinents")] [Header("Continents:")]
        public int maxAmountOfContinents;

        [FormerlySerializedAs("ContinentsMinSize")]
        public float continentsMinSize;

        [FormerlySerializedAs("ContinentsMaxSize")]
        public float continentsMaxSize;

        [FormerlySerializedAs("MinLandExtrusionHeight")]
        public float minLandExtrusionHeight;

        [FormerlySerializedAs("MaxLandExtrusionHeight")]
        public float maxLandExtrusionHeight;

        private List<MeshTriangle> _meshTriangles = new();
        private List<Vector3> _vertices = new();

        private GameObject _planetGameObject;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private Mesh _planetMesh;

        private TriangleHashSet _continents;
        private TriangleHashSet _continents2;

        private Color _land1Color;
        private Color _land2Color;

        private VegetationSpawner _vegetationSpawner;


        public void Start()
        {
            StartGeneration();

            GameObject treeSpawnerObject = GameObject.Find("TreeSpawner");
            _vegetationSpawner = treeSpawnerObject.GetComponent<VegetationSpawner>();
            _vegetationSpawner.SpawnPrefabs(gameObject, radius, _land1Color, _land2Color);

            GameObject stoneSpawnerObject = GameObject.Find("StoneSpawner");
            _vegetationSpawner = stoneSpawnerObject.GetComponent<VegetationSpawner>();
            _vegetationSpawner.SpawnPrefabs(gameObject, radius, _land1Color, _land2Color);
        }

        public void StartGeneration()
        {
            _planetGameObject = gameObject;
            _planetGameObject.transform.parent = transform;

            _meshRenderer = _planetGameObject.AddComponent<MeshRenderer>();
            _meshRenderer.material = planetMaterial;

            _meshFilter = _planetGameObject.AddComponent<MeshFilter>();

            _planetMesh = new Mesh();
            GenerateIcosphere();
            CalculateNeighbors();

            AddPrimaryLandmass();
            AddSecondaryLandmass();
            AddBedrock();
            GenerateMesh();

            _planetGameObject.AddComponent<MeshCollider>();
        }

        private void AddPrimaryLandmass()
        {
            _continents = new TriangleHashSet();

            for (int i = 0; i < maxAmountOfContinents; i++)
            {
                float continentSize = Random.Range(continentsMinSize, continentsMaxSize);
                TriangleHashSet addedLandmass = GetTriangles(Random.onUnitSphere, continentSize, _meshTriangles);

                _continents.UnionWith(addedLandmass);
            }

            _land1Color = SampleRandomColor();
            _continents.ApplyColor(_land1Color);

            Extrude(_continents, Random.Range(minLandExtrusionHeight, maxLandExtrusionHeight));
        }

        private void AddSecondaryLandmass()
        {
            _continents2 = new TriangleHashSet();

            foreach (MeshTriangle triangle in _meshTriangles)
            {
                if (!_continents.Contains(triangle))
                    _continents2.Add(triangle);
            }

            TriangleHashSet addedLandmass2 = new TriangleHashSet(_continents2);

            _land2Color = SampleRandomColor();
            addedLandmass2.ApplyColor(_land2Color);
        }

        private void AddBedrock()
        {
            // Create empty GameObject
            GameObject bedrock = new GameObject("Bedrock");
            bedrock.tag = "Bedrock";
            bedrock.transform.parent = transform;
            bedrock.AddComponent<SphereCollider>().radius = radius - 0.5f;
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
            this.transform.localScale = Vector3.one * radius;
            _meshTriangles = new List<MeshTriangle>();
            _vertices = new List<Vector3>();

            float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

            _vertices.Add(new Vector3(-1, t, 0).normalized);
            _vertices.Add(new Vector3(1, t, 0).normalized);
            _vertices.Add(new Vector3(-1, -t, 0).normalized);
            _vertices.Add(new Vector3(1, -t, 0).normalized);
            _vertices.Add(new Vector3(0, -1, t).normalized);
            _vertices.Add(new Vector3(0, 1, t).normalized);
            _vertices.Add(new Vector3(0, -1, -t).normalized);
            _vertices.Add(new Vector3(0, 1, -t).normalized);
            _vertices.Add(new Vector3(t, 0, -1).normalized);
            _vertices.Add(new Vector3(t, 0, 1).normalized);
            _vertices.Add(new Vector3(-t, 0, -1).normalized);
            _vertices.Add(new Vector3(-t, 0, 1).normalized);

            _meshTriangles.Add(new MeshTriangle(0, 11, 5));
            _meshTriangles.Add(new MeshTriangle(0, 5, 1));
            _meshTriangles.Add(new MeshTriangle(0, 1, 7));
            _meshTriangles.Add(new MeshTriangle(0, 7, 10));
            _meshTriangles.Add(new MeshTriangle(0, 10, 11));
            _meshTriangles.Add(new MeshTriangle(1, 5, 9));
            _meshTriangles.Add(new MeshTriangle(5, 11, 4));
            _meshTriangles.Add(new MeshTriangle(11, 10, 2));
            _meshTriangles.Add(new MeshTriangle(10, 7, 6));
            _meshTriangles.Add(new MeshTriangle(7, 1, 8));
            _meshTriangles.Add(new MeshTriangle(3, 9, 4));
            _meshTriangles.Add(new MeshTriangle(3, 4, 2));
            _meshTriangles.Add(new MeshTriangle(3, 2, 6));
            _meshTriangles.Add(new MeshTriangle(3, 6, 8));
            _meshTriangles.Add(new MeshTriangle(3, 8, 9));
            _meshTriangles.Add(new MeshTriangle(4, 9, 5));
            _meshTriangles.Add(new MeshTriangle(2, 4, 11));
            _meshTriangles.Add(new MeshTriangle(6, 2, 10));
            _meshTriangles.Add(new MeshTriangle(8, 6, 7));
            _meshTriangles.Add(new MeshTriangle(9, 8, 1));

            Subdivide();
        }

        public void Subdivide()
        {
            var midPointCache = new Dictionary<int, int>();

            for (int i = 0; i < icosphereSubdivisions; i++)
            {
                var newPolys = new List<MeshTriangle>();
                foreach (var poly in _meshTriangles)
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

                _meshTriangles = newPolys;
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

            Vector3 p1 = _vertices[indexA];
            Vector3 p2 = _vertices[indexB];
            Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

            ret = _vertices.Count;
            _vertices.Add(middle);

            // Add our new midpoint to the cache so we don't have
            // to do this again. =)

            cache.Add(key, ret);
            return ret;
        }

        public void GenerateMesh()
        {
            int vertexCount = _meshTriangles.Count * 3;

            int[] indices = new int[vertexCount];

            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            Color32[] colors = new Color32[vertexCount];
            Vector2[] uvs = new Vector2[vertexCount];

            for (int i = 0; i < _meshTriangles.Count; i++)
            {
                var poly = _meshTriangles[i];

                indices[i * 3 + 0] = i * 3 + 0;
                indices[i * 3 + 1] = i * 3 + 1;
                indices[i * 3 + 2] = i * 3 + 2;

                vertices[i * 3 + 0] = _vertices[poly.VertexIndices[0]];
                vertices[i * 3 + 1] = _vertices[poly.VertexIndices[1]];
                vertices[i * 3 + 2] = _vertices[poly.VertexIndices[2]];

                uvs[i * 3 + 0] = poly.UVs[0];
                uvs[i * 3 + 1] = poly.UVs[1];
                uvs[i * 3 + 2] = poly.UVs[2];

                colors[i * 3 + 0] = poly.Color;
                colors[i * 3 + 1] = poly.Color;
                colors[i * 3 + 2] = poly.Color;

                if (smoothNormals)
                {
                    normals[i * 3 + 0] = _vertices[poly.VertexIndices[0]].normalized;
                    normals[i * 3 + 1] = _vertices[poly.VertexIndices[1]].normalized;
                    normals[i * 3 + 2] = _vertices[poly.VertexIndices[2]].normalized;
                }
                else
                {
                    Vector3 ab = _vertices[poly.VertexIndices[1]] - _vertices[poly.VertexIndices[0]];
                    Vector3 ac = _vertices[poly.VertexIndices[2]] - _vertices[poly.VertexIndices[0]];

                    Vector3 normal = Vector3.Cross(ab, ac).normalized;

                    normals[i * 3 + 0] = normal;
                    normals[i * 3 + 1] = normal;
                    normals[i * 3 + 2] = normal;
                }
            }

            _planetMesh.vertices = vertices;
            _planetMesh.normals = normals;
            _planetMesh.colors32 = colors;
            _planetMesh.uv = uvs;

            _planetMesh.SetTriangles(indices, 0);

            _meshFilter.mesh = _planetMesh;
        }

        public TriangleHashSet GetTriangles(Vector3 center, float radius, IEnumerable<MeshTriangle> source)
        {
            TriangleHashSet newSet = new TriangleHashSet();

            foreach (MeshTriangle p in source)
            {
                foreach (int vertexIndex in p.VertexIndices)
                {
                    float distanceToSphere = Vector3.Distance(center, _vertices[vertexIndex]);

                    if (distanceToSphere <= radius)
                    {
                        newSet.Add(p);
                        break;
                    }
                }
            }

            return newSet;
        }

        public List<int> CloneVertices(List<int> oldVerts)
        {
            List<int> newVerts = new List<int>();
            foreach (int oldVert in oldVerts)
            {
                Vector3 clonedVert = _vertices[oldVert];
                newVerts.Add(_vertices.Count);
                _vertices.Add(clonedVert);
            }

            return newVerts;
        }

        public TriangleHashSet StitchPolys(TriangleHashSet polys, out BorderHashSet stitchedEdge)
        {
            TriangleHashSet stichedPolys = new TriangleHashSet();

            stichedPolys.IterationIndex = _vertices.Count;

            stitchedEdge = polys.CreateBoarderHashSet();
            var originalVerts = stitchedEdge.RemoveDublicates();
            var newVerts = CloneVertices(originalVerts);

            stitchedEdge.Seperate(originalVerts, newVerts);

            foreach (TriangleBorder edge in stitchedEdge)
            {
                // Create new polys along the stitched edge. These
                // will connect the original poly to its former
                // neighbor.

                var stitchPoly1 = new MeshTriangle(edge.OuterVertices[0],
                    edge.OuterVertices[1],
                    edge.InnerVertices[0]);
                var stitchPoly2 = new MeshTriangle(edge.OuterVertices[1],
                    edge.InnerVertices[1],
                    edge.InnerVertices[0]);
                // Add the new stitched faces as neighbors to
                // the original Polys.
                edge.InnerTriangle.UpdateNeighbour(edge.OuterTriangle, stitchPoly2);
                edge.OuterTriangle.UpdateNeighbour(edge.InnerTriangle, stitchPoly1);

                _meshTriangles.Add(stitchPoly1);
                _meshTriangles.Add(stitchPoly2);

                stichedPolys.Add(stitchPoly1);
                stichedPolys.Add(stitchPoly2);
            }

            //Swap to the new vertices on the inner polys.
            foreach (MeshTriangle poly in polys)
            {
                for (int i = 0; i < 3; i++)
                {
                    int vertID = poly.VertexIndices[i];
                    if (!originalVerts.Contains(vertID))
                        continue;
                    int vertIndex = originalVerts.IndexOf(vertID);
                    poly.VertexIndices[i] = newVerts[vertIndex];
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
                Vector3 v = _vertices[vert];
                v = v.normalized * (v.magnitude + height);
                _vertices[vert] = v;
            }

            return stitchedPolys;
        }

        public void CalculateNeighbors()
        {
            foreach (MeshTriangle poly in _meshTriangles)
            {
                foreach (MeshTriangle otherPoly in _meshTriangles)
                {
                    if (poly == otherPoly)
                        continue;

                    if (poly.IsNeighbouring(otherPoly))
                        poly.Neighbours.Add(otherPoly);
                }
            }
        }
    }
}