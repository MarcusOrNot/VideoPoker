using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TableControl : MonoBehaviour
{
    [SerializeField] private List<ColumnStrings> columnStrings = new List<ColumnStrings>();
    [SerializeField] [HideInInspector] private List<ColumnObject> tableList;
    
    [SerializeField] private GameObject columnPrefub;
    [SerializeField] private GameObject textPrefub;

    private void Awake()
    {
        //refreshData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            if (columnStrings.Count != tableList.Count)
            {
                RefreshData();
            }
            
        }
    }

    private void RefreshData()
    {
        //Debug.Log("str len = " + columnStrings.Count.ToString() + "; table " + tableList.Count.ToString());
        for (int i = 0; i < tableList.Count; i++)
            tableList[i].DestroyData();
        tableList.Clear();

        for (int i = 0; i < columnStrings.Count; i++)
        {
            ColumnObject newColumn = new ColumnObject(Instantiate(columnPrefub, transform), textPrefub);
            tableList.Add(newColumn);
            newColumn.CreateTexts(columnStrings[i].strings);
        }
    }

    public void HighlightColumn(int columnPos)
    {
        for (int i = 0; i < tableList.Count; i++)
            tableList[i].HighlightColumn(false);
        if (columnPos>=0) tableList[columnPos].HighlightColumn(true);
    }

    public void HighlightCell(int columnPos, int rowPos)
    {
        for (int i = 0; i < tableList.Count; i++)
            tableList[i].HighlightCell(-1);

        if (columnPos >= 0) tableList[columnPos].HighlightCell(rowPos);
    }

    public string GetCellValue(int columnPos, int rowPos)
    {
        return tableList[columnPos].GetString(rowPos);
    }

    [System.Serializable]
    public class ColumnStrings
    {
        public string[] strings;
    }

    [System.Serializable]
    public class ColumnObject
    {
        //public string[] columnStrings;
        [SerializeField] private GameObject _columnObject;
        private GameObject _textPrefub;
        [SerializeField] private GameObject[] _stringObjects;

        public ColumnObject(GameObject columnObj, GameObject textPrefub)
        {
            _columnObject = columnObj;
            _textPrefub = textPrefub;
        } 

        public int GetStringsCount()
        {
            if (_stringObjects != null) return _stringObjects.Length;
            return 0;
        }
        
        public void CreateTexts(string[] columnStrings)
        {
            ClearColumn();
            _stringObjects = new GameObject[columnStrings.Length];
            for (int i = 0; i < columnStrings.Length; i++)
            {
                _stringObjects[i] = Instantiate(_textPrefub, _columnObject.transform);
                _stringObjects[i].GetComponentInChildren<Text>().text = columnStrings[i];
            }
        }

        public void ClearColumn()
        {
            if (_stringObjects!=null)
                for (int i = 0; i < _stringObjects.Length; i++)
                    DestroyImmediate(_stringObjects[i]);
            _stringObjects = null;
        }

        public void DestroyData()
        {
            ClearColumn();
            DestroyImmediate(_columnObject);
            _columnObject = null;
        }

        public void HighlightColumn(bool isHighlight)
        {
            _columnObject.GetComponent<Image>().enabled = isHighlight;
        }

        public void HighlightCell(int cellPos)
        {
            for (int i = 0; i < _stringObjects.Length; i++)
                _stringObjects[i].GetComponent<Image>().enabled = false;

            if (cellPos >= 0) _stringObjects[cellPos].GetComponent<Image>().enabled = true;
        }

        public string GetString(int cellPos)
        {
            if (cellPos >= 0)
                return _stringObjects[cellPos].GetComponentInChildren<Text>().text;

            return "";
        }
    }
}


