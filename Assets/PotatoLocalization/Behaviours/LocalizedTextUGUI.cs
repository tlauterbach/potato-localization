using TMPro;
using UnityEngine;

namespace PotatoLocalization {


	[RequireComponent(typeof(TextMeshPro))]
	public class LocalizedTextUGUI : LocalizedTextBase {

		private TextMeshProUGUI m_component;

		protected override void OnEnable() {
			base.OnEnable();
			m_component = GetComponent<TextMeshProUGUI>();
			RefreshText();
		}

		protected override void RefreshText() {
			m_component.text = LocalizationMgr.GetText(Key);
		}

	}

}