using UnityEngine;

[ExecuteAlways]  // so it works in edit mode
public class GridVisualizer : MonoBehaviour {
    public Grid grid;
    public int width = 20;
    public int height = 10;

    void OnDrawGizmos() {
        if (grid == null) return;

        Gizmos.color = Color.green;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Vector3Int cell = new Vector3Int(x, y, 0);
                Vector3 worldPos = grid.CellToWorld(cell);

                // Draw a wire cube for each cell
                Gizmos.DrawWireCube(
                    worldPos + grid.cellSize / 2,   // center of cell
                    grid.cellSize                   // size of cell
                );
            }
        }
    }
}