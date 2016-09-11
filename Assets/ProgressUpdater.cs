using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ProgressUpdater : MonoBehaviour {

    /// <summary>
    /// The <c>MarchingCuber</c> to monitor for progress changes.
    /// </summary>
    public MarchingCuber Cuber;

    /// <summary>
    /// Reference to the text that should be updated.
    /// </summary>
    private Text _Text;

	// Use this for initialization
	void Start () {
        _Text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        float progress = Cuber.Progress;
        _Text.text = progress.ToString("P0");

        if (progress >= 1.0f)
            Destroy(gameObject, 1.0f);
	}
}
