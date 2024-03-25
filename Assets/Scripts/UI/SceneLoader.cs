
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace UI
{
    /// <summary>
    /// Component responsible for loading scene
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private Slider _ProgressBar = null;

        /// <summary>
        /// Shows loading screen and starts loading new scene
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public IEnumerator LoadScene(int sceneId)
        {
            gameObject.SetActive(true);
            var asyncLoad = SceneManager.LoadSceneAsync(sceneId);
            while (!asyncLoad.isDone)
            {
                Report(asyncLoad.progress);
                yield return null;
            }
        }
        /// <summary>
        /// Updates progress bar
        /// </summary>
        /// <param name="progress">progress of scene loading</param>
        private void Report(float progress)
        {
            _ProgressBar.value = progress;
        }
    }
}