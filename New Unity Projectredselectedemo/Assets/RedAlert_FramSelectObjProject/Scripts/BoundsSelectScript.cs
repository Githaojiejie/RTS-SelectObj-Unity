using UnityEngine;
using System.Collections.Generic;

//框选功能的脚本
public class BoundsSelectScript : MonoBehaviour
{
	#region Demo功能描述

	/// RTS=Real-Time Strategy Game：即时战略游戏 -【红警】
	/// 框选功能：RTS游戏框选游戏对象功能
	/// 3D/2D的RTS游戏中鼠标在屏幕中划过，可以框选出来3D/2D世界中可选的游戏对象，
	/// 然后统一指挥这些选中的对象动作
	#endregion


	Vector3 m_StartMousePos;
	bool m_IsSelecting = false;
	Bounds m_ViewPortBounds;

	void Update ()
	{
        if (Input.GetMouseButtonDown(1))
        {
            
        }
        //01_场景搭建：地形+对象+可框选脚本组件
        //02_
        if (Input.GetMouseButtonDown (0))
        {
	        m_StartMousePos = Input.mousePosition;
	        m_IsSelecting = true;
	        Transform cubetran = GameObject.Find("Cube").transform;
		}
		if (Input.GetMouseButtonUp (0)) {
			//使用列表存储
			List<SelectableObjScript> objs = new List<SelectableObjScript> ();
			foreach (SelectableObjScript item in FindObjectsOfType<SelectableObjScript> ()) {
				if (isWithinSelecttionBounds (item.gameObject)) {
					objs.Add (item);
					print (item.gameObject.name);
					//取消显示出来UI表示已经选中的对象看起来已经不被
					item.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
				}
			}
			m_IsSelecting = false;
		}
		//03_如果这个是否正在选取中的标识为真，说明玩家正在选取中
		if (m_IsSelecting) {
			//此时要罗列并展示出来所有可框选对象，而且要判断这些东西是否在鼠标框选过的可视范围内
			foreach (SelectableObjScript item in FindObjectsOfType<SelectableObjScript> ()) {
				if (isWithinSelecttionBounds (item.gameObject)) {
					//显示出来UI表示已经选中该对象
					item.gameObject.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
				}
			}
		}
	}

	bool isWithinSelecttionBounds (GameObject obj)
	{
/// 视口空间（ViewPort Space）说明：
		/// 是一个x，y在0~1之间，z为物体到相机的距离空间
/// 查看Unity官方API可见，Camera组件上的ViewPort是相对于Screen屏幕来说的单位空间，左下角(0,0)右上角(1,1)
/// 生活中建立模型启发就是鱼缸，你看到的是那一个平面，也就是屏幕平面，然而里面是有深度的，加上深度就形成了3维空间
/// 也就像平时看到显示器屏幕也好手机屏幕也好，不管他的厚度如何，都是有厚度，这样就形成了一个3维的空间
/// 所以我们鼠标在屏幕2D平面上划过，形成的一个2维的矩形框，在这个视口空间中要从摄像机near开始一直到可见最远的far这个深度
/// 3D游戏对象在这个可视空间内就能看得见，这也是我们人眼看3D世界的原理，你近视就是你的视口深度的far降低了，跟3维世界无关

		//本次功能都统一到视口空间ViewPort中来搞逻辑，因为ViewPort Space就像一个Cube鱼缸空间，可控制，在本空间内就可见
		//鼠标开始框选的点(startX,startY)
		//鼠标结束框选的点(endX,endY),这两个点就会形成一个框选好的平面
		//Camera的near和far,上面框选好的平面加上一个深度z就形成一个3维的空间ViewPort Space，跟个Cube(鱼缸)似的
		//
		if (!m_IsSelecting) {
			return false;
		}

		m_ViewPortBounds = AnySpaceUtilityTools.GetViewPortBounds (Camera.main, m_StartMousePos, Input.mousePosition);
		//判断在这个视口空间内是否包含可框选游戏对象
		return m_ViewPortBounds.Contains (Camera.main.WorldToViewportPoint (obj.transform.position));
	}


	//绘制框选框
	void OnGUI ()
	{
		if (m_IsSelecting) {
			Rect rect = AnySpaceUtilityTools.GetBoundsRectForDrawingOnScreen (m_StartMousePos, Input.mousePosition);
			AnySpaceUtilityTools.DrawBoundsOnScreen (rect, new Color (0.2f, 0.4f, 0.6f, 0.5f));
			AnySpaceUtilityTools.DrawBoundsBoarder (rect, 3f, Color.red);
		}
	}
}
