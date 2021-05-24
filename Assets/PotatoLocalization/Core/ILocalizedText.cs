namespace PotatoLocalization {

	public interface ILocalizedText {

		LocalizationKey Key { get; set; }

		void SetKey(LocalizationKey key);

	}


}