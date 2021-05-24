using BeauData;
using PotatoUtil;
using System;
using System.Globalization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace PotatoLocalization {
	[Serializable]
	public struct LanguageCode : IEquatable<LanguageCode>, ISerializedProxy<string> {

		public static LanguageCode Empty = new LanguageCode();

		[SerializeField]
		private string m_code;

		public LanguageCode(string twoLetterISO) {
			m_code = twoLetterISO;
		}
		public LanguageCode(CultureInfo cultureInfo) {
			m_code = cultureInfo.TwoLetterISOLanguageName;
		}

		public static bool operator ==(LanguageCode lhs, LanguageCode rhs) {
			return lhs.Equals(rhs);
		}
		public static bool operator !=(LanguageCode lhs, LanguageCode rhs) {
			return !lhs.Equals(rhs);
		}

		public bool Equals(LanguageCode other) {
			return GetHashCode() == other.GetHashCode();
		}
		public override bool Equals(object obj) {
			if (obj is LanguageCode element) {
				return Equals(element);
			} else {
				return false;
			}
		}
		public override int GetHashCode() {
			return new FNVHash(m_code);
		}

		public string GetProxyValue(ISerializerContext inContext) {
			return m_code;
		}

		public void SetProxyValue(string inValue, ISerializerContext inContext) {
			m_code = inValue;
		}

#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(LanguageCode))]
		private class Editor : PropertyDrawer {

			private static string[] m_displayNames;
			private static string[] m_languageCodes;

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				if (m_displayNames == null || m_languageCodes == null) {
					CultureInfo[] info = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
					m_displayNames = new string[info.Length];
					m_languageCodes = new string[info.Length];
					for (int index = 0; index < info.Length; index++) {
						m_displayNames[index] = info[index].DisplayName;
						m_languageCodes[index] = info[index].TwoLetterISOLanguageName;
					}
				}
				SerializedProperty code = property.FindPropertyRelative("m_code");
				int selected = Array.FindIndex(m_languageCodes, (item) => {
					return item == code.stringValue;
				});
				selected = EditorGUI.Popup(position, label.text, selected, m_displayNames);

				code.stringValue = m_languageCodes[selected];
				property.serializedObject.ApplyModifiedProperties();
			}
			public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
				return EditorGUIUtility.singleLineHeight;
			}
		}
#endif

	}
}