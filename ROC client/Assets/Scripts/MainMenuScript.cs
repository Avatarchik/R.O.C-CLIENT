using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ProgressBar;

public class MainMenuScript : MonoBehaviour {

    // List of GameObjects needed
    private NetworkScript networkScript;
    private CommunicationManagerScript communicationManagerScript;
    private GameObject loadingPanel;
    private Text errorText;
    private Text statusRtspText;
    private Image statusImage;
    private GameObject initButton;
    private GameObject resetButton;
    private Button connectButton;
    private ProgressBarBehaviour progressBarLabel;

    private AsyncOperation async;

    private void Start() {
        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        statusRtspText = GameObject.Find("StatusRtspText").GetComponent<Text>();
        networkScript = GameObject.Find("ManagerObject").GetComponent<NetworkScript>();
        communicationManagerScript = GameObject.Find("ManagerObject").GetComponent<CommunicationManagerScript>();
        loadingPanel = GameObject.Find("LoadingPanel");
        statusImage = GameObject.Find("StatusImage").GetComponent<Image>();
        initButton = GameObject.Find("MD-Button-Initialiser");
        resetButton = GameObject.Find("MD-Button-Reset");
        connectButton = GameObject.Find("MD-Button-Connect").GetComponentsInChildren<Button>()[0];
        progressBarLabel = GameObject.Find("ProgressBarLabelAbove").GetComponent<ProgressBarBehaviour>();

        loadingPanel.SetActive(false);
        resetButton.SetActive(false);
        connectButton.interactable = false;
    }

    private void Update() {
    }

    // Function called upon initialization of connection
    // Checks that the ip and port are valid and manages the display of errors
    public void InitConnection()
    {
        int portParsed;
        string ip = GameObject.Find("MD-Input-Addr").GetComponentsInChildren<InputField>()[0].text;
        string port = GameObject.Find("MD-Input-Port").GetComponentsInChildren<InputField>()[0].text;

        // Check that ip and port are not empty
        if (string.IsNullOrEmpty(port) == true || string.IsNullOrEmpty(ip) == true) {
            errorText.text = "ERROR : String or port cannot be empty.";
            return;
        }
        // Check that port can be parsed
        else if ((portParsed = System.Int32.Parse(port)) == -1) {
            errorText.text = "ERROR : Port cannot be converted to a number.";
            return;
        }

        // Connect to the cameras and check return value
        if (networkScript.SetUpNetwork(ip, portParsed) == -1) {
            errorText.text = "ERROR : Cannot initialize connection to cameras.";
            return;
        }
        else if (communicationManagerScript.StartGoLink() == -1) {// Connect to the GO software and check return value
            errorText.text = "ERROR : Cannot initialize connection to GO input software.";
            networkScript.ResetNetwork();
            return;
        }
        else {
            statusRtspText.text = networkScript.GetRtspAddr();
            statusImage.color = new Color32(0, 255, 0, 255);
            errorText.text = "";
            resetButton.SetActive(true);
            initButton.SetActive(false);
            connectButton.interactable = true;
        }
    }

    // Reset the connection button
    public void ResetConnection()
    {
        networkScript.ResetNetwork();
        statusImage.color = new Color32(255, 0, 0, 255);
        errorText.text = "";
        statusRtspText.text = "";
        resetButton.SetActive(false);
        initButton.SetActive(true);
        connectButton.interactable = false;
    }

    // Function called upon switching to view scene
    public void SwitchToViewScene()
    {
        StartCoroutine("loading");
    }

    public void SetNbScreen(bool multiScreenOpt)
    {
        if (multiScreenOpt == true)
            networkScript.SetNbCamera(2);
        else
            networkScript.SetNbCamera(1);
    }

    // Routine allowing a loading bar to show while scene changes
    private IEnumerator loading()
    {
        loadingPanel.SetActive(true);
        async = Application.LoadLevelAsync("MainScene");
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress < 0.9f)
                progressBarLabel.Value = async.progress * 100.0f;
            else
                async.allowSceneActivation = true;
            yield return null;
        }
    }
}