using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Qipan : MonoBehaviour
{
    // Start is called before the first frame update
    /// <summary>
    /// 初始定位和间距
    /// </summary>
    public float zeroY = 0.38f;
    public float zeroX = 3.38f;
    public float zeroZ = 3.44f;
    public float feetX = 0.84f;
    public float feetZ = 0.78f;
    /// <summary>
    /// 是否执红
    /// </summary>
    public bool isRed = true;


    /// <summary>
    /// 棋子类型数组，每种棋子类型一个预制体
    /// </summary>
    public GameObject[] redChessPrefabs;
    public GameObject[] blackChessPrefabs;

    /// <summary>
    /// 棋子权重
    /// </summary>
    public float[] chessValue = { 9, 4, 2, 2, 10000, 4.5f, 1 };
    /// <summary>
    /// 对应车，马，象，士，将，炮，卒棋子的数量
    /// </summary>
    public int[] chessCounts= { 2,2,2,2,1,2,5 };
    /// <summary>
    /// 初始时敌我棋子坐标位置
    /// </summary>
    public int[] startX = { 0, 8, 1, 7, 2, 6, 3, 5, 4, 1, 7, 0, 2, 4, 6, 8 };
    public int[] startZ = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 3, 3, 3, 3, 3 };
    public int[] startEnemyZ = { 9, 9, 9, 9, 9, 9, 9, 9, 9, 7, 7, 6, 6, 6, 6, 6 };
    public S_ChessAction[][] chessState = new S_ChessAction[9][];

    /// <summary>
    /// 棋子延迟降落倍数
    /// </summary>
    public float delayTimes = 0.05f;

    /// <summary>
    /// 选中的棋子
    /// </summary>
    public Animator selectedChess=null;
    Vector3 originalPosition;
    S_ChessAction s_ChessAction;

    /// <summary>
    /// 棋手状态，0为对方回合，1为我方选子，2为我方走子
    /// </summary>
    public int qiPanState = 1;
    /// <summary>
    /// 鼠标点击误差容忍
    /// </summary>
    public float mousePointTolerance = 0.25f;

    void Start()
    {
        initialChessState();
    }

    // Update is called once per frame
    void Update()
    {
        // 检查鼠标左键是否被按下 qiPanState==2&&
        if (qiPanState == 1&&Input.GetMouseButtonDown(0))
        {
            Vector2Int mousePos=GetMousePosition();
            //Debug.Log("鼠标点击位置：X = " + mousePos.x + ", Y= " + mousePos.y);
            if (mousePos.x >= 0&& mousePos.y >= 0) 
            {
                Debug.Log("鼠标点击位置：X = " + mousePos.x + ", Y= " + mousePos.y);
                if(selectedChess != null) MoveChess(mousePos);
            }
            else
            {
                Debug.Log("不合法");
            }
        }
        
    }

    public void CreateChess()
    {
        // 根据isRed变量决定使用红方或黑方的棋子预制体数组
        if (isRed)
        {
            CreateSideChess(redChessPrefabs, startZ,true);
            CreateSideChess(blackChessPrefabs, startEnemyZ,false);
        }
        else
        {
            CreateSideChess(blackChessPrefabs, startZ, false);
            CreateSideChess(redChessPrefabs, startEnemyZ,true);
        }
    }
    void CreateSideChess(GameObject[] prefabs, int[] positionsZ,bool isred)
    {
        // 遍历每种棋子类型
        int flag = 0;
        for (int typeIndex = 0; typeIndex < chessCounts.Length; typeIndex++)
        {
            // 根据当前棋子类型的数量创建棋子
            for (int count = 0; count < chessCounts[typeIndex]; count++)
            {
                // 计算棋子在棋盘上的列位置
                int column = startX[flag];
                // 计算棋子在棋盘上的行位置
                int row = positionsZ[flag++];

                // 实例化棋子预制体
                GameObject chess = Instantiate(prefabs[typeIndex], Vector3.zero, Quaternion.identity);
                //Debug.Log(string.Format("{0}: {1}", zeroX + column * feet, zeroZ + row * feet));
                //Debug.LogFormat("{ 0}:{ 1}", zeroX + column * feet, zeroZ + row * feet);
                // 将棋子设置为棋盘的子对象
                chess.transform.parent = transform;
                chess.transform.localPosition = new Vector3(zeroX - column * feetX, zeroY, zeroZ - row * feetZ);
                //初始化棋子属性
                chess.GetComponent<S_ChessAction>().InitialProperties(isred, typeIndex, !(isred^isRed), new Vector2Int(column, row), chessValue[typeIndex]);
                chessState[column][row] = chess.GetComponent<S_ChessAction>();

                //播放下落动画
                StartCoroutine(DelayedCallWithParameters(delayTimes*(6-typeIndex),chess.GetComponent<Animator>() ));
            }
        }
    }
    /// <summary>
    /// 协程用于棋子异步降落
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="animator"></param>
    /// <returns></returns>
    IEnumerator DelayedCallWithParameters(float delay, Animator animator)
    {
        yield return new WaitForSeconds(delay);
        PlayInitialAnim(animator);
    }
    /// <summary>
    /// 棋子下落动画
    /// </summary>
    /// <param name="animator"></param>
    void PlayInitialAnim(Animator animator)
    {
        animator.SetBool("ToInitial",true);
    }
    /// <summary>
    /// 选中动画
    /// </summary>
    /// <param name="animator"></param>
    public void PlaySelectedAnim(Animator animator)
    {
        if (selectedChess)
        {
            selectedChess.SetBool("Selected", false);
            selectedChess.transform.localPosition = originalPosition;
            s_ChessAction.chessIsSelected = false;
        }
        selectedChess = animator;
        s_ChessAction =animator.GetComponent<S_ChessAction>();
        s_ChessAction.chessIsSelected = true;
        originalPosition = selectedChess.transform.localPosition;
        selectedChess.SetBool("Selected", true);
    }

    /// <summary>
    /// 初始化棋子位置
    /// </summary>
    public void initialChessState()
    {
        for (int i = 0; i < 9; i++)
        {
            chessState[i] = new S_ChessAction[10];
            // 初始化当前行，例如：
            for (int j = 0; j < 10; j++)
            {
                chessState[i][j] = null; // 默认为空
            }
        }
    }
    /*public void initialChessState()
    {
        for (int i = 0; i < 9; i++)
        {
            chessState[i] = new int[10];
            // 初始化当前行，例如：
            for (int j = 0; j < 10; j++)
            {
                chessState[i][j] = 0; // 默认为空
            }
        }
        int k = isRed ? 1 : -1;
        for (int i = 0; i < 9; i++)
        {
            chessState[i][0] = k;
            if(i%2==0)chessState[i][3] = k;
        }
        chessState[1][2] = k;
        chessState[7][2] = k;

        k = -k;
        for (int i = 0; i < 9; i++)
        {
            chessState[i][9] = k;
            if (i % 2 == 0) chessState[i][6] = k;
        }
        chessState[1][7] = k;
        chessState[7][7] = k;
    }
*/
    ///点击棋盘事件,获得棋盘坐标
    public Vector2Int GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector2Int answer = new Vector2Int(-1,-1);
        // 在物体上执行射线检测
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePosition = transform.InverseTransformPoint(hit.point);
           

            // 转换为坐标
            Vector2 customPosition = new Vector2(zeroX- mousePosition.x, zeroZ- mousePosition.z);
            //在坐标外，不处理
            if(customPosition.x>feetX*8+mousePointTolerance||customPosition.x<-mousePointTolerance
                || customPosition.y > feetZ * 9 + mousePointTolerance || customPosition.y < -mousePointTolerance)
            {

            }
            else
            {
                int x=(int)(customPosition.x/feetX);
                int y= (int)(customPosition.y/feetZ);
                if (System.Math.Abs(customPosition.x - x * feetX ) > mousePointTolerance)
                {
                    if ((x + 1) * feetX - customPosition.x <= mousePointTolerance)
                    {
                        x += 1;
                    }
                    else x = -x;
                }
                if (System.Math.Abs(customPosition.y - y * feetZ ) > mousePointTolerance)
                {
                    if ((y + 1) * feetZ - customPosition.y <= mousePointTolerance)
                    {
                        y += 1;
                    }
                    else y = -y;
                }
                answer.Set(x, y);
            }
        }
        return answer;
    }

    public void MoveChess(Vector2Int targetPos)
    {
        if (selectedChess != null&& s_ChessAction.IsAvailablePos(targetPos))
        {
            selectedChess.SetBool("Selected", false);

            if (chessState[targetPos.x][targetPos.y] == null)
            {
                s_ChessAction.Move(targetPos, 0);
            }
            else
            {
                s_ChessAction.Move(targetPos, 1);
            }
        }
        //selectedChess = null;
        //s_ChessAction=null;
    }
}
