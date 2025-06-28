using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Qipan : MonoBehaviour
{
    // Start is called before the first frame update
    /// <summary>
    /// ��ʼ��λ�ͼ��
    /// </summary>
    public float zeroY = 0.38f;
    public float zeroX = 3.38f;
    public float zeroZ = 3.44f;
    public float feetX = 0.84f;
    public float feetZ = 0.78f;
    /// <summary>
    /// �Ƿ�ִ��
    /// </summary>
    public bool isRed = true;


    /// <summary>
    /// �����������飬ÿ����������һ��Ԥ����
    /// </summary>
    public GameObject[] redChessPrefabs;
    public GameObject[] blackChessPrefabs;

    /// <summary>
    /// ����Ȩ��
    /// </summary>
    public float[] chessValue = { 9, 4, 2, 2, 10000, 4.5f, 1 };
    /// <summary>
    /// ��Ӧ��������ʿ�������ڣ������ӵ�����
    /// </summary>
    public int[] chessCounts= { 2,2,2,2,1,2,5 };
    /// <summary>
    /// ��ʼʱ������������λ��
    /// </summary>
    public int[] startX = { 0, 8, 1, 7, 2, 6, 3, 5, 4, 1, 7, 0, 2, 4, 6, 8 };
    public int[] startZ = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 3, 3, 3, 3, 3 };
    public int[] startEnemyZ = { 9, 9, 9, 9, 9, 9, 9, 9, 9, 7, 7, 6, 6, 6, 6, 6 };
    public S_ChessAction[][] chessState = new S_ChessAction[9][];

    /// <summary>
    /// �����ӳٽ��䱶��
    /// </summary>
    public float delayTimes = 0.05f;

    /// <summary>
    /// ѡ�е�����
    /// </summary>
    public Animator selectedChess=null;
    Vector3 originalPosition;
    S_ChessAction s_ChessAction;

    /// <summary>
    /// ����״̬��0Ϊ�Է��غϣ�1Ϊ�ҷ�ѡ�ӣ�2Ϊ�ҷ�����
    /// </summary>
    public int qiPanState = 1;
    /// <summary>
    /// ������������
    /// </summary>
    public float mousePointTolerance = 0.25f;

    void Start()
    {
        initialChessState();
    }

    // Update is called once per frame
    void Update()
    {
        // ����������Ƿ񱻰��� qiPanState==2&&
        if (qiPanState == 1&&Input.GetMouseButtonDown(0))
        {
            Vector2Int mousePos=GetMousePosition();
            //Debug.Log("�����λ�ã�X = " + mousePos.x + ", Y= " + mousePos.y);
            if (mousePos.x >= 0&& mousePos.y >= 0) 
            {
                Debug.Log("�����λ�ã�X = " + mousePos.x + ", Y= " + mousePos.y);
                if(selectedChess != null) MoveChess(mousePos);
            }
            else
            {
                Debug.Log("���Ϸ�");
            }
        }
        
    }

    public void CreateChess()
    {
        // ����isRed��������ʹ�ú췽��ڷ�������Ԥ��������
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
        // ����ÿ����������
        int flag = 0;
        for (int typeIndex = 0; typeIndex < chessCounts.Length; typeIndex++)
        {
            // ���ݵ�ǰ�������͵�������������
            for (int count = 0; count < chessCounts[typeIndex]; count++)
            {
                // ���������������ϵ���λ��
                int column = startX[flag];
                // ���������������ϵ���λ��
                int row = positionsZ[flag++];

                // ʵ��������Ԥ����
                GameObject chess = Instantiate(prefabs[typeIndex], Vector3.zero, Quaternion.identity);
                //Debug.Log(string.Format("{0}: {1}", zeroX + column * feet, zeroZ + row * feet));
                //Debug.LogFormat("{ 0}:{ 1}", zeroX + column * feet, zeroZ + row * feet);
                // ����������Ϊ���̵��Ӷ���
                chess.transform.parent = transform;
                chess.transform.localPosition = new Vector3(zeroX - column * feetX, zeroY, zeroZ - row * feetZ);
                //��ʼ����������
                chess.GetComponent<S_ChessAction>().InitialProperties(isred, typeIndex, !(isred^isRed), new Vector2Int(column, row), chessValue[typeIndex]);
                chessState[column][row] = chess.GetComponent<S_ChessAction>();

                //�������䶯��
                StartCoroutine(DelayedCallWithParameters(delayTimes*(6-typeIndex),chess.GetComponent<Animator>() ));
            }
        }
    }
    /// <summary>
    /// Э�����������첽����
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
    /// �������䶯��
    /// </summary>
    /// <param name="animator"></param>
    void PlayInitialAnim(Animator animator)
    {
        animator.SetBool("ToInitial",true);
    }
    /// <summary>
    /// ѡ�ж���
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
    /// ��ʼ������λ��
    /// </summary>
    public void initialChessState()
    {
        for (int i = 0; i < 9; i++)
        {
            chessState[i] = new S_ChessAction[10];
            // ��ʼ����ǰ�У����磺
            for (int j = 0; j < 10; j++)
            {
                chessState[i][j] = null; // Ĭ��Ϊ��
            }
        }
    }
    /*public void initialChessState()
    {
        for (int i = 0; i < 9; i++)
        {
            chessState[i] = new int[10];
            // ��ʼ����ǰ�У����磺
            for (int j = 0; j < 10; j++)
            {
                chessState[i][j] = 0; // Ĭ��Ϊ��
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
    ///��������¼�,�����������
    public Vector2Int GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector2Int answer = new Vector2Int(-1,-1);
        // ��������ִ�����߼��
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePosition = transform.InverseTransformPoint(hit.point);
           

            // ת��Ϊ����
            Vector2 customPosition = new Vector2(zeroX- mousePosition.x, zeroZ- mousePosition.z);
            //�������⣬������
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
