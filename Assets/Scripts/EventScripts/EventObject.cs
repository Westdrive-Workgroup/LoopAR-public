using UnityEngine;

namespace DefaultNamespace
{
    public abstract class EventObject : MonoBehaviour, IEventInterface
    {
        public virtual void StartEventAction()
        {
        }

        public virtual void EndEventAction()
        {
        }
    }
}