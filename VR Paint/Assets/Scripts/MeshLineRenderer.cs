using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshLineRenderer : MonoBehaviour {

    public Material material;

	private Mesh mesh;

	private Vector3 startPoint;

    private float lineSize = .1f;

    private bool firstQuad = true;

    void Start() {
        mesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshRenderer>().material = material;
    }

	// Function to set the line width
    public void setWidth(float width) {
        lineSize = width;
    }

	// Function to add points to our custom mesh
    public void AddPoint(Vector3 endPoint) {
        if (startPoint != Vector3.zero) {
            AddLine(mesh, MakeQuad(startPoint, endPoint, lineSize, firstQuad));
            firstQuad = false;
        }

        startPoint = endPoint;
    }

	// Function to make a Quad or a Line giving him:
	// - Start point (Vector3)
	// - End point (Vector3)
	// - Width
	// - True to make a Quad. False to make a Line
	Vector3[] MakeQuad(Vector3 startPoint, Vector3 endPoint, float width, bool all) {
        width = width / 2;

		Vector3[] quadPoints;
        if (all) {
            quadPoints = new Vector3[4];
        } else {
            quadPoints = new Vector3[2];
        }

        Vector3 n = Vector3.Cross(startPoint, endPoint);
        Vector3 l = Vector3.Cross(n, endPoint - startPoint);
        l.Normalize();

        if (all) {
            quadPoints[0] = transform.InverseTransformPoint(startPoint + l * width);
            quadPoints[1] = transform.InverseTransformPoint(startPoint + l * -width);
            quadPoints[2] = transform.InverseTransformPoint(endPoint + l * width);
            quadPoints[3] = transform.InverseTransformPoint(endPoint + l * -width);
        } else {
            quadPoints[0] = transform.InverseTransformPoint(startPoint + l * width);
            quadPoints[1] = transform.InverseTransformPoint(startPoint + l * -width);
        }
        return quadPoints;
    }


	// Function to make a custom mesh using procedural building.
	// We generate the vertices, triangles, uvs, and finally we calculate the bounds and the normals.
	void AddLine(Mesh mesh, Vector3[] quadPoints) {
        int vl = mesh.vertices.Length;

		Vector3[] vertices = mesh.vertices;
        vertices = resizeVertices(vertices, 2 * quadPoints.Length);

        for (int i = 0; i < 2 * quadPoints.Length; i += 2) {
            vertices[vl + i] = quadPoints[i / 2];
            vertices[vl + i + 1] = quadPoints[i / 2];
        }

        Vector2[] uvs = mesh.uv;
        uvs = resizeUVs(uvs, 2 * quadPoints.Length);

        if (quadPoints.Length == 4) {
            uvs[vl] = Vector2.zero;
            uvs[vl + 1] = Vector2.zero;
            uvs[vl + 2] = Vector2.right;
            uvs[vl + 3] = Vector2.right;
            uvs[vl + 4] = Vector2.up;
            uvs[vl + 5] = Vector2.up;
            uvs[vl + 6] = Vector2.one;
            uvs[vl + 7] = Vector2.one;
        } else {
            if (vl % 8 == 0) {
                uvs[vl] = Vector2.zero;
                uvs[vl + 1] = Vector2.zero;
                uvs[vl + 2] = Vector2.right;
                uvs[vl + 3] = Vector2.right;

            } else {
                uvs[vl] = Vector2.up;
                uvs[vl + 1] = Vector2.up;
                uvs[vl + 2] = Vector2.one;
                uvs[vl + 3] = Vector2.one;
            }
        }

        int tl = mesh.triangles.Length;

		int[] triangles = mesh.triangles;
        triangles = resizeTriangles(triangles, 12);

        if (quadPoints.Length == 2) {
            vl -= 4;
        }

		// Generate 2 triangles to make the Front-Facing quad
        triangles[tl] = vl;
        triangles[tl + 1] = vl + 2;
        triangles[tl + 2] = vl + 4;

        triangles[tl + 3] = vl + 2;
        triangles[tl + 4] = vl + 6;
        triangles[tl + 5] = vl + 4;

		// Generate 2 triangles to make the Back-Facing quad
        triangles[tl + 6] = vl + 5;
        triangles[tl + 7] = vl + 3;
        triangles[tl + 8] = vl + 1;

        triangles[tl + 9] = vl + 5;
        triangles[tl + 10] = vl + 7;
        triangles[tl + 11] = vl + 3;

		// Asign the vertices to the mesh
        mesh.vertices = vertices;
		// Asign the uvs to the mesh
        mesh.uv = uvs;
		// Asign the triangles to the mesh
        mesh.triangles = triangles;

		// Recalculate the bounds of the mesh
        mesh.RecalculateBounds();

		// Recalculate the normals of the mesh
        mesh.RecalculateNormals();
    }

	// Function to resize the vertex points
	Vector3[] resizeVertices(Vector3[] oldVertices, int newSize) {
		Vector3[] newVertices = new Vector3[oldVertices.Length + newSize];
        for (int i = 0; i < oldVertices.Length; i++) {
            newVertices[i] = oldVertices[i];
        }

        return newVertices;
    }

	// Function to resize the uv points
	Vector2[] resizeUVs(Vector2[] oldUvs, int newSize) {
        Vector2[] newUvs = new Vector2[oldUvs.Length + newSize];
        for (int i = 0; i < oldUvs.Length; i++) {
            newUvs[i] = oldUvs[i];
        }

		return newUvs;
    }

	// Function to resize the triangle points
	int[] resizeTriangles(int[] oldTriangles, int newSize) {
        int[] newTriangles = new int[oldTriangles.Length + newSize];
        for (int i = 0; i < oldTriangles.Length; i++) {
			newTriangles[i] = oldTriangles[i];
        }

        return newTriangles;
    }
}
