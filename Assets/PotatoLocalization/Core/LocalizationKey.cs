using BeauData;
using PotatoUtil;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace PotatoLocalization {

	[Serializable]
	public struct LocalizationKey : IEquatable<LocalizationKey>, ISerializedProxy<string> {

		public static readonly LocalizationKey Empty = new LocalizationKey();

		private const string OFFSET = "LocKey_";

		[SerializeField]
		private string m_key;
		[SerializeField]
		private FNVHash m_hash;


		public LocalizationKey(string key) {
			m_key = key;
			m_hash = new FNVHash(string.Concat(OFFSET, m_key));
		}

		public bool Equals(LocalizationKey other) {
			return GetHashCode() == other.GetHashCode();
		}
		public override int GetHashCode() {
			return m_hash;
		}

		public string GetProxyValue(ISerializerContext inContext) {
			return m_key;
		}

		public void SetProxyValue(string inValue, ISerializerContext inContext) {
			m_key = inValue;
			m_hash = new FNVHash(string.Concat(OFFSET, m_key));
		}

		#if UNITY_EDITOR

		[CustomPropertyDrawer(typeof(LocalizationKey))]
		private class Editor : PropertyDrawer {
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				SerializedProperty key = property.FindPropertyRelative("m_key");
				key.stringValue = EditorGUI.TextField(position, label, key.stringValue);
				property.FindPropertyRelative("m_hash").FindPropertyRelative("m_hash").intValue = new FNVHash(string.Concat(OFFSET, key.stringValue));
				property.serializedObject.ApplyModifiedProperties();
			}
			public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
				return EditorGUIUtility.singleLineHeight;
			}
		}

		#endif


	}

}