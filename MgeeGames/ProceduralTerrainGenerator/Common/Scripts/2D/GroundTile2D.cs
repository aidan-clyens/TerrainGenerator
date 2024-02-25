using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GroundTile2D : Tile {
    public Sprite center;
    public Sprite corner;

    public int Height { get; set; }
    public float Temperature { get; set; }
    public float Moisture { get; set; }

    private bool smoothEdges = false;

    enum GroundTileRotation {
        Center,
        TopRight,
        TopLeft,
        BottomRight,
        BottomLeft
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                Vector3Int pos = new Vector3Int(position.x + x, position.y + y, position.z);
                if (HasGroundTile(tilemap, pos)) {
                    tilemap.RefreshTile(pos);
                }
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData) {
        GroundTileRotation rotation;
        if (smoothEdges) {
            rotation = GetTileRotation(position, tilemap);
        }
        else {
            rotation = GroundTileRotation.Center;
        }

        tileData.color = Color.white;
        tileData.colliderType = ColliderType.Sprite;

        if (rotation == GroundTileRotation.Center) {
            tileData.sprite = center;
        }
        else {
            tileData.sprite = corner;
            Matrix4x4 matrix = tileData.transform;
            matrix.SetTRS(Vector3.zero, GetRotation(rotation), Vector3.one);
            tileData.transform = matrix;
        }

        tileData.flags = TileFlags.LockTransform;
    }

    public void SetSmoothEdges(bool smoothEdges) {
        this.smoothEdges = smoothEdges;
    }

    private bool HasGroundTile(ITilemap tilemap, Vector3Int position) {
        return tilemap.GetTile(position) == this;
    }

    private GroundTileRotation GetTileRotation(Vector3Int position, ITilemap tilemap) {
        int height = GetTileHeight(new Vector3Int(position.x, position.y, 0), tilemap);
        int heightTop = GetTileHeight(new Vector3Int(position.x, position.y + 1, 0), tilemap);
        int heightBottom = GetTileHeight(new Vector3Int(position.x, position.y - 1, 0), tilemap);
        int heightLeft = GetTileHeight(new Vector3Int(position.x - 1, position.y, 0), tilemap);
        int heightRight = GetTileHeight(new Vector3Int(position.x + 1, position.y, 0), tilemap);

        if ((heightTop == heightBottom) || (heightLeft == heightRight)) {
            return GroundTileRotation.Center;
        }

        if ((height != heightRight) && (height != heightBottom)) {
            if (height > heightRight) {
                return GroundTileRotation.TopLeft;
            }
        }

        if ((height != heightLeft) && (height != heightBottom)) {
            if (height > heightLeft) {
               return GroundTileRotation.TopRight;
            }
        }

        if ((height != heightRight) && (height != heightTop)) {
            if (height > heightRight) {
                return GroundTileRotation.BottomLeft;
            }
        }

        if ((height != heightLeft) && (height != heightTop)) {
            if (height > heightLeft) {
                return GroundTileRotation.BottomRight;
            }
        }

        return GroundTileRotation.Center;
    }

    private int GetTileHeight(Vector3Int position, ITilemap tilemap) {
        if (HasGroundTile(tilemap, position)) {
            GroundTile2D tile = (GroundTile2D)tilemap.GetTile(position);
            return tile.Height;
        }

        return -1;
    }

    private Quaternion GetRotation(GroundTileRotation rotation) {
        switch (rotation) {
            case GroundTileRotation.Center:
            case GroundTileRotation.TopLeft:
                return Quaternion.Euler(0f, 0f, 0f);
            case GroundTileRotation.TopRight:
                return Quaternion.Euler(0f, 180f, 0f);
            case GroundTileRotation.BottomLeft:
                return Quaternion.Euler(180f, 0f, 0f);
            case GroundTileRotation.BottomRight:
                return Quaternion.Euler(180f, 180f, 0f);
        }

        return Quaternion.Euler(0f, 0f, 0f);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/GroundTile2D")]
    public static void CreateGroundTile() {
        string path = EditorUtility.SaveFilePanelInProject("Save Ground Tile", "New Ground Tile", "Asset", "Save Ground Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GroundTile2D>(), path);
    }
#endif
}