using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class PoolSpawner : MonoBehaviour
    {
        [SerializeField] private PoolData _data;
        [SerializeField] private Queue<GameObject> pool = new Queue<GameObject>();

        private void Start()
        {
            if (_data.GetObject.activeSelf)
                _data.GetObject.SetActive(false);
            for (int i = 0; i < _data.GetCapacity; i++)
                pool.Enqueue(CreateItem());
        }

        /// <summary>
        /// Return object (don't forget return back this)
        /// </summary>
        /// <param name="activateItem">Set active object or not</param>
        /// <returns></returns>
        public GameObject Dequeue(bool activateItem = true)
        {
            if (pool.Count == 0)
                pool.Enqueue(CreateItem());
            if (activateItem)
            {
                GameObject item = pool.Dequeue();
                item.SetActive(true);
                return item;
            }
            return pool.Dequeue();
        }
        /// <summary>
        /// Return object and return it back after a while
        /// </summary>
        /// <param name="time">After this time in seconds object return back</param>
        /// <param name="activateItem">Set active object or not</param>
        /// <returns></returns>
        public GameObject Dequeue(float time, bool activateItem = true)
        {
            GameObject item = Dequeue(activateItem);
            StartCoroutine(DequeueDestroy(item, time));
            return item;
        }
        /// <summary>
        /// Return back object
        /// </summary>
        /// <param name="item">Item, which returned</param>
        public void Enqueue(GameObject item)
        {
            item.SetActive(false);
            pool.Enqueue(item);
        }

        private GameObject CreateItem()
        {
            GameObject item = Instantiate(_data.GetObject);
            DontDestroyOnLoad(item);
            return item;
        }
        private IEnumerator DequeueDestroy(GameObject item, float time)
        {
            yield return new WaitForSeconds(time);
            Enqueue(item);
        }
    }
}