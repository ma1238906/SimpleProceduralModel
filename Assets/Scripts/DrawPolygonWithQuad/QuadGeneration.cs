using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//目前只完成了很简单的使用Quad生成多个gameobject，还没有进行mesh的vertex合并。
//使用quad去生成墙，todo使用预设的墙的mesh、底部设计、标准层设计、顶部设计。
//两种方式：1.总长/单元数量  没有两侧设计
//          2.总长/单元宽度+两侧剩余宽度
public class QuadGeneration : MonoBehaviour
{
    public float Width;//墙单元的宽度(mesh的宽度)
    public float Height;//墙单元的高度(mesh的高度)
    public GameObject WallUnit;//墙的预设
    public int floors;//层数

    private int wallCount;


#if UNITY_EDITOR
    [ContextMenu("使用quad生成墙体")]
#endif
    public void Test()
    {
        Generate(Vector3.zero, Vector3.right * 8.6f);
        Generate(Vector3.right * 8.6f, new Vector3(8.6f, 0, 8.6f));
        Generate(new Vector3(8.6f, 0, 8.6f),new Vector3(0,0,8.6f));
        Generate(new Vector3(0, 0, 8.6f), Vector3.zero);
    }


    //通过两个点生成一面墙
    public void Generate(Vector3 startPos,Vector3 endPos)
    {
        float wallLength = Vector3.Distance(startPos, endPos);
        Vector3 wallDir = (endPos - startPos).normalized;
        if(wallLength > Width)
        {
            wallCount = (int)Mathf.Floor(wallLength / Width);//使用整面的数量
            float completelyWallLength = wallCount * Width;
            float remainingLength = wallLength - completelyWallLength;

            Quaternion rot = Quaternion.FromToRotation(WallUnit.transform.right,wallDir);//墙的转向
            Instantiate(WallUnit, startPos, rot, transform).transform.localScale = new Vector3(remainingLength/Width/2,1,1);//第一面边缘墙
            for (int i = 0; i < wallCount; i++)
            {
                Instantiate(WallUnit, startPos + wallDir * (remainingLength / 2 + Width * i), rot, transform);//中间墙
            }
            Instantiate(WallUnit, endPos - (wallDir * remainingLength / 2), rot, transform).transform.localScale = new Vector3(remainingLength / Width / 2, 1, 1);//终点处墙
        }
        else
        {
            Debug.Log("宽度不足");
        }
    }
}