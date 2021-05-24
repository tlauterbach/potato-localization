using BeauData;
using PotatoUtil;
using System;
using UnityEngine;

namespace PotatoLocalization {

	[Serializable]
	public struct LocalizationKey : IEquatable<LocalizationKey>, ISerializedProxy<string> {

		public static readonly LocalizationKey Empty = new LocalizationKey();

		private const string OFFSET = "LocKey_";

		[SerializeField]
		private string m_key;

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
	}

}