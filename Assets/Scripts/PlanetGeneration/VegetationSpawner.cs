using System.Collections.Generic;
using UnityEngine;

namespace PlanetGeneration
{
    public class VegetationSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private Material[] materials;

        [SerializeField] private int minNumberOfObjects = 10;
        [SerializeField] private int maxNumberOfObjects = 20;

        [SerializeField] private float minScale = 0.2f;
        [SerializeField] private float maxScale = 0.55f;

        [SerializeField] private int numberOfDifferentPrefabs = 2;
        [SerializeField] private int numberOfDifferentMaterials = 2;

        [SerializeField] private int maxTries = 1000;
        [SerializeField] private float minDistance = 0.5f;

        [SerializeField] private bool useLandColors;

        public void SpawnPrefabs(GameObject planet, float radius, Color land1Color, Color land2Color)
        {
            // Exclude the "Planet" layer for the first layer mask and include only it for the second layer mask
            int planetLayer = LayerMask.NameToLayer("Planet");
            int layerMask = ~(1 << planetLayer);
            int layerMask2 = 1 << planetLayer;

            // Sample a random number of objects to spawn
            int numberOfObjects = Random.Range(minNumberOfObjects, maxNumberOfObjects);

            // Sample prefabs from the prefabs array
            List<GameObject> sampledPrefabs = SampleObjects<GameObject>(prefabs, numberOfDifferentPrefabs);
            // Sample materials from the materials array
            List<Material> sampledMaterials = SampleObjects<Material>(materials, numberOfDifferentMaterials);

            
            for (int i = 0; i < numberOfObjects; i++)
            {
                // Sample a random point on the sphere
                Vector3 spawnPosition = GetRandomSpawnPosition(radius);

                // Check for collisions, excluding the "Planet" layer
                while (Physics.CheckSphere(spawnPosition, minDistance, layerMask) && maxTries > 0)
                {
                    spawnPosition = GetRandomSpawnPosition(radius);
                    maxTries--;
                }

                if (maxTries <= 0)
                {
                    Debug.Log("Max tries exceeded");
                    break;
                }

                maxTries = 1000;

                // Raycast downwards to find the surface position
                // ToDo: Raycast cannot find the planet surface, why?
                RaycastHit hit;
                if (Physics.Raycast(spawnPosition, -spawnPosition.normalized, out hit, radius, layerMask2))
                {
                    spawnPosition = hit.point;
                }
                else
                {
                    // If no surface is found, skip spawning this object
                    Debug.Log("No surface found");
                    continue;
                }

                // Calculate rotation so that the prefab is always facing outwards from the sphere
                Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, spawnPosition.normalized);

                // Instantiate a random prefab at the sampled position and rotation
                GameObject spawnedPrefab = InstantiateRandomPrefab(sampledPrefabs, spawnPosition, spawnRotation);

                // Set Prefab color to one of the to land colors if UseLandColors is true
                // else set a random material
                if (useLandColors)
                {
                    SetRandomLandColor(spawnedPrefab, land1Color, land2Color);
                }
                else
                {
                    SetRandomMaterial(spawnedPrefab, sampledMaterials);
                }

                // Generate a random scale value within the specified range
                SetPrefabScale(spawnedPrefab, minScale, maxScale);

                // Randomize the rotation of the spawned prefab and parent it to the planet
                Transform spawnedPrefabTransform = spawnedPrefab.transform;
                RandomizePrefabRotation(spawnedPrefabTransform);
                ParentPrefabToPlanet(spawnedPrefabTransform, planet);

                // Add mesh collider and set tag
                AddMeshCollider(spawnedPrefab);
                SetPrefabTag(spawnedPrefab);
            }
        }

        private List<T> SampleObjects<T>(T[] objects, int count)
        {
            List<T> sampledObjects = new List<T>();
            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, objects.Length);
                T sampledObject = objects[randomIndex];
                sampledObjects.Add(sampledObject);
            }
            return sampledObjects;
        }

        private Vector3 GetRandomSpawnPosition(float radius)
        {
            // An offset is added to the radius to avoid spawning objects inside the planet
            return Random.onUnitSphere * (radius + 0.5f);
        }

        private GameObject InstantiateRandomPrefab(List<GameObject> prefabs, Vector3 position, Quaternion rotation)
        {
            int randomIndex = Random.Range(0, prefabs.Count);
            GameObject spawnedPrefab = Instantiate(prefabs[randomIndex], position, rotation);
            return spawnedPrefab;
        }

        private void SetRandomLandColor(GameObject prefab, Color land1Color, Color land2Color)
        {
            Color randomColor = Random.Range(0, 2) == 0 ? land1Color : land2Color;
            prefab.GetComponent<Renderer>().material.color = randomColor;
        }

        private void SetRandomMaterial(GameObject prefab, List<Material> materials)
        {
            int randomIndex = Random.Range(0, materials.Count);
            prefab.GetComponent<Renderer>().material = materials[randomIndex];
        }

        private void SetPrefabScale(GameObject prefab, float minScale, float maxScale)
        {
            float randomScale = Random.Range(minScale, maxScale);
            prefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }

        private void RandomizePrefabRotation(Transform prefabTransform)
        {
            prefabTransform.RotateAround(prefabTransform.position, prefabTransform.up, Random.Range(0f, 360f));
        }

        private void ParentPrefabToPlanet(Transform prefabTransform, GameObject planet)
        {
            prefabTransform.parent = planet.transform;
        }

        private void AddMeshCollider(GameObject prefab)
        {
            prefab.AddComponent<MeshCollider>();
        }

        private void SetPrefabTag(GameObject prefab)
        {
            prefab.tag = "Terrain";
        }
    }
}