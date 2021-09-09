using TMPro;
using UnityEngine;

namespace PotatoLocalization {


	[RequireComponent(typeof(TextMeshPro))]
	public class LocalizedText : LocalizedTextBase {

		private TextMeshPro m_component;

		protected override void OnEnable() {
			base.OnEnable();
			m_component = GetComponent<TextMeshPro>();
			RefreshText();
		}

		protected override void RefreshText() {
			if (m_component == null) {
				m_component = GetComponent<TextMeshPro>();
			}
			m_component.text = LocalizationMgr.GetText(Key);
		}

	}

}