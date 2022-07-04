using UnityEngine;

namespace Events
{
    public class Block : MonoBehaviour
    {
        void Start()
        {

            EventManager.Subscribe("Button1", Helper);
        }

        public void Helper(params object[] parameter)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);

            EventManager.UnSubscribe("Button1", Helper);
        }

    }
}
