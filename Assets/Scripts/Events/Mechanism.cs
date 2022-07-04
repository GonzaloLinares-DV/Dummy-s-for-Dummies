using UnityEngine;

namespace Events
{
    public class Mechanism : MonoBehaviour
    {
        void Start()
        {
            EventManager.Subscribe("Button1", MechanismSound);
        }

        public void MechanismSound(params object[] parameter)
        {
            AudioManager.instance.Play("Mechanism");

            EventManager.UnSubscribe("Button1", MechanismSound);
        }
    }
}
