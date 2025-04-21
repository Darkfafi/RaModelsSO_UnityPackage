using System.Threading;
using UnityEngine;

namespace RaModelsSO
{
	public abstract class RaModelSOBase : ScriptableObject
	{
		public bool IsInitialized
		{
			get; private set;
		}

		public RaModelSOLocator Locator
		{
			get; private set;
		}

		private CancellationTokenSource _cancellationTokenSource;

		public CancellationToken CancellationToken => _cancellationTokenSource != null ? _cancellationTokenSource.Token : new CancellationToken(canceled: true);

		protected void Awake()
		{
			hideFlags = HideFlags.DontUnloadUnusedAsset | HideFlags.HideInHierarchy;
		}

		public void Init(RaModelSOLocator locator)
		{
			if(!IsInitialized)
			{
				Locator = locator;
				_cancellationTokenSource = new CancellationTokenSource();
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
				_cancellationTokenSource.Cancel();
				_cancellationTokenSource = null;
			}
		}

		protected abstract void OnInit();
		protected abstract void OnDeinit();
	}
}