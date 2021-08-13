using System;
using System.Collections;
using System.Collections.Generic;
using UIWidgets.Runtime.material;
using UIWidgetsSample;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

public class UITable : UIWidgetsSamplePanel
{
    public Func<List<Widget>> TableInfo = () => new List<Widget>();

    private bool _needUpdate = true;

    public void UpdateUi()
    {
        if (_needUpdate == false)
        {
            return;
        }
        recreateWidget();
        _needUpdate = false;
    }

    protected override void Awake()
    {
        base.Awake();

        ServiceLocator.Instance?.SetService(this);
    }

    protected override Widget createWidget()
    {
        return new MaterialApp(
            showPerformanceOverlay: false,
            home: new TableWidget(ref TableInfo));
    }

    protected override void OnEnable()
    {
        FontManager.instance.addFont(Resources.Load<Font>(path: "fonts/MaterialIcons-Regular"), "Material Icons");
        base.OnEnable();
    }

    public class TableWidget : StatelessWidget
    {
        private Func<List<Widget>> _tableInfo;

        public TableWidget(ref Func<List<Widget>> tableInfo, Key key = null) : base(key)
        {
            _tableInfo = tableInfo;
        }

        public override Widget build(BuildContext context)
        {
            return new MaterialApp(
                home: new Scaffold(
                    appBar: new AppBar(),
                    body: new SingleChildScrollView(
                        scrollDirection: Axis.horizontal,
                        child: new SingleChildScrollView(
                            scrollDirection: Axis.vertical,
                            child: new Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: _tableInfo()
                            )
                        )
                    )
                )
            );
        }
    }
}
