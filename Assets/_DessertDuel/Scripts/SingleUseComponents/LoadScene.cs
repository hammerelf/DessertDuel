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
        [SerializeField]
        private bool isAdditive;

        private void Start()
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
