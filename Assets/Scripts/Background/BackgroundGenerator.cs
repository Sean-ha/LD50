using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
	[SerializeField] private GameObject closeTree;
	[SerializeField] private GameObject midTree;
	[SerializeField] private GameObject farTree;

	[SerializeField] private Transform closeTreeHolderLeft;
	[SerializeField] private Transform closeTreeHolderMid;
	[SerializeField] private Transform closeTreeHolderRight;

	[SerializeField] private Transform midTreeHolderLeft;
	[SerializeField] private Transform midTreeHolderMid;
	[SerializeField] private Transform midTreeHolderRight;

	[SerializeField] private Transform farTreeHolderLeft;
	[SerializeField] private Transform farTreeHolderMid;
	[SerializeField] private Transform farTreeHolderRight;

	private int treeYPos = -15;

	private void Start()
	{
		GenerateBackground(new Vector2(-30f, treeYPos));
	}

	private void GenerateBackground(Vector2 startPos)
	{
		// Max x pos to right of startPos: 60
		int rightDistance = 60;

		List<Transform> treeList;


		int distance = 3;
		float minWidth = 2f;
		float maxWidth = 2.75f;

		// Generate far trees
		treeList = GenerateTrees(startPos, farTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = farTreeHolderMid;
		// Right side
		treeList = GenerateTrees(startPos + new Vector2(rightDistance, 0), farTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = farTreeHolderRight;
		// Left side
		treeList = GenerateTrees(startPos - new Vector2(rightDistance, 0), farTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = farTreeHolderLeft;


		distance = 5;
		minWidth = 2.8f;
		maxWidth = 3.7f;

		// Generate mid trees
		treeList = GenerateTrees(startPos, midTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = midTreeHolderMid;
		treeList = GenerateTrees(startPos + new Vector2(rightDistance, 0), midTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = midTreeHolderRight;
		treeList = GenerateTrees(startPos - new Vector2(rightDistance, 0), midTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = midTreeHolderLeft;



		distance = 8;
		minWidth = 3.75f;
		maxWidth = 4.65f;

		// Generate close trees
		treeList = GenerateTrees(startPos, closeTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = closeTreeHolderMid;
		// Generate right side trees
		treeList = GenerateTrees(startPos + new Vector2(rightDistance, 0), closeTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = closeTreeHolderRight;
		// Generate left side trees
		treeList = GenerateTrees(startPos - new Vector2(rightDistance, 0), closeTree, distance, minWidth, maxWidth, rightDistance);
		foreach (Transform t in treeList)
			t.parent = closeTreeHolderLeft;
	}

	private List<Transform> GenerateTrees(Vector2 startPos, GameObject treeType, int distancePerTree, float minWidth, float maxWidth, int rightDistance)
	{
		List<Transform> treeList = new List<Transform>();

		for (int i = 0; i < rightDistance / distancePerTree; i++)
		{
			Vector2 pos = startPos + new Vector2(i * distancePerTree + Random.Range(-maxWidth / 2, maxWidth / 2), 0);
			GameObject tree = Instantiate(treeType, pos, Quaternion.Euler(0, 0, Random.Range(-2.75f, 2.75f)));

			LineRenderer treeLR = tree.GetComponent<LineRenderer>();
			treeLR.startWidth = Random.Range(minWidth, maxWidth);
			treeLR.endWidth = treeLR.startWidth - 0.35f;
			
			treeList.Add(tree.transform);
		}
		return treeList;
	}
}
