using UnityEngine;

namespace Events
{
    public class Door : MonoBehaviour
    {
   
        void Start()
        {
            EventManager.Subscribe("Button1", Open);
        }

        public void Open(params object[] parameter)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
            EventManager.UnSubscribe("Button1", Open);

        }
    }
}
