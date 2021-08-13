using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using DatabaseData;
using UIWidgets.Runtime.material;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DataGrid : MonoBehaviour
{
    private List<string> _fieldNames;
    private List<PropertyInfo> _fieldInfos;
    private bool _isInitialized = false;

    private List<Widget> CreateColumns()
    {
        if (_isInitialized == false)
        {
            return new List<Widget>();
        }
        var list = new List<Widget>();
        foreach (var item in _fieldNames)
        {
            var column = new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget>()
                {
                    new Container(
                        decoration: new BoxDecoration(
                            color: Colors.blue
                        ), 
                        width: 100f,
                        alignment: Alignment.center, margin: EdgeInsets.all(4.0f),
                        child: new Text(maxLines: 1, data: item)
                    )
                }
            );
            list.Add(column);
        }
        return list;
    }

    public void InitializeGridDataType<T>()
    {
        _fieldNames = typeof(T).GetProperties().Select(f => f.Name).ToList();
        _fieldInfos = new List<PropertyInfo>();
        GameData test = new GameData();
        foreach (var item in _fieldNames)
        {
            var field = typeof(T).GetProperty($"{item}");
            var field2 = test.GetType().GetProperty($"{item}");
            if (field == null)
            {
                continue;
            }
            _fieldInfos.Add(field);
        }

        _isInitialized = true;
    }

    public void SetData<T>(List<T> data)
    {
        if (_isInitialized == false ||
            _fieldInfos == null ||
            _fieldInfos.Count == 0)
        {
            return;
        }

        Func<List<Widget>> tableInfo = () =>
        {
            var tableRows = CreateColumns();
            var i = 0;
            var j = 0;
            foreach (var dataItem in data)
            {
                var rowIndex = j;
                foreach (var field in _fieldInfos)
                {
                    var columnIndex = i;
                    Column col = (Column)tableRows[i];
                    col.children.Add(new Container(
                        width: 100f,
                        alignment: Alignment.center,
                        margin: EdgeInsets.all(4.0f),
                        child: new TextFormField(
                            initialValue: $"{field.GetValue(dataItem)}",
                            onFieldSubmitted: (s) =>
                            {
                                ValueChanged(s, columnIndex, rowIndex);
                            }
                        )
                    ));
                    i++;
                }
                i = 0;
                j++;
            }

            return tableRows;
        };

        ServiceLocator.Instance.GetService<UITable>().TableInfo = tableInfo;
        ServiceLocator.Instance.GetService<UITable>().UpdateUi();
    }

    void Start()
    {
        InitializeGridDataType<GameData>();
        List<GameData> data = new List<GameData>();

        var db = ServiceLocator.Instance.GetService<DatabaseManager>();
        db.GetAll(DatabaseManager.GameDatabaseName, ref data);
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        SetData(data.Take(100).ToList());
        stopWatch.Stop();
        Debug.Log($"Setting {data.Count} items took around {stopWatch.ElapsedMilliseconds/1000:2} seconds");
    }

    private void ValueChanged(string newText, int columnIndex, int rowIndex)
    {
        Debug.Log($"test {newText} column {columnIndex} row {rowIndex}");
    }
}