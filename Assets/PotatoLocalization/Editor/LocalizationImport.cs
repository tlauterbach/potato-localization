using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System;

namespace PotatoLocalization.Editor {

	public class LocalizationImporter : EditorWindow {

		private class Table {

			public int Width {
				get { return m_width; }
			}
			public int Height {
				get { return m_height; }
			}

			private string[] m_values;
			private int m_height;
			private int m_width;

			public string this[int column, int row] {
				get { return m_values[column + row * m_width]; }
				set { m_values[column + row * m_width] = value; }
			}

			public Table(string firstLine) {
				List<string> values = new List<string>();
				m_height = 1;
				ParseLoop(firstLine, (s, c) => {
					values.Add(s);
				});
				m_width = values.Count;
				m_values = new string[values.Count];
				for (int ix=0; ix < m_values.Length; ix++) {
					m_values[ix] = values[ix];
				}
			}

			public Table(int columns) {
				m_width = columns;
				m_height = 0;
				m_values = new string[0];
			}
			public void ParseLine(string line) {
				AddRow();
				ParseLoop(line, (s,c) => {
					this[c, m_height - 1] = s;
				});
			}

			private void ParseLoop(string line, Action<string,int> setter) {
				int column = 0;
				int length = 0;
				int offset = 0;
				bool inQuote = false;
				while (offset + length < line.Length) {
					char c = line[offset + length];
					if (c == ',' && !inQuote) {
						string value = line.Substring(offset, length);
						if (value.Length > 0 && value[0] == '"') {
							value = value.Substring(1);
						}
						if (value.Length > 0 && value[value.Length - 1] == '"') {
							value = value.Substring(0, value.Length - 1);
						}
						setter(value.Replace("\"\"", "\"").Replace("\\n", "\n"), column);
						column++;
						offset = offset + length + 1;
						length = 0;
						continue;
					} else if (c == '"') {
						if (inQuote && line[offset + length + 1] == '"') {
							// this does not break us out
							length += 2;
							continue;
						} else {
							inQuote = !inQuote;
						}
					}
					length++;
				}
			}

			private void AddRow() {
				m_height += 1;
				Array.Resize(ref m_values, m_values.Length + m_width);
			}
		}

		private TextAsset m_csvFile = null;
		[SerializeField]
		private string m_exportFolder = "Assets/Data/Localization";

		[MenuItem("Window/Localization Importer")]
		public static void Open() {
			LocalizationImporter window = GetWindow<LocalizationImporter>("Localization Importer", true);
			window.Show();
		}

		public void OnGUI() {
			m_csvFile = EditorGUILayout.ObjectField(new GUIContent("CSV File"), m_csvFile, typeof(TextAsset), false) as TextAsset;
			m_exportFolder = EditorGUILayout.TextField(new GUIContent("Export Directory"), m_exportFolder).Trim().Trim('/');

			EditorGUI.BeginDisabledGroup(m_csvFile == null);
			if (GUILayout.Button(new GUIContent("Import"))) {
				List<LocalizationMap> all = new List<LocalizationMap>();
				string[] allGuids = AssetDatabase.FindAssets("t:LocalizationMap");
				foreach (string guid in allGuids) {
					all.Add(AssetDatabase.LoadAssetAtPath<LocalizationMap>(AssetDatabase.GUIDToAssetPath(guid)));
				}
				string[] lines = m_csvFile.text.Split('\n');
				Table table = new Table(lines[0]);
				// parse all lines of CSV file
				for (int index = 1; index < lines.Length; index++) {
					table.ParseLine(lines[index]);
				}
				// traverse each column, using the first column as the key,
				// current column as the data asset, rows as value
				for (int column = 1; column < table.Width; column++) {
					if (string.IsNullOrEmpty(table[column,0])) {
						continue;
					}
					LocalizationMap asset = GetAsset(all, table[column,0], m_exportFolder);
					asset.Clear();
					for (int row = 1; row < table.Height; row++) {
						if (string.IsNullOrEmpty(table[0,row])) {
							continue;
						}
						string value = table[column, row];
						if (string.IsNullOrEmpty(value)) {
							value = string.Format("<{0}>", table[0, row]);
						}
						asset.Add(new LocalizationKey(table[0, row]),value);
					}
				}
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
			EditorGUI.EndDisabledGroup();

		}

		private const string DATA_NAME = "LocalizationMap_{0}";

		
		private static LocalizationMap GetAsset(List<LocalizationMap> all, string twoLetterIsoCode, string exportFolder) {
			string name = string.Format(DATA_NAME,twoLetterIsoCode.ToUpper());
			foreach (LocalizationMap data in all) {
				if (data.name == name) {
					return data;
				}
			}
			// create a new one
			LocalizationMap asset = CreateInstance<LocalizationMap>();
			AssetDatabase.CreateAsset(asset, exportFolder + "/" + name + ".asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return asset;
		}
		
	}


}


