using UnityEngine;

namespace PotatoLocalization {


	public abstract class LocalizedTextBase : MonoBehaviour, ILocalizedText {

		public LocalizationKey Key {
			get { return m_key; }
			set { SetKey(value); }
		}

		[SerializeField]
		private LocalizationKey m_key;


		protected virtual void OnEnable() {
			LocalizationMgr.OnLanguageChanged += HandleLanguageChanged;
		}
		protected virtual void OnDisable() {
			LocalizationMgr.OnLanguageChanged -= HandleLanguageChanged;
		}

		public void SetKey(LocalizationKey key) {
			m_key = key;
			RefreshText();
		}
		private void HandleLanguageChanged() {
			RefreshText();
		}

		protected abstract void RefreshText();


	}

}