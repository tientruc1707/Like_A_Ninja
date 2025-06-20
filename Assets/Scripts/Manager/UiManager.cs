using System;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : Singleton<UiManager>
{
    private readonly Stack<View> _history = new Stack<View>();
    private View _currentView;
    [SerializeField] private View _initialView;
    [SerializeField] private View[] _views;

    public void OnSceneLoaded()
    {
        if (_views.Length != 0)
        {
            Array.Clear(_views, 0, _views.Length);
            Debug.Log("Views array cleared.");
        }
        if (_history.Count != 0)
        {
            _history.Clear();
            Debug.Log("View stack cleared.");
        }
        _views = FindObjectsByType<View>(FindObjectsSortMode.None);

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


}
