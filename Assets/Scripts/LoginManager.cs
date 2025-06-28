using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField accountInput;
    public TMP_InputField passwordInput;
    public Button loginButton;

    public LoginRotation heijiang;
    public LoginRotation hongshuai;
    public Canvas Canvas;
    //����˭�ɳ�ȥ
    bool randomValue ;
    //ײ������
    public AudioSource audioSource;
    //����������
    public string homeSceneName = "Home";
    public float durationDelay = 1f;
    private void Start()
    {
        // ����¼��ť��ӵ���¼�����
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        randomValue = Random.Range(0f, 1f) > 0.5f ? true : false;
    }

    void OnLoginButtonClicked()
    {
        // ��ȡ������˺ź�����
        string account = accountInput.text;
        string password = passwordInput.text;

        // ����ɹ�����ӡ���ص�����
        //Debug.Log("Login Response: " + www.downloadHandler.text);
        //������ת
        LoginRotation.startRotationFlag = false;
        //���ŵ�¼����
        heijiang.LoginAnim(randomValue);
        hongshuai.LoginAnim(!randomValue);
        Invoke("ChessMask", hongshuai.duration);
        Invoke("PlayHitSound", hongshuai.duration * 0.9f);
        //�л�����ҳ�泡��
        Invoke("StartLoadMainScene", hongshuai.duration + hongshuai.durationMask + durationDelay);

        /*// ����˺ź������Ƿ�Ϊ��
        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("�˺Ż����벻��Ϊ�գ�");
            return;
        }

        // ������������
        WWWForm form = new WWWForm();
        form.AddField("account", account);
        form.AddField("password", password);

        // ����POST����Node.js������
        StartCoroutine(PostToServer("http://47.236.29.50:8888/login", form));*/
    }

    IEnumerator PostToServer(string url, WWWForm form)
    {
        // ʹ��Unity��UnityWebRequest������������
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        // ����ʱʱ�����ã���ѡ��
        www.timeout = 10;

        // �������󲢵ȴ����
        yield return www.SendWebRequest();

        if (www.result==UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            // ����ʧ�ܣ���ӡ������Ϣ
            Debug.LogError(www.error);
        }
        else
        {
            // ����ɹ�����ӡ���ص�����
            Debug.Log("Login Response: " + www.downloadHandler.text);
            //������ת
            LoginRotation.startRotationFlag = false;
            //���ŵ�¼����
            heijiang.LoginAnim(randomValue);
            hongshuai.LoginAnim(!randomValue);
            Invoke("ChessMask", hongshuai.duration);
            Invoke("PlayHitSound", hongshuai.duration*0.9f);
            //�л�����ҳ�泡��
            Invoke("StartLoadMainScene", hongshuai.duration+hongshuai.durationMask+durationDelay);
        }
    }

    void PlayHitSound()
    {
        audioSource.Play();
    }
    /// <summary>
    /// ѡ��������Ļ������
    /// </summary>
    void ChessMask()
    {
        
        if (randomValue)
        {
            heijiang.MaskAnim();
            hongshuai.OutAnim();
        }
        else {
            hongshuai.MaskAnim();
            heijiang.OutAnim();
        }
    }
    void StartLoadMainScene()
    {
        StartCoroutine(LoadMainScene(homeSceneName));
    }

    /// <summary>
    /// ������ҳ�泡��
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadMainScene(string sceneName)
{
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

    // �ڼ��ع����У�����Լ�������UI��ִ����������
    while (!asyncLoad.isDone)
    {
        // ���磬������Ը��¼��ؽ�����
        yield return null;
    }
}
}