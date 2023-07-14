using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainInfinity : MonoBehaviour {

	public const float maxViewDst = 450;
	public Transform player;

	public static Vector2 playerPosition;
	int chunkSize;
	int chunksViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

	void Start() {
		chunkSize = MapGenerator.mapChunkSize - 1;
		chunksViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
	}

	void Update() {
		playerPosition = new Vector2 (player.position.x, player.position.z);
		UpdateVisibleChunks ();
	}
		
	void UpdateVisibleChunks() {

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++) {
			terrainChunksVisibleLastUpdate [i].SetVisible (false);
		}
		terrainChunksVisibleLastUpdate.Clear ();
			
		int currentChunkCoordX = Mathf.RoundToInt (playerPosition.x / chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt (playerPosition.y / chunkSize);

		for (int yOffset = -chunksViewDst; yOffset <= chunksViewDst; yOffset++) {
			for (int xOffset = -chunksViewDst; xOffset <= chunksViewDst; xOffset++) {
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

				if (terrainChunkDictionary.ContainsKey (viewedChunkCoord)) {
					terrainChunkDictionary [viewedChunkCoord].UpdateTerrainChunk ();
					if (terrainChunkDictionary [viewedChunkCoord].IsVisible ()) {
						terrainChunksVisibleLastUpdate.Add (terrainChunkDictionary [viewedChunkCoord]);
					}
				} else {
					terrainChunkDictionary.Add (viewedChunkCoord, new TerrainChunk (viewedChunkCoord, chunkSize, transform));
				}

			}
		}
	}

	public class TerrainChunk {

		GameObject meshObject;
		Vector2 position;
		Bounds bounds;


		public TerrainChunk(Vector2 coord, int size, Transform parent) {
			position = coord * size;
			bounds = new Bounds(position,Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

			meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
			meshObject.layer = LayerMask.NameToLayer("Ground");
			meshObject.transform.position = positionV3;
			meshObject.transform.localScale = Vector3.one * size /10f;
			meshObject.transform.parent = parent;
			SetVisible(false);
		}

		public void UpdateTerrainChunk() {
			float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance (playerPosition));
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