using UnityEngine;
using System.Collections;

public class AnySpaceUtilityTools : MonoBehaviour
{
	
	public static Bounds GetViewPortBounds (Camera camera, Vector3 screenStartPonit, Vector3 screenEndPoint)
	{
		//从屏幕2D平面转到3D视口空间
		Vector3 S_point = camera.ScreenToViewportPoint (screenStartPonit);
		Vector3 E_point = camera.ScreenToViewportPoint (screenEndPoint);


		//拿到最小和最大的x和y
		Vector3 min = Vector3.Min (S_point, E_point);//就是左下角
		Vector3 max = Vector3.Max (S_point, E_point);//就是右上角                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            

//		min.z = camera.near;
//		max.z = camera.far;
		min.z = camera.nearClipPlane;
		max.z = camera.farClipPlane;
	
		Bounds bound = new Bounds ();
		bound.SetMinMax (min, max);
		return bound;
	}

	#region 在屏幕上绘制框选

	static Texture2D m_Texture;

	public static Texture2D m_Texture2D {
		get {
			if (m_Texture == null) {
				//在内存中创建一个空的2D纹理图片
				m_Texture = new Texture2D (1, 1);
				m_Texture.SetPixel (0, 0, Color.white);
				m_Texture.Apply ();
			}
			return m_Texture;
		}
	}
	//绘制框选区域
	public static void DrawBoundsOnScreen (Rect rect, Color color)
	{
		GUI.color = color;
		GUI.DrawTexture (rect, m_Texture2D);
		//GUI.color = Color.white;
	}
	//绘制框选区域外边框  [thickness:厚度]
	public static void DrawBoundsBoarder (Rect rect, float thickness, Color color)
	{
		//上边框
		DrawBoundsOnScreen (new Rect (rect.xMin, rect.yMin, rect.width, thickness), color);
		//下边框
		DrawBoundsOnScreen (new Rect (rect.xMin, rect.yMin, thickness, rect.height), color);
		//左边框
		DrawBoundsOnScreen (new Rect (rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
		//右边框
		DrawBoundsOnScreen (new Rect (rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
	}
	//屏幕坐标转换GUI坐标
	public static Rect GetBoundsRectForDrawingOnScreen (Vector3 screenStartPonit, Vector3 screenEndPoint)
	{
		//因为外面我想使用OnGUI来绘制，也就是坐标系不是左下角原点，OnGUI是左上角原点
		screenStartPonit.y = Screen.height - screenStartPonit.y;
		screenEndPoint.y = Screen.height - screenEndPoint.y;

		Vector2 topLeftPoint = Vector2.Min (screenStartPonit, screenEndPoint);
		Vector2 bottomRightPoint = Vector2.Max (screenStartPonit, screenEndPoint);

		return Rect.MinMaxRect (topLeftPoint.x, topLeftPoint.y, bottomRightPoint.x, bottomRightPoint.y);
	}

	#endregion


}
