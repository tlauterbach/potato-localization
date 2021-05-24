using PotatoUtil;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PotatoLocalization {


	public sealed class LocalizationMgr : Singleton<LocalizationMgr> {

		public static event Action OnLanguageChanged;

		public static LanguageCode CurrentLanguage {
			get { return Instance.m_currentLanguage; }
		}

		[SerializeField]
		private LanguageCode m_currentLanguage = new LanguageCode("en");
		[SerializeField]
		private LocalizationMap[] m_maps = null;

		[NonSerialized]
		private LocalizationMap m_currentMap;

		
		public static void SetLanguage(LanguageCode code) {
			Instance.m_currentLanguage = code;
			Instance.m_currentMap = Instance.FindLanguageMap(code);
			OnLanguageChanged?.Invoke();
		}

		public static IEnumerable<LanguageCode> GetLanguages() {
			foreach (LocalizationMap map in Instance.m_maps) {
				yield return map.LanguageCode;
			}
		}

		/// <summary>
		/// Returns text represented by the given localization 
		/// key for the currently selected language.
		/// </summary>
		public static string GetText(LocalizationKey key) {
			return Instance.m_currentMap[key];
		}


		protected override void OnAwake() {
			m_currentMap = FindLanguageMap(m_currentLanguage);
		}

		private LocalizationMap FindLanguageMap(LanguageCode code) {
			LocalizationMap map = Array.Find(m_maps, (item) => {
				return item.LanguageCode == code;
			});
			if (map == null) {
				throw new KeyNotFoundException(string.Format("Language code `{0}' is " +
					"not currently supported by the LocalizationMgr (did you forget " +
					"to place a LocalizationMap in the manager?)", code
				));
			}
			return map;
		}

	}

}