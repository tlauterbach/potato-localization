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
		private int m_hash;

		public LocalizationKey(string key) {
			m_key = key;
			m_hash = Hash(m_key);
		}

		public bool Equals(LocalizationKey other) {
			return m_hash == other.m_hash;
		}
		public override int GetHashCode() {
			return m_hash;
		}

		public string GetProxyValue(ISerializerContext inContext) {
			return m_key;
		}

		public void SetProxyValue(string inValue, ISerializerContext inContext) {
			m_key = inValue;
			m_hash = Hash(m_key);
		}

		public override string ToString() {
			return m_key;
		}

		private static int Hash(string str) {
			if (string.IsNullOrEmpty(str)) {
				return 0;
			} else {
				return new FNVHash(string.Concat(OFFSET, str));
			}
		}


#if UNITY_EDITOR

		[CustomPropertyDrawer(typeof(LocalizationKey))]
		private class Editor : PropertyDrawer {
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				SerializedProperty key = property.FindPropertyRelative("m_key");
				SerializedProperty hash = property.FindPropertyRelative("m_hash");

				EditorGUI.BeginChangeCheck();
				key.stringValue = EditorGUI.TextField(position, label, key.stringValue);
				if (EditorGUI.EndChangeCheck()) {
					hash.intValue = Hash(key.stringValue);
					property.serializedObject.ApplyModifiedProperties();
					Debug.Log(hash.intValue);
				}
			}
			public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
				return EditorGUIUtility.singleLineHeight;
			}
		}

#endif


	}

}