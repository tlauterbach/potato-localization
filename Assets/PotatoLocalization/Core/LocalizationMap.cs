using System;
using System.Collections.Generic;
using UnityEngine;

namespace PotatoLocalization {

	/// <summary>
	/// Holds key value pairs for a language. Add these
	/// assets to the LocalizationMgr
	/// </summary>
	[CreateAssetMenu(fileName = "NewLocalizationMap", menuName = "Localization/Localization Map")]
	public class LocalizationMap : ScriptableObject {

		public LanguageCode LanguageCode {
			get {
				return m_languageCode;
			}
		}
		public string this[LocalizationKey key] {
			get {
				return GetText(key);
			}
		}

		[SerializeField]
		private LanguageCode m_languageCode = new LanguageCode("en");

		[SerializeField]
		private List<LocalizationKey> m_keys = new List<LocalizationKey>();
		[SerializeField]
		private List<string> m_values = new List<string>();

		[NonSerialized]
		private Dictionary<LocalizationKey, string> m_map;

		private const string NULL_FORMAT = "[{0}]";

		/// <summary>
		/// Returns text represented by the given localization 
		/// key for this language.
		/// </summary>
		public string GetText(LocalizationKey key) {
			if (m_map == null) {
				m_map = new Dictionary<LocalizationKey, string>();
				for (int ix = 0; ix < m_keys.Count; ix++) {
					m_map.Add(m_keys[ix], m_values[ix]);
				}
			}
			if (m_map.ContainsKey(key)) {
				return m_map[key];
			} else {
				return string.Format(NULL_FORMAT, key.ToString());
			}
		}

		/// <summary>
		/// Use to clear usage of the runtime localization map.
		/// Useful if you support multiple languages and need
		/// to free up memory
		/// </summary>
		public void Free() {
			// free all of the keys and values
			m_map.Clear();
			m_map = null;
		}

#if UNITY_EDITOR

		public void Add(LocalizationKey key, string value) {
			m_keys.Add(key);
			m_values.Add(value);
		}

		public void Clear() {
			if (m_map != null) {
				m_map.Clear();
			}
			m_keys.Clear();
			m_values.Clear();
		}

#endif


	}

}