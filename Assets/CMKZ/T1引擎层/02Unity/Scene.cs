using System;//Action
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CMKZ {
    public static partial class LocalStorage {
        public static void 加载场景(string 场景名, Action 回调) {
            UnityAction<Scene, LoadSceneMode> A = (a, b) => { };
            A = (a, b) => {
                回调();
                SceneManager.sceneLoaded -= A;
            };
            SceneManager.sceneLoaded += A;
            SceneManager.LoadScene(场景名);
        }
    }
}