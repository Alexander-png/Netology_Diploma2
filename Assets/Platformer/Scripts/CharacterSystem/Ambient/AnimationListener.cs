using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Ambient
{
	public class AnimationListener : MonoBehaviour
	{
		private Action<string> _eventListner;

		public void SetListener(Action<string> eventListener) =>
			_eventListner = eventListener;

		public void OnAnimationEnd(string animationClipName)
        {
			if (_eventListner != null)
            {
				_eventListner(animationClipName);
            }
		}	
	}
}