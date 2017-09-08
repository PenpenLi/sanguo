using UnityEngine;
using System.Collections;
using Assets.Scripts.manager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class SceneTool {

    public static bool isLoading = false;
    //注意这里返回值一定是 IEnumerator  
    /// <summary>
    /// 统一 异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public static IEnumerator LoadScene(string sceneName) {
        Debug.LogFormat("开始加载场景,scene name is {0}", sceneName);
        if (!isLoading) {
            isLoading = true;
            GameObject go = PopupManager.AddWindow(PopupWindowName.LOADING_PANEL);
            Text progress = go.transform.Find("TitleText").gameObject.GetComponent<Text>();
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
            while (!async.isDone) {
                //  Debug.Log(async.progress);
                progress.text = (int)(async.progress * 100) + "%";
                yield return new WaitForEndOfFrame();//<strong>加上这么一句就可以先显示加载画面然后再进行加载</strong>  
            }
            progress.text =  "100%";
            //读取完毕后返回， 系统会自动进入C场景  
            yield return async;
        } else {
            Debug.Log("loading is acting !!! ");
            yield return null;
        }
    }
}
