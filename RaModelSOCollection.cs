using NestedSO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaModelsSO
{
	[CreateAssetMenu(menuName = "RaModelsSO/Create RaModelSOCollection")]
	public class RaModelSOCollection : NestedSOCollectionBase<RaModelSOBase>, IDisposable
	{
		protected void Awake()
		{
			hideFlags = HideFlags.DontUnloadUnusedAsset;

#if UNITY_EDITOR
			SetupEditorListeners();
#endif
		}

#if UNITY_EDITOR

	[UnityEditor.Callbacks.DidReloadScripts]
	private static void SetupEditorListeners()
	{
		UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
		UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
	}

	private static void OnPlayModeChanged(UnityEditor.PlayModeStateChange state)
	{
		switch(state)
		{
			case UnityEditor.PlayModeStateChange.EnteredPlayMode:
			case UnityEditor.PlayModeStateChange.ExitingPlayMode:
				return;
		}

		EditorDisposeCollections();
	}

	private static void EditorDisposeCollections()
	{
		string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(RaModelSOCollection)}");
		for (int i = 0; i < guids.Length; i++)
		{
			string pathToCollection = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
			if (!string.IsNullOrEmpty(pathToCollection))
			{
				RaModelSOCollection collection = UnityEditor.AssetDatabase.LoadAssetAtPath<RaModelSOCollection>(pathToCollection);
				if (collection != null)
				{
					collection.Dispose();
				}
			}
		}
	}
#endif

		public T GetModelSO<T>(Predicate<T> predicate = null)
			where T : RaModelSOBase
		{
			predicate = predicate ?? new Predicate<T>((x) => true);
			IReadOnlyList<RaModelSOBase> items = GetItems();
			for(int i = 0; i < items.Count; i++)
			{
				if(items[i] is T castedItem && predicate(castedItem))
				{
					castedItem.Init(this);
					return castedItem;
				}
			}

			return null;
		}

		public void Dispose()
		{
			for(int i = Count - 1; i >= 0; i--)
			{
				RaModelSOBase item = this[i];
				item.Deinit();
			}
		}
	}
}