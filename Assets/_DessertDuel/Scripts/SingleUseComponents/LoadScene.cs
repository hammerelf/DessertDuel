//Created by: Ryan King

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HammerElf.Games.DessertDuel
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField]
        private float delayTime;
        [SerializeField]
        private int indexToLoad;
        [Space, Space, SerializeField]
        private bool isAdditive;
        [SerializeField]
        private bool isOnlyCall;

        private void Start()
        {
            if (!isOnlyCall)
            {
                StartCoroutine(LoadDelay(delayTime));
            }
        }

        public void CallLoad()
        {
            StartCoroutine(LoadDelay(delayTime));
        }

        private IEnumerator LoadDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (isAdditive)
            {
                SceneManager.LoadScene(indexToLoad, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(indexToLoad);
            }
        }
    }
}
