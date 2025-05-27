using UnityEngine;

namespace Components.InputComponents
{
    public class InputComponent : MonoBehaviour
    {
    }

    public class ShootComponent : MonoBehaviour
    {
        public float cooldownTime;
        public float lastShotTime;
    }
}