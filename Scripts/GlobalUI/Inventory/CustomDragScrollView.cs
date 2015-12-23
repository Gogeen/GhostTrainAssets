using UnityEngine;
using System.Collections;

public class CustomDragScrollView : MonoBehaviour {

	public UIScrollView scrollView;

	void OnScroll (float delta)
	{
		if (scrollView && NGUITools.GetActive(this))
			scrollView.Scroll(delta);
	}
}
