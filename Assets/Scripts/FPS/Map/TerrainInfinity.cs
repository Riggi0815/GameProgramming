using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainInfinity : MonoBehaviour {

	public const float maxViewDst = 450;
	public Transform player;
	public Material mapMat;

	public static Vector2 playerPos;
	private static MapGenerator mapGen;
	int chunkSize;
	int chunksViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	void Start() {
		mapGen = FindObjectOfType<MapGenerator>();
		chunkSize = MapGenerator.mapChunkSize - 1;
		chunksViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
	}

	void Update() {
		playerPos = new Vector2 (player.position.x, player.position.z);
		UpdateVisibleChunks ();
	}
		
	void UpdateVisibleChunks() {

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) {
			terrainChunksVisibleLastUpdate [i].SetVisible (false);
		}
		terrainChunksVisibleLastUpdate.Clear ();
			
		int currentChunkCoordX = Mathf.RoundToInt (playerPos.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt (playerPos.y / chunkSize);

		for (int yOffset = -chunksViewDst; yOffset <= chunksViewDst; yOffset++) {
			for (int xOffset = -chunksViewDst; xOffset <= chunksViewDst; xOffset++) {
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
					terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					if (terrainChunkDictionary [viewedChunkCoord].IsVisible ()) {
						terrainChunksVisibleLastUpdate.Add (terrainChunkDictionary [viewedChunkCoord]);
					}
				} else {
					terrainChunkDictionary.Add (viewedChunkCoord, new TerrainChunk (viewedChunkCoord, chunkSize, transform, mapMat));
				}

			}
		}
	}

	public class TerrainChunk {

		GameObject meshObject;
		Vector2 position;
		Bounds bounds;

		MapData mapData;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;

		public TerrainChunk(Vector2 coord, int size, Transform parent, Material mat) {
			position = coord * size;
			bounds = new Bounds(position,Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

			meshObject = new GameObject("Terrain Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshRenderer.material = mat; 
			meshObject.layer = LayerMask.NameToLayer("Ground");
			meshObject.transform.position = positionV3;
			meshObject.transform.parent = parent;
			SetVisible(false);
			
			mapGen.RequestMapData(OnMapDataReceived);
		}

		void OnMapDataReceived(MapData mapData) {
			mapGen.RequestMeshData(mapData, OnMeshDataReceived);
		}

		void OnMeshDataReceived(MeshData meshData) {
			meshFilter.mesh = meshData.CreateMesh();
		}

		public void UpdateTerrainChunk() {
			float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance (playerPos));
			bool visible = viewerDstFromNearestEdge <= maxViewDst;
			SetVisible (visible);
		}

		public void SetVisible(bool visible) {
			meshObject.SetActive (visible);
		}

		public bool IsVisible() {
			return meshObject.activeSelf;
		}

	}
}