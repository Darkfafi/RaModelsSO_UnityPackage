using NestedSO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RaModelsSO
{
	[CreateAssetMenu(menuName = "RaModelsSO/Create RaModelSOLocator")]
	public class RaModelSOLocator : NestedSOCollectionBase<RaModelSOBase>, IDisposable
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
			string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(RaModelSOLocator)}");
			for (int i = 0; i < guids.Length; i++)
			{
				string pathToCollection = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
				if (!string.IsNullOrEmpty(pathToCollection))
				{
					RaModelSOLocator locator = UnityEditor.AssetDatabase.LoadAssetAtPath<RaModelSOLocator>(pathToCollection);
					if (locator != null)
					{
						locator.Dispose();
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