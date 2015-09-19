using UnityEngine;
using System.Collections;

public class NetWorkBaseTest : MonoBehaviour {

	public NetWorkBase netWorkBase;
	// Use this for initialization
	void Start () {

		WWW results = netWorkBase.GET("http://52.69.250.119/");

		Debug.Log(results.text);
	}
}
