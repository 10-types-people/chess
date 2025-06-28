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
    //决定谁飞出去
    bool randomValue ;
    //撞击声音
    public AudioSource audioSource;
    //主场景名字
    public string homeSceneName = "Home";
    public float durationDelay = 1f;
    private void Start()
    {
        // 给登录按钮添加点击事件监听
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        randomValue = Random.Range(0f, 1f) > 0.5f ? true : false;
    }

    void OnLoginButtonClicked()
    {
        // 获取输入的账号和密码
        string account = accountInput.text;
        string password = passwordInput.text;

        // 请求成功，打印返回的数据
        //Debug.Log("Login Response: " + www.downloadHandler.text);
        //结束旋转
        LoginRotation.startRotationFlag = false;
        //播放登录动画
        heijiang.LoginAnim(randomValue);
        hongshuai.LoginAnim(!randomValue);
        Invoke("ChessMask", hongshuai.duration);
        Invoke("PlayHitSound", hongshuai.duration * 0.9f);
        //切换到主页面场景
        Invoke("StartLoadMainScene", hongshuai.duration + hongshuai.durationMask + durationDelay);

        /*// 检查账号和密码是否为空
        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("账号或密码不能为空！");
            return;
        }

        // 构建请求数据
        WWWForm form = new WWWForm();
        form.AddField("account", account);
        form.AddField("password", password);

        // 发送POST请求到Node.js服务器
        StartCoroutine(PostToServer("http://47.236.29.50:8888/login", form));*/
    }

    IEnumerator PostToServer(string url, WWWForm form)
    {
        // 使用Unity的UnityWebRequest进行网络请求
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        // 请求超时时间设置（可选）
        www.timeout = 10;

        // 发送请求并等待结果
        yield return www.SendWebRequest();

        if (www.result==UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            // 请求失败，打印错误信息
            Debug.LogError(www.error);
        }
        else
        {
            // 请求成功，打印返回的数据
            Debug.Log("Login Response: " + www.downloadHandler.text);
            //结束旋转
            LoginRotation.startRotationFlag = false;
            //播放登录动画
            heijiang.LoginAnim(randomValue);
            hongshuai.LoginAnim(!randomValue);
            Invoke("ChessMask", hongshuai.duration);
            Invoke("PlayHitSound", hongshuai.duration*0.9f);
            //切换到主页面场景
            Invoke("StartLoadMainScene", hongshuai.duration+hongshuai.durationMask+durationDelay);
        }
    }

    void PlayHitSound()
    {
        audioSource.Play();
    }
    /// <summary>
    /// 选择来遮屏幕的棋子
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
    /// 加载主页面场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadMainScene(string sceneName)
{
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

    // 在加载过程中，你可以继续更新UI或执行其他任务
    while (!asyncLoad.isDone)
    {
        // 例如，这里可以更新加载进度条
        yield return null;
    }
}
}