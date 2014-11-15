using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {
	public static int margin = 8;
	public static float unit = 0.4f;
	private Dictionary<intPair,bool> vertical = new Dictionary<intPair, bool>();
	private Dictionary<intPair,bool> horizonal = new Dictionary<intPair, bool>();

	public struct intPair{
		public int x;
		public int y;
		public intPair(int _x, int _y){
			x = _x;
			y = _y;
		}
		public intPair(Vector2 vec){
			x = (int)Mathf.Round(vec.x/unit);
			y = (int)Mathf.Round(vec.y/unit);
		}
		public Vector2 position(){
			return new Vector2 (x * unit, y * unit);
		}
		public intPair[] neighbor(){
			return new intPair[] {new intPair(x+1,y+1),
				new intPair(x-1,y+1),
				new intPair(x+1,y-1),
				new intPair(x-1,y-1),
				new intPair(x+1,y),
				new intPair(x-1,y),
				new intPair(x,y+1),
				new intPair(x,y-1)};
		}
		public int sum(){
			return x + (y*10);
		}
	}
	List<intPair> closed = new List<intPair>();
	Vector2 drawPos = new Vector2();

//	void OnDrawGizmos(){
//		foreach(intPair dot in closed){
//			Gizmos.DrawSphere(drawPos + dot.position(),0.1f);
//		}
//	}
	public List<Vector2> FindPath(GameObject target){
		drawPos = transform.position;
		closed.Clear();
		vertical.Clear ();
		horizonal.Clear ();
		//1. set search range
		intPair targetLoc = new intPair(target.transform.position - transform.position);
		intPair LBound = new intPair(
			targetLoc.x>0?-margin:targetLoc.x-margin,
			targetLoc.y>0?-margin:targetLoc.y-margin);
		intPair UBound = new intPair(
			targetLoc.x<0?margin:targetLoc.x+margin,
			targetLoc.y<0?margin:targetLoc.y+margin);
		//2. start recursive valuemarker
		Dictionary<intPair,intPair> values = new Dictionary<intPair, intPair>();
		values.Add(new intPair(0,0),new intPair(0,
		                                        (0>targetLoc.x?-targetLoc.x:targetLoc.x)
		                                        +(0>targetLoc.y?-targetLoc.y:targetLoc.y)));
		if(markValue(new intPair(0,0),targetLoc,0,LBound,UBound,values,closed)){
			//markValue(new intPair(0,0),targetLoc,0,LBound,UBound,values,closed);
			//3. start recursive returner
			List<Vector2> path = new List<Vector2>();
			return returner(targetLoc,new intPair(0,0),values,path);
		}
		return new List<Vector2>();
	}
	bool markValue(intPair cursor, intPair target, int distFromStart, intPair lower, intPair upper, Dictionary<intPair,intPair> values, List<intPair> closed){
		if(cursor.Equals(target)){
			return true;
		}else{
			closed.Add(cursor);
			int distance = int.MaxValue;
			intPair nextPoint = new intPair(int.MaxValue,int.MaxValue);
			for (int i = 0; i<cursor.neighbor().Length; i++) {
				intPair point = cursor.neighbor()[i];
				if (!closed.Contains (point)
				    && point.x > lower.x
				    && point.x < upper.x
				    && point.y > lower.y
				    && point.y < upper.y) {
					if (values.ContainsKey (point)) {
						if(values[point].sum ()<distance){
							nextPoint = point;
							distance = values[point].sum();
						}
					}else if (testIntPair (point)) {
						values.Add(point,new intPair(distFromStart+(i>3?10:14),
						                             (point.x>target.x?point.x-target.x:target.x-point.x)
						                             +(point.y>target.y?point.y-target.y:target.y-point.y)));
						if(values[point].sum ()<distance){
							nextPoint = point;
							distance = values[point].sum();
						}
					} else {
							closed.Add (point);
					}
				}
			}
			if(nextPoint.x == int.MaxValue)
				return false;
			else
				return markValue(nextPoint, target, values[nextPoint].x, lower, upper, values, closed);
		}
	}
	List<Vector2> returner(intPair cursor, intPair target, Dictionary<intPair,intPair> values, List<Vector2> path){
		if(cursor.Equals(target)){
			return path;
		}else{
			path.Insert(0,cursor.position() + (Vector2)transform.position);
			int distance = values[cursor].x;
			intPair nextPoint = new intPair(int.MaxValue,int.MaxValue); 
			for (int i = 0; i<cursor.neighbor().Length; i++) {
				intPair point = cursor.neighbor()[i];
				if(point.x==0 && point.y==0)
					return path;
				if(values.ContainsKey(point)){
					if(values[point].x<distance){
						nextPoint = point;
						distance = values[point].x;
					}
				}
			}
			if(nextPoint.x == values[cursor].x)
				return path;
			else{
				return returner (nextPoint, target, values, path);
			}
		}
	}

	bool testIntPair(intPair test){
		intPair upTest = new intPair (test.x, test.y + 1);
		intPair rightTest = new intPair (test.x+1, test.y);
		return
			testPoint (true, test)
			&& testPoint (true, upTest)
			&& testPoint (false, test)
			&& testPoint (false, rightTest);
	}
	private Collider2D[] tempAlloc = new Collider2D[1];
	bool testPoint(bool isVertical, intPair pos){
		if(isVertical){
			if(!vertical.ContainsKey (pos))
				vertical.Add(pos,Physics2D.OverlapPointNonAlloc(pos.position()+Vector2.up*-unit*0.5f+(Vector2)transform.position,tempAlloc,1)==0);
			return vertical[pos];
		}else{
			if(!horizonal.ContainsKey (pos))
				horizonal.Add(pos,Physics2D.OverlapPointNonAlloc(pos.position()+Vector2.right*-unit*0.5f+(Vector2)transform.position,tempAlloc,1)==0);
			return horizonal[pos];
		}
	}
}
