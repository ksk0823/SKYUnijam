using Mirror;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI;

public class CustomNetworkUI : MonoBehaviour
{
    public TMP_InputField ipInputField; // TMP InputField 연결
    public Button connectButton;       // 클라이언트 연결 버튼
    public Button startHostButton;     // 호스트 시작 버튼
    public TextMeshProUGUI statusText; // 서버 상태 표시 텍스트

    void Start()
    {
        // 버튼 클릭 시 메서드 실행
        connectButton.onClick.AddListener(ConnectToServer);
        startHostButton.onClick.AddListener(StartHost);
    }

    void ConnectToServer()
    {
        string ipAddress = ipInputField.text; // TMP InputField에서 입력값 가져오기
        if (!string.IsNullOrEmpty(ipAddress))
        {
            NetworkManager.singleton.networkAddress = ipAddress; // 네트워크 주소 설정
            NetworkManager.singleton.StartClient(); // Client 시작
            Debug.Log("Trying to connect to: " + ipAddress);

            // 상태 표시
            if (statusText != null)
            {
                statusText.text = $"Connecting to {ipAddress}...";
            }
        }
        else
        {
            Debug.LogError("IP 주소를 입력하세요!");
            if (statusText != null)
            {
                statusText.text = "Please enter an IP address.";
            }
        }
    }

    void StartHost()
    {
        NetworkManager.singleton.StartHost(); // Host 시작

        // 서버 상태 즉시 확인
        if (NetworkServer.active)
        {
            Debug.Log("Server started successfully.");
            if (statusText != null)
            {
                statusText.text = "Server started successfully!";
            }

            // Host 버튼 비활성화
            startHostButton.interactable = false;
        }
        else
        {
            Debug.LogError("Failed to start the server.");
            if (statusText != null)
            {
                statusText.text = "Failed to start the server.";
            }
        }
    }

    void Update()
    {
        // 서버 상태를 주기적으로 UI에 반영
        if (statusText != null)
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                statusText.text = "Server is running and client is connected.";
            }
            else if (NetworkServer.active)
            {
                statusText.text = "Server is running.";
            }
            else if (NetworkClient.isConnected)
            {
                statusText.text = "Connected to the server.";
            }
        }
    }
}
