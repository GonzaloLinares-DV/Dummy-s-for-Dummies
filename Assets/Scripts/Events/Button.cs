using UnityEngine;

namespace Events
{
    public class Button : MonoBehaviour
    {
        public void TouchButton()
        {
            EventManager.Trigger("Button1");
        }

    }
}
