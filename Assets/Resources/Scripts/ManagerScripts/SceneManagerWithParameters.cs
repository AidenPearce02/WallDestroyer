using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public static class SceneManagerWithParameters
{
    private static Dictionary<string, string> parameters;

    /// <summary>
    /// Wrapper/convenience method for normal SceneManager interface.
    /// </summary>
    /// <returns>The active scene name.</returns>
    public static string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Wrapper for normal SceneManager.LoadScene(), with LoadSceneMode parameter.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    /// <param name="mode">LoadSceneMode.</param>
    public static void Load(string sceneName, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
    {
        SceneManager.LoadScene(sceneName, mode);
    }

    /// <summary>
    /// Wrapper for normal SceneManager.LoadScene()
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Convenience method to set key/value and load scene in one line, with LoadSceneMode parameter.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    /// <param name="paramKey">Parameter key.</param>
    /// <param name="paramValue">Parameter value.</param>
    /// <param name="mode">LoadSceneMode.</param>
    public static void Load(string sceneName, string paramKey, string paramValue, [DefaultValue("LoadSceneMode.Single")] LoadSceneMode mode)
    {
        SetParam(paramKey, paramValue);
        SceneManager.LoadScene(sceneName, mode);
    }

    /// <summary>
    /// Convenience method to set key/value and load scene in one line.
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    /// <param name="paramKey">Parameter key.</param>
    /// <param name="paramValue">Parameter value.</param>
    public static void Load(string sceneName, string paramKey, string paramValue)
    {
        SetParam(paramKey, paramValue);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Gets the scene parameters.
    /// </summary>
    /// <returns>The scene parameter dictionary object.</returns>
    public static Dictionary<string, string> GetSceneParameters()
    {
        return parameters;
    }

    /// <summary>
    /// Gets a parameter from the parameter dictionary.
    /// </summary>
    /// <returns>The parameter, or empty string if it doesn't exist.</returns>
    /// <param name="paramKey">Parameter key.</param>
    public static string GetParam(string paramKey)
    {
        if (parameters == null || !parameters.ContainsKey(paramKey))
        {
            return "";
        }
        return parameters[paramKey];
    }

    /// <summary>
    /// Sets the parameter. If the key already exists, it overwrites the existing value.
    /// </summary>
    /// <param name="paramKey">Parameter key.</param>
    /// <param name="paramValue">Parameter value.</param>
    public static void SetParam(string paramKey, string paramValue)
    {
        if (parameters == null)
        {
            parameters = new Dictionary<string, string>();
        }
        if (parameters.ContainsKey(paramKey))
        {
            parameters.Remove(paramKey);
        }
        parameters.Add(paramKey, paramValue);
    }

}