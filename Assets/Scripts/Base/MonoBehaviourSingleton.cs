namespace Base
{
    using UnityEngine;

    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();

                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            Debug.LogError($"[Singleton] Multiple instances of singleton {typeof(T)} detected!");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject(typeof(T).Name);
                            _instance = singleton.AddComponent<T>();
                            DontDestroyOnLoad(singleton);
                            Debug.Log($"[Singleton] Created new singleton instance: {typeof(T)}");
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"[Singleton] Duplicate {typeof(T)} found. Destroying this one.");
                Destroy(this.gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }

}