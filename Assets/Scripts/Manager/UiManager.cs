using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : Singleton<UiManager>
{
    private readonly Stack<View> _history = new();
    private View _currentView;
    private View _initialView;
    private View[] _views = Array.Empty<View>();

    public static T GetView<T>() where T : View
    {
        foreach (var view in Instance._views)
        {
            if (view is T typedView)
            {
                return typedView;
            }
        }

        Debug.LogWarning($"View of type {typeof(T)} not found.");
        return null;
    }

    public void OnSceneLoaded()
    {
        _views = FindObjectsByType<View>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].Initialize();
            _views[i].Hide();
        }

    }

    public void OnSceneUnloaded()
    {
        _views = Array.Empty<View>();
        _currentView = null;
        _initialView = null;
        _history.Clear();
    }

    public void RegisterStartingView(View view)
    {
        if (_initialView == null)
        {
            _initialView = view;
            Show(_initialView, true);
        }
        else
        {
            Show<View>();
        }
    }

    public static void Show<T>(bool remember = true) where T : View
    {
        for (int i = 0; i < Instance._views.Length; i++)
        {
            if (Instance._views[i] is T)
            {
                if (Instance._currentView != null)
                {
                    if (remember)
                    {
                        Instance._history.Push(Instance._currentView);
                    }

                    Instance._currentView.Hide();
                }

                Instance._views[i].Show();
                Instance._currentView = Instance._views[i];
            }
        }
    }

    public static void Show(View view, bool remember)
    {
        if (Instance._currentView != null)
        {
            if (remember)
            {
                Instance._history.Push(Instance._currentView);
            }

            Instance._currentView.Hide();
        }

        view.Show();
        Instance._currentView = view;
    }

    public static void ShowLast()
    {
        if (Instance._history.Count > 0)
        {
            Show(Instance._history.Pop(), false);
        }
        else
        {
            Debug.LogWarning("No previous view to show.");
        }
    }

    public void LoadScene(string sceneName)
    {
        OnSceneUnloaded();
        SceneManager.LoadScene(sceneName);
    }
    
}
