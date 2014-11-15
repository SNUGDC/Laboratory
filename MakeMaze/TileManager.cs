using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {
	public mazeSet[] mazeSettings;
	public GameObject tile;

	private Dictionary<intPair,GameObject> tiles = new Dictionary<intPair, GameObject>();
	private intPair startTile;
	private List<intPair> branchableTiles = new List<intPair>();
	private List<intPair> endTiles = new List<intPair>();

	[System.Serializable]
	public struct mazeSet{
		public int doorSwitch;
		public int bossEnemy;
		public int item;
		public int maxLength;
		public Color color;
		public int maxBranch{
			get {return doorSwitch + bossEnemy + item;}
		}
	}
	public struct intPair{
		public int x;
		public int y;
		public intPair(int _x, int _y){
			x = _x;
			y = _y;
		}
		public intPair nextTile(int dirc){
			switch(dirc){
			case 0:
				return new intPair(x,y+1);
			case 1:
				return new intPair(x+1,y+((x%2==0)?0:1));
			case 2:
				return new intPair(x+1,y+((x%2==0)?-1:0));
			case 3:
				return new intPair(x,y-1);
			case 4:
				return new intPair(x-1,y+((x%2==0)?-1:0));
			case 5:
				return new intPair(x-1,y+((x%2==0)?0:1));
			default:
				return new intPair(0,0);
			}
		}
	}
	// Use this for initialization
	void OnEnable () {
		startTile = new intPair(0,0);
		endTiles.Add(startTile);
		tiles.Add(new intPair(0,0),(GameObject)Instantiate(tile, Vector3.zero, Quaternion.identity));
		tiles[new intPair(0,0)].transform.parent = gameObject.transform;

		makeMaze(0, startTile);
	}
	void makeMaze(int mazeNum, intPair start){
		tiles[start].SendMessage("setColor",Color.green);
		
		List<intPair> ends = digLength(
			mazeSettings[mazeNum].maxLength,
			mazeSettings[mazeNum].maxLength,
			mazeSettings[mazeNum].maxBranch-((mazeNum < mazeSettings.Length - 1)?0:1),
			start,new List<intPair>());

		foreach(intPair _tile in ends){
			tiles[_tile].SendMessage("setColor",mazeSettings[mazeNum].color);
		}

		if(ends.Count < mazeSettings[mazeNum].maxBranch-((mazeNum < mazeSettings.Length - 1)?0:1)){
			OnDisable();
			OnEnable();
			return;
		}

		if(mazeNum < mazeSettings.Length - 1){
			intPair newStart;
			do{
				newStart = ends[Random.Range(0,ends.Count)];
			}while(findOpenDirc(newStart)==-1);
			//do not allow closed start
			
			endTiles.AddRange(ends);
			endTiles.AddRange(branchableTiles);
			endTiles.Add (newStart);
			branchableTiles.Clear();
			makeMaze(++mazeNum, newStart);
		}
	}
	void OnDisable(){
		endTiles.Clear();
		branchableTiles.Clear();
		tiles = new Dictionary<intPair, GameObject>();
		for(int i=0; i<transform.childCount; i++){
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	intPair openTile(intPair target, int dirc){
		tiles[target].SendMessage("setDirc",dirc,SendMessageOptions.DontRequireReceiver);
		intPair nextTarget = target.nextTile(dirc);
		if(!tiles.ContainsKey(nextTarget)){
			tiles.Add(nextTarget,(GameObject)Instantiate(
				tile,
				new Vector3(nextTarget.x*0.86f,nextTarget.y+((nextTarget.x%2==0)?0f:0.5f)),
				Quaternion.identity));
			tiles[nextTarget].transform.parent = gameObject.transform;
		}
		tiles[nextTarget].SendMessage("setDirc",(dirc+3)%6,SendMessageOptions.DontRequireReceiver);
		return nextTarget;
	}

	int findOpenDirc(intPair target){
		int startDirc = Random.Range(0,6);
		int testDirc;
		for (int i=0; i<6; i++){
			testDirc = (startDirc+i)%6;
			intPair testTile = target.nextTile(testDirc);
			if(!endTiles.Contains(testTile) && !branchableTiles.Contains(testTile))
				return testDirc;
		}
		return -1;
	}

	List<intPair> digLength(int mazeLength,int maxLength, int mazeBranch, intPair startTile, List<intPair> ends){
		int dirc = findOpenDirc(startTile);
		if(dirc == -1){
			Debug.Log ("Blocked");
			tiles[startTile].SendMessage("setColor",Color.red);
			branchableTiles.Remove(startTile);
			ends.Add(startTile);
			endTiles.Add (startTile);
			List<intPair> Branchable = new List<intPair>();
			foreach(intPair closedSearch in branchableTiles){
				if(findOpenDirc(closedSearch)!=-1){
					Branchable.Add(closedSearch);
				}
			}
			if(Branchable.Count>0 && mazeBranch>0){
				return digLength (Random.Range(0,maxLength), maxLength, --mazeBranch, Branchable[Random.Range(0,Branchable.Count)],ends);
			}else{
				return ends;
			}
		}
		
		intPair nextTile = openTile (startTile,dirc);

		if(mazeLength>0){
			branchableTiles.Add(nextTile);
			return digLength (--mazeLength, maxLength, mazeBranch, nextTile,ends);
		}else{
			ends.Add(nextTile);
			endTiles.Add(nextTile);
			Debug.Log ("Ended");
			tiles[nextTile].SendMessage("setColor",Color.red);
			List<intPair> Branchable = new List<intPair>();
			foreach(intPair closedSearch in branchableTiles){
				if(findOpenDirc(closedSearch)!=-1){
					Branchable.Add(closedSearch);
				}
			}
			if(Branchable.Count>0 && mazeBranch>0){
				return digLength (Random.Range(0,maxLength), maxLength, --mazeBranch, Branchable[Random.Range(0,Branchable.Count)],ends);
			}else{
				return ends;
			}
		}
	}
}
