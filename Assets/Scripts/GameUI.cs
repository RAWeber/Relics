using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    Text waveCounter;

	// Use this for initialization
	void Start () {
	    if(FindObjectOfType<Spawner>() != null)
        {
            FindObjectOfType<Spawner>().OnNewWave += UpdateWaveCounter;
        }

        waveCounter = transform.FindChild("WaveCounter").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void UpdateWaveCounter(int waveCount)
    {
        waveCounter.text = "WAVE " + waveCount;
    }
}
