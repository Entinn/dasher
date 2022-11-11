using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dasher
{
    public static class ComponentExtensions
    {
        public static T Clone<T>(this T origin, bool setAcitve = true) where T : Component
        {
            T item = GameObject.Instantiate(origin, origin.transform.parent);
            var transform = item.transform;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            item.gameObject.SetActive(setAcitve);
            return item;
        }

        public static void AddSingleAction(this Button button, Action action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action.Invoke);
        }
    }
}