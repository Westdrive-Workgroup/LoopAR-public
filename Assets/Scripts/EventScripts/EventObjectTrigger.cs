namespace DefaultNamespace
{
    public class EventObjectTrigger : EventObject
    {
        public override void StartEventAction()
        {
            if (gameObject.active == false)
            {
                gameObject.SetActive(true);
            }
        }

        public override void EndEventAction()
        {
            if (gameObject.active)
            {
                gameObject.SetActive(false);
            }
        }

    }
}