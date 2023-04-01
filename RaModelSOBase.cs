using UnityEngine;

namespace RaModelsSO
{
	public abstract class RaModelSOBase : ScriptableObject
	{
		public bool IsInitialized
		{
			get; private set;
		}

		public RaModelSOCollection DataSOCollection
		{
			get; private set;
		}

		protected void Awake()
		{
			hideFlags = HideFlags.DontUnloadUnusedAsset | HideFlags.HideInHierarchy;
		}

		public void Init(RaModelSOCollection dataSOCollection)
		{
			if(!IsInitialized)
			{
				DataSOCollection = dataSOCollection;
				IsInitialized = true;
				OnInit();
			}
		}

		public void Deinit()
		{
			if(IsInitialized)
			{
				OnDeinit();
				IsInitialized = false;
			}
		}

		protected abstract void OnInit();
		protected abstract void OnDeinit();
	}
}