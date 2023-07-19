using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainInfinity : MonoBehaviour {
	private const float viewerChunkUpdateNum = 25f;
	private const float sqrViewerChunkUpdateNum = viewerChunkUpdateNum * viewerChunkUpdateNum;
	
	public LODInfo[] detailLevel;
	public static float maxViewDst;
	
	public Transform player;
	public Material mapMat;

	public static Vector2 playerPos;
	private Vector2 playerPosOld;
	private static MapGenerator mapGen;
	int chunkSize;
	int chunksViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	void Start() {
		mapGen = FindObjectOfType<MapGenerator>();

		maxViewDst = detailLevel[detailLevel.Length - 1].visibleDistThresh;
		chunkSize = MapGenerator.mapChunkSize - 1;
		chunksViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
		
		UpdateVisibleChunks();
	}

	void Update() {
		playerPos = new Vector2 (player.position.x, player.position.z);

		if ((playerPosOld - playerPos).sqrMagnitude > sqrViewerChunkUpdateNum) {
			playerPosOld = playerPos;
			UpdateVisibleChunks();
		}
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
					terrainChunkDictionary.Add (viewedChunkCoord, new TerrainChunk (viewedChunkCoord, chunkSize, detailLevel, transform, mapMat));
				}

			}
		}
	}

	public class TerrainChunk {

		GameObject meshObject;
		Vector2 position;
		Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;

		private LODInfo[] detailLevel;
		private LODMesh[] lodMeshes;
		
		MapData mapData;
		private bool mapDataReceived;
		private int prevLODIndex = -1;

		public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevel, Transform parent, Material mat) {
			this.detailLevel = detailLevel;
			
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

			lodMeshes = new LODMesh[detailLevel.Length];
			for (int i = 0; i < detailLevel.Length; i++) {
				lodMeshes[i] = new LODMesh(detailLevel[i].lod, UpdateTerrainChunk);
			}
			
			mapGen.RequestMapData(position, OnMapDataReceived);
		}

		void OnMapDataReceived(MapData mapData) {
			this.mapData = mapData;
			mapDataReceived = true;

			Texture2D texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
			meshRenderer.material.mainTexture = texture;
			UpdateTerrainChunk();
		}

		public void UpdateTerrainChunk() {
			if (mapDataReceived) {
				float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance (playerPos));
				bool visible = viewerDstFromNearestEdge <= maxViewDst;

				if (visible) {
					int lodIndex = 0;
	
					for (int i = 0; i < detailLevel.Length -1 ; i++) {
						if (viewerDstFromNearestEdge > detailLevel[i].visibleDistThresh) {
							lodIndex = i + 1;
						} else {
							break;
						}	
					}
	
					if (lodIndex != prevLODIndex) {
						LODMesh lodMesh = lodMeshes[lodIndex];
						if (lodMesh.hasMesh) {
							prevLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
						} else if (!lodMesh.hasRquestMesh) {
							lodMesh.RequestMesh(mapData);
						}
					}
				}
				SetVisible (visible);
			}
			
		}

		public void SetVisible(bool visible) {
			meshObject.SetActive (visible);
		}

		public bool IsVisible() {
			return meshObject.activeSelf;
		}

	}

	//LevelOfDetailMesh
	class LODMesh {
		public Mesh mesh;
		public bool hasRquestMesh;
		public bool hasMesh;
		private int lod;
		private System.Action updateCallback;

		public LODMesh(int lod, System.Action updateCallback) {
			this.lod = lod;
			this.updateCallback = updateCallback;
		}

		void OnMeshDataReceived(MeshData meshData) {
			mesh = meshData.CreateMesh();
			hasMesh = true;

			updateCallback();
		}

		public void RequestMesh(MapData mapData) {
			hasRquestMesh = true;
			mapGen.RequestMeshData(mapData, lod, OnMeshDataReceived);
		}
	}
	
	[System.Serializable]
	public struct LODInfo {
		public int lod;
		public float visibleDistThresh;
	}
	
}